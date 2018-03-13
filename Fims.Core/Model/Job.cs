namespace Fims.Core.Model
{
    public class Job : Resource
    {
        public string JobStatus { get; set; }

        public string JobStatusReason { get; set; }
        
        public JobProfile JobProfile { get; set; }

        public AsyncEndpoint AsyncEndpoint { get; set; }

        public string JobProcess { get; set; }

        public JobParameterBag JobInput { get; set; }

        public JobParameterBag JobOutput { get; set; }
    }
}