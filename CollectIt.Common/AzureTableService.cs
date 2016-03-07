using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Table;

namespace CollectIt.Common
{
    public class AzureTableService
    {
        private const string AccountName = "";
        private const string Key = ""; 
        private readonly CloudTableClient _client;

        public enum InsertOption { MergeIfExist, ReplaceIfExist }

        public AzureTableService()
        {
            var account = new CloudStorageAccount(new StorageCredentials(AccountName, Key), true);
            _client = account.CreateCloudTableClient();
        }

        public async Task<TableResult> Delete(AzureEntityBase entity, string tableName)
        {
            var table = await GetTable(tableName);
            return await table.ExecuteAsync(TableOperation.Delete(entity));
        }

        public async Task<T> Get<T>(string partitionKey, string rowKey, string tableName) where T : AzureEntityBase
        {
            var table = await GetTable(tableName);
            var result = await table.ExecuteAsync(TableOperation.Retrieve<T>(partitionKey, rowKey));
            return (T)result.Result;
        }

        public async Task<TableResult> Insert(AzureEntityBase entity, string tableName, InsertOption? option = null)
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

        public async Task<CloudTable> GetTable(string tableName)
        {
            var table = _client.GetTableReference(tableName);
            await table.CreateIfNotExistsAsync();
            return table;

        }
    }
}
