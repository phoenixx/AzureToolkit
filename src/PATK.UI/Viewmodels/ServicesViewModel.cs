using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Win32;
using Microsoft.WindowsAzure.Management.Compute.Models;
using PATK.Common.XML;
using PATK.Rest.Repositories;
using PATK.UI.Annotations;
using PATK.UI.Commands;
using Container = PATK.UI.DI.Container;

namespace PATK.UI.Viewmodels
{
    public class ServicesViewModel : INotifyPropertyChanged
    {
        private readonly IPublishSettingsReader _publishSettingsReader;
        private readonly ICloudServicesRepository _cloudServicesRepository;
        private HostedServiceListResponse.HostedService _selectedService;

        public ServicesViewModel()
        {
            _publishSettingsReader = Container.Get<IPublishSettingsReader>();
            _cloudServicesRepository = Container.Get<ICloudServicesRepository>();
            LoadSubscriptionCommand = new DelegateCommand(LoadSubscription, CanLoadSubscription);
            ClearSubscriptionCommand = new DelegateCommand(ClearSubscription, CanClearSubscription);
        }

        private bool _subscriptionLoaded;

        public bool SubscriptionLoaded
        {
            get { return _subscriptionLoaded; }
            set
            {
                _subscriptionLoaded = value;
                NotifyPropertyChanged("SubscriptionLoaded");
            }
        }

        public HostedServiceListResponse.HostedService SelectedService
        {
            get
            {
                return _selectedService;
            }
            set
            {
                _selectedService = value;
                NotifyPropertyChanged("SelectedService");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private IList<HostedServiceListResponse.HostedService> _cloudServices;

        public IList<HostedServiceListResponse.HostedService> CloudServices
        {
            get { return _cloudServices; }
            set
            {
                _cloudServices = value;
                NotifyPropertyChanged("CloudServices");
            }
        }

        public ICommand LoadSubscriptionCommand { get; internal set; }
        public ICommand ClearSubscriptionCommand { get; internal set; }
        public ICommand SelectServiceCommnd { get; internal set; }

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
                    SubscriptionLoaded = true;
                    LoadCloudServices();
                }
            }
        }

        private bool CanLoadSubscription(object state)
        {
            return !_subscriptionLoaded;
        }

        private void ClearSubscription(object state)
        {
            SubscriptionLoaded = false;
            CloudServices = null;
        }

        private bool CanClearSubscription(object state)
        {
            return _subscriptionLoaded;
        }

        private void NotifyPropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private async void LoadCloudServices()
        {
            if (!SubscriptionLoaded)
            {
                return;
            }

            var cloudServices = await _cloudServicesRepository.GetCloudServices();
            CloudServices = cloudServices.HostedServices;
        }
    }
}
