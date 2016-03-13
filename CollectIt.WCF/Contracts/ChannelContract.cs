using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace CollectIt.WCF.Contracts
{
    [DataContract]
    public class ChannelContract
    {
        [Required, DataMember]
        public bool IsSubscribing { get; set; }
        [DataMember]
        public ICollection<string> Filters { get; set; }
        [Required]
        public string CategoryName { get; set; }
        [Required, DataMember]
        public string RowKey { get; set; }
        [Required, DataMember]
        public string PartitionKey { get; set; }
        [Required]
        public string Url { get; set; }
        [Required, DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Description { get; set; }
    }
}