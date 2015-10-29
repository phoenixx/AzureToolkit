using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using MahApps.Metro.Controls;
using Microsoft.Win32;
using PATK.Common.Certificates;
using PATK.Common.XML;
using PATK.Domain;
using PATK.Rest.RestConsumer;

namespace PATK.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        private readonly IPublishSettingsReader _publishSettingsReader;

        public MainWindow(IPublishSettingsReader publishSettingsReader)
        {
            _publishSettingsReader = publishSettingsReader;
            InitializeComponent();
        }

        private void SelectSubscriptionFile(object sender, RoutedEventArgs e)
        {
            ButtonLoadSubscription.IsEnabled = false;

            const string filter = "Azure Publish Settings|*.publishSettings";

            var ofd = new OpenFileDialog()
            {
                Filter = filter,
                Title = "Select your publish settings file"
            };

            var selected = ofd.ShowDialog();

            if (selected.HasValue && selected.Value)
            {
                var filename = ofd.FileName;
                var fileContents = File.ReadAllBytes(filename);
                var subscription = _publishSettingsReader.ReadPublishSettings(fileContents);

                if (subscription != null)
                {
                    var tester = new Test();
                    tester.TestTheThings(subscription.First());

                    //var certUtl = new CertificateUtility();
                    //var certificate = certUtl.GenerateCertificate(subscription.ManagementCertificate);
                    //var restConsumer = new RestConsumer(subscription.ServiceManagementUrl.AbsoluteUri, certificate);

                    //var serviceUrl = $"{subscription.Id}/services/hostedservices";
                    //var cloudServices = restConsumer.Get<string>(serviceUrl);
                    //var p = "";
                }
            }
        }
    }
}
