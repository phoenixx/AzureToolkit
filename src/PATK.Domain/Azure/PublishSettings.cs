using System;
using System.Security.Cryptography.X509Certificates;
using Microsoft.WindowsAzure;

namespace PATK.Domain.Azure
{
    public class PublishSettings
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public Uri ServiceUrl { get; set; }
        public X509Certificate2 Certificate { get; set; }

        public SubscriptionCloudCredentials GetCredentials()
        {
            return new CertificateCloudCredentials(Id, Certificate);    
        }
    }
}