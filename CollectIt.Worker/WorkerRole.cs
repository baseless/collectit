using System.Diagnostics;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using CollectIt.Common.Entities;
using CollectIt.Common.Services;
using Microsoft.WindowsAzure.ServiceRuntime;

namespace CollectIt.Worker
{
    public class WorkerRole : RoleEntryPoint
    {
        private readonly AzureTableService _tableService = new AzureTableService(RoleEnvironment.GetConfigurationSettingValue("AzureStorageConnString"));
        private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        private readonly ManualResetEvent _runCompleteEvent = new ManualResetEvent(false);
        private readonly RssFeedReader _feedReader = new RssFeedReader();
        private const int DelayTime = 10000;

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

                await Task.Delay(DelayTime, cancellationToken);
            }
        }

        private void StartReadingFeeds()
        {
            var subs = _tableService.All<Subscription>(Subscription.TableName);

            if (subs.Count > 0)
                Debug.WriteLine("FOUND SUBS: " + subs.Count);


            // todo: bygg latest fetch etc
            foreach (var sub in subs)
            {
                
                // Get the channel in the sub
                var chan = _tableService.Get<Channel>(sub.ChannelPartitionKey, sub.ChannelRowKey,
                    Channel.TableName);

                // Get all items from sub depending if filters exist or not
                var items = sub.Filters != null ? _feedReader.ReadFeedWithFilters(sub.FilterList, chan) : _feedReader.ReadFeed(chan);

                // Insert every item into tables
                foreach (var i in items)
                {
                    Debug.WriteLine("Title: " + i.Title + "\nDesc: " + i.Description + "\nLink: " + i.Link);
                    _tableService.Insert(i, Item.TableName, AzureTableService.InsertOption.MergeIfExist);
                }
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

            // Insert a channel
            Channel chan2 = new Channel(Channel.ChannelCategory.Misc, "Reuters LifeStyle")
            {
                Description = "Carpe diem!",
                Url = "http://feeds.reuters.com/reuters/lifestyle?format=xml"
            };

            // Insert a sub
            Subscription sub1 = new Subscription("123", chan1.PartitionKey, chan1.RowKey)
            {
                Filters = "Boost,Budget,Bluetooth"
            };

            Subscription sub2 = new Subscription("1234", chan2.PartitionKey, chan2.RowKey);

            _tableService.Insert(chan1, Channel.TableName, AzureTableService.InsertOption.MergeIfExist);
            _tableService.Insert(chan2, Channel.TableName, AzureTableService.InsertOption.MergeIfExist);
            _tableService.Insert(sub1, Subscription.TableName, AzureTableService.InsertOption.MergeIfExist);
            _tableService.Insert(sub2, Subscription.TableName, AzureTableService.InsertOption.MergeIfExist);
        }
    }
}
