using System;

namespace Fims.Core.Model
{
    public class JobParameter : Resource
    {
        public string JobProperty { get; set; }

        public Type ParameterType { get; set; }
    }
}