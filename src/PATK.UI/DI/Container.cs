using Castle.Windsor;

namespace PATK.UI.DI
{
    public static class Container
    {
        private static IWindsorContainer _container;

        public static void InitializeContainerWith(IWindsorContainer container)
        {
            _container = container;
        }

        public static T Get<T>()
        {
            return _container.Resolve<T>();
        }
    }
}
