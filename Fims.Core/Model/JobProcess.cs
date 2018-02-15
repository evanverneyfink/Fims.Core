using System;

namespace Fims.Core.Model
{
    public class JobProcess : JobStatusObject
    {
        public JobProcess()
        {
        }

        public JobProcess(string jobId)
        {
            JobId = jobId;
        }

        public string JobId { get; set; }

        public string JobAssignmentId { get; set; }
    
        public DateTime? JobStart { get; set; }

        public TimeSpan? JobDuration { get; set; }

        public DateTime? JobEnd { get; set; }
    }
}