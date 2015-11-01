using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Microsoft.WindowsAzure.Management.Compute.Models;
using PATK.UI.Annotations;

namespace PATK.UI.Viewmodels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
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

        private void NotifyPropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
