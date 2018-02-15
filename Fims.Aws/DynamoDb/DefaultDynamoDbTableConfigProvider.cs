using Fims.Core.Model;

namespace Fims.Aws.DynamoDb
{
    public class DefaultDynamoDbTableConfigProvider : IDynamoDbTableConfigProvider
    {
        /// <summary>
        /// Gets the table name for a type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public string GetTableName<T>()
        {
            return "fims-" + typeof(T).Name;
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