using Fims.Core.Model;

namespace Fims.Server.Data
{
    public interface IHttpResourceDataHandler<T> : IResourceDataHandler<T> where T : Resource
    {

    }
}