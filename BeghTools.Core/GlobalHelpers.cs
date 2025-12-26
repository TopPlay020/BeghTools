using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;
using System.Reflection;

namespace BeghTools.Core
{
    public static class GlobalHelpers
    {
        private static IEnumerable<Assembly> GetAllAssemblies()
        {
            // Includes executing assembly and all referenced assemblies
            var assemblies = AppDomain.CurrentDomain.GetAssemblies().ToList();
            return assemblies;
        }

        public static IEnumerable<Type> GetTypesImplementing<TInterface>()
        {
            return GetAllAssemblies()
                   .SelectMany(a => a.GetTypes())
                   .Where(t => typeof(TInterface).IsAssignableFrom(t) && t.IsClass);
        }

        public static IEnumerable<Type> GetTypesWithAttribute<TAttribute>() where TAttribute : Attribute
        {
            return GetAllAssemblies()
                   .SelectMany(a => a.GetTypes())
                   .Where(t => t.IsClass && t.GetCustomAttribute<TAttribute>() != null);
        }
        public static Type GetRegisterType(Type implementationType)
        {
            var marker = implementationType.GetInterfaces()
                .FirstOrDefault(i =>
                    i.IsGenericType &&
                    i.GetGenericTypeDefinition() == typeof(IRegisterByInterface<>));

            return marker?.GetGenericArguments()[0];
        }

        public static Type GetTypeByName(string typeName)
        {
            return GetAllAssemblies()
                   .SelectMany(a => a.GetTypes())
                   .FirstOrDefault(t => t.Name == typeName);
        }

        public static string GetCurrentExecutablePath()
        {
            return System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;
        }

        public static T GetRequiredService<T>()
        {
            return BeghCore.Services.GetRequiredService<T>();
        }

        public static T GetRequiredService<T>(Type serviceType)
        {
            return (T)BeghCore.Services.GetRequiredService(serviceType);
        }

        public static object GetRequiredService(Type serviceType)
        {
            return BeghCore.Services.GetRequiredService(serviceType);
        }


        public static List<string> WPFImageUnsupportedFormats = new List<string>() { "webp" };

        public static Uri GetUri(string relativePath, string assemblyName = null)
        {
            if (string.IsNullOrEmpty(assemblyName))
                assemblyName = Assembly.GetExecutingAssembly().GetName().Name;

            return new Uri($"pack://application:,,,/{assemblyName};component/{relativePath}", UriKind.Absolute);
        }

    }
}
