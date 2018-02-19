namespace Fims.Core.Model
{
    public class AsyncEndpoint : Resource
    {
        public AsyncEndpoint()
        {
        }

        public AsyncEndpoint(string asyncSuccess, string asyncFailure)
        {
            AsyncSuccess = asyncSuccess;
            AsyncFailure = asyncFailure;
        }

        public string AsyncSuccess
        {
            get => GetString(nameof(AsyncSuccess));
            set => Set(nameof(AsyncSuccess), value);
        }

        public string AsyncFailure
        {
            get => GetString(nameof(AsyncFailure));
            set => Set(nameof(AsyncFailure), value);
        }
    }
}