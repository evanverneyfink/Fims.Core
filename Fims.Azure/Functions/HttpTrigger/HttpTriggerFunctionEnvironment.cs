using Environment = Fims.Server.Environment;

namespace Fims.Azure.Functions.HttpTrigger
{
    public class HttpTriggerFunctionEnvironment : Environment
    {
        protected override string GetTextValue(string key) => System.Environment.GetEnvironmentVariable(key);

        protected override void SetTextValue(string key, string value) => System.Environment.SetEnvironmentVariable(key, value);
    }
}
