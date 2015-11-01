using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Win32;
using PATK.Common.XML;
using PATK.Rest.Repositories;
using PATK.UI.Commands;
using PATK.UI.DI;
using PATK.UI.Viewmodels;

namespace PATK.UI.Controls
{
    /// <summary>
    /// Interaction logic for Home.xaml
    /// </summary>
    public partial class Home : UserControl
    {
        private readonly IPublishSettingsReader _publishSettingsReader;
        private readonly ICloudServicesRepository _cloudServicesRepository;
        private ServicesViewModel _viewModel;
        private bool _subscriptionLoaded = false;

        public Home()
        {
            _publishSettingsReader = Container.Get<IPublishSettingsReader>();
            _cloudServicesRepository = Container.Get<ICloudServicesRepository>();
            LoadSubscriptionCommand = new DelegateCommand(LoadSubscription, CanLoadSubscription);
            _viewModel = new ServicesViewModel()
            {
                WindowCommand = LoadSubscriptionCommand
            };

            DataContext = _viewModel;
            InitializeComponent();

        }

        public ICommand LoadSubscriptionCommand { get; }

        private void LoadSubscription(object state)
        {
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

        private bool CanLoadSubscription(object state)
        {
            // return true/false here is enabled/disable button
            return true;
        }

        private void LoadSubscription_OnClick(object sender, RoutedEventArgs e)
        {
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
            _viewModel.CloudServices = cloudServices.HostedServices;
            DataContext = _viewModel;
        }
    }
}
