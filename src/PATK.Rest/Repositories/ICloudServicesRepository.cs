using System.Threading.Tasks;
using Microsoft.WindowsAzure.Management.Compute.Models;

namespace PATK.Rest.Repositories
{
    public interface ICloudServicesRepository
    {
        Task<HostedServiceListResponse> GetCloudServices();
        Task<byte[]> GetRdpFile(string serviceName);
    }
}