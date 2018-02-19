using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace Fims.Core.Model
{
    public class JobProfile : Resource
    {
        public JobProfile()
        {
        }

        public JobProfile(string label,
                          JToken inputParameters,
                          JToken outputParameters,
                          JToken hasOptionalInputParameter = null)
        {
            Label = label;
            HasInputParameter = inputParameters;
            HasOutputParameter = outputParameters;
            HasOptionalInputParameter = hasOptionalInputParameter;
        }

        public string Label
        {
            get => GetString(nameof(Label));
            set => Set(nameof(Label), value);
        }

        public JToken HasInputParameter
        {
            get => Get(nameof(HasInputParameter));
            set => Set(nameof(HasInputParameter), value);
        }

        public ICollection<JobParameter> InputParameters => HasInputParameter.ToResourceCollection<JobParameter>();

        public JToken HasOutputParameter
        {
            get => Get(nameof(HasOutputParameter));
            set => Set(nameof(HasOutputParameter), value);
        }

        public ICollection<JobParameter> OutputParameters => HasOutputParameter.ToResourceCollection<JobParameter>();

        public JToken HasOptionalInputParameter
        {
            get => Get(nameof(HasOptionalInputParameter));
            set => Set(nameof(HasOptionalInputParameter), value);
        }

        public ICollection<JobParameter> OpttionalInputParameters => HasOptionalInputParameter.ToResourceCollection<JobParameter>();
    }
}