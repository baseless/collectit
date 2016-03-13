using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace CollectIt.WCF.Contracts
{
    [DataContract]
    public class Channel
    {
        [Required, DataMember]
        public Guid ChannelId { get; set; }
        [Required, DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Description { get; set; }
    }
}