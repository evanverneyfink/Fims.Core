using System.Collections.Generic;

namespace Fims.Core.Model
{
    public class JobProfile : Resource
    {
        public JobProfile()
        {
        }

        public JobProfile(string label, ICollection<JobParameter> inputParameters, ICollection<JobParameter> outputParameters)
        {
            Label = label;
            InputParameters = inputParameters;
            OutputParameters = outputParameters;
        }

        public string Label { get; set; }

        public ICollection<JobParameter> InputParameters { get; set; }

        public ICollection<JobParameter> OutputParameters { get; set; }
    }
}