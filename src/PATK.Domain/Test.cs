using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure;
using PATK.Domain.Azure;

namespace PATK.Domain
{
    public class Test
    {
        public async void TestTheThings(PublishSettings settings)
        {
            var computeClient = CloudContext.Clients.CreateCloudServiceManagementClient(settings.GetCredentials());
            var cloudServices = await computeClient.CloudServices.ListAsync();

            foreach (var cloudService in cloudServices)
            {
                Console.WriteLine(cloudService.Name);
            }

            var otherClient = CloudContext.Clients.CreateComputeManagementClient(settings.GetCredentials());
            var stuff = await otherClient.HostedServices.ListAsync();

            foreach (var wotsit in stuff)
            {
                Console.WriteLine(wotsit.ServiceName);
            }
        }
    }
}
