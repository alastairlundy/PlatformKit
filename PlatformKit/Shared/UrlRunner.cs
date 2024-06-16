/*
        MIT License
       
       Copyright (c) 2020-2024 Alastair Lundy
       
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

using System;
using System.Diagnostics;
using System.Threading.Tasks;
using PlatformKit.Internal.Deprecation;


#if NETSTANDARD2_0
    using OperatingSystem = PlatformKit.Extensions.OperatingSystem.OperatingSystemExtension;
#endif

namespace PlatformKit;

public class UrlRunner
{
    
    protected static string MakeUrlSecure(string url)
    {
        if (url.StartsWith("http://"))
        {
            url = url.Replace("http://", "https://");
        }
        else if (url.StartsWith("www."))
        {
            url = url.Replace("www.", "https://www.");
        }

        return url;
    }

    /// <summary>
    /// Open a URL in the default browser.
    /// Courtesy of https://github.com/dotnet/corefx/issues/10361
    /// </summary>
    /// <param name="url">The URL to be opened.</param>
    public static void OpenUrlInDefaultBrowser(string url)
    {
        url = UrlHttpFormatting(url, false);
        url = MakeUrlSecure(url);
        
        OpenUrl(url);
    }

    protected static void OpenUrl(string url)
    {
        if (OperatingSystem.IsWindows())
        {
            CommandRunner.RunCmdCommand($"/c start {url.Replace("&", "^&")}", new ProcessStartInfo { CreateNoWindow = true});
        }
        if (OperatingSystem.IsLinux())
        {
            CommandRunner.RunCommandOnLinux($"xdg-open {url}");
        }
        if (OperatingSystem.IsMacOS())
        {
            Task task = new Task(() => Process.Start("open", url));
            task.Start();
        }
        if (OperatingSystem.IsFreeBSD())
        {
            CommandRunner.RunCommandOnFreeBsd($"xdg-open {url}");
        }
    }

            /// <summary>
            /// Open a URL in the default browser.
            /// Courtesy of https://github.com/dotnet/corefx/issues/10361
            /// </summary>
            /// <param name="url">The URL to be opened.</param>
            /// <param name="allowNonSecureHttp">Whether to allow non HTTPS links to be opened.</param>
            /// <returns></returns>
            [Obsolete(DeprecationMessages.DeprecationV5)]
            public static void OpenUrlInDefaultBrowser(string url, bool allowNonSecureHttp = false)
            {
                url = UrlHttpFormatting(url, allowNonSecureHttp);

                if (!allowNonSecureHttp)
                {
                   url = MakeUrlSecure(url);
                }
    
                OpenUrl(url);
            }

            protected static string UrlHttpFormatting(string url, bool allowNonSecureHttp)
            {
                if ((!url.StartsWith("https://") || !url.StartsWith("www.")) && (!url.StartsWith("file://")))
                {
                    if (allowNonSecureHttp)
                    {
                        url = "http://" + url;
                    }
                    else
                    {
                        url = "https://" + url;
                    }
                }

                return url;
            }
}