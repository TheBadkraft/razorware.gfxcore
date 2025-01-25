
using System.Reflection;
using System.Xml;

using RazorWare.GfxCore.Utilities;

namespace RazorWare.GfxCore.Extensibility;

public static class Project
{
    public static bool Load(string path, Manifest manifest)
    {
        string file = string.Empty;
        //  validate the cs project directory & file
        try
        {
            if (!Directory.GetFiles(path, "*.csproj", SearchOption.TopDirectoryOnly)
            .FirstOrDefault().ResolvePathArgs(out path, out file))
            {
                var message = $"Could not find .csproj file in {path}";
                Packager.Log(message);

                throw new FileNotFoundException(message);
            }
        }
        catch (Exception ex)
        {
            Packager.Log($"Error reading .csproj file: {ex.Message}");
        }

        /*
            We will use AssemblyInformationalVersion for the extension version.

            Example:
                "1.2.3" - Simple version.
                "1.2.3-alpha.1" - Pre-release version.
                "1.2.3-alpha.1+20230101" - Pre-release version with build metadata.
                "1.2.3+build.123" - Stable (release) version with build metadata.
        */
        XmlDocument xmlDoc = new XmlDocument();
        try
        {
            xmlDoc.Load(Path.Combine(path, file));
            //  get the doc root
            XmlNode root = xmlDoc.DocumentElement;
            //  get the assembly name
            XmlNode name = root.SelectSingleNode("PropertyGroup/AssemblyName");
            XmlNode asmVersion = root.SelectSingleNode("PropertyGroup/AssemblyFileVersion");
            //  "razorware.gfxcore.domain, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
            var asmName = $"{name.InnerText}, Version={asmVersion.InnerText}, Culture=neutral, PublicKeyToken=null";
            manifest.Assembly = new() { Name = new(asmName), EntryTag = name.InnerText, FileName = $"{name.InnerText}.dll" };
            //  get the title
            XmlNode title = root.SelectSingleNode("PropertyGroup/AssemblyTitle");
            manifest.Title = title.InnerText;
            //  get the assembly version
            XmlNode version = root.SelectSingleNode("PropertyGroup/AssemblyInformationalVersion");
            manifest.Version = version.InnerText;
            //  get the assembly description
            XmlNode description = root.SelectSingleNode("PropertyGroup/AssemblyDescription");
            manifest.Description = description.InnerText;
            //  get the assembly author
            XmlNode author = root.SelectSingleNode("PropertyGroup/AssemblyAuthors");
            manifest.Author = author.InnerText;
            //  get the copyright
            XmlNode copyright = root.SelectSingleNode("PropertyGroup/AssemblyCopyright");
            manifest.Copyright = copyright.InnerText;
        }
        catch (Exception ex)
        {
            Packager.Log($"Error loading .csproj file: {ex.Message}");
        }

        return true;
    }
}
