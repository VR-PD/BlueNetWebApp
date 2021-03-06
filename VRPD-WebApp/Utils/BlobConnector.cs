﻿using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Web.Configuration;

namespace BlueNetWebApp
{
    public class BlobConnector
    {
        public const string MobileApp = "vrpdscanner";

        private static readonly Uri blobUri = new Uri(@"https://vrpdgamestore.blob.core.windows.net/");

        private CloudBlobClient blobClient;

        public BlobConnector()
        {
            byte[] key = Convert.FromBase64String(WebConfigurationManager.AppSettings["storekey"]);
            blobClient = new CloudBlobClient(blobUri, new StorageCredentials("vrpdgamestore", key));
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Security", "CA5377:Use Container Level Access Policy", Justification = "<Pending>")]
        public Uri GetGameSAS(string gameName)
        {
            CloudBlobContainer container = blobClient.GetContainerReference("vrpdgames");
            CloudBlockBlob blob = container.GetBlockBlobReference(gameName);
            SharedAccessBlobPolicy policy = new SharedAccessBlobPolicy()
            {
                Permissions = SharedAccessBlobPermissions.Read,
                SharedAccessExpiryTime = DateTimeOffset.UtcNow.AddMinutes(10)
            };

            return new Uri(blob.Uri, blob.GetSharedAccessSignature(policy));
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Security", "CA5377:Use Container Level Access Policy", Justification = "<Pending>")]
        public Uri GetScannerSAS()
        {
            CloudBlobContainer container = blobClient.GetContainerReference("publicread");
            CloudBlockBlob blob = container.GetBlockBlobReference(MobileApp);
            SharedAccessBlobPolicy policy = new SharedAccessBlobPolicy()
            {
                Permissions = SharedAccessBlobPermissions.Read,
                SharedAccessExpiryTime = DateTimeOffset.UtcNow.AddMinutes(10)
            };

            return new Uri(blob.Uri, blob.GetSharedAccessSignature(policy));
        }
    }
}
