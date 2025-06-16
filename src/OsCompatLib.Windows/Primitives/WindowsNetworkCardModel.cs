/*
    WinOsCompatLib
    Copyright (c) 2020-2025 Alastair Lundy

    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.
 */


// ReSharper disable once CheckNamespace

using System;

namespace WinOsCompatLib;

/// <summary>
/// 
/// </summary>
public class WindowsNetworkCardModel
{
    internal WindowsNetworkCardModel()
    {
        ConnectionName = string.Empty;
        Name = string.Empty;
        DhcpServer = string.Empty;
        IpAddresses = new string[0];
    }
        
    /// <summary>
    /// 
    /// </summary>
    public WindowsNetworkCardModel(string name,
        string connectionName,
        bool isDhcpEnabled,
        string dhcpServer,
        string[] ipAddresses)
    {
        Name = name;
        ConnectionName = connectionName;
        IsDhcpEnabled = isDhcpEnabled;
        DhcpServer = dhcpServer;
        IpAddresses = ipAddresses;
    }

    public string Name { get; internal set; }

    public string ConnectionName { get; internal set; }

    public bool IsDhcpEnabled { get; internal set; }

    public string DhcpServer { get; internal set; }

    public string[] IpAddresses { get; internal set; }
}