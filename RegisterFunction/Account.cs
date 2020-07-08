using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RegisterFunction.Models
{
    public class Account
    {
        public string Name { get; set; }
        public string Photo { get; set; }
        public string Thumbnail { get; set; }
        public string RowKey { get; set; }
        public string PartitionKey { get; set; }
    }
}