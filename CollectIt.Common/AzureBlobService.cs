using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CollectIt.Common
{
    public class AzureBlobService
    {
        private const string AccountName = "";
        private const string Key = "";
        public int NrContainers { get; }

        public Dictionary<int, CloudBlobContainer> Containers { get; }


        public AzureBlobService(string prefix, int nrContainers)
        {
            var credentials = new StorageCredentials(AccountName, Key);
            var account = new CloudStorageAccount(credentials, true);
            var client = account.CreateCloudBlobClient();
            Containers = new Dictionary<int, CloudBlobContainer>();
            CreateContainers(prefix, nrContainers, client);
            NrContainers = nrContainers;
        }

        private void CreateContainers(string prefix, int nrContainers, CloudBlobClient client)
        {
            for (var i = 1; i <= nrContainers; i++)
            {
                var container = client.GetContainerReference(prefix + i);
                container.CreateIfNotExists();
                Containers.Add(i, container);
            }
        }

        public void Upload(int container, string fileName, string sourcePath) //Server.MapPath(fileName)
        {
            try
            {
                var blob = Containers[container].GetBlockBlobReference(fileName);
                using (var file = File.OpenRead(sourcePath))
                {
                    blob.UploadFromStream(file);
                }
            }
            catch (Exception ex)
            {
                throw new IOException(ex.Message);
            }
        }

        public void Upload(int container, string fileName, Stream fileStream) //Server.MapPath(fileName)
        {
            try
            {
                var blob = Containers[container].GetBlockBlobReference(fileName);
                blob.UploadFromStream(fileStream);
            }
            catch (Exception ex)
            {
                throw new IOException(ex.Message);
            }
        }

        public void Download(int container, string fileName, string targetPath)
        {
            try
            {
                var blob = Containers[container].GetBlockBlobReference(fileName);
                blob.DownloadToFile(targetPath, FileMode.OpenOrCreate);
            }
            catch (Exception ex)
            {
                throw new IOException(ex.Message);
            }
        }

        public List<string> GetNames(int container)
        {
            var blobs = Containers[container].ListBlobs();
            return blobs.Select(m => m.Uri.LocalPath).ToList();
        }
    }
}
