using System;
using System.Diagnostics;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
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

            // For information on handling configuration changes
            // see the MSDN topic at http://go.microsoft.com/fwlink/?LinkId=166357.

            bool result = base.OnStart();

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
            // TODO: Replace the following with your own logic.
            while (!cancellationToken.IsCancellationRequested)
            {
                Trace.TraceInformation("Running..");
                var baba = new TableEntity { PartitionKey = DateTime.Now.ToLongTimeString(), RowKey = DateTime.Now.ToLongTimeString() };
                await _tableService.Insert(baba, "testTable");
                await Task.Delay(1000, cancellationToken);
            }
        }
    }
}
