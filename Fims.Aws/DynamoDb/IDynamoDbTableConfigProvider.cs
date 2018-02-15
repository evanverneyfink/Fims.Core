namespace Fims.Aws.DynamoDb
{
    public interface IDynamoDbTableConfigProvider
    {
        /// <summary>
        /// Gets the table name for a type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        string GetTableName<T>();

        /// <summary>
        /// Gets the name of the key for a table of type <see cref="T"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        string GetTableKeyName<T>();
    }
}