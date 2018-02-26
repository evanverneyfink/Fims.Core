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
        /// Gets flag indicating if tables should be created if they don't exist
        /// </summary>
        public bool CreateIfNotExists => false;

        /// <summary>
        /// Gets the table name for a type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public string GetTableName<T>() => Environment.TableName();

        /// <summary>
        /// Gets the name of the key for a table of type <see cref="T"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public string GetTableHashKeyName<T>() => DynamoDbDefaults.ResourceTypeAttribute;

        /// <summary>
        /// Gets the name of the key for a table of type <see cref="T"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public string GetTableRangeKeyName<T>() => DynamoDbDefaults.ResourceIdAttribute;
    }
}