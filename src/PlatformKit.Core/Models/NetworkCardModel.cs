/*
    PlatformKit
    Copyright (c) 2020-2025 Alastair Lundy

    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.
 */


// ReSharper disable once CheckNamespace

namespace PlatformKit.Core
{
    /// <summary>
    /// 
    /// </summary>
    public class NetworkCardModel(
        string name,
        string connectionName,
        bool isDhcpEnabled,
        string dhcpServer,
        string[] ipAddresses)
    {
        public string Name { get; protected set; } = name;

        public string ConnectionName { get; protected set; } = connectionName;

        public bool IsDhcpEnabled { get; protected set; } = isDhcpEnabled;

        public string DhcpServer { get; protected set; } = dhcpServer;

        public string[] IpAddresses { get; protected set; } = ipAddresses;
    }
}