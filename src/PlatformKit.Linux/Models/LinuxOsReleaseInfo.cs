/*
    PlatformKit
    Copyright (c) 2020-2025 Alastair Lundy

    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.
 */

// ReSharper disable once CheckNamespace
namespace PlatformKit.Linux
{
    /// <summary>
    /// Represents a Linux Distribution's OsRelease file and information contained therein.
    /// </summary>
    public class LinuxOsReleaseInfo
    {
        public LinuxOsReleaseInfo()
        {
        
        }
    
        public LinuxOsReleaseInfo(string name, string version, string identifier, string identifierLike, string prettyName, string versionId, string versionCodeName, string homeUrl, string bugReportUrl, string privacyPolicyUrl, string supportUrl)
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
            IsLongTermSupportRelease = VersionId.ToLower().Contains("lts");
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
}