namespace Fims.Core.Model
{
    public abstract class JobStatusObject : Resource
    {
        public string JobStatus { get; set; } = "New";

        public string JobStatusReason { get; set; }
    }
}