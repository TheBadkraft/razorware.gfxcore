
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
    /// Resolve the path arguments.
    /// </summary>
    /// <param name="argPath">The path argument to resolve.</param>
    /// <param name="path">The resolved directory path.</param>
    /// <param name="file">The resolved file name.</param>
    /// <returns>TRUE if the directory path exists; otherwise, FALSE.</returns>
    public static bool ResolvePathArgs(this string argPath, out string path, out string file)
    {
        //  if a path is provided, use it
        path = string.IsNullOrEmpty(argPath) ? string.Empty : argPath;
        file = string.Empty;

        //  if no path is provided, use the current directory
        if (string.IsNullOrEmpty(path))
        {
            path = Directory.GetCurrentDirectory();
        }
        else
        {
            file = Path.GetFileName(path);
            //  if the path starts with a "./", use the executing directory
            if (path.StartsWith("./"))
            {
                //  remove the "./" from the path
                path = Path.GetDirectoryName(path.Remove(0, 2));
                //  base path is the current directory
                var basePath = Directory.GetCurrentDirectory();
                //  absolute path is the executing directory combined with the path
                var absolutePath = Path.Combine(Utils.GetExecutingDirectory(), path ?? string.Empty);
                path = Path.GetRelativePath(basePath, absolutePath);
            }
            else if (path.StartsWith("/"))
            {
                //  remove the "/" from the path
                path = Path.GetDirectoryName(path.Remove(0, 1));
                // //  base path is the current directory
                // var basePath = Directory.GetCurrentDirectory();
                // //  absolute path is the executing directory combined with the path
                // var absolutePath = Path.Combine(Utils.GetExecutingDirectory(), path ?? string.Empty);
                // path = Path.GetRelativePath(basePath, absolutePath);
            }
        }

        //  unless we can find a reason to fail ...
        return Directory.Exists(path);
    }
}
