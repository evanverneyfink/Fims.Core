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
            var hashKeyName = TableConfigProvider.GetTableHashKeyName<T>();
            var rangeKeyName = TableConfigProvider.GetTableRangeKeyName<T>();

            Logger.Debug("Checking if table '{0}' exists in DynamoDB...", tableName);

            try
            {
                if (!DynamoDbTable.TryLoadTable(DynamoDb, tableName, out var table))
                {
                    if (!TableConfigProvider.CreateIfNotExists)
                        throw new Exception($"Table {tableName} does not exist in DynamoDB.");

                    Logger.Info("Table '{0}' does not exist in DynamoDB. Creating it now...", tableName);

                    var createResp =
                        await DynamoDb.CreateTableAsync(tableName,
                                                        new List<KeySchemaElement>
                                                        {
                                                            new KeySchemaElement
                                                            {
                                                                AttributeName = hashKeyName,
                                                                KeyType = KeyType.HASH
                                                            },
                                                            new KeySchemaElement
                                                            {
                                                                AttributeName = rangeKeyName,
                                                                KeyType = KeyType.RANGE
                                                            }
                                                        },
                                                        new List<AttributeDefinition>
                                                        {
                                                            new AttributeDefinition
                                                            {
                                                                AttributeName = hashKeyName,
                                                                AttributeType = ScalarAttributeType.S
                                                            },
                                                            new AttributeDefinition
                                                            {
                                                                AttributeName = rangeKeyName,
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
            catch (Exception exception)
            {
                Logger.Error($"An error occurred loading the DynamoDB table for type {typeof(T).Name}.", exception);
                throw;
            }
        }

        /// <summary>
        /// Gets a resource of type <see cref="T"/> by its ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<T> Get<T>(string id) where T : Resource
        {
            var table = await Table<T>();
            var hashKey = new Primitive($"{typeof(T).Name}");
            var rangeKey = new Primitive(id);

            Logger.Debug("Getting item with hash key {0} and range key {1} from table {2}...", hashKey, rangeKey, table.TableName);

            var result = await table.GetItemAsync(hashKey, rangeKey);

            return result?.ToObject<T>();
        }

        /// <summary>
        /// Queries resources of type <see cref="T"/> using the provided criteria, in the form of key/value pairs
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public async Task<IEnumerable<T>> Query<T>(IDictionary<string, string> parameters) where T : Resource
        {
            return (await (await Table<T>()).Query(new Primitive(typeof(T).Name), parameters.ToQueryFilter()).GetRemainingAsync())
                .Select(d => d.ToObject<T>());
        }

        /// <summary>
        /// Creates a resource of type <see cref="T"/>
        /// </summary>
        /// <param name="resource"></param>
        /// <returns></returns>
        public Task<T> Create<T>(T resource) where T : Resource
        {
            return CreateOrUpdate(resource);
        }

        /// <summary>
        /// Updates a resource of type <see cref="T"/>
        /// </summary>
        /// <param name="resource"></param>
        /// <returns></returns>
        public Task<T> Update<T>(T resource) where T : Resource
        {
            return CreateOrUpdate(resource);
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

        /// <summary>
        /// Creates or updates record in DynamoDB
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="resource"></param>
        /// <returns></returns>
        private async Task<T> CreateOrUpdate<T>(T resource) where T : Resource
        {
            // we don't need to store the context
            if (resource.Context != null)
                resource.Remove("@context");

            await (await Table<T>()).PutItemAsync(resource.ToDocument());

            return await Get<T>(resource.Id);
        }
    }
}
