namespace Fims.Server
{
    public interface IEnvironment
    {
        T Get<T>(string key);
    }
}