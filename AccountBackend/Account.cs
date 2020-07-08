using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AccountBackend.Models
{
    public class Account: TableEntity
    {
        public string Name { get; set; }
        public string Photo { get; set; }
        public string Thumbnail { get; set; }
    }
}