using System;
using System.Collections.Generic;
using System.ServiceModel;
using CollectIt.Common.Entities;
using CollectIt.WCF.Contracts;

namespace CollectIt.WCF
{
    [ServiceContract]
    public interface IItemService
    {
        [OperationContract]
        ICollection<ItemContract> All(string userId);

        [OperationContract]
        ICollection<ItemContract> Latest(string userId);

        [OperationContract]
        ICollection<ItemContract> Query(string userId, string searchString);
    }
}
