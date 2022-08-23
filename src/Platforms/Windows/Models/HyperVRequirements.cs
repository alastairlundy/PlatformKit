
namespace PlatformKit.Windows;

public class HyperVRequirements
{
    public bool VmMonitorModeExtensions { get; set; }
    
    public bool VirtualizationEnabledInFirmware { get; set; }
    
    public bool SecondLevelAddressTranslation { get; set; }
    
    public bool DataExecutionPreventionAvailable { get; set; }
}