using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Xml.Linq;
using PATK.Domain.Azure;
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

        public IList<PublishSettings> ReadPublishSettings(byte[] publishSettingsContents)
        {
            var xmlRaw = Encoding.UTF8.GetString(publishSettingsContents);
            var document = XDocument.Parse(xmlRaw);

            return document.Descendants("Subscription").Select(ToPublishSettings).ToList();
        }

        private static PublishSettings ToPublishSettings(XElement element)
        {
            var settings = new PublishSettings
            {
                Id = Get(element, "Id"),
                Name = Get(element, "Name"),
                ServiceUrl = GetUri(element, "ServiceManagementUrl"),
                Certificate = GetCertificate(element, "ManagementCertificate")
            };
            return settings;
        }

        private static string Get(XElement element, string name)
        {
            return (string) element.Attribute(name);
        }

        private static Uri GetUri(XElement element, string name)
        {
            return new Uri(Get(element, name));
        }

        private static X509Certificate2 GetCertificate(XElement element, string name)
        {
            var encodedData = Get(element, name);
            var certificateAsBytes = Convert.FromBase64String(encodedData);
            return new X509Certificate2(certificateAsBytes);
        }

        //public Subscription ReadPublishSettings(byte[] publishSettingsContent)
        //{
        //    var xmlRaw = Encoding.UTF8.GetString(publishSettingsContent);

        //    var xmlDoc = new XmlDocument();
        //    xmlDoc.LoadXml(xmlRaw);

        //    var json = JsonConvert.SerializeXmlNode(xmlDoc);

        //    dynamic publishSettingsJson = JObject.Parse(json);

        //    var subscriptionSettings = publishSettingsJson.PublishData.PublishProfile.Subscription;

        //    if (subscriptionSettings != null)
        //    {
        //        var id = subscriptionSettings["@Id"].Value;
        //        var serviceManagementUrl = subscriptionSettings["@ServiceManagementUrl"].Value;
        //        var managementCertificate = subscriptionSettings["@ManagementCertificate"].Value;
        //        var name = subscriptionSettings["@Name"].Value;

        //        var subscription = new Subscription()
        //        {
        //            Id = id,
        //            ServiceManagementUrl = new Uri(serviceManagementUrl),
        //            Name = name,
        //            ManagementCertificate = managementCertificate
        //        };

        //        _logger.Trace("Loaded subscription {@subscription}", subscription);
        //        return subscription;
        //    }

        //    throw new PublishSettingsException("Invalid or incorrect publishSettings file. No subscription element found.");
        //}
    }
}