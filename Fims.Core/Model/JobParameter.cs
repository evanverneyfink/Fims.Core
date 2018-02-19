using Newtonsoft.Json.Linq;

namespace Fims.Core.Model
{
    public class JobParameter : Resource
    {
        public JobParameter()
        {
        }

        public JobParameter(JToken jobProperty, JToken parameterType = null)
        {
            JobProperty = jobProperty;
            if (parameterType != null)
                ParameterType = parameterType;
        }

        public JToken JobProperty
        {
            get => Get(nameof(JobProperty));
            set => Set(nameof(JobProperty), value);
        }

        public JToken ParameterType
        {
            get => Get(nameof(ParameterType));
            set => Set(nameof(ParameterType), value);
        }

        public string JobPropertyId => JobProperty.ToId();

        public string ParameterTypeId => ParameterType.ToId();
    }
}