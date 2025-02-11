﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Versioning;
using System.Threading.Tasks;

using CliRunner;
using CliRunner.Abstractions;
using CliRunner.Builders;
using CliRunner.Builders.Abstractions;
using CliRunner.Extensions;
using PlatformKit.Core.Models;
using PlatformKit.Internal.Localizations;

namespace PlatformKit.OperatingSystems.Linux;

#if NETSTANDARD2_0 || NETSTANDARD2_1
using OperatingSystem = Polyfills.OperatingSystemPolyfill;

#endif

public partial class LinuxOperatingSystem
{
    private readonly ICommandRunner _commandRunner;

    public LinuxOperatingSystem(ICommandRunner commandRunner)
    {
        _commandRunner = commandRunner;
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    /// <exception cref="PlatformNotSupportedException"></exception>
#if NET5_0_OR_GREATER
    [SupportedOSPlatform("linux")]
#endif
    public async Task<IEnumerable<AppModel>> GetInstalledAppsAsync()
    {
        if (OperatingSystem.IsLinux() == false)
        {
            throw new PlatformNotSupportedException(Resources.Exceptions_PlatformNotSupported_WindowsOnly);
        }
        
        List<AppModel> apps = new List<AppModel>();

        ICommandBuilder commandBuilder = new CommandBuilder("/usr/bin/ls")
            .WithArguments(" -F /usr/bin | grep -v /")
            .WithWorkingDirectory(Environment.SystemDirectory);
        
        Command command = commandBuilder.Build();
        
        var result = await _commandRunner.ExecuteBufferedAsync(command);
        
#if NET5_0_OR_GREATER
        string[] array = result.StandardOutput.Split(Environment.NewLine);
#else
        string[] array = result.StandardOutput.Split('\n');
#endif
        foreach (string app in array)
        {
            apps.Add(new AppModel(app, $"{Path.DirectorySeparatorChar}usr{Path.DirectorySeparatorChar}bin"));
        }
            
        // if (includeBrewCasks)
        // {
        //     HomeBrewPackageManager homeBrew = new HomeBrewPackageManager();
        //     
        //     foreach (AppModel app in await homeBrew.GetInstalledAsync())
        //     {
        //         apps.Add(app);
        //     }
        // }

        return apps.ToArray();
    }
}