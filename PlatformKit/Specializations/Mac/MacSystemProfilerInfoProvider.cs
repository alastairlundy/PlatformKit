/*
    PlatformKit
    Copyright (c) 2020-2025 Alastair Lundy

    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.
 */

using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

using CliRunner;
using CliRunner.Abstractions;
using CliRunner.Buffered;
using CliRunner.Extensions;

#if NETSTANDARD2_0 || NETSTANDARD2_1
using OperatingSystem = AlastairLundy.OSCompatibilityLib.Polyfills.OperatingSystem;
#else
using System.Runtime.Versioning;
#endif

using PlatformKit.Internal.Localizations;
// ReSharper disable ConvertToPrimaryConstructor

namespace PlatformKit.Specializations.Mac;

public class MacSystemProfilerInfoProvider : IMacSystemProfilerInfoProvider
{
    private readonly ICommandRunner _commandRunner;

    public MacSystemProfilerInfoProvider(ICommandRunner commandRunner)
    {
        _commandRunner = commandRunner;
    }
    
    /// <summary>
    /// Returns whether the Mac running this method has Secure Virtual Memory enabled or not.
    /// </summary>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="PlatformNotSupportedException">Throw if run on an Operating System that isn't macOS.</exception>
#if NET5_0_OR_GREATER
    [SupportedOSPlatform("macos")]
    [UnsupportedOSPlatform("windows")]
    [UnsupportedOSPlatform("linux")]
#endif
    public async Task<bool> IsSecureVirtualMemoryEnabledAsync()
    {
        if (OperatingSystem.IsMacOS() == false)
        {
            throw new PlatformNotSupportedException(Resources.Exceptions_PlatformNotSupported_MacOnly);
        }
        
        string result = await GetMacSystemProfilerValue(
            MacSystemProfilerDataType.SoftwareDataType, "Secure Virtual Memory");

        if (result.ToLower().Contains("disabled"))
        {
            return false;
        }

        if (result.ToLower().Contains("enabled"))
        {
            return true;
        }

        return false;
    }
    
    /// <summary>
    ///  Returns whether the Mac running this method has Activation Lock enabled or not.
    /// </summary>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="PlatformNotSupportedException">Throw if run on an Operating System that isn't macOS.</exception>
#if NET5_0_OR_GREATER
    [SupportedOSPlatform("macos")]
    [SupportedOSPlatform("maccatalyst")]
#endif
    public async Task<bool> IsActivationLockEnabledAsync()
    {
        if (OperatingSystem.IsMacOS() == false)
        {
            throw new PlatformNotSupportedException(Resources.Exceptions_PlatformNotSupported_MacOnly);
        }
        
        string result = await GetMacSystemProfilerValue(
            MacSystemProfilerDataType.HardwareDataType, "Activation Lock Status");

        if (result.ToLower().Contains("disabled"))
        {
            return false;
        }

        if (result.ToLower().Contains("enabled"))
        {
            return true;
        }

        throw new ArgumentException();
    }
    
    /// <summary>
    /// Gets a value from the Mac System Profiler information associated with a key
    /// </summary>
    /// <param name="macSystemProfilerDataType"></param>
    /// <param name="key"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="PlatformNotSupportedException">Throw if run on an Operating System that isn't macOS.</exception>
#if NET5_0_OR_GREATER
    [SupportedOSPlatform("macos")]
    [SupportedOSPlatform("maccatalyst")]
#endif
    [SuppressMessage("Interoperability", "CA1416:Validate platform compatibility")]
    public async Task<string> GetMacSystemProfilerValue(MacSystemProfilerDataType macSystemProfilerDataType,string key)
    {
        if (OperatingSystem.IsMacOS() == false)
        {
            throw new PlatformNotSupportedException(Resources.Exceptions_PlatformNotSupported_MacOnly);
        }
                
        BufferedCommandResult result = await Command.CreateInstance("/usr/bin/system_profiler")
            .WithArguments("SP" + macSystemProfilerDataType)
            .WithWorkingDirectory("/usr/bin/")
            .WithValidation(CommandResultValidation.None)
            .ExecuteBufferedAsync(_commandRunner);

        string info = result.StandardOutput;

#if NETSTANDARD2_0_OR_GREATER
        string[] array = info.Split(Convert.ToChar(Environment.NewLine));
#elif NET5_0_OR_GREATER
        string[] array = info.Split(Environment.NewLine);
#endif
        foreach (string str in array)
        {
            if (str.ToLower().Contains(key.ToLower()))
            {
                return str.Replace(key, string.Empty).Replace(":", string.Empty);
            }
        }

        throw new ArgumentException();
    }
}