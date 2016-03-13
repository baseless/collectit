using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using CollectIt.WCF.Contracts;

namespace CollectIt.WCF
{
    [ServiceContract]
    public interface IChannelService
    {
        [OperationContract]
        ICollection<RichChannel> AllChannels(string userId);

        [OperationContract]
        ICollection<Channel> SubscribedChannels(string userId);

        [OperationContract]
        void Subscribe(string userId, Channel channel, string[] filters);

        [OperationContract]
        void Unsubscribe(string userId, Channel channel);
    }
}
