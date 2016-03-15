﻿using System;
using System.Diagnostics;
using System.Runtime.Serialization;
using CollectIt.Common.Services;
using Microsoft.WindowsAzure.Storage.Table;

namespace CollectIt.Common.Entities
{
    [DataContract]
    public class Item : TableEntity
    {
        public static string TableName => "Items";

        public Item() { }

        public Item(string channelPartitionKey, string channelRowKey)
        {
            PartitionKey = SlugService.ToSlug(channelPartitionKey + channelRowKey);
            RowKey = Stopwatch.GetTimestamp().ToString();
            Debug.WriteLine("Is Highresol: " + Stopwatch.IsHighResolution);
        }

        public string Title { get; set; }

        public string Link { get; set; }

        public string Description { get; set; }

        // ReSharper disable once InconsistentNaming
        public string PublishedDateRFC822 { get; set; }

        public string PublishedDate { get; set; }

        public DateTime? GetPublishedDateTime()
        {
            DateTime publishedDate;
            if (!DateTime.TryParse(PublishedDate, out publishedDate))
            {
                return publishedDate;
            }
            return null;
        }

    
    }
}