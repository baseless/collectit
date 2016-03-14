using System.Collections.Generic;
using System.ServiceModel;
using CollectIt.WCF.Contracts;

namespace CollectIt.WCF
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "ISubscriptionService" in both code and config file together.
    [ServiceContract]
    public interface ISubscriptionService
    {
        [OperationContract]
        SubscriptionContract Get(string userId, string channelPartitionKey, string channelRowKey);

        [OperationContract]
        void Subscribe(string userId, string channelPartitionKey, string channelRowKey, string filters);

        [OperationContract]
        void Unsubscribe(string userId, string channelPartitionKey, string channelRowKey);
    }
}
