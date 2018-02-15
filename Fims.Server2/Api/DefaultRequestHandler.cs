using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Fims.Core.JsonLd;
using Fims.Core.Model;
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
        /// <param name="jsonLdContextManager"></param>
        /// <param name="jsonLdResourceHelper"></param>
        /// <param name="environment"></param>
        /// <param name="resourceUrlParser"></param>
        /// <param name="resourceHandlerProvider"></param>
        /// <param name="requestContext"></param>
        public DefaultRequestHandler(ILogger logger,
                                     IJsonLdContextManager jsonLdContextManager,
                                     IJsonLdResourceHelper jsonLdResourceHelper,
                                     IEnvironment environment,
                                     IResourceUrlParser resourceUrlParser,
                                     IResourceHandlerProvider resourceHandlerProvider,
                                     IRequestContext requestContext)
        {
            Logger = logger;
            JsonLdContextManager = jsonLdContextManager;
            JsonLdResourceHelper = jsonLdResourceHelper;
            Environment = environment;
            ResourceUrlParser = resourceUrlParser;
            ResourceHandlerProvider = resourceHandlerProvider;
            RequestContext = requestContext;
        }

        /// <summary>
        /// Gets the logger
        /// </summary>
        private ILogger Logger { get; }

        /// <summary>
        /// Gets the JSON LD context manager
        /// </summary>
        private IJsonLdContextManager JsonLdContextManager { get; }

        /// <summary>
        /// Gets the JSON LD resource helper
        /// </summary>
        private IJsonLdResourceHelper JsonLdResourceHelper { get; }

        /// <summary>
        /// Gets the environment
        /// </summary>
        private IEnvironment Environment { get; }

        /// <summary>
        /// Gets the resource type mapper
        /// </summary>
        private IResourceUrlParser ResourceUrlParser { get; }

        /// <summary>
        /// Gets the request processor
        /// </summary>
        private IResourceHandlerProvider ResourceHandlerProvider { get; }

        /// <summary>
        /// Gets the request context
        /// </summary>
        private IRequestContext RequestContext { get; }

        /// <summary>
        /// Handles an HTTP request
        /// </summary>
        /// <returns></returns>
        public async Task<IResponse> HandleRequest()
        {
            Logger.Debug("Handling request to {0} {1}", RequestContext.Method, RequestContext.Path);

            // check if this is the default context
            if (RequestContext.Path.Equals(Urls.DefaultJsonLdContext, StringComparison.OrdinalIgnoreCase))
            {
                Logger.Debug("Request was made for default JSON-LD context");
                return RequestContext.CreateResponse().WithJsonBody(JsonLdContextManager.GetDefault());
            }

            Logger.Debug("Parsing resource descriptor from path...");

            // get the resource descriptor
            var resourceDescriptor = ResourceUrlParser.GetResourceDescriptor(RequestContext.Path);
            if (resourceDescriptor == null)
                return RequestContext.CreateResponse(HttpStatusCode.NotFound);

            Logger.Debug("Parsing resource descriptor from path...");

            // if the resource type is not valid for this service, it's an unrecognized route
            if (!ResourceHandlerProvider.IsSupportedResourceType(resourceDescriptor.RootType))
                return RequestContext.CreateResponse(HttpStatusCode.NotFound);

            Logger.Debug("Successfully parsed resource descriptor for type {0} from path. Creating resource handler...", resourceDescriptor.Type.FullName);

            // get handler for resource type
            var resourceHandler = ResourceHandlerProvider.CreateResourceHandler(resourceDescriptor);

            Logger.Debug("Successfully created resource handler of type {0} for resource type {1}. Processing {2} request...",
                         resourceHandler.GetType().FullName,
                         resourceDescriptor.Type.Name,
                         RequestContext.Method);

            IResponse response;

            // get or delete do not have a body - just use the route
            if (HttpMethods.IsGet(RequestContext.Method))
                response = await HandleGet(RequestContext, resourceHandler, resourceDescriptor);
            else if (HttpMethods.IsDelete(RequestContext.Method))
                response = await HandleDelete(RequestContext, resourceHandler, resourceDescriptor);
            else
            {
                // read body of request as JSON
                var resource = await GetResourceFromJson(RequestContext, resourceDescriptor);

                // ensure that the provided resource ID matches the ID from the route
                // in the case of a POST, this should be null
                if (resource.Id != resourceDescriptor.Id)
                    return RequestContext.CreateResponse(HttpStatusCode.BadRequest)
                                         .WithPlainTextBody(
                                             $"Resource ID does not match ID in payload ('{resourceDescriptor.Id}' != '{resource.Id}'");

                // create or update based on the POST vs PUT
                // if we have an ID for a POST or no ID for a PUT, the method is not supported for the route
                if (HttpMethods.IsPost(RequestContext.Method) && resourceDescriptor.Id == null)
                    response = await HandlePost(RequestContext, resourceHandler, resourceDescriptor, resource);
                else if (HttpMethods.IsPut(RequestContext.Method) && resourceDescriptor.Id != null)
                    response = await HandlePut(RequestContext, resourceHandler, resourceDescriptor, resource);
                else
                    return RequestContext.CreateResponse(HttpStatusCode.MethodNotAllowed);
            }

            return response.WithHeader("Access-Control-Allow-Origin", "*");
        }

        /// <summary>
        /// Handles a GET by either getting a resource by ID or querying against all resources
        /// </summary>
        /// <param name="requestContext"></param>
        /// <param name="resourceHandler"></param>
        /// <param name="resourceDescriptor"></param>
        /// <returns></returns>
        private async Task<IResponse> HandleGet(IRequestContext requestContext, IResourceHandler resourceHandler, ResourceDescriptor resourceDescriptor)
        {
            if (resourceDescriptor.Id != null)
            {
                // get the single resource
                var resource = await resourceHandler.Get(resourceDescriptor);

                // if found, render the resource and return in the body
                // otherwise, indicate not found
                return resource != null
                           ? requestContext.CreateResponse().WithJsonBody(await GetJsonFromResource(requestContext, resource))
                           : requestContext.CreateResponse(HttpStatusCode.NotFound);
            }

            Logger.Debug("Executing query for resources of type {0}...", resourceDescriptor.Type);

            // no id, so this is a query
            var resourceCollection = await resourceHandler.Query(resourceDescriptor);

            Logger.Debug("Completed query for resources of type {0}.", resourceDescriptor.Type);

            // create JSON array from results and return as body
            return requestContext.CreateResponse().WithJsonBody(
                new JArray(await Task.WhenAll(resourceCollection.Select(r => GetJsonFromResource(requestContext, r)))));
        }

        /// <summary>
        /// Handles a POST by creating a resource
        /// </summary>
        /// <param name="requestContext"></param>
        /// <param name="resourceHandler"></param>
        /// <param name="resourceDescriptor"></param>
        /// <param name="resource"></param>
        /// <returns></returns>
        private async Task<IResponse> HandlePost(IRequestContext requestContext, IResourceHandler resourceHandler, ResourceDescriptor resourceDescriptor, Resource resource)
        {
            try
            {
                // create the resource
                var result = await resourceHandler.Create(resourceDescriptor, resource);
                
                // return the new object rendered as JSON
                return requestContext.CreateResponse().WithJsonBody(await GetJsonFromResource(requestContext, result));
            }
            catch (Exception e)
            {
                return requestContext.CreateResponse(HttpStatusCode.InternalServerError).WithPlainTextBody(e.ToString());
            }
        }

        /// <summary>
        /// Handles a PUT by updating a resource
        /// </summary>
        /// <param name="requestContext"></param>
        /// <param name="resourceHandler"></param>
        /// <param name="resourceDescriptor"></param>
        /// <param name="resource"></param>
        /// <returns></returns>
        private async Task<IResponse> HandlePut(IRequestContext requestContext, IResourceHandler resourceHandler, ResourceDescriptor resourceDescriptor, Resource resource)
        {
            try
            {
                // get the object first to ensure it exists
                var existing = await resourceHandler.Get(resourceDescriptor);
                if (existing == null)
                    return requestContext.CreateResponse(HttpStatusCode.NotFound);

                // update resource using handler
                var result = await resourceHandler.Update(resourceDescriptor, resource);
                
                // return the updated object rendered as JSON
                return requestContext.CreateResponse().WithJsonBody(await GetJsonFromResource(requestContext, result));
            }
            catch (Exception e)
            {
                return requestContext.CreateResponse(HttpStatusCode.InternalServerError).WithPlainTextBody(e.ToString());
            }
        }

        /// <summary>
        /// Handles DELETE by removing a resource
        /// </summary>
        /// <param name="requestContext"></param>
        /// <param name="resourceHandler"></param>
        /// <param name="resourceDescriptor"></param>
        /// <returns></returns>
        private async Task<IResponse> HandleDelete(IRequestContext requestContext, IResourceHandler resourceHandler, ResourceDescriptor resourceDescriptor)
        {
            try
            {
                // get the resource to ensure it exists
                var existing = await resourceHandler.Get(resourceDescriptor);
                if (existing == null)
                    return requestContext.CreateResponse(HttpStatusCode.NotFound);

                // delete the resource using the handler
                await resourceHandler.Delete(resourceDescriptor);

                // return OK to indicate the resource was successfully deleted
                return requestContext.CreateResponse();
            }
            catch (Exception e)
            {
                return requestContext.CreateResponse(HttpStatusCode.InternalServerError).WithPlainTextBody(e.ToString());
            }
        }

        /// <summary>
        /// Gets a resource from the JSON in the body of the request
        /// </summary>
        /// <param name="requestContext"></param>
        /// <param name="resourceDescriptor"></param>
        /// <returns></returns>
        private async Task<Resource> GetResourceFromJson(IRequestContext requestContext, ResourceDescriptor resourceDescriptor)
        {
            return await JsonLdResourceHelper.GetResourceFromJson(JToken.Parse(await requestContext.ReadBodyAsText()),
                                                                  resourceDescriptor.Type,
                                                                  Environment.PublicUrl);
        }

        /// <summary>
        /// Renders a resource to JSON using the provided JSON LD context
        /// </summary>
        /// <param name="requestContext"></param>
        /// <param name="resource"></param>
        /// <returns></returns>
        private async Task<JToken> GetJsonFromResource(IRequestContext requestContext, Resource resource)
        {
            return await JsonLdResourceHelper.GetJsonFromResource(resource,
                                                                  requestContext.QueryParameters.ContainsKey("context")
                                                                      ? requestContext.QueryParameters["context"]
                                                                      : null);
        }
    }
}