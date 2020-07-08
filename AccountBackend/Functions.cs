using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AccountBackend.Models;
using Microsoft.Azure.WebJobs;
using Newtonsoft.Json;
using Microsoft.WindowsAzure.Storage.Table;
using Microsoft.WindowsAzure.Storage;
using System.Drawing;
using System.Drawing.Imaging;
using Microsoft.WindowsAzure.Storage.Blob;

namespace AccountBackend
{
    public class Functions
    {
        static string constring = "DefaultEndpointsProtocol=https;AccountName=csb10032000cd952116;AccountKey=/0hhYfdE9wDNGRzwSYE4Hq1J4Laqpaezwu5GkEu+9TXArhV9DnboZLVKZbBR8JIEgDmAP6yD0x6h4THW5ur1yQ==;EndpointSuffix=core.windows.net";

        public static void HandleQueue([QueueTrigger("registrations")] string payload)
        {
            string name = JsonConvert.DeserializeObject<string>(payload);

            CloudStorageAccount csa = CloudStorageAccount.Parse(constring);
            var ctc = csa.CreateCloudTableClient();
            var table = ctc.GetTableReference("account");
            table.CreateIfNotExists();

            try
            {
                table.Execute(TableOperation.Insert(new Account
                {
                    RowKey = name,
                    PartitionKey = name[0].ToString().ToUpper(),
                    Name = name,
                    Photo = name + ".jpg",
                    Thumbnail = name + "_th.jpg",
                }));

                var cbc = csa.CreateCloudBlobClient();
                var photos = cbc.GetContainerReference("photos");
                photos.CreateIfNotExists(BlobContainerPublicAccessType.Container);
                var photo = photos.GetBlockBlobReference(name + ".jpg");
                var thumbnail = photos.GetBlockBlobReference(name + "_th.jpg");

                MemoryStream photoStream = new MemoryStream();
                photo.DownloadToStream(photoStream);
                Image photoImage = Image.FromStream(photoStream);
                Image thumnailImage = photoImage.GetThumbnailImage(50, 50, null, IntPtr.Zero);
                MemoryStream thumbnailStream = new MemoryStream();
                thumnailImage.Save(thumbnailStream, ImageFormat.Jpeg);
                thumbnailStream.Position = 0;
                thumbnail.UploadFromStream(thumbnailStream);

                Console.WriteLine("New registration: " + name);
            } catch
            {
                Console.WriteLine("Failed. Double registration: " + name);
            }
        }
    }
}
