using Fims.Core.Model;

namespace Fims.Server.Data
{
    public interface IRepositoryResourceDataHandler<T> : IResourceDataHandler<T> where T : Resource
    {

    }
}