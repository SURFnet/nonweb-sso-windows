using System;
using System.Reflection;

namespace SurfnetSSO {
    /// <summary>
    /// Resolves the correct web launcher for the target.
    /// Code taken from: https://github.com/JonnyWideFoot/pcl-enlightenment
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public static class PlatformSupport<T> where T : class {
        private static T _cached;

        public static T Get() {
            if (_cached == null) {
                // Starting from our core assembly, determine the matching enlightenment assembly (with the same version/strong name if applicable)
                var enlightenmentAssemblyName = new AssemblyName(typeof(IWebLauncher).FullName) {
                    Name = "SurfnetSSO.Platform",
                };

                // Attempt to load the enlightenment provider from that assembly.
                string interfaceName = typeof(T).FullName;
                interfaceName = interfaceName.Replace("SurfnetSSO.I", "SurfnetSSO.Platform.");
                interfaceName = interfaceName + ", " + enlightenmentAssemblyName.FullName;
                Type enlightenmentProviderType = Type.GetType(interfaceName, false);

                if (enlightenmentProviderType == null) {
                    throw new InvalidOperationException("Unable to load platform abstraction");
                }

                _cached = (T)Activator.CreateInstance(enlightenmentProviderType);
            }

            return _cached;
        }
    }
}
