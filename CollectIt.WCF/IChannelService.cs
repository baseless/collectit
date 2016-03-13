using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using CollectIt.WCF.Contracts;

namespace CollectIt.WCF
{
    [ServiceContract]
    public interface IChannelService
    {
        [OperationContract]
        ICollection<ChannelContract> All(string userId);

        [OperationContract]
        ChannelContract Get(string userId, string partitionKey, string rowKey);
    }
}
