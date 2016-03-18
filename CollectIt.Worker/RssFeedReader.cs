using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;
using CollectIt.Common.Entities;

namespace CollectIt.Worker
{
    public class RssFeedReader
    {
        // Read the specified channel and return ALL items.
        public List<Item> ReadFeed(Channel channel)
        {
            var rssFeed = XDocument.Load(channel.Url);

            var items = from item in rssFeed.Descendants("item")
                        let elementPubDate = item.Element("pubDate")
                        where elementPubDate != null
                        let elementDesc = item.Element("description")
                        where elementDesc != null
                        let elementLink = item.Element("link")
                        where elementLink != null
                        let elementTitle = item.Element("title")
                        where elementTitle != null
                        select new Item(channel.PartitionKey, channel.RowKey)
                        {
                            Title = elementTitle.Value,
                            Description = elementDesc.Value,
                            Link = elementLink.Value,
                            PublishedDateRFC822 = elementPubDate.Value,
                            PublishedDate = Rfc822ToDateTimeParseableString(elementPubDate.Value)
                        };

            return items.ToList();
        }

        // Filter feed and return only NEW items
        public List<Item> ReadFeedWithFilters(List<string> keywords, Channel channel)
        {
            var items = ReadFeed(channel);

            // Filter out *items* and get only items with either Title or Desc containing any of the keywords.
            List<Item> filteredItems = new List<Item>();
            foreach (Item item in items)
            {
                filteredItems.AddRange(from keyword in keywords where item.Description.Contains(keyword) || item.Title.Contains(keyword) select item);
            }

            return filteredItems;
        }


        // Filter feed and return only NEW items
        public List<Item> ReadFeedFromDate(DateTime date, Channel channel)
        {
            var items = ReadFeed(channel);

            // Filter out *items* and get only items with DateTime higher than specified date
            List<Item> filteredItems = items.Where(item => item.GetPublishedDateTime() > date).ToList();

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
    }
}
