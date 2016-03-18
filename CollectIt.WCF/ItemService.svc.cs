using System;
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
            var words = GetSearchWords(userId).ToList();
            var query = new TableQuery<Item>
            {
                FilterString =
                    TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.GreaterThanOrEqual,
                        DateTime.UtcNow.AddDays(-14).ToString("u"))
            };

            return (from item in TableService.Query<Item>(Item.TableName, query)
                select new ItemContract
                {
                    Title = item.Title,
                    Link = item.Link,
                    Description = item.Description,
                    PublishedDateRFC822 = item.PublishedDateRFC822,
                    PublishedDate = item.GetPublishedDateTime()
                }).Where(item => words.Count().Equals(0) || words.Any(word => item.Description.Contains(word) || item.Title.Contains(word))).Take(MaxRows).ToList();
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

        private IEnumerable<string> GetSearchWords(string userId)
        {
            var subscriptions = TableService.ByPartitionKey<Subscription>(Subscription.ToPartitionKey(userId), Subscription.TableName, 999);
            return (from subscription in subscriptions where !string.IsNullOrEmpty(subscription.Filters) from filter in subscription.Filters.Split(',') select filter).ToList();
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
