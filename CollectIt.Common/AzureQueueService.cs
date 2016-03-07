using System.Collections.Generic;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using Newtonsoft.Json;

namespace CollectIt.Common
{
    public class AzureQueueService
    {
        private readonly Dictionary<int, CloudQueue> _queues;

        public AzureQueueService(string prefix, int nrQueues, string connectionString)
        {
            _queues = new Dictionary<int, CloudQueue>();
            var storageAccount = CloudStorageAccount.Parse(connectionString);
            var client = storageAccount.CreateCloudQueueClient();
            CreateQueues(client, prefix, nrQueues);
        }

        private void CreateQueues(CloudQueueClient client, string prefix, int nrQueues)
        {
            for (var i = 1; i <= nrQueues; i++)
            {
                var queue = client.GetQueueReference(prefix + i);
                queue.CreateIfNotExists();
                _queues.Add(i, queue);
            }
        }

        public void Send(int queue, string message)
        {
            _queues[queue].AddMessage(new CloudQueueMessage(message));
        }

        public string Receive(int queue, bool delete)
        {
            var message = _queues[queue].GetMessage();
            if (message == null) return null;
            var strMessage = message.AsString;
            if (delete)
            {
                _queues[queue].DeleteMessage(message);
            }
            return strMessage;
        }

        public int Count(int queue)
        {
            var count = _queues[queue].ApproximateMessageCount;
            return count ?? 0;
        }

        public void Send(int queue, Dictionary<string, string> message)
        {
            _queues[queue].AddMessage(new CloudQueueMessage(JsonConvert.SerializeObject(message)));
        }

        public Dictionary<string, string> RecieveMultiple(int queue, bool delete)
        {
            var message = _queues[queue].GetMessage();
            if (message == null) return null;
            if (delete)
            {
                _queues[queue].DeleteMessage(message);
            }
            return JsonConvert.DeserializeObject<Dictionary<string, string>>(message.AsString);
        }

        /*
        public void SendObject(int queue, MessageDTO message)
        {
            _queues[queue].AddMessage(new CloudQueueMessage(JsonConvert.SerializeObject(message)));
        }

        public MessageDTO ReceiveObject(int queue, bool delete)
        {
            var message = _queues[queue].GetMessage();
            if (message == null) return null;
            if (delete)
            {
                _queues[queue].DeleteMessage(message);
            }
            return JsonConvert.DeserializeObject<MessageDTO>(message.AsString);
        }
        */
    }
}