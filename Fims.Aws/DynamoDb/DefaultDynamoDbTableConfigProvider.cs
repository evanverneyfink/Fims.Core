using Fims.Core.Model;
using Fims.Server;

namespace Fims.Aws.DynamoDb
{
    public class DefaultDynamoDbTableConfigProvider : IDynamoDbTableConfigProvider
    {
        /// <summary>
        /// Instantiates a <see cref="DefaultDynamoDbTableConfigProvider"/>
        /// </summary>
        /// <param name="environment"></param>
        public DefaultDynamoDbTableConfigProvider(IEnvironment environment)
        {
            Environment = environment;
        }

        /// <summary>
        /// Gets the environment
        /// </summary>
        private IEnvironment Environment { get; }

        /// <summary>
        /// Gets the table name for a type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public string GetTableName<T>()
        {
            return $"fims.{Environment.ServiceName}.{typeof(T).Name}";
        }

        /// <summary>
        /// Gets the name of the key for a table of type <see cref="T"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public string GetTableKeyName<T>()
        {
            return nameof(Resource.Id);
        }
    }
}