using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;
using CollectIt.Common.Entities;
using CollectIt.Common.Services;
using Microsoft.WindowsAzure.ServiceRuntime;

namespace CollectIt.Worker
{
    public class RssFeedReader
    {
        private static AzureTableService TableService => new AzureTableService(RoleEnvironment.GetConfigurationSettingValue("AzureStorageConnString"));

        // Read the specified channel
        public List<Item> ReadFeed(Channel channel, List<string> filters = null)
        {
            var rssFeed = XDocument.Load(channel.Url);
            
            if (rssFeed.Equals(null))
            {
                throw new ArgumentNullException();
            }

            if (filters != null)
            {
                Debug.WriteLine("FOUND FILTERS.. Checking last build date..");
                return IsFeedBuildDateNewer(channel, rssFeed) ? FilterFeed(filters, rssFeed, channel) : new List<Item>();
            }
            Debug.WriteLine("FOUND NO FILTERS.. Checking last build date..");
            return IsFeedBuildDateNewer(channel, rssFeed) ? GetItemsFromFeed(channel, rssFeed) : new List<Item>();
        }

        private bool IsFeedBuildDateNewer(Channel channel, XDocument feed)
        {
            // Check that lastBuildDate exist (it's optional in RSS)
            var xElement = feed.Root.Element("channel").Element("lastBuildDate");
            if (xElement != null)
            {
                var feedBuildDate = Rfc822ToDateTime(xElement.Value);
                // If feed is newest return true
                if (channel.LastBuildDate < feedBuildDate)
                {
                    channel.LastBuildDate = feedBuildDate;

                    // Update database
                    TableService.Insert(channel, Channel.TableName, AzureTableService.InsertOption.MergeIfExist);

                    Debug.WriteLine("The feed build date is newer than the stored date.. ");
                    return true;
                }
                Debug.WriteLine("The feed build date is NOT newer than the stored date.. The feed date is: " + feedBuildDate);

                return false;

            }
            // In case lastBuildDate doesn't exit just return true
            Debug.WriteLine("The lastBuildDate attribute does not exist in this feed: " + channel.Name);
            return true;                         
        }

        private List<Item> GetItemsFromFeed(Channel channel, XDocument rssFeed)
        {
            var items = ParseFeed(channel, rssFeed);

            // Getting all items which belongs to the specified channel.
            var allItemsInChannelPartition = TableService.ListByPartition<Item>(new string[] { items.ElementAtOrDefault<Item>(0).PartitionKey }, Item.TableName);
            // If no items found, we will return the complete list of items.
            if (allItemsInChannelPartition.Count <= 0)
                return items.ToList();
            Debug.WriteLine("Getting all items belonging to partition: " +
                            allItemsInChannelPartition.ElementAtOrDefault(0).PartitionKey +
                            " current number of items is: " + allItemsInChannelPartition.Count);
            // Now we need to find out the latest published date of the items from the Item table.
            var itemWithLatestPubDate = allItemsInChannelPartition.ElementAtOrDefault(0);
            foreach (Item i in allItemsInChannelPartition.Where(i => Rfc822ToDateTime(itemWithLatestPubDate.PublishedDateRFC822) < Rfc822ToDateTime(i.PublishedDateRFC822)))
            {
                itemWithLatestPubDate = i;
            }
            Debug.WriteLine("The newest item in table is: " + itemWithLatestPubDate.RowKey);
            // Now we sort out the items which are newer then the latest item in table
            var listWithNewItems = items.Where(i => Rfc822ToDateTime(itemWithLatestPubDate.PublishedDateRFC822) < Rfc822ToDateTime(i.PublishedDateRFC822)).ToList();

            Debug.WriteLine("Found " + listWithNewItems.Count + " items which were newer..");

            return listWithNewItems;
        }

        private IEnumerable<Item> ParseFeed(Channel channel, XDocument rssFeed)
        {
            Debug.WriteLine("Parsing feed and collecting items...");
            var items = from item in rssFeed.Descendants("item")
                        let elementPubDate = item.Element("pubDate")
                        where elementPubDate != null
                        let elementDesc = item.Element("description")
                        where elementDesc != null
                        let elementLink = item.Element("link")
                        where elementLink != null
                        let elementTitle = item.Element("title")

                        select new Item(channel.PartitionKey, channel.RowKey)
                        {
                            Title = elementTitle.Value,
                            Description = elementDesc.Value,
                            Link = elementLink.Value,
                            PublishedDateRFC822 = elementPubDate.Value,
                            PublishedDate = Rfc822ToDateTimeParseableString(elementPubDate.Value)
                        };
            return items;
        }

        // Filter feed and return only NEW items
        private List<Item> FilterFeed(List<string> keywords, XDocument feed, Channel channel)
        {
            Debug.WriteLine("Filtering items from feed..");
            // Filter out *items* and get only items with either Title or Desc containing any of the keywords.
            List<Item> filteredItems = new List<Item>();
            foreach (Item item in GetItemsFromFeed(channel, feed))
            {
                filteredItems.AddRange(from keyword in keywords where item.Description.Contains(keyword) || item.Title.Contains(keyword) select item);
            }

            return filteredItems;
        }

        
        // Convert RSS-time (RFC822) to a DateTime object
        public string Rfc822ToDateTimeParseableString(string date)
        {
            const string FORMAT = "ddd, d MMM yyyy HH:mm:ss zzz";
            const string FORMAT2 = "ddd, dd MMM yyyy HH:mm:ss zzz";
            const string FORMAT3 = "dd MMM yyyy HH:mm:ss zzz";
            const string FORMAT4 = "d MMM yyyy HH:mm:ss zzz";
            const string FORMAT5 = "ddd, d MMM yyyy HH:mm:ss";

            // Removes GMT from end of dateString. Parser cannot handle it.
            if (date.Contains("GMT"))
                date = date.Replace("GMT", " ");

            DateTime d;
            if (DateTime.TryParseExact(date, new string[] { FORMAT, FORMAT2, FORMAT3, FORMAT4, FORMAT5 }, CultureInfo.InvariantCulture, DateTimeStyles.AllowLeadingWhite | DateTimeStyles.AllowTrailingWhite, out d))
                return d.ToString();
            return null;
        }

        // Convert RSS-time (RFC822) to a DateTime object
        public DateTime Rfc822ToDateTime(string date)
        {
            const string FORMAT = "ddd, d MMM yyyy HH:mm:ss zzz";
            const string FORMAT2 = "ddd, dd MMM yyyy HH:mm:ss zzz";
            const string FORMAT3 = "dd MMM yyyy HH:mm:ss zzz";
            const string FORMAT4 = "d MMM yyyy HH:mm:ss zzz";
            const string FORMAT5 = "ddd, d MMM yyyy HH:mm:ss";

            // Removes GMT from end of dateString. Parser cannot handle it.
            if (date.Contains("GMT"))
                date = date.Replace("GMT", " ");

            DateTime d;
            if (DateTime.TryParseExact(date, new string[] { FORMAT, FORMAT2, FORMAT3, FORMAT4, FORMAT5 }, CultureInfo.InvariantCulture, DateTimeStyles.AllowLeadingWhite | DateTimeStyles.AllowTrailingWhite, out d))
                return d;
            return new DateTime();
        }
    }
}
