
using System.Text.RegularExpressions;

namespace RazorWare.GfxCore.Extensibility;

/// <summary>
/// The version of the extension.
/// </summary>
public class GfxVersion : IComparable<GfxVersion>
{
    /// <summary>
    /// Get the extensions base version.
    /// </summary>
    public Version BaseVersion { get; private set; }
    /// <summary>
    /// Get the extensions release type.
    /// </summary>
    public ExtRelease? ReleaseType { get; private set; }
    /// <summary>
    /// Get the extensions release number.
    /// </summary>
    public int? ReleaseNumber { get; private set; }
    /// <summary>
    /// Get the extensions build metadata.
    /// </summary>
    public string BuildMetadata { get; private set; }

    /// <summary>
    /// Create a new instance of the extension version.
    /// </summary>
    /// <param name="baseVersion">The base version of the extension.</param>
    /// <param name="releaseType">The release type of the extension.</param>
    /// <param name="releaseNumber">The release number of the extension.</param>
    /// <param name="buildMetadata">The build metadata of the extension.</param>
    public GfxVersion(Version baseVersion, ExtRelease? releaseType = null, int? releaseNumber = null, string buildMetadata = null)
    {
        BaseVersion = baseVersion;
        ReleaseType = releaseType;
        ReleaseNumber = releaseNumber;
        BuildMetadata = buildMetadata;
    }

    /// <summary>
    /// Get the string representation of the version.
    /// </summary>
    /// <returns>The string representation of the version.</returns>
    public override string ToString()
    {
        string result = BaseVersion.ToString();
        if (ReleaseType.HasValue)
        {
            result += $"-{ReleaseType.Value.ToString().ToLower()}";
            if (ReleaseNumber.HasValue)
            {
                result += $".{ReleaseNumber.Value}";
            }
        }
        if (!string.IsNullOrEmpty(BuildMetadata))
        {
            result += $"+{BuildMetadata}";
        }
        return result;
    }

    /// <summary>
    /// Compare this version to another version.
    /// </summary>
    /// <param name="other">The other version to compare to.</param>
    /// <returns>The comparison result.</returns>
    public int CompareTo(GfxVersion other)
    {
        if (other == null) return 1;

        int baseCompare = BaseVersion.CompareTo(other.BaseVersion);
        if (baseCompare != 0) { return baseCompare; }

        // Compare release types if both exist
        if (ReleaseType.HasValue && other.ReleaseType.HasValue)
        {
            int relTypeCompare = ReleaseType.Value.CompareTo(other.ReleaseType.Value);
            if (relTypeCompare != 0) { return relTypeCompare; }

            // If release types are the same, compare release numbers if both exist
            if (ReleaseNumber.HasValue && other.ReleaseNumber.HasValue)
            {
                return ReleaseNumber.Value.CompareTo(other.ReleaseNumber.Value);
            }
            else if (ReleaseNumber.HasValue) { return 1; }
            else if (other.ReleaseNumber.HasValue) { return -1; }
        }
        else if (ReleaseType.HasValue) { return -1; }  // This has release info, other doesn't
        else if (other.ReleaseType.HasValue) { return 1; }  // Other has release info, this doesn't

        return 0; // If we've made it here, versions are considered equal for comparison purposes
    }

    /// <summary>
    /// Try to parse the version string into a GfxVersion.
    /// </summary>
    /// <param name="version">The version string to parse.</param>
    /// <param name="gfxVersion">The GfxVersion instance.</param>
    /// <returns>TRUE if the version string was parsed successfully; otherwise, FALSE.</returns>
    public static bool TryParse(string version, out GfxVersion gfxVersion)
    {
        gfxVersion = null;

        if (string.IsNullOrEmpty(version) || version.Length > 30)
        {
            return false;
        }

        if (TryParseVersion(version, out var baseVersion, out var releaseType, out var releaseNumber, out var buildMetadata))
        {
            gfxVersion = new GfxVersion(baseVersion, releaseType, releaseNumber, buildMetadata);
        }

        return gfxVersion != null;
    }

    //  attempt to parse into the version components
    private static bool TryParseVersion(string versionString, out Version baseVersion, out ExtRelease? releaseType, out int? releaseNumber, out string buildMetadata)
    {
        baseVersion = default;
        releaseType = default;
        releaseNumber = default;
        buildMetadata = default;

        var match = Regex.Match(versionString, @"^(\d+\.\d+\.\d+)(-(alpha|beta|rc)(\.\d+)?)?(\+([0-9A-Za-z-]+\.?)+)?$", RegexOptions.IgnoreCase);

        if (match.Success)
        {
            //  get the base version
            baseVersion = new Version(match.Groups[1].Value);
            //  get the relase type
            if (match.Groups[3].Success)
            {
                releaseType = (ExtRelease)Enum.Parse(typeof(ExtRelease), match.Groups[3].Value, true);
                //  get the release number
                if (match.Groups[4].Success)
                {
                    releaseNumber = int.Parse(match.Groups[4].Value.TrimStart('.'));
                }
            }
            else
            {
                releaseType = null;
                releaseNumber = null;
            }

            buildMetadata = match.Groups[6].Success ? match.Groups[6].Value.TrimStart('+') : null;
        }
        else
        {
            return false;
        }

        return true;
    }
}
