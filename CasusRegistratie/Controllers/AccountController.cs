using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CasusRegistratie.Models;
using CasusRegistratie.Data;
using Microsoft.WindowsAzure.Storage.Table;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using Newtonsoft.Json;

namespace CasusRegistratie.Controllers
{
    public class AccountController : Controller
    {
        private string constring = "DefaultEndpointsProtocol=https;AccountName=csb10032000cd952116;AccountKey=/0hhYfdE9wDNGRzwSYE4Hq1J4Laqpaezwu5GkEu+9TXArhV9DnboZLVKZbBR8JIEgDmAP6yD0x6h4THW5ur1yQ==;EndpointSuffix=core.windows.net";

        // GET: Account
        public ActionResult Register()
        {
            return View();
        }

        // POST: Account/Create
        [HttpPost]
        public ActionResult Register(Registration registration)
        {
            try
            {
                CloudStorageAccount csa = CloudStorageAccount.Parse(constring);

                var cbc = csa.CreateCloudBlobClient();
                var photos = cbc.GetContainerReference("photos");
                photos.CreateIfNotExists();
                var photo = photos.GetBlockBlobReference(registration.Name + ".jpg");
                photo.UploadFromStream(registration.Photo.InputStream);

                var qcl = csa.CreateCloudQueueClient();
                var queue = qcl.GetQueueReference("registrations");
                queue.CreateIfNotExists();
                string payload = JsonConvert.SerializeObject(registration.Name);
                queue.AddMessage(new CloudQueueMessage(payload));

                return RedirectToAction("AccountOverview");
            }
            catch(Exception e)
            {
                return View();
            }
        }

        public ActionResult AccountOverview()
        {
            CloudStorageAccount csa = CloudStorageAccount.Parse(constring);
            CloudTableClient ctc = csa.CreateCloudTableClient();
            CloudTable table = ctc.GetTableReference("account");
            table.CreateIfNotExists();

            List<Account> accounts = table.ExecuteQuery(new TableQuery<Account>()).ToList();
            return View(accounts);
        }
    }
}
