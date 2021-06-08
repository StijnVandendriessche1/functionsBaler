using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Text;

namespace IOTHubExample.Entities
{
    class LogEntity : TableEntity
    {
        public LogEntity()
        {

        }
        public LogEntity(string deviceID, string guid)
        {
            this.PartitionKey = deviceID;
            this.RowKey = guid;
        }
        public string Body { get; set; }
    }
}
