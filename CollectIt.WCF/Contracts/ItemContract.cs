using System;
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

        public void MapFromItem(Item item)
        {
            Title = item.Title;
            Link = item.Link;
            Description = item.Description;
            PublishedDateRFC822 = item.PublishedDateRFC822;
            DateTime publishedDate;
            if (!DateTime.TryParse(item.PublishedDate, out publishedDate))
            {
                PublishedDate = publishedDate;
            }
        }
    }
}