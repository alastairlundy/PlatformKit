/*
    PlatformKit
    Copyright (c) 2020-2025 Alastair Lundy

    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.
 */

using System.Threading.Tasks;
using CliRunner.Abstractions;
using PlatformKit.Specializations.Windows.Abstractions;

namespace PlatformKit.Specializations.Windows;

public class WinRegistrySearcher : IWinRegistrySearcher
{
    private readonly ICommandRunner _commandRunner;

    public WinRegistrySearcher(ICommandRunner commandRunner)
    {
        _commandRunner = commandRunner;
    }
    
    public async Task<string> GetValueAsync(string query)
    {
        throw new System.NotImplementedException();
    }

    public async Task<string> GetValueAsync(string query, string key)
    {
        throw new System.NotImplementedException();
    }
}