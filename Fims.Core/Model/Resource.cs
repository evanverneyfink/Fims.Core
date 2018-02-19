using System;

namespace Fims.Core.Model
{
    public abstract class Resource : FimsObject
    {
        protected Resource(string type = null)
            : base(type)
        {
        }

        public DateTime? DateCreated
        {
            get => Get<DateTime>(nameof(DateCreated));
            set => Set(nameof(DateCreated), value);
        }

        public DateTime? DateModified
        {
            get => Get<DateTime>(nameof(DateModified));
            set => Set(nameof(DateModified), value);
        }
    }
}