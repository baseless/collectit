using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using CollectIt.Common.Entities;
using CollectIt.Common.Services;
using CollectIt.WCF.Contracts;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.Storage.Table;

namespace CollectIt.WCF
{
    public class ItemService : IItemService
    {
        private const int MaxRows = 666;
        private AzureTableService _tableService;

        public AzureTableService TableService => _tableService ?? (_tableService = new AzureTableService(RoleEnvironment.GetConfigurationSettingValue("AzureStorageConnString")));

        public ICollection<ItemContract> All(string userId)
        {
            return (from item in TableService.All<Item>(Item.TableName)
                select new ItemContract
                {
                    Title = item.Title,
                    Link = item.Link,
                    Description = item.Description,
                    PublishedDateRFC822 = item.PublishedDateRFC822,
                    PublishedDate = item.GetPublishedDateTime()
                }).ToList();
        }

        public ICollection<ItemContract> Latest(string userId)
        {
            var itemsResult = new List<ItemContract>();
            var subscriptions = TableService.ByPartitionKey<Subscription>(Subscription.ToPartitionKey(userId), Subscription.TableName, 999);

            foreach (var subscription in subscriptions)
            {
                var filters = subscription.FilterList;

                var channelQuery = new TableQuery<Item>
                {
                    FilterString = TableQuery.CombineFilters(
                    TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, SlugService.ToSlug(subscription.ChannelPartitionKey + subscription.ChannelRowKey)),
                    TableOperators.And,
                    TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.GreaterThanOrEqual, DateTime.UtcNow.AddDays(-14).ToString("u")))
                };

                itemsResult.AddRange((from item in TableService.Query<Item>(Item.TableName, channelQuery)
                              select new ItemContract
                              {
                                  Title = item.Title,
                                  Link = item.Link,
                                  Description = item.Description,
                                  PublishedDateRFC822 = item.PublishedDateRFC822,
                                  PublishedDate = item.GetPublishedDateTime()
                              }).Where(
                                item => filters == null || filters.Count == 0  || filters.Any(filter => item.Description.Contains(filter) || item.Title.Contains(filter)))
                                .Take(MaxRows)
                                .ToList());
            }

            return itemsResult;
        }

        public ICollection<ItemContract> Query(string userId, string searchString)
        {
            IList<string> words;
            TryGetSearchWords(searchString, out words); //blank search possible atm..

            return (from item in TableService.All<Item>(Item.TableName)
                    select new ItemContract
                    {
                        Title = item.Title,
                        Link = item.Link,
                        Description = item.Description,
                        PublishedDateRFC822 = item.PublishedDateRFC822,
                        PublishedDate = item.GetPublishedDateTime()
                    }).Where(item => words.Count().Equals(0) || words.Any(word => item.Description.Contains(word) || item.Title.Contains(word))).Take(MaxRows).ToList();
        }
        
        private static bool TryGetSearchWords(string searchString, out IList<string> words)
        {
            try
            {
                if (!string.IsNullOrEmpty(searchString))
                {
                    words = searchString.Split(',').ToList();
                    return true;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            words = new List<string>();
            return false;
        }
    }
}
