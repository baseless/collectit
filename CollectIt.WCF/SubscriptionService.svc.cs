using System.Collections.Generic;
using System.Diagnostics;
using CollectIt.Common.Entities;
using CollectIt.Common.Services;
using CollectIt.WCF.Contracts;
using Microsoft.WindowsAzure.ServiceRuntime;

namespace CollectIt.WCF
{
    public class SubscriptionService : ISubscriptionService
    {
        public AzureTableService TableService => new AzureTableService(RoleEnvironment.GetConfigurationSettingValue("AzureStorageConnString"));

        public SubscriptionContract Get(string userId, string channelPartitionKey, string channelRowKey)
        {
            var subscription = TableService.Get<Subscription>(Subscription.ToPartitionKey(userId), Subscription.ToRowKey(channelPartitionKey, channelRowKey), Subscription.TableName);
            if(subscription == null)
                return null;
            return new SubscriptionContract
            {
                Filters = subscription.Filters,
                PartitionKey = subscription.PartitionKey,
                RowKey = subscription.RowKey,
                SubscribedDate = subscription.SubscribedDate
            };
        }

        public void Subscribe(string userId, string channelPartitionKey, string channelRowKey, string filters)
        {
            var subscription = new Subscription(userId, channelPartitionKey, channelRowKey) { Filters = filters };
            TableService.Insert(subscription, Subscription.TableName, AzureTableService.InsertOption.MergeIfExist);
        }

        public void Unsubscribe(string userId, string channelPartitionKey, string channelRowKey)
        {
            TableService.Delete<Subscription>(Subscription.ToPartitionKey(userId), Subscription.ToRowKey(channelPartitionKey, channelRowKey), Subscription.TableName);
        }
    }
}
