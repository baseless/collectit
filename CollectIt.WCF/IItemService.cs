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
        ICollection<ItemContract> All();

        [OperationContract]
        ICollection<ItemContract> Latest();
    }
}
