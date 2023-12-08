/*
      PlatformKit
      
      Copyright (c) Alastair Lundy 2018-2023
      
      This Source Code Form is subject to the terms of the Mozilla Public
      License, v. 2.0. If a copy of the MPL was not distributed with this
      file, You can obtain one at http://mozilla.org/MPL/2.0/.
   */

namespace PlatformKit.Windows;

public class NetworkCard
{
    public string Name { get; set; }
    
    public string ConnectionName { get; set; }
    
    public bool DhcpEnabled { get; set; }
    
    public string DhcpServer { get; set; }

    public string[] IpAddresses { get; set; }
}