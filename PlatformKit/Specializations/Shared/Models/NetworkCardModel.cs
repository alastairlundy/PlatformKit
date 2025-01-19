/*
    PlatformKit
    Copyright (c) 2020-2025 Alastair Lundy

    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.
 */


// ReSharper disable once CheckNamespace

namespace PlatformKit.Specializations.Shared
{
    /// <summary>
    /// 
    /// </summary>
    public class NetworkCardModel
    {
        public string Name { get; set; }
    
        public string ConnectionName { get; set; }
    
        public bool IsDhcpEnabled { get; set; }
    
        public string DhcpServer { get; set; }

        public string[] IpAddresses { get; set; }
    }
}