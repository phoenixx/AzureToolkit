using System.Linq;
using System.Threading.Tasks;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Management.Compute;
using Microsoft.WindowsAzure.Management.Compute.Models;
using PATK.Domain.Exceptions;

namespace PATK.Rest.Repositories
{
    public class CloudServicesRepository : ICloudServicesRepository
    {
        private ComputeManagementClient _managementClient;

        public CloudServicesRepository()
        {
        }

        public async Task<HostedServiceListResponse> GetCloudServices()
        {
            GetManagementClient();
            var hostedServices = await _managementClient.HostedServices.ListAsync();
            return hostedServices;
        }

        public async Task<byte[]> GetRdpFile(string serviceName)
        {
            GetManagementClient();

            var service = await _managementClient.HostedServices.GetDetailedAsync(serviceName);
            var firstDeployment = service.Deployments.First();
            var machineName = firstDeployment.RoleInstances.First().HostName;

            var rdp = await _managementClient.VirtualMachines.GetRemoteDesktopFileAsync(serviceName, firstDeployment.Name, machineName);
            return rdp.RemoteDesktopFile;
        }

        private void GetManagementClient()
        {
            if (Common.Global.Settings.PublishSettings == null)
            {
                throw new PublishSettingsException("No settings loaded. Please load your publishSettings file.");
            }
            else
            {
                _managementClient = CloudContext.Clients.CreateComputeManagementClient(Common.Global.Settings.PublishSettings.GetCredentials());
            }
        }
    }
}