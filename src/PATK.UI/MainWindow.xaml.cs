using System.IO;
using System.Linq;
using System.Windows;
using MahApps.Metro.Controls;
using Microsoft.Win32;
using PATK.Common.XML;
using PATK.Rest.Repositories;
using PATK.UI.Viewmodels;

namespace PATK.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        private readonly IPublishSettingsReader _publishSettingsReader;
        private readonly ICloudServicesRepository _cloudServicesRepository;
        private MainWindowViewModel _viewModel;

        private bool _subscriptionLoaded = false;

        public MainWindow(
            IPublishSettingsReader publishSettingsReader,
            ICloudServicesRepository cloudServicesRepository)
        {
            _publishSettingsReader = publishSettingsReader;
            _cloudServicesRepository = cloudServicesRepository;
            InitializeComponent();
        }

        /// <summary>
        /// Load publishSettings file and convert to subscription info
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelectSubscriptionFile(object sender, RoutedEventArgs e) {

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
                    Common.Global.Settings.PublishSettings = subscription.First();
                    _subscriptionLoaded = true;
                    LoadCloudServices();
                }
            }
        }

        private async void LoadCloudServices()
        {
            if (!_subscriptionLoaded)
            {
                return;
            }

            var cloudServices = await _cloudServicesRepository.GetCloudServices();
            _viewModel = new MainWindowViewModel()
            {
                CloudServices = cloudServices.HostedServices
            };

            this.DataContext = _viewModel;
        }
    }
}
