namespace RazorWare.GfxCore.Extensibility;

/// <summary>
/// Represents a GfxCore extension attribute.
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
public class GfxExtensionAttribute : Attribute
{
    /// <summary>
    /// The extension type implementing <see cref="IGfxExtension"/> 
    /// </summary>
    public Type ImplementedType { get; init; }
    /// <summary>
    /// The extension type.
    /// </summary>
    public GfxExtensionType Type { get; init; } = GfxExtensionType.Unknown;

    /// <summary>
    /// Initializes a new instance of the <see cref="GfxExtensionAttribute"/> class.
    /// </summary>
    /// <param name="implType">The extension type implementing <see cref="IGfxExtension"/>.</param>
    public GfxExtensionAttribute(Type implType)
    {
        ImplementedType = implType;
    }

}
