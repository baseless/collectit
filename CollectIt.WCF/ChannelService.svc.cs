using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using CollectIt.Common.Entities;
using CollectIt.Common.Services;
using CollectIt.WCF.Contracts;
using Microsoft.WindowsAzure.ServiceRuntime;

namespace CollectIt.WCF
{
    public class ChannelService : IChannelService
    {

        public AzureTableService TableService => new AzureTableService(RoleEnvironment.GetConfigurationSettingValue("AzureStorageConnString"));

        public ChannelContract Get(string userId, string partitionKey, string rowKey)
        {
            var channel = TableService.Get<Channel>(partitionKey, rowKey, Channel.TableName);
            var subscription = TableService.Get<Subscription>(Subscription.ToPartitionKey(userId), Subscription.ToRowKey(partitionKey, rowKey), Subscription.TableName);
            return new ChannelContract
            {
                PartitionKey = channel.PartitionKey,
                Name = channel.Name,
                RowKey = channel.RowKey,
                CategoryName = channel.Category.ToString(),
                Url = channel.Url,
                IsSubscribing = subscription != null,
                Filters = subscription?.Filters,
                Description = channel.Description
            };
        }

        public ICollection<ChannelContract> All(string userId)
        {
            return (from channel in TableService.All<Channel>(Channel.TableName)
                let subscription = TableService.Get<Subscription>(Subscription.ToPartitionKey(userId), Subscription.ToRowKey(channel.PartitionKey, channel.RowKey), Subscription.TableName)
                let isSubscriber = subscription != null
                let filters = subscription?.Filters
                select new ChannelContract
                {
                    PartitionKey = channel.PartitionKey,
                    Name = channel.Name,
                    RowKey = channel.RowKey,
                    CategoryName = channel.Category.ToString(),
                    Url = channel.Url,
                    IsSubscribing = isSubscriber,
                    Filters = filters,
                    Description = channel.Description
                }).ToList();
        }
    }
}
