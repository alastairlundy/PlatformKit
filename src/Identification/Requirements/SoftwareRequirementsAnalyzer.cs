using System;

using PlatformKit.Linux;
using PlatformKit.Mac;
using PlatformKit.Windows;

namespace PlatformKit.Identification.Requirements;

public class SoftwareRequirementsAnalyzer
{
    protected OSAnalyzer _osAnalyzer;

    public SoftwareRequirementsAnalyzer()
    {
        _osAnalyzer = new OSAnalyzer();
    }

    public bool HasRequiredLinuxKernelVersion(Version requiredLinuxKernel)
    {
        return new LinuxAnalyzer().IsAtLeastKernelVersion(requiredLinuxKernel);
    }

    public bool HasRequiredMacOsVersion(MacOsVersion requiredMacOsVersionVersion)
    {
        return new MacOSAnalyzer().IsAtLeastMacOSVersion(requiredMacOsVersionVersion);
    }

    public bool HasRequiredWindowsVersion(WindowsVersion requiredWindowsVersion)
    {
        return new WindowsAnalyzer().IsAtLeastWindowsVersion(requiredWindowsVersion);
    }
}