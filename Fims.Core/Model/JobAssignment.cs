namespace Fims.Core.Model
{
    public class JobAssignment : JobStatusObject
    {
        public JobAssignment()
        {
        }

        public JobAssignment(string jobProcessId)
        {
            JobProcessId = jobProcessId;
        }

        public string JobProcessId { get; set; }
    }
}