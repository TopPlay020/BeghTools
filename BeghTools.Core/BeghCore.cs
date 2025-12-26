using System.Diagnostics;
using System.Reflection;

namespace BeghTools.Core
{
    public static class BeghCore
    {
        public static IServiceProvider Services { get; private set; }

        public static void InitializeServices()
        {
            Assembly.Load("BeghTools.ViewModels");
            var services = new ServiceCollection();

            //Load Transientable classes
            foreach (var type in GetTypesImplementing<ITransientable>())
            {
                var serviceType = GetRegisterType(type);
                if (serviceType != null)
                    services.AddTransient(serviceType, type);
                else
                    services.AddTransient(type);
            }


            //Load Singleton classes
            foreach (var type in GetTypesImplementing<ISingletonable>())
            {
                var serviceType = GetRegisterType(type);
                if (serviceType != null)
                    services.AddSingleton(serviceType, type);
                else
                    services.AddSingleton(type);
            }

            Services = services.BuildServiceProvider();
        }
    }
}
