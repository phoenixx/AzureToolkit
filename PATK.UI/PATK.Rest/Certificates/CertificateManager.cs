using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace PATK.Rest.Certificates
{
    public class CertificateManager
    {
        public static X509Certificate2 GetStoreCertificate(string thumbprint)
        {
              var locations = new List<StoreLocation>
              {
                StoreLocation.CurrentUser,
                StoreLocation.LocalMachine
              };

            foreach (var store in locations.Select(location => new X509Store("My", location)))
            {
                try
                {
                    store.Open(OpenFlags.ReadOnly | OpenFlags.OpenExistingOnly);
                    var certificates = store.Certificates.Find(
                        X509FindType.FindByThumbprint, thumbprint, false);
                    if (certificates.Count == 1)
                    {
                        return certificates[0];
                    }
                }
                finally
                {
                    store.Close();
                }
            }
            throw new ArgumentException($"A Certificate with Thumbprint '{thumbprint}' could not be located.");
        }
    }
}