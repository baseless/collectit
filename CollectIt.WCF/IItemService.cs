using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace CollectIt.WCF
{
    [ServiceContract]
    public interface IItemService
    {
        [OperationContract]
        void DoWork();
    }
}
