using System.Text;
using System.Xml.Linq;
using PATK.Domain.Exceptions;

namespace PATK.Common.XML
{
    public class PublishSettingsReader
    {
        public string ReadPublishSettings(byte[] publishSettingsContent)
        {
            var xmlRaw = Encoding.UTF8.GetString(publishSettingsContent);
            var publishSettings = XDocument.Parse(xmlRaw);

            var xElement = publishSettings.Root?.Element("Subscription");
            if (xElement != null)
            {
                var managementCertificate =
                    xElement.Attribute("ManagementCertificate").Value;
                return managementCertificate;
            }
            throw new PublishSettingsException("No management certificate found in publishSettings file.");
        }
    }
}