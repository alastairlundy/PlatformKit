﻿/*
        MIT License

       Copyright (c) 2020-2024 Alastair Lundy

       Permission is hereby granted, free of charge, to any person obtaining a copy
       of this software and associated documentation files (the "Software"), to deal
       in the Software without restriction, including without limitation the rights
       to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
       copies of the Software, and to permit persons to whom the Software is
       furnished to do so, subject to the following conditions:

       The above copyright notice and this permission notice shall be included in all
       copies or substantial portions of the Software.

       THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
       IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
       FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
       AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
       LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
       OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
       SOFTWARE.
   */

// ReSharper disable once CheckNamespace
namespace PlatformKit.Linux.Models;

/// <summary>
/// Represents a Linux Distribution's OsRelease file and information contained therein.
/// </summary>
public class LinuxOsReleaseModel
{
    public LinuxOsReleaseModel()
    {
        
    }
    
    public LinuxOsReleaseModel(string name, string version, string identifier, string identifierLike, string prettyName, string versionId, string versionCodeName, string homeUrl, string bugReportUrl, string privacyPolicyUrl, string supportUrl)
    {
        Name = name;
        Version = version;
        Identifier = identifier;
        Identifier_Like = identifierLike;
        PrettyName = prettyName;
        VersionId = versionId;
        VersionCodename = versionCodeName;
        HomeUrl = homeUrl;
        SupportUrl = supportUrl;
        BugReportUrl = bugReportUrl;
        PrivacyPolicyUrl = privacyPolicyUrl;
    }
    
    public bool IsLongTermSupportRelease { get; set; }

    public string Name { get; set; }

    public string Version { get; set; }

    public string Identifier { get; set; }

    // ReSharper disable once InconsistentNaming
    public string Identifier_Like { get; set; }

    public string PrettyName { get; set; }

    public string VersionId { get; set; }

    public string HomeUrl { get; set; }
    public string SupportUrl { get; set; }
    public string BugReportUrl { get; set; }
    public string PrivacyPolicyUrl { get; set; }

    public string VersionCodename { get; set; }
}