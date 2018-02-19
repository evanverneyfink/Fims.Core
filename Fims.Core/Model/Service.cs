using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace Fims.Core.Model
{
    public class Service : Resource
    {
        public Service()
        {
        }

        public Service(string label,
                       JToken hasResource,
                       JToken acceptsJobType,
                       JToken acceptsJobProfile,
                       JToken inputLocation,
                       JToken outputLocation)
        {
            Label = label;
            HasResource = hasResource;
            AcceptsJobType = acceptsJobType;
            AcceptsJobProfile = acceptsJobProfile;
            InputLocation = inputLocation;
            OutputLocation = outputLocation;
        }

        public string Label
        {
            get => GetString(nameof(Label));
            set => Set(nameof(Label), value);
        }

        public JToken HasResource
        {
            get => Get(nameof(HasResource));
            set => Set(nameof(HasResource), value);
        }

        public ICollection<ServiceResource> Resources => HasResource.ToResourceCollection<ServiceResource>();

        public JToken AcceptsJobType
        {
            get => Get(nameof(AcceptsJobType));
            set => Set(nameof(AcceptsJobType), value);
        }

        public ICollection<IdResource> JobTypes => AcceptsJobType.ToResourceCollection<IdResource>();

        public JToken AcceptsJobProfile
        {
            get => Get(nameof(AcceptsJobProfile));
            set => Set(nameof(AcceptsJobProfile), value);
        }

        public ICollection<JobProfile> JobProfiles => AcceptsJobProfile.ToResourceCollection<JobProfile>();

        public JToken InputLocation
        {
            get => Get(nameof(InputLocation));
            set => Set(nameof(InputLocation), value);
        }

        public ICollection<Locator> InputLocations => InputLocation.ToResourceCollection<Locator>();

        public JToken OutputLocation
        {
            get => Get(nameof(OutputLocation));
            set => Set(nameof(OutputLocation), value);
        }

        public ICollection<Locator> OutputLocations => OutputLocation.ToResourceCollection<Locator>();
    }
}
