using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PATK.Domain.Azure
{
    public class Subscription
    {
        public string Name { get; set; }
        public string Id { get; set; }
        public string ManagementCertificate { get; set; }
    }
}