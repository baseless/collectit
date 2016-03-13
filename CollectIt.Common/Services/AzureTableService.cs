using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Table;

namespace CollectIt.Common.Services
{
    /// <summary>
    /// Azure Table serviceclass
    /// </summary>
    public class AzureTableService
    {
        private readonly CloudTableClient _client;

        public enum InsertOption { MergeIfExist, ReplaceIfExist }

        public AzureTableService(string connectionString)
        {
            var account = CloudStorageAccount.Parse(connectionString);
            _client = account.CreateCloudTableClient();
        }

        public TableResult Delete(TableEntity entity, string tableName)
        {
            var table = GetTable(tableName);
            return table.Execute(TableOperation.Delete(entity));
        }

        public TableResult Delete<T>(string partitionKey, string rowKey, string tableName) where T : TableEntity, new()
        {
            var table = GetTable(tableName);
            var entity = (T)table.Execute(TableOperation.Retrieve<T>(partitionKey, rowKey)).Result;
            return table.Execute(TableOperation.Delete(entity));
        }

        public bool Exists<T>(string partitionKey, string rowKey, string tableName) where T : TableEntity, new()
        {
            var table = GetTable(tableName);
            var query = new TableQuery<T>
            {
                FilterString = TableQuery.CombineFilters(
                    TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, partitionKey),
                    TableOperators.And,
                    TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, rowKey))
            };
            var result = table.ExecuteQuery(query);
            return result.Any();
        }

        public T Get<T>(string partitionKey, string rowKey, string tableName) where T : TableEntity
        {
            var table = GetTable(tableName);
            var result = table.Execute(TableOperation.Retrieve<T>(partitionKey, rowKey));
            return (T) result.Result;
        }

        public TableResult Insert(TableEntity entity, string tableName, InsertOption? option = null)
        {
            var table = GetTable(tableName);
            TableOperation operation;
            switch (option)
            {
                case InsertOption.MergeIfExist: operation = TableOperation.InsertOrMerge(entity); break;
                case InsertOption.ReplaceIfExist: operation = TableOperation.InsertOrReplace(entity); break;
                case null: default: operation = TableOperation.Insert(entity); break;
            }
            return table.Execute(operation);
        }

        /// <summary>
        /// List all items for each partition mentioned by name in the array.
        /// </summary>
        /// <typeparam name="T">Type of entity</typeparam>
        /// <param name="partitionKeys">Which partition keys to include</param>
        /// <param name="tableName">The name of the azure table</param>
        /// <returns></returns>
        public List<T> ListByPartition<T>(string[] partitionKeys, string tableName) where T : TableEntity, new()
        {
            var result = new List<T>();
            var table = GetTable(tableName);
            foreach (var partitionKey in partitionKeys)
            {
                var selectQuery = new TableQuery<T>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, partitionKey.ToLower()));
                var lst = table.ExecuteQuery(selectQuery).ToList();
                result.AddRange(lst);
            }
            return result;
        }

        public List<T> All<T>(string tableName) where T : TableEntity, new()
        {
            var table = GetTable(tableName);
            var selectQuery = new TableQuery<T>();
            return table.ExecuteQuery(selectQuery).ToList();
        }

        private CloudTable GetTable(string tableName)
        {
            var table = _client.GetTableReference(tableName);
            table.CreateIfNotExists();
            return table;
        }
    }
}
