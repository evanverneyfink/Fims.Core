namespace Fims.Core.Model
{
    public class AsyncEndpoint : Resource
    {
        public AsyncEndpoint()
        {
        }

        public AsyncEndpoint(string successEndpoint, string failureEndpoint)
        {
            SuccessEndpoint = successEndpoint;
            FailureEndpoint = failureEndpoint;
        }

        public string SuccessEndpoint { get; set; }

        public string FailureEndpoint { get; set; }
    }
}