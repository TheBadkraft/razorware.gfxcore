
using System.Reflection;

namespace RazorWare.GfxCore.Utilities;

/// <summary>
/// A collection of utility methods.
/// </summary>
public static class Utils
{
    /// <summary>
    /// Get the execution URI.
    /// </summary>
    /// <returns>The execution URI.</returns>
    public static Uri GetExecutionUri()
    {
        return new Uri(Assembly.GetExecutingAssembly().Location);
    }
    /// <summary>
    /// Get the executing directory.
    /// </summary>
    /// <returns>The executing directory.</returns>
    public static string GetExecutingDirectory()
    {
        //  get the executing directory
        var location = GetExecutionUri();
        return new FileInfo(location.AbsolutePath).Directory.FullName;
    }
    /// <summary>
    /// Resolve the path arguments, including validation, using Uri objects internally.
    /// </summary>
    /// <param name="argPath">The path argument to resolve.</param>
    /// <param name="path">The resolved directory path.</param>
    /// <param name="file">The resolved file name.</param>
    /// <returns>TRUE if the path is valid and the directory exists; otherwise, FALSE.</returns>
    public static bool ResolvePathArgs(this string argPath, out string path, out string file)
    {
        path = string.Empty;
        file = string.Empty;

        if (argPath.StartsWith("./"))
        {
            // Combine with the executing directory
            string basePath = GetExecutingDirectory();
            path = Path.Combine(basePath, argPath.Substring(2));
        }
        else if (argPath.StartsWith("/"))
        {
            // For absolute paths, use the root of the current drive on Windows
            string basePath = Path.GetPathRoot(Directory.GetCurrentDirectory());
            path = Path.Combine(basePath, argPath.Substring(1));
        }
        else
        {
            path = argPath; // Assume relative path
        }

        if (!IsValidPath(path))
        {
            return false;
        }

        // if (!uri.IsAbsoluteUri)
        // {
        //     uri = new Uri(new Uri(Directory.GetCurrentDirectory()), uri);
        // }
        // path = uri.LocalPath;

        // If the path doesn't exist as a directory, perhaps it was intended to be a file?
        // Here we check if path could be interpreted as ending with a file
        if (File.Exists(path))
        {
            file = Path.GetFileName(path);
            path = Path.GetDirectoryName(path);
        }

        return Directory.Exists(path);
    }
    /// <summary>
    /// Normalize the directory path using Uri for consistency across platforms.
    /// </summary>
    /// <param name="argPath">The Uri to normalize.</param>
    /// <returns>A new Uri with the normalized directory path.</returns>
    public static Uri NormalizeDirectoryPath(this string argPath)
    {
        if (argPath == null)
        {
            return new Uri(Directory.GetCurrentDirectory(), UriKind.Absolute);
        }
        // For absolute paths, use the root of the current drive on Windows
        string basePath = Path.GetPathRoot(Directory.GetCurrentDirectory());
        argPath = Path.Combine(basePath, argPath.Substring(1));


        // Convert back to Uri, ensuring it's absolute
        return new Uri(Path.GetFullPath(argPath), UriKind.Absolute);
    }
    /// <summary>
    /// Checks if a given Uri represents a valid path.
    /// </summary>
    /// <param name="argPath">The Uri to validate.</param>
    /// <returns>True if the path appears to be valid; otherwise, false.</returns>
    private static bool IsValidPath(string argPath)
    {
        if (argPath == null) return false;

        //  Check for invalid characters
        if (argPath.IndexOfAny(Path.GetInvalidPathChars()) >= 0)
        {
            return false;
        }

        //  Check for potentially problematic path constructions
        if (argPath.Contains("..") || argPath.Contains("..."))
        {
            return false;
        }

        if (argPath.EndsWith(".") || argPath.EndsWith(".."))
        {
            return false;
        }

        //  Check path length (Windows restriction)
        if (argPath.Length > 260) // Adjust based on whether long paths are enabled
        {
            return false;
        }

        //  Check for reserved names on Windows
        string[] reservedNames = { "CON", "PRN", "AUX", "NUL", "COM1", "LPT1", "CLOCK$" };
        if (reservedNames.Contains(Path.GetFileName(argPath).ToUpper()))
        {
            return false;
        }

        return true;
    }
}
