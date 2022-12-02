/*
 PlatformKit is dual-licensed under the GPLv3 and the PlatformKit Licenses.
 
 You may choose to use PlatformKit under either license so long as you abide by the respective license's terms and restrictions.
 
 You can view the GPLv3 in the file GPLv3_License.md .
 You can view the PlatformKit Licenses at https://neverspy.tech/platformkit-commercial-license or in the file PlatformKit_Commercial_License.txt

 To use PlatformKit under a commercial license you must purchase a license from https://neverspy.tech
 
 Copyright (c) AluminiumTech 2018-2022
 Copyright (c) NeverSpy Tech Limited 2022
 */

using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Reflection;
using System.Text;
using System.Threading;

using PlatformKit.FreeBSD;
using PlatformKit.Identification;
using PlatformKit.Internal.Licensing;
using PlatformKit.Linux;
using PlatformKit.Mac;
using PlatformKit.Windows;

namespace PlatformKit.Internal.Analytics;

internal class PlatformKitAnalytics
{
    private static WebSocket _webSocket;

    private static Uri uri = new Uri("wss://app.nucleus.sh/6387f260fae4a13be78fc664");

    internal PlatformKitAnalytics()
    {
        _webSocket = new ClientWebSocket();
    }

    internal static string ToMessageString(KeyValuePair<string, string>[] pairs)
    {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append("{\r\n");
        stringBuilder.Append('"');
        stringBuilder.Append("data");
        stringBuilder.Append('"');
        stringBuilder.Append(": [{\r\n");

        foreach (var pair in pairs)
        {
            stringBuilder.Append('"');
            stringBuilder.Append(pair.Key);
            stringBuilder.Append('"');
            stringBuilder.Append(':');
            
            stringBuilder.Append('"');
            stringBuilder.Append(pair.Value);
            stringBuilder.Append('"');
            stringBuilder.Append(",\r\n");
        }

        //Hopefully remove the last comma.
        stringBuilder.Remove(stringBuilder.Length - 1, 1);
        
        stringBuilder.Append("}]");
        stringBuilder.Append("}");

        return stringBuilder.ToString();
    }

    internal static void InitializeAnalytics()
    {
        List<KeyValuePair<string, string>> pairs = new List<KeyValuePair<string, string>>();
        pairs.Add(new KeyValuePair<string, string>("type", "init"));
        pairs.Add(new KeyValuePair<string, string>("event", "Initializing PlatformKit in Project"));

        if (PlatformKitSettings.LicenseKey != null)
        {
            pairs.Add(new KeyValuePair<string, string>("userId", PlatformKitSettings.LicenseKey));
        }
        
        pairs.Add(new KeyValuePair<string, string>("deviceId", Assembly.GetEntryAssembly().GetName().Name));
        pairs.Add(new KeyValuePair<string, string>("version", PlatformIdentification.GetPlatformKitVersion().ToString()));

        pairs.Add(new KeyValuePair<string, string>("payload", "{"));
        pairs.Add(new KeyValuePair<string, string>("licenseSelected", PlatformKitSettings.SelectedLicense.Name + "} \r\n"));

        
        SendWebsocketData(ToMessageString(pairs.ToArray()));
    }

    internal static void ReportError(Exception exception, string methodName)
    {
        if (PlatformKitSettings.AllowErrorReporting == true)
        {
            List<KeyValuePair<string, string>> pairs = new List<KeyValuePair<string, string>>();
            pairs.Add(new KeyValuePair<string, string>("type", "error"));
            
            //pairs.Add(new KeyValuePair<string, string>("userId", PlatformKitSettings.LicenseKey));
            //pairs.Add(new KeyValuePair<string, string>("deviceId", Assembly.GetEntryAssembly().GetName().Name));

            var osVersion = "";
            var platform = "";

            if (OSAnalyzer.IsWindows())
            {
                platform = "Windows";
                osVersion = new WindowsAnalyzer().DetectWindowsVersion().ToString();
            }
            else if (OSAnalyzer.IsMac())
            {
                platform = "macOS";
                osVersion = new MacOSAnalyzer().DetectMacOsVersion().ToString();
            }
            else if (OSAnalyzer.IsLinux())
            {
                platform = "Linux";
                osVersion = new LinuxAnalyzer().DetectLinuxDistributionVersionAsString();
            }
            else if (OSAnalyzer.IsFreeBSD())
            {
                platform = "FreeBSD";
                osVersion = new FreeBSDAnalyzer().DetectFreeBSDVersion().ToString();
            }
            
            pairs.Add(new KeyValuePair<string, string>("platform", platform));
            pairs.Add(new KeyValuePair<string, string>("osVersion", osVersion));
            pairs.Add(new KeyValuePair<string, string>("version", PlatformIdentification.GetPlatformKitVersion().ToString()));

            pairs.Add(new KeyValuePair<string, string>("payload", "{"));
            pairs.Add(new KeyValuePair<string, string>("exception", exception.Message));
            pairs.Add(new KeyValuePair<string, string>("methodName", methodName + "} \r\n"));

            SendWebsocketData(ToMessageString(pairs.ToArray()));
        }
    }

    internal static void SendWebsocketData(string message)
    {
        var bytes = Encoding.Unicode.GetBytes(message);

        _webSocket.SendAsync(bytes, WebSocketMessageType.Text, true, CancellationToken.None);
    }
}