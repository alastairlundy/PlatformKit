/*
      PlatformKit
      
      Copyright (c) Alastair Lundy 2018-2024
      
      This Source Code Form is subject to the terms of the Mozilla Public
      License, v. 2.0. If a copy of the MPL was not distributed with this
      file, You can obtain one at http://mozilla.org/MPL/2.0/.
   */

namespace PlatformKit.Windows;

public class HyperVRequirements
{
    public bool VmMonitorModeExtensions { get; set; }
    
    public bool VirtualizationEnabledInFirmware { get; set; }
    
    public bool SecondLevelAddressTranslation { get; set; }
    
    public bool DataExecutionPreventionAvailable { get; set; }
}