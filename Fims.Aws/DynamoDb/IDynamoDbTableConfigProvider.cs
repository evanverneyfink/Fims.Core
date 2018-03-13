using System;

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
        /// <param name="type"></param>
        /// <returns></returns>
        string GetTableName(Type type);

        /// <summary>
        /// Gets the name of the hash key for a table of the specified type
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        string GetTableHashKeyName(Type type);

        /// <summary>
        /// Gets the name of the sort key for a table of the specified type
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        string GetTableRangeKeyName(Type type);
    }
}