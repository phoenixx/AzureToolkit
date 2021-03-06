﻿using System;
using System.Windows;
using Castle.Windsor;
using PATK.Logging;
using PATK.UI.DI;

namespace PATK.UI
{
    public partial class App : Application
    {
        private IWindsorContainer _container;
        private ILogger _logger;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            _container = new WindsorContainer();
            _container.Install(new AppInstaller());

            _logger = _container.Resolve<ILogger>();

            Container.InitializeContainerWith(_container);

            LogStartUp();

            AppDomain.CurrentDomain.UnhandledException += CurrentDomainOnUnhandledException;

            var mainWindow = _container.Resolve<MainWindow>();
            mainWindow.Show();
        }

        private void LogStartUp()
        {
            _logger.Trace("Application start up: {@StartUpAt}", DateTime.Now);
        }

        private void CurrentDomainOnUnhandledException(object sender, UnhandledExceptionEventArgs unhandledExceptionEventArgs)
        {
            var exception = unhandledExceptionEventArgs.ExceptionObject as Exception;
            if (exception != null)
            {
                _logger.Error(exception.Message, exception);
            }
        }
    }
}