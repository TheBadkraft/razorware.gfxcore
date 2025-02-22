namespace RazorWare.GfxCore.Extensibility;

/// <summary>
/// The extension type.
/// </summary>
public enum GfxExtensionType
{
    /// <summary>
    /// The extension type is unknown.
    /// </summary>
    Unknown,
    /// <summary>
    /// The extension is a resource extension.
    /// </summary>
    Resource,
    /// <summary>
    /// The extension is a service extension.
    /// </summary>
    Service,
    /// <summary>
    /// The extension is a system extension.
    /// </summary>
    System,
    /// <summary>
    /// The extension is an SDK extension.
    /// </summary>
    Sdk
}