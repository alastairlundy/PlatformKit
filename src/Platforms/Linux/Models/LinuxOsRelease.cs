/*
 PlatformKit is dual-licensed under the GPLv3 and the PlatformKit Licenses.
 
 You may choose to use PlatformKit under either license so long as you abide by the respective license's terms and restrictions.
 
 You can view the GPLv3 in the file GPLv3_License.md .
 You can view the PlatformKit Licenses at https://neverspy.tech
  
  To use PlatformKit under a commercial license you must purchase a license from https://neverspy.tech
 
 Copyright (c) AluminiumTech 2018-2022
 Copyright (c) NeverSpy Tech Limited 2022
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