using System;
using Fims.Core.JsonLd;

namespace Fims.Core.Model
{
    public abstract class Resource
    {
        protected Resource(string type = null)
        {
            ContextUrl = Contexts.Default.Url;
            Type = type ?? GetType().Name;
        }

        public string Id { get; set; }

        public string ContextUrl { get; set; }

        public string Type { get; set; }

        public DateTime? Created { get; set; }

        public DateTime? Modified { get; set; }
    }
}