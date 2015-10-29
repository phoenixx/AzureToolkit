using System;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PATK.Domain.Azure;
using PATK.Domain.Exceptions;
using PATK.Logging;

namespace PATK.Common.XML
{
    public class PublishSettingsReader : IPublishSettingsReader
    {
        private readonly ILogger _logger;

        public PublishSettingsReader(ILogger logger)
        {
            _logger = logger;
        }

        public Subscription ReadPublishSettings(byte[] publishSettingsContent)
        {
            var xmlRaw = Encoding.UTF8.GetString(publishSettingsContent);

            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlRaw);

            var json = JsonConvert.SerializeXmlNode(xmlDoc);

            dynamic publishSettingsJson = JObject.Parse(json);

            var subscriptionSettings = publishSettingsJson.PublishData.PublishProfile.Subscription;

            if (subscriptionSettings != null)
            {
                var id = subscriptionSettings["@Id"].Value;
                var serviceManagementUrl = subscriptionSettings["@ServiceManagementUrl"].Value;
                var managementCertificate = subscriptionSettings["@ManagementCertificate"].Value;
                var name = subscriptionSettings["@Name"].Value;

                var subscription = new Subscription()
                {
                    Id = id,
                    ServiceManagementUrl = new Uri(serviceManagementUrl),
                    Name = name,
                    ManagementCertificate = managementCertificate
                };

                _logger.Trace("Loaded subscription {@subscription}", subscription);
                return subscription;
            }

            throw new PublishSettingsException("Invalid or incorrect publishSettings file. No subscription element found.");
        }
    }
}