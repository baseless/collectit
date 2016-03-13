using System;

namespace CollectIt.WCF
{
    public class StatusService : IStatusService
    {
        private static readonly DateTime Started = DateTime.Now;

        public string GetStatus()
        {
            var span = DateTime.Now - Started;
            return "Started: " + Started.ToLongDateString() + ", " + Started.ToLongTimeString() + ". Runnning time: " + span.Days + " days, " + span.Hours + " hours, " + span.Minutes + " minutes, " + span.Seconds + " seconds";
        }
    }
}
