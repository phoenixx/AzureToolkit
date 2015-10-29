using System.Text;
using System.Xml.Linq;
using PATK.Domain.Exceptions;
using PATK.Logging;

namespace PATK.Common.XML
{
    public class PublishSettingsReader
    {
        private readonly ILogger _logger;

        public PublishSettingsReader(ILogger logger)
        {
            _logger = logger;
        }

        public string ReadPublishSettings(byte[] publishSettingsContent)
        {
            var xmlRaw = Encoding.UTF8.GetString(publishSettingsContent);
            var publishSettings = XDocument.Parse(xmlRaw);

            var xElement = publishSettings.Root?.Element("Subscription");
            if (xElement != null)
            {
                var managementCertificate =
                    xElement.Attribute("ManagementCertificate").Value;

                _logger.Debug("Loading management certificate {0}", managementCertificate);

                return managementCertificate;
            }
            throw new PublishSettingsException("No management certificate found in publishSettings file.");
        }
    }
}