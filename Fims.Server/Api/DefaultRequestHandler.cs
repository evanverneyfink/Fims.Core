﻿using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Fims.Core;
using Fims.Core.Model;
using Fims.Core.Serialization;
using Fims.Server.Business;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;

namespace Fims.Server.Api
{
    internal class DefaultRequestHandler : IRequestHandler
    {
        /// <summary>
        /// Instantiates a <see cref="DefaultRequestHandler"/>
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="resourceDescriptorHelper"></param>
        /// <param name="resourceHandlerRegistry"></param>
        /// <param name="requestContext"></param>
        /// <param name="resourceSerializer"></param>
        public DefaultRequestHandler(ILogger logger,
                                     IResourceDescriptorHelper resourceDescriptorHelper,
                                     IResourceHandlerRegistry resourceHandlerRegistry,
                                     IRequestContext requestContext,
                                     IResourceSerializer resourceSerializer)
        {
            Logger = logger;
            ResourceDescriptorHelper = resourceDescriptorHelper;
            ResourceHandlerRegistry = resourceHandlerRegistry;
            RequestContext = requestContext;
            ResourceSerializer = resourceSerializer;
        }

        /// <summary>
        /// Gets the logger
        /// </summary>
        private ILogger Logger { get; }

        /// <summary>
        /// Gets the resource type mapper
        /// </summary>
        private IResourceDescriptorHelper ResourceDescriptorHelper { get; }

        /// <summary>
        /// Gets the request processor
        /// </summary>
        private IResourceHandlerRegistry ResourceHandlerRegistry { get; }

        /// <summary>
        /// Gets the request context
        /// </summary>
        private IRequestContext RequestContext { get; }

        /// <summary>
        /// Gets the resource serializer
        /// </summary>
        private IResourceSerializer ResourceSerializer { get; }

        /// <summary>
        /// Handles an HTTP request
        /// </summary>
        /// <returns></returns>
        public async Task<IResponse> HandleRequest()
        {
            try
            {
                // add access control header first
                RequestContext.Response.WithHeader("Access-Control-Allow-Origin", "*");
                
                Logger.Debug("Handling request to {0} {1}. Parsing resource descriptor from path...", RequestContext.Method, RequestContext.Path);

                // get the resource descriptor
                var resourceDescriptor = ResourceDescriptorHelper.GetResourceDescriptor(RequestContext.Path);
                if (resourceDescriptor == null)
                    return RequestContext.Response.WithStatus(HttpStatusCode.NotFound);

                Logger.Debug("Parsing resource descriptor from path...");

                // if the resource type is not valid for this service, it's an unrecognized route
                if (!ResourceHandlerRegistry.IsSupported(resourceDescriptor.RootType))
                    return RequestContext.Response.WithStatus(HttpStatusCode.NotFound);

                Logger.Debug("Successfully parsed resource descriptor for type {0} from path. Creating resource handler...",
                             resourceDescriptor.Type.FullName);

                // get handler for resource type
                var resourceHandler = ResourceHandlerRegistry.Get(resourceDescriptor.Type);
                if (resourceHandler == null)
                {
                    Logger.Error("Failed to created resource handler for type '{0}' even though its a supported type for this API.",
                                 resourceDescriptor.Type);
                    return RequestContext.Response.WithStatus(HttpStatusCode.InternalServerError);
                }

                Logger.Debug("Successfully created resource handler of type {0} for resource type {1}. Processing {2} request...",
                             resourceHandler.GetType().FullName,
                             resourceDescriptor.Type.Name,
                             RequestContext.Method);

                // get or delete do not have a body - just use the route
                if (HttpMethods.IsGet(RequestContext.Method))
                    await HandleGet(resourceHandler, resourceDescriptor);
                else if (HttpMethods.IsDelete(RequestContext.Method))
                    await HandleDelete(resourceHandler, resourceDescriptor);
                else
                {
                    // read body of request as JSON
                    var resource = await ResourceSerializer.Deserialize(await RequestContext.ReadBodyAsText());

                    // ensure that the provided resource ID matches the ID from the route
                    // in the case of a POST, this should be null
                    if (resource.Id != null && resource.Id != resourceDescriptor.Url)
                        return RequestContext.Response
                                             .WithStatus(HttpStatusCode.BadRequest)
                                             .WithPlainTextBody(
                                                 $"Resource ID does not match ID in payload ('{resourceDescriptor.Id}' != '{resource.Id}'");

                    // create or update based on the POST vs PUT
                    // if we have an ID for a POST or no ID for a PUT, the method is not supported for the route
                    if (HttpMethods.IsPost(RequestContext.Method) && resourceDescriptor.Id == null)
                        await HandlePost(resourceHandler, resourceDescriptor, resource);
                    else if (HttpMethods.IsPut(RequestContext.Method) && resourceDescriptor.Id != null)
                        await HandlePut(resourceHandler, resourceDescriptor, resource);
                    else
                        return RequestContext.Response.WithStatus(HttpStatusCode.MethodNotAllowed);
                }

                return RequestContext.Response;
            }
            catch (Exception e)
            {
                return RequestContext.Response.WithStatus(HttpStatusCode.InternalServerError).WithPlainTextBody(e.ToString());
            }
        }

        /// <summary>
        /// Handles a GET by either getting a resource by ID or querying against all resources
        /// </summary>
        /// <param name="resourceHandler"></param>
        /// <param name="resourceDescriptor"></param>
        /// <returns></returns>
        private async Task HandleGet(IResourceHandler resourceHandler, ResourceDescriptor resourceDescriptor)
        {
            if (resourceDescriptor.Id != null)
            {
                // get the single resource
                var resource = await resourceHandler.Get(resourceDescriptor);

                // if found, render the resource and return in the body
                // otherwise, indicate not found
                if (resource != null)
                    RequestContext.Response.WithStatus(HttpStatusCode.OK).WithJsonBody(ResourceSerializer.Serialize(resource));
                else
                    RequestContext.Response.WithStatus(HttpStatusCode.NotFound);
            }
            else
            {
                Logger.Debug("Executing query for resources of type {0}...", resourceDescriptor.Type);

                // no id, so this is a query
                var resourceCollection = await resourceHandler.Query(resourceDescriptor);

                Logger.Debug("Completed query for resources of type {0}.", resourceDescriptor.Type);

                // create JSON array from results and return as body
                RequestContext.Response.WithStatus(HttpStatusCode.OK)
                              .WithJsonBody(new JArray(resourceCollection.Select(r => ResourceSerializer.Serialize(r))));
            }
        }

        /// <summary>
        /// Handles a POST by creating a resource
        /// </summary>
        /// <param name="resourceHandler"></param>
        /// <param name="resourceDescriptor"></param>
        /// <param name="resource"></param>
        /// <returns></returns>
        private async Task HandlePost(IResourceHandler resourceHandler, ResourceDescriptor resourceDescriptor, Resource resource)
        {
            try
            {
                Logger.Info("Creating resource {0}...", resource.Id);
                Logger.Debug("Resource JSON: {0}", resource);

                // create the resource
                var result = await resourceHandler.Create(resourceDescriptor, resource);

                Logger.Info("Successfully created resource {0}.", resource.Id);

                var responseJson = ResourceSerializer.Serialize(result);
                Logger.Debug("Response JSON: {0}", responseJson);

                // return the new object rendered as JSON
                RequestContext.Response.WithStatus(HttpStatusCode.OK).WithJsonBody(responseJson);
            }
            catch (Exception e)
            {
                RequestContext.Response.WithStatus(HttpStatusCode.InternalServerError).WithPlainTextBody(e.ToString());
            }
        }

        /// <summary>
        /// Handles a PUT by updating a resource
        /// </summary>
        /// <param name="resourceHandler"></param>
        /// <param name="resourceDescriptor"></param>
        /// <param name="resource"></param>
        /// <returns></returns>
        private async Task HandlePut(IResourceHandler resourceHandler, ResourceDescriptor resourceDescriptor, Resource resource)
        {
            try
            {
                // get the object first to ensure it exists
                var existing = await resourceHandler.Get(resourceDescriptor);
                if (existing == null)
                {
                    RequestContext.Response.WithStatus(HttpStatusCode.NotFound);
                    return;
                }

                // update resource using handler
                var result = await resourceHandler.Update(resourceDescriptor, resource);
                
                // return the updated object rendered as JSON
                RequestContext.Response.WithStatus(HttpStatusCode.OK).WithJsonBody(ResourceSerializer.Serialize(result));
            }
            catch (Exception e)
            {
                RequestContext.Response.WithStatus(HttpStatusCode.InternalServerError).WithPlainTextBody(e.ToString());
            }
        }

        /// <summary>
        /// Handles DELETE by removing a resource
        /// </summary>
        /// <param name="resourceHandler"></param>
        /// <param name="resourceDescriptor"></param>
        /// <returns></returns>
        private async Task HandleDelete(IResourceHandler resourceHandler, ResourceDescriptor resourceDescriptor)
        {
            try
            {
                // get the resource to ensure it exists
                var existing = await resourceHandler.Get(resourceDescriptor);
                if (existing == null)
                {
                    RequestContext.Response.WithStatus(HttpStatusCode.NotFound);
                    return;
                }

                // delete the resource using the handler
                await resourceHandler.Delete(resourceDescriptor);

                // return OK to indicate the resource was successfully deleted
                RequestContext.Response.WithStatus(HttpStatusCode.OK);
            }
            catch (Exception e)
            {
                RequestContext.Response.WithStatus(HttpStatusCode.InternalServerError).WithPlainTextBody(e.ToString());
            }
        }
    }
}