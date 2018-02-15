using Fims.Core.Model;
using Fims.Server.Business;

namespace Fims.Server.Data
{
    public interface IResourceDataHandler<T> : IResourceHandler<T> where T : Resource
    {
    }
}