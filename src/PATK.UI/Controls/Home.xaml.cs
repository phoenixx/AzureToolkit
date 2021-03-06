﻿using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Win32;
using Microsoft.WindowsAzure.Management.Compute.Models;
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
        private ServicesViewModel _viewModel;
        private readonly ICloudServicesRepository _cloudServicesRepository;

        public Home()
        {
            _cloudServicesRepository = Container.Get<ICloudServicesRepository>();
            _viewModel = new ServicesViewModel();
            DataContext = _viewModel;
            InitializeComponent();
        }

        private void GetRdpFile(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button != null)
            {
                var service = button.CommandParameter.ToString();
                //var service = ((string)button.DataContext);
                var t = _cloudServicesRepository.GetRdpFile(service);
            }
        }
    }
}
