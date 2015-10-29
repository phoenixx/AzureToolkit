﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure;

namespace PATK.Domain.Azure
{
    public class PublishSettings
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public Uri ServiceUrl { get; set; }
        public X509Certificate2 Certificate { get; set; }

        //public SubscriptionCloudCredentials GetCredentials()
        //{
            
        //}
    }
}
