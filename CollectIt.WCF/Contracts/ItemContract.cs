using System;
using System.Diagnostics;
using System.Runtime.Serialization;
using CollectIt.Common.Entities;

namespace CollectIt.WCF.Contracts
{
    [DataContract]
    public class ItemContract
    {
        [DataMember]
        public string Title { get; set; }

        [DataMember]
        public string Link { get; set; }

        [DataMember]
        public string Description { get; set; }

        // ReSharper disable once InconsistentNaming
        [DataMember]
        public string PublishedDateRFC822 { get; set; }

        [DataMember]
        public DateTime? PublishedDate { get; set; }
    }
}