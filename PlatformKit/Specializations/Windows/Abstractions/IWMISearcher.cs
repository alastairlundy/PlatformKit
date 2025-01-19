﻿/*
    PlatformKit
    Copyright (c) 2020-2025 Alastair Lundy

    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.
 */

using System.Threading.Tasks;
// ReSharper disable InconsistentNaming

namespace PlatformKit.Specializations.Windows.Abstractions;

public interface IWMISearcher
{
    public Task<string> GetWMIClassAsync(string wmiClass);

    public Task<string> GetWMIValueAsync(string property, string wmiClass);

}