﻿using System;
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
        /// <param name="type"></param>
        /// <returns></returns>
        public string GetTableName(Type type) => Environment.TableName();

        /// <summary>
        /// Gets the name of the hash key for a table of the specified type
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public string GetTableHashKeyName(Type type) => DynamoDbDefaults.ResourceTypeAttribute;

        /// <summary>
        /// Gets the name of the sort key for a table of the specified type
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public string GetTableRangeKeyName(Type type) => DynamoDbDefaults.ResourceIdAttribute;
    }
}