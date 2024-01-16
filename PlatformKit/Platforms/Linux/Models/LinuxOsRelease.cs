/*
      PlatformKit
      
      Copyright (c) Alastair Lundy 2018-2024
      
      This Source Code Form is subject to the terms of the Mozilla Public
      License, v. 2.0. If a copy of the MPL was not distributed with this
      file, You can obtain one at http://mozilla.org/MPL/2.0/.
   */

namespace PlatformKit.Linux;

    /// <summary>
    /// Represents a Linux Distribution's OsRelease file and information contained therein.
    /// </summary>
    public class LinuxOsRelease
    {
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