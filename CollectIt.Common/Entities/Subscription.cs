using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using CollectIt.Common.Services;
using Microsoft.WindowsAzure.Storage.Table;

namespace CollectIt.Common.Entities
{
    public class Subscription : TableEntity
    {
        public static string TableName => "Subscriptions";

        public Subscription()  { }

        public Subscription(string userId, string channelPartitionKey, string channelRowKey)
        {
            ChannelPartitionKey = channelPartitionKey;
            ChannelRowKey = channelRowKey;
            PartitionKey = ToPartitionKey(userId);
            RowKey = ToRowKey(channelPartitionKey, channelRowKey);
            SubscribedDate = DateTime.Now;
        }

        [Required]
        public string ChannelRowKey { get; set; }

        [Required]
        public string ChannelPartitionKey { get; set; }

        public string Filters { get; set; }

        [Required]
        public DateTime SubscribedDate { get; set; }

        public static string ToRowKey(string channelPartitionKey, string channelRowKey)
        {
            return string.Concat(channelPartitionKey, channelRowKey);
        }

        public static string ToPartitionKey(string userId)
        {
            return SlugService.ToSlug(userId);
        }

        public List<string> FilterList => Filters.Split(',').ToList();
    }
}
