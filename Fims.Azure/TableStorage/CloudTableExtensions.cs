﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;

namespace Fims.Azure.TableStorage
{
    public static class CloudTableExtensions
    {
        public static async Task<IEnumerable<dynamic>> ExecuteQueryAsync(this CloudTable table, IAzureStorageTableConfigProvider tableConfigProvider, TableQuery query)
        {
            var results = new List<dynamic>();

            TableContinuationToken continuationToken = null;
            do
            {
                // execute query and resolve results to ResourceTableEntity objects
                var result =
                    await table.ExecuteQuerySegmentedAsync(
                        query,
                        (partitionKey, rowKey, timestamp, properties, etag) =>
                            tableConfigProvider.GetResourceTableEntity(table.Name, partitionKey, rowKey, timestamp, properties, etag),
                        continuationToken);

                // add resource docs to results
                results.AddRange(result.Results.Select(e => e.ResourceDocument));

                continuationToken = result.ContinuationToken;
            }
            while (continuationToken != null);

            return results;
        }
    }
}