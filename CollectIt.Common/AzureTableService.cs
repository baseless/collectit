using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Table;

namespace CollectIt.Common
{
    /// <summary>
    /// Azure Table serviceclass
    /// </summary>
    public class AzureTableService
    {
        private readonly CloudTableClient _client;

        public enum InsertOption { MergeIfExist, ReplaceIfExist }

        public AzureTableService(string accountName, string key)
        {
            var account = new CloudStorageAccount(new StorageCredentials(accountName, key), true);
            _client = account.CreateCloudTableClient();
        }

        public async Task<TableResult> Delete(TableEntity entity, string tableName)
        {
            var table = await GetTable(tableName);
            return await table.ExecuteAsync(TableOperation.Delete(entity));
        }

        public async Task<T> Get<T>(string partitionKey, string rowKey, string tableName) where T : TableEntity
        {
            var table = await GetTable(tableName);
            var result = await table.ExecuteAsync(TableOperation.Retrieve<T>(partitionKey, rowKey));
            return (T)result.Result;
        }

        public async Task<TableResult> Insert(TableEntity entity, string tableName, InsertOption? option = null)
        {
            var table = await GetTable(tableName);
            TableOperation operation;
            switch (option)
            {
                case InsertOption.MergeIfExist: operation = TableOperation.InsertOrMerge(entity); break;
                case InsertOption.ReplaceIfExist: operation = TableOperation.InsertOrReplace(entity); break;
                case null: default: operation = TableOperation.Insert(entity); break;
            }
            return await table.ExecuteAsync(operation);
        }

        /// <summary>
        /// List all items for each partition mentioned by name in the array.
        /// </summary>
        /// <typeparam name="T">Type of entity</typeparam>
        /// <param name="partitionKeys">Which partition keys to include</param>
        /// <param name="tableName">The name of the azure table</param>
        /// <returns></returns>
        public async Task<List<T>> ListAll<T>(string[] partitionKeys, string tableName) where T : TableEntity, new()
        {
            var result = new List<T>();
            var table = await GetTable(tableName);
            foreach (var partitionKey in partitionKeys)
            {
                var selectQuery = new TableQuery<T>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, partitionKey.ToLower()));
                var lst = table.ExecuteQuery(selectQuery).ToList();
                result.AddRange(lst);
            }
            return result;
        }

        private async Task<CloudTable> GetTable(string tableName)
        {
            var table = _client.GetTableReference(tableName);
            await table.CreateIfNotExistsAsync();
            return table;

        }
    }
}
