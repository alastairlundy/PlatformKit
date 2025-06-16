/*
      WinOsCompatLib
      Copyright (c) 2020-2025 Alastair Lundy

      This Source Code Form is subject to the terms of the Mozilla Public
      License, v. 2.0. If a copy of the MPL was not distributed with this
      file, You can obtain one at http://mozilla.org/MPL/2.0/.
   */

// ReSharper disable once CheckNamespace
namespace WinOsCompatLib;

/// <summary>
/// 
/// </summary>
public class HyperVRequirementsInfo
{
    /// <summary>
    /// 
    /// </summary>
    public bool VmMonitorModeExtensions { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    public bool VirtualizationEnabledInFirmware { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    public bool SecondLevelAddressTranslation { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    public bool DataExecutionPreventionAvailable { get; set; }
}