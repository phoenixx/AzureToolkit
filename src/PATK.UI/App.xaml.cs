using System.Windows;
using Castle.Windsor;
using Castle.Windsor.Installer;

namespace PATK.UI
{
    public partial class App : Application
    {
        private IWindsorContainer _container;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            _container = new WindsorContainer();
            _container.Install(FromAssembly.This());
        }
    }
}
