using System;

namespace RazorWare.GfxCore.Extensibility;

/// <summary>
/// Represents a GfxCore extension interface.
/// </summary>
/// <remarks>
/// This interface is used to define a GfxCore extension. When this interface 
/// is implemented, the extension will be automatically loaded and initialized.
/// The extension attribute is used to define the name, version, and other 
/// information about the extension.
/// <para>
/// By inheriting the interface, the extension attribute will be visible to the 
/// class. It is nested in the interface to make it easier to access the attribute.
/// You may apply the attribute to the class without inheriting the interface by 
/// using the fully qualified name of the attribute, <see cref="IGfxExtension.GfxExtensionAttribute"/> 
/// </para>
/// </remarks>
public interface IGfxExtension
{


    //  nested GfxExtension attribute
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class GfxExtensionAttribute : Attribute
    {
        public string Name { get; }

        public GfxExtensionAttribute(string name)
        {
            Name = name;
        }

    }
}
