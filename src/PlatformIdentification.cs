using System;
using System.Reflection;

namespace AluminiumTech.DevKit.PlatformKit;

public class PlatformIdentification
{
    public PlatformIdentification()
    {
        
    }
    
        /// <summary>
        /// Return's the executing app's assembly.
        /// </summary>
        /// <returns></returns>
        // ReSharper disable once MemberCanBePrivate.Global
        // ReSharper disable once MemberCanBeMadeStatic.Global
        public Assembly GetAssembly()
        {
            return Assembly.GetEntryAssembly();
        }

        /// <summary>
        /// Gets the running App's name as reported by the Assembly.
        /// </summary>
        /// <returns></returns>
        // ReSharper disable once UnusedMember.Global
        public string GetAppName()
        {
            return GetAssembly().GetName().Name;
        }
        
        /// <summary>
        /// Return an app's version as a Version data type.
        /// </summary>
        /// <returns></returns>
        // ReSharper disable once UnusedMember.Global
        public Version GetAppVersion()
        {
            return GetAssembly().GetName().Version;
        }
        
}