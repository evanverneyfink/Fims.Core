namespace Fims.Core.Model
{
    public class JobParameter : Resource
    {
        public JobParameter()
        {
        }

        public JobParameter(string jobProperty, string parameterType)
        {
            JobProperty = jobProperty;
            ParameterType = parameterType;
        }

        public string JobProperty { get; set; }

        public string ParameterType { get; set; }
    }
}