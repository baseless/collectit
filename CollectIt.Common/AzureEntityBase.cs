using System;
using Microsoft.WindowsAzure.Storage.Table;

namespace CollectIt.Common
{
    public abstract class AzureEntityBase : TableEntity
    {
        private static long _rowKey = 0;
        private static readonly string PartitionKeyGuid = Guid.NewGuid().ToString();

        protected AzureEntityBase()
        {
            this.RowKey = (++_rowKey).ToString();
            this.PartitionKey = PartitionKeyGuid;

        }
    }
}
