using System;
using System.IO;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;

namespace RegisterFunction
{
    public static class CreateThumbnail
    {
        [FunctionName("CreateThumbnail")]
        public static void Run(
            [BlobTrigger("photos/{name}", Connection = "RegistratieApp")] Stream photoStream,
            [Blob("thumbnails/{name}", FileAccess.Write)] Stream thumbnailStream,
            string name, ILogger log)
        {
            log.LogInformation($"Creating thumbnail for {name}");
            Image photo = Image.Load(photoStream);
            photo.Mutate(x => x.Resize(50, 50));
            photo.Save(thumbnailStream, JpegFormat.Instance);
        }
    }
}
