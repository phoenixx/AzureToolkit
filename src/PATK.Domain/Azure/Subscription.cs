using System;

namespace PATK.Domain.Azure
{
    public class Subscription
    {
        public string Name { get; set; }
        public string Id { get; set; }
        public string ManagementCertificate { get; set; }
        public Uri ServiceManagementUrl { get; set; }
    }
}