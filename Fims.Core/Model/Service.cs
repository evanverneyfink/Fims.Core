using System.Collections.Generic;

namespace Fims.Core.Model
{
    public class Service : Resource
    {
        public Service()
        {
        }

        public Service(string label,
                       ICollection<ServiceResource> resources,
                       ICollection<string> jobTypes,
                       ICollection<JobProfile> jobProfiles,
                       ICollection<Locator> inputLocations,
                       ICollection<Locator> outputLocations)
        {
            Label = label;
            Resources = resources;
            JobTypes = jobTypes;
            JobProfiles = jobProfiles;
            InputLocations = inputLocations;
            OutputLocations = outputLocations;
        }

        public string Label { get; set; }

        public ICollection<string> JobTypes { get; set; }

        public ICollection<ServiceResource> Resources { get; set; }

        public ICollection<JobProfile> JobProfiles { get; set; }

        public ICollection<Locator> InputLocations { get; set; }

        public ICollection<Locator> OutputLocations { get; set; }
    }
}
