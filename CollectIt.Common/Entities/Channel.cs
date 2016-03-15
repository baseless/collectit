using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.WindowsAzure.Storage.Table;
using System.Runtime.Serialization;
using CollectIt.Common.Services;

namespace CollectIt.Common.Entities
{
    [DataContract]
    public class Channel : TableEntity
    {
        public static string TableName => "Channels";

        public enum ChannelCategory{ Food, Motor, Sports, Technology, Misc }

        public Channel() { }

        public Channel(ChannelCategory category, string name)
        {
            Category = category;
            Name = name;
            PartitionKey = SlugService.ToSlug(Category.ToString());
            RowKey = SlugService.ToSlug(Name);
        }

        [Required]
        public ChannelCategory Category { get; set; }
        [Required]
        public string Url { get; set; }
        [Required, DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Description { get; set; }

    }
}