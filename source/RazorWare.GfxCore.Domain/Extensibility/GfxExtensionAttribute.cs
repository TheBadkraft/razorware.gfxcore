namespace RazorWare.GfxCore.Extensibility;

/// <summary>
/// Represents a GfxCore extension attribute.
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
public class GfxExtensionAttribute : Attribute
{
    public Type ExtensionType { get; init; }

    public GfxExtensionAttribute(Type extensionType)
    {
        ExtensionType = extensionType;
    }

}