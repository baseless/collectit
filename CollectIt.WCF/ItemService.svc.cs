using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using CollectIt.Common.Entities;
using CollectIt.Common.Services;
using CollectIt.WCF.Contracts;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.Storage.Table;

namespace CollectIt.WCF
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "ItemService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select ItemService.svc or ItemService.svc.cs at the Solution Explorer and start debugging.
    public class ItemService : IItemService
    {
        public AzureTableService TableService => new AzureTableService(RoleEnvironment.GetConfigurationSettingValue("AzureStorageConnString"));

        public ICollection<ItemContract> All()
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

        public ICollection<ItemContract> Latest()
        {
            var query = new TableQuery<Item>
            {
                FilterString =
                    TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.GreaterThanOrEqual,
                        DateTime.UtcNow.AddDays(-14).ToString("O"))
            };
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
    }
}
