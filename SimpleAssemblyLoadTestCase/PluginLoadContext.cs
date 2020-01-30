using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Loader;

namespace SimpleAssemblyLoadTestCase
{
    public class PluginLoadContext : AssemblyLoadContext
    {
        private readonly AssemblyDependencyResolver _resolver;
        private readonly HashSet<string> _shared;

        public PluginLoadContext(string pluginPath, IEnumerable<string> sharedAssemblies)
        {
            _resolver = new AssemblyDependencyResolver(pluginPath);
            _shared = new HashSet<string>(sharedAssemblies);
        }

        // Enables plugins to have their own dependencies.
        protected override Assembly Load(AssemblyName assemblyName)
        {
            if (_shared.Contains(assemblyName.Name))
            {
                // Load from the default context
                return null;
            }

            string assemblyPath = _resolver.ResolveAssemblyToPath(assemblyName);
            if (assemblyPath != null)
            {
                return LoadFromAssemblyPath(assemblyPath);
            }

            return null;
        }

        // Enables plugins to have their own dependencies.
        protected override IntPtr LoadUnmanagedDll(string unmanagedDllName)
        {
            string libraryPath = _resolver.ResolveUnmanagedDllToPath(unmanagedDllName);
            if (libraryPath != null)
            {
                return LoadUnmanagedDllFromPath(libraryPath);
            }

            return IntPtr.Zero;
        }
    }
}
