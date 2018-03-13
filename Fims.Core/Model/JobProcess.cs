using System;

namespace Fims.Core.Model
{
    public class JobProcess : Resource
    {
        public Job Job { get; set; }

        public string JobAssignment { get; set; }

        public string JobProcessStatus { get; set; }

        public string JobProcessStatusReason { get; set; }

        public DateTime? JobStart { get; set; }

        public TimeSpan? JobDuration { get; set; }

        public DateTime? JobEnd { get; set; }
    }
}