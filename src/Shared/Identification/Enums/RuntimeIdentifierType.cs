
namespace PlatformKit.Identification;

/// <summary>
/// The type of RuntimeIdentifier generated or detected.
/// </summary>
public enum RuntimeIdentifierType
{
    AnyGeneric,
    Generic,
    Specific,
    /// <summary>
    /// This is meant for Linux use only. DO NOT USE ON WINDOWS or MAC.
    /// </summary>
    DistroSpecific,
    /// <summary>
    /// This is meant for Linux use only. DO NOT USE ON WINDOWS or MAC.
    /// </summary>
    VersionLessDistroSpecific
}