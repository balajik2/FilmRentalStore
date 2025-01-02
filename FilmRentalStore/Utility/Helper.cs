using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure.Messaging.EventGrid;
using Azure;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using FilmRentalStore.Models;
using FilmRentalStore.DTO;

namespace FilmRentalStore.Utility
{
    public class Helper
    {
        public static async Task PublishToEventGrid(IConfiguration configuration, StaffDTO staffDTO)
        {
            var endPoint = configuration.GetValue<string>("TopicEndPoint");
            var accessKey = configuration.GetValue<string>("AccessKey");

            EventGridPublisherClient client = new EventGridPublisherClient(new Uri(endPoint), new AzureKeyCredential(accessKey));

            var event1 = new EventGridEvent("PMS", "PMS.SupplierEvent", "1.0", JsonConvert.SerializeObject(staffDTO));
            event1.Id = (new Guid()).ToString();
            event1.EventTime = DateTime.Now;
            event1.Topic = configuration.GetValue<string>("EventSubscription");

            List<EventGridEvent> eventList = new List<EventGridEvent> { event1 };

            await client.SendEventsAsync(eventList);
        }
    }
}
