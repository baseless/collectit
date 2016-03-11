using System;
using Microsoft.WindowsAzure.Storage.Table;

namespace CollectIt.Common.Entities.Azure
{
    public abstract class AutoKeyEntityBase : TableEntity
    {
        private static long _rowKey = 0;
        private static readonly string PartitionKeyGuid = Guid.NewGuid().ToString();

        protected AutoKeyEntityBase()
        {
            this.RowKey = (++_rowKey).ToString();
            this.PartitionKey = PartitionKeyGuid;

        }
    }
}
