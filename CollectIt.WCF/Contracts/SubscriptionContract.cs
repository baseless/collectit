using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace CollectIt.WCF.Contracts
{
    [DataContract]
    public class SubscriptionContract
    {
        [Required, DataMember]
        public string RowKey { get; set; }
        [Required, DataMember]
        public string PartitionKey { get; set; }
        [DataMember]
        public string Filters { get; set; }
        [Required, DataMember]
        public DateTime SubscribedDate { get; set; }
    }
}