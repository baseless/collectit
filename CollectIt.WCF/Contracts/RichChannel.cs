using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace CollectIt.WCF.Contracts
{
    [DataContract]
    public class RichChannel : Channel
    {
        [Required, DataMember]
        public bool IsSubscribing { get; set; }
        [DataMember]
        public string[] Filters { get; set; }
    }
}