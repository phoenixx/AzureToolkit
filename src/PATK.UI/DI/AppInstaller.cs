using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using PATK.Common.XML;
using PATK.Logging;
using PATK.Rest.RestConsumer;

namespace PATK.UI.DI
{
    internal class AppInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<IRestConsumer>().ImplementedBy<RestConsumer>());
            container.Register(Component.For<IPublishSettingsReader>().ImplementedBy<PublishSettingsReader>().LifestyleSingleton());
            container.Register(Component.For<ILogger>().ImplementedBy<Logger>().LifestyleSingleton());
            container.Register(Component.For<MainWindow>().LifestyleTransient());
        }
    }
}