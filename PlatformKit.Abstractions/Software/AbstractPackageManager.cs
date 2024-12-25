/*
        MIT License

    Copyright (c) 2024 Alastair Lundy

    Permission is hereby granted, free of charge, to any person obtaining a copy
    of this software and associated documentation files (the "Software"), to deal
    in the Software without restriction, including without limitation the rights
    to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
    copies of the Software, and to permit persons to whom the Software is
    furnished to do so, subject to the following conditions:

    The above copyright notice and this permission notice shall be included in all
    copies or substantial portions of the Software.

    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
    AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
    LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
    OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
    SOFTWARE.
 */

using System.Collections.Generic;
using System.IO;

using System.Threading.Tasks;

using PlatformKit.Abstractions.Exceptions;
using PlatformKit.Abstractions.Models;

namespace PlatformKit.Abstractions.Software
{
    public abstract class AbstractPackageManager
    {
        public string Name { get; protected set; }
    
        public abstract bool SupportedByCurrentOperatingSystem();

        public abstract Task<bool> IsPackageManagerInstalledAsync();

        public abstract Task<IEnumerable<AppModel>> GetUpdatableAsync();
        public abstract Task<IEnumerable<AppModel>> GetInstalledAsync();

        private string CleanUpExecutableName(string executableName)
        {
            return Path.GetFileNameWithoutExtension(executableName);
        }
    
        /// <summary>
        /// 
        /// </summary>
        /// <param name="packageName"></param>
        /// <returns></returns>
        /// <exception cref="PackageManagerNotInstalledException"></exception>
        /// <exception cref="PackageManagerNotSupportedException"></exception>
        public async Task<bool> IsPackageInstalledAsync(string packageName)
        {
            // ReSharper disable once RedundantBoolCompare
            if (SupportedByCurrentOperatingSystem() == true)
            {
                if (await IsPackageManagerInstalledAsync() == false)
                {
                    throw new PackageManagerNotInstalledException(Name);
                }

                bool foundPackage = false;

                string newPackageName = CleanUpExecutableName(packageName);
            
                foreach (AppModel app in await GetInstalledAsync())
                {
                    string tempAppName = CleanUpExecutableName(app.ExecutableName);
                
                    if (tempAppName.Equals(newPackageName.ToLower()))
                    {
                        foundPackage = true;
                    }
                }

                return foundPackage;
            }

            throw new PackageManagerNotSupportedException(Name);
        }
    }
}