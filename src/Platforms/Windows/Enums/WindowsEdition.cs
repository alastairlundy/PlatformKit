
namespace PlatformKit.Windows;

/// <summary>
/// An enum representing different Windows Editions.
/// </summary>
public enum WindowsEdition
{
    Home,
    Education,
    Professional,
    ProfessionalForEducation,
    ProfessionalForWorkstations,
    // ReSharper disable once InconsistentNaming
    EnterpriseLTSC,
    EnterpriseSemiAnnualChannel,
    IoTCore,
    IoTEnterprise,
    IoTEnterpriseLTSC,
    Team,
    Server,
    /// <summary>
    /// Windows 11 or newer only
    /// </summary>
    SE,
}