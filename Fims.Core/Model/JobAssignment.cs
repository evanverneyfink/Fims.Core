namespace Fims.Core.Model
{
    public class JobAssignment : Resource
    {
        public string JobProcess { get; set; }

        public string JobProcessStatus { get; set; }

        public string JobProcessStatusReason { get; set; }

        public JobParameterBag JobOutput { get; set; }
    }
}