using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Security.Policy;
using System.Threading;
using System.Threading.Tasks;
using CollectIt.Common.Entities;
using CollectIt.Common.Services;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.Storage.Table;

namespace CollectIt.Worker
{
    public class WorkerRole : RoleEntryPoint
    {
        private readonly AzureTableService _tableService = new AzureTableService(RoleEnvironment.GetConfigurationSettingValue("AzureStorageConnString"));
        private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        private readonly ManualResetEvent _runCompleteEvent = new ManualResetEvent(false);
        private readonly RssFeedReader _feedReader = new RssFeedReader();

        public override void Run()
        {
            Trace.TraceInformation("CollectIt.Worker is running");

            try
            {
                this.RunAsync(this._cancellationTokenSource.Token).Wait();
   }
            finally
            {
                this._runCompleteEvent.Set();
            }
        }

        public override bool OnStart()
        {
            // Set the maximum number of concurrent connections
            ServicePointManager.DefaultConnectionLimit = 12;

            bool result = base.OnStart();

            // Insert dummy data
            InsertTestData();
            Trace.TraceInformation("CollectIt.Worker has been started");

            return result;
        }

        public override void OnStop()
        {
            Trace.TraceInformation("CollectIt.Worker is stopping");

            this._cancellationTokenSource.Cancel();
            this._runCompleteEvent.WaitOne();

            base.OnStop();

            Trace.TraceInformation("CollectIt.Worker has stopped");
        }

        private async Task RunAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                Trace.TraceInformation("Running..");

                StartReadingFeeds();

                await Task.Delay(10000, cancellationToken);
            }
        }

        private void StartReadingFeeds()
        {
            var subs = _tableService.All<Subscription>(Subscription.TableName);

            if (subs.Count > 0)
                Debug.WriteLine("FOUND SUBS..");

            var items = new List<Item>();
            foreach (Subscription sub in subs)
            {
                var chan = _tableService.Get<Channel>(sub.ChannelPartitionKey, sub.ChannelRowKey,
                    Channel.TableName);

                // Om inga filter så spara hela kanalen. 

                items = _feedReader.ReadFeedWithFilters(sub.FilterList, chan);

                // BEHÖVER SÄTTA CHANNEL PART/ROW KEY I VARJE ITEM. VAR/HUR GÖR MAN DET BÄST?
                

            }

            if (items.Count > 0)
                Debug.WriteLine("\nFOUND ITEMS!!!!\n");
            foreach (Item i in items)
            {
                Debug.WriteLine("Title: " + i.Title + "\nDesc: " + i.Description + "\nLink: " + i.Link);   
                _tableService.Insert(i, Item.TableName, AzureTableService.InsertOption.ReplaceIfExist);
            }

            
        }

        private void InsertTestData()
        {
            // Insert a channel
            Channel chan1 = new Channel(Channel.ChannelCategory.Technology, "Gawker Rss")
            {
                Description = "This is some shiiat!",
                Url = "http://feeds.gawker.com/lifehacker/full"
            };

            // Insert a sub
            Subscription sub1 = new Subscription("123", chan1.PartitionKey, chan1.RowKey)
            {
                Filters = "Calculator,Cadbury,struggling"
            };

            _tableService.Insert(chan1, Channel.TableName, AzureTableService.InsertOption.MergeIfExist);
            _tableService.Insert(sub1, Subscription.TableName, AzureTableService.InsertOption.MergeIfExist);
        }
    }
}
