using IoTHubTrigger = Microsoft.Azure.WebJobs.EventHubTriggerAttribute;

using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Azure.EventHubs;
using System.Text;
using System.Net.Http;
using Microsoft.Extensions.Logging;
using System;
using IOTHubExample.Entities;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace IOTHubExample
{
    public static class Function1
    {
        private static HttpClient client = new HttpClient();

        [FunctionName("IOTTest")]
        public static void Run([IoTHubTrigger("messages/events", Connection = "ConnectionString")]EventData message, ILogger log)
        {
            try
            {
                string connectionstring = Environment.GetEnvironmentVariable("STORAGECONNECTIONSTRING");
                string body = Encoding.UTF8.GetString(message.Body.Array);
                log.LogInformation(body);
                LogEntity entity = new LogEntity("test", Convert.ToString(Guid.NewGuid()))
                {
                    Body = body
                };
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connectionstring);
                CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
                CloudTable table = tableClient.GetTableReference("logs");
                TableOperation insertOperation = TableOperation.Insert(entity);
                table.ExecuteAsync(insertOperation);
            }
            catch (Exception ex)
            {
                log.LogInformation(ex + "                -------->IOTTest");
            }            
        }
    }
}