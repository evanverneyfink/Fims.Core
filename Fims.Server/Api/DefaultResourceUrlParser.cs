﻿using System.Linq;

namespace Fims.Server.Api
{
    internal class DefaultResourceUrlParser : IResourceUrlParser
    {
        /// <summary>
        /// Instantiates a <see cref="DefaultResourceUrlParser"/>
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="environment"></param>
        /// <param name="urlSegmentResourceMapper"></param>
        public DefaultResourceUrlParser(ILogger logger, IEnvironment environment, IUrlSegmentResourceMapper urlSegmentResourceMapper)
        {
            Logger = logger;
            Environment = environment;
            UrlSegmentResourceMapper = urlSegmentResourceMapper;
        }

        /// <summary>
        /// Gets the logger
        /// </summary>
        private ILogger Logger { get; }

        /// <summary>
        /// Gets the environment
        /// </summary>
        private IEnvironment Environment { get; }

        /// <summary>
        /// Gets the url segment to resource mapper
        /// </summary>
        private IUrlSegmentResourceMapper UrlSegmentResourceMapper { get; }

        /// <summary>
        /// Gets a resource descriptor from the path of a resource
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public ResourceDescriptor GetResourceDescriptor(string path)
        {
            // get the root path from the environment
            var root = $"/{Environment.RootPath ?? string.Empty}";
            if (!path.StartsWith(root))
            {
                Logger.Warning("Received request to path that does not match configured root. Root = {0}, Path = {1}", root, path);
                return null;
            }

            // remove root from path
            path = path.Replace(root, string.Empty);

            if (string.IsNullOrWhiteSpace(path))
            {
                Logger.Warning("Request was made to the root url. Resource type not specified.");
                return null;
            }

            Logger.Debug("Path without root: {0}", path);

            // remove root and then split
            var pathParts = path.TrimStart('/').Split('/');

            var curType = UrlSegmentResourceMapper.GetResourceType(pathParts[0]);
            if (curType == null)
            {
                Logger.Warning("Invalid path part '{0}' in path {1}. Type not found.", pathParts[0], path);
                return null;
            }

            var cur = new ResourceDescriptor(curType);

            // first should be an ID
            var isId = true;

            foreach (var pathPart in pathParts.Skip(1))
            {
                // if this is an ID segment, just 
                if (isId)
                    cur.Id = pathPart;
                else
                {
                    // get the type as a collection of the current type
                    curType = UrlSegmentResourceMapper.GetResourceType(pathPart, curType);
                    if (curType == null)
                    {
                        Logger.Warning("Invalid path part '{0}' in path {1}. Type not found.", pathPart, path);
                        return null;
                    }

                    // create new with existing as parent
                    cur = new ResourceDescriptor(curType) { Parent = cur };
                }

                isId = !isId;
            }

            cur.Url = Environment.PublicUrl.TrimEnd('/') + "/" + path.TrimStart('/');

            return cur;
        }
    }
}