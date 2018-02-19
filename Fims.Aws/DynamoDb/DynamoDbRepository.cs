using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using Fims.Core.Model;
using Fims.Server;
using Fims.Server.Data;

using DynamoDbTable = Amazon.DynamoDBv2.DocumentModel.Table;

namespace Fims.Aws.DynamoDb
{
    public class DynamoDbRepository : IRepository
    {
        /// <summary>
        /// Instantiates a <see cref="DynamoDbRepository"/>
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="tableConfigProvider"></param>
        public DynamoDbRepository(ILogger logger, IDynamoDbTableConfigProvider tableConfigProvider)
        {
            Logger = logger;
            TableConfigProvider = tableConfigProvider;
        }

        /// <summary>
        /// Gets the DynamoDB client
        /// </summary>
        private IAmazonDynamoDB DynamoDb { get; } = new AmazonDynamoDBClient();

        /// <summary>
        /// Gets the logger
        /// </summary>
        private ILogger Logger { get; }

        /// <summary>
        /// Gets the DynamoDB table config provider
        /// </summary>
        private IDynamoDbTableConfigProvider TableConfigProvider { get; }

        /// <summary>
        /// Gets the DynamoDB table
        /// </summary>
        private async Task<DynamoDbTable> Table<T>()
        {
            var tableName = TableConfigProvider.GetTableName<T>();
            var keyName = TableConfigProvider.GetTableKeyName<T>();

            Logger.Debug("Checking if table '{0}' exists in DynamoDB...", tableName);

            if (!DynamoDbTable.TryLoadTable(DynamoDb, tableName, out var table))
            {
                Logger.Info("Table '{0}' does not exist in DynamoDB. Creating it now...", tableName);

                var createResp =
                    await DynamoDb.CreateTableAsync(tableName,
                                                    new List<KeySchemaElement>
                                                    {
                                                        new KeySchemaElement
                                                        {
                                                            AttributeName = keyName,
                                                            KeyType = KeyType.HASH
                                                        }
                                                    },
                                                    new List<AttributeDefinition>
                                                    {
                                                        new AttributeDefinition
                                                        {
                                                            AttributeName = keyName,
                                                            AttributeType = ScalarAttributeType.S
                                                        }
                                                    },
                                                    new ProvisionedThroughput(1, 1));

                if (createResp.HttpStatusCode != HttpStatusCode.OK)
                    throw new Exception($"Failed to create table '{tableName}' in DynamoDB. Response code is {createResp.HttpStatusCode}.");

                Logger.Info("Successfully created DynamoDB table '{0}'.", tableName);

                table = DynamoDbTable.LoadTable(DynamoDb, tableName);
            }

            return table;
        }

        /// <summary>
        /// Gets a resource of type <see cref="T"/> by its ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<T> Get<T>(string id) where T : Resource
        {
            return (await (await Table<T>()).GetItemAsync(new Primitive(id))).ToObject<T>();
        }

        /// <summary>
        /// Queries resources of type <see cref="T"/> using the provided criteria, in the form of key/value pairs
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public async Task<IEnumerable<T>> Query<T>(IDictionary<string, string> parameters) where T : Resource
        {
            return (await (await Table<T>()).Scan(parameters.ToScanFilter()).GetRemainingAsync()).Select(d => d.ToObject<T>());
        }

        /// <summary>
        /// Creates a resource of type <see cref="T"/>
        /// </summary>
        /// <param name="resource"></param>
        /// <returns></returns>
        public async Task<T> Create<T>(T resource) where T : Resource
        {
            return (await (await Table<T>()).PutItemAsync(resource.ToDocument())).ToObject<T>();
        }

        /// <summary>
        /// Updates a resource of type <see cref="T"/>
        /// </summary>
        /// <param name="resource"></param>
        /// <returns></returns>
        public async Task<T> Update<T>(T resource) where T : Resource
        {
            return (await (await Table<T>()).PutItemAsync(resource.ToDocument())).ToObject<T>();
        }

        /// <summary>
        /// Deletes a resource of type <see cref="T"/> by its ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task Delete<T>(string id) where T : Resource
        {
            await (await Table<T>()).DeleteItemAsync(new Primitive(id));
        }
    }
}
