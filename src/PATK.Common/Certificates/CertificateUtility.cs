using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace PATK.Common.Certificates
{
    public class CertificateUtility
    {
        public X509Certificate2 GenerateCertificate(string certificateContent)
        {
            try
            {
                var rawData = Convert.FromBase64String(certificateContent);
                return new X509Certificate2(rawData);
            }
            catch
            {
                return null;
            }
        }
    }
}
