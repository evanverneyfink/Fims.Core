namespace Fims.Aws.DynamoDb
{
    public interface IDynamoDbTableConfigProvider
    {
        /// <summary>
        /// Gets flag indicating if tables should be created if they don't exist
        /// </summary>
        bool CreateIfNotExists { get; }

        /// <summary>
        /// Gets the table name for a type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        string GetTableName<T>();

        /// <summary>
        /// Gets the name of the hash key for a table of type <see cref="T"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        string GetTableHashKeyName<T>();

        /// <summary>
        /// Gets the name of the sort key for a table of type <see cref="T"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        string GetTableRangeKeyName<T>();
    }
}