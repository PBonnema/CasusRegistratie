using System;
using RegisterFunction.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage;
using Newtonsoft.Json;

namespace RegisterFunction
{
    public static class Registreer
    {
        [FunctionName("Registreer")]
        [return: Table("account", Connection = "RegistratieApp")]
        public static Account Run(
            [QueueTrigger("registrations", Connection = "RegistratieApp")] string payload,
            ILogger log)
        {
            log.LogInformation($"Registering {payload}");
            string name = JsonConvert.DeserializeObject<string>(payload);
            return new Account
            {
                RowKey = name,
                PartitionKey = name[0].ToString().ToUpper(),
                Name = name,
                Photo = name + ".jpg",
                Thumbnail = name + ".jpg",
            };
        }
    }
}
