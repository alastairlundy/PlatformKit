using System;
using System.Text;
using PlatformKit.Internal.Deprecation;
using PlatformKit.Models;

// ReSharper disable once CheckNamespace
namespace PlatformKit.Windows.Models;

[Obsolete(DeprecationMessages.DeprecationV5)]
public class WindowsSystemInformationModel
{
    public string HostName { get; set; }
    
    public string OsName { get; set; }
    public string OsVersion { get; set; }
    public string OsManufacturer { get; set; }
    public string OsConfiguration { get; set; }
    public string OsBuildType { get; set; }
    
    public string RegisteredOwner { get; set; }
    public string RegisteredOrganization { get; set; }
    
    public string ProductId { get; set; }
    
    public DateTime OriginalInstallDate { get; set; }
    public DateTime SystemBootTime { get; set; }
    
    public string SystemManufacturer { get; set; }
    public string SystemModel { get; set; }
    public string SystemType { get; set; }
    
    public string[] Processors { get; set; }
    
    public string BiosVersion { get; set; }
    
    
    public string WindowsDirectory { get; set; }
    public string SystemDirectory { get; set; }
    
    public string BootDevice { get; set; }
    
    
    public string SystemLocale { get; set; }
    public string InputLocale { get; set; }
    
    public TimeZoneInfo TimeZone { get; set; }
    
    public int TotalPhysicalMemoryMB { get; set; }
    public int AvailablePhysicalMemoryMB { get; set; }
    
    public int VirtualMemoryMaxSizeMB { get; set; }
    public int VirtualMemoryAvailableSizeMB { get; set; }
    public int VirtualMemoryInUse { get; set; }
    
    public string Domain { get; set; }
    
    public string[] PageFileLocations { get; set; }
    
    public string LogonServer { get; set; }
    
    public string[] HotfixesInstalled { get; set; }
    
    public NetworkCardModel[] NetworkCards { get; set; }
    
    public HyperVRequirementsModel HyperVRequirements { get; set; }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    [Obsolete(DeprecationMessages.DeprecationV5)]
    public override string ToString()
    {
        StringBuilder stringBuilder = new StringBuilder();
        
        stringBuilder.AppendLine($"{nameof(HostName)}: {HostName}");
        stringBuilder.AppendLine($"{nameof(OsName)}: {OsName}");
        stringBuilder.AppendLine($"{nameof(OsVersion)}: {OsVersion}");
        stringBuilder.AppendLine($"{nameof(OsManufacturer)}: {OsManufacturer}");
        stringBuilder.AppendLine($"{nameof(OsConfiguration)}: {OsConfiguration}");
        stringBuilder.AppendLine($"{nameof(OsBuildType)}: {OsBuildType}");
        stringBuilder.AppendLine($"{nameof(RegisteredOwner)}: {RegisteredOwner}");
        stringBuilder.AppendLine($"{nameof(RegisteredOrganization)}: {RegisteredOrganization}");
        stringBuilder.AppendLine($"{nameof(ProductId)}: {ProductId}");
        stringBuilder.AppendLine($"{nameof(OriginalInstallDate)}: {OriginalInstallDate}");
        stringBuilder.AppendLine($"{nameof(SystemBootTime)}: {SystemBootTime}");
        stringBuilder.AppendLine($"{nameof(SystemManufacturer)}: {SystemManufacturer}");
        stringBuilder.AppendLine($"{nameof(SystemModel)}: {SystemModel}");

        foreach (string str in Processors)
        {
            stringBuilder.AppendLine($"{nameof(Processors)}: {str}");
        }
        
        stringBuilder.AppendLine($"{nameof(BiosVersion)}: {BiosVersion}");
        stringBuilder.AppendLine($"{nameof(WindowsDirectory)}: {WindowsDirectory}");
        stringBuilder.AppendLine($"{nameof(SystemDirectory)}: {SystemDirectory}");
        stringBuilder.AppendLine($"{nameof(BootDevice)}: {BootDevice}");;
        stringBuilder.AppendLine($"{nameof(SystemLocale)}: {SystemLocale}");
        stringBuilder.AppendLine($"{nameof(InputLocale)}: {InputLocale}");
        stringBuilder.AppendLine($"{nameof(TimeZone)}: {TimeZone}");
        stringBuilder.AppendLine($"{nameof(TotalPhysicalMemoryMB)}: {TotalPhysicalMemoryMB}");
        stringBuilder.AppendLine($"{nameof(AvailablePhysicalMemoryMB)}: {AvailablePhysicalMemoryMB}");
        stringBuilder.AppendLine($"{nameof(VirtualMemoryMaxSizeMB)}: {VirtualMemoryMaxSizeMB}");
        stringBuilder.AppendLine($"{nameof(VirtualMemoryAvailableSizeMB)}: {VirtualMemoryAvailableSizeMB}");
        stringBuilder.AppendLine($"{nameof(VirtualMemoryInUse)}: {VirtualMemoryInUse}");
        
        foreach (string str in PageFileLocations)
        {
            stringBuilder.AppendLine($"{nameof(PageFileLocations)}: {str}");
        }
        
        stringBuilder.AppendLine($"{nameof(Domain)}: {Domain}");
        stringBuilder.AppendLine($"{nameof(LogonServer)}: {LogonServer}");

        foreach (string str in HotfixesInstalled)
        {
            stringBuilder.AppendLine($"{nameof(HotfixesInstalled)}: {str}");
        }

        foreach (NetworkCardModel networkCard in NetworkCards)
        {
            stringBuilder.AppendLine($"{nameof(networkCard)}.{nameof(networkCard.Name)}: {networkCard.Name}");
            stringBuilder.AppendLine(
                $"{nameof(networkCard)}.{nameof(networkCard.ConnectionName)}: {networkCard.ConnectionName}");
            stringBuilder.AppendLine(
                $"{nameof(networkCard)}.{nameof(networkCard.DhcpEnabled)}: {networkCard.DhcpEnabled}");
            stringBuilder.AppendLine(
                $"{nameof(networkCard)}.{nameof(networkCard.DhcpServer)}: {networkCard.DhcpServer}");
            

            foreach (string ipAddress in networkCard.IpAddresses)
            {
                stringBuilder.AppendLine($"{nameof(networkCard.IpAddresses)}: {ipAddress}");
            }
        }
          
        stringBuilder.AppendLine(
            $"{nameof(HyperVRequirements.DataExecutionPreventionAvailable)}: {HyperVRequirements.DataExecutionPreventionAvailable}");
        stringBuilder.AppendLine(
            $"{nameof(HyperVRequirements.SecondLevelAddressTranslation)}: {HyperVRequirements.SecondLevelAddressTranslation}");
        stringBuilder.AppendLine(
            $"{nameof(HyperVRequirements.VirtualizationEnabledInFirmware)}: {HyperVRequirements.VirtualizationEnabledInFirmware}");
        stringBuilder.AppendLine(
            $"{nameof(HyperVRequirements.VmMonitorModeExtensions)}: {HyperVRequirements.VmMonitorModeExtensions}");

        return stringBuilder.ToString();
    }
    
    [Obsolete(DeprecationMessages.DeprecationV5)]
    public void ToConsoleWriteLine()
    {
        Console.WriteLine(this.ToString());
    }
}