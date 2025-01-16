using System;

namespace RazorWare.GfxCore.Runtime;

/// <summary>
/// Represents a facade interface.
/// </summary>
public interface IFacade
{
    /// <summary>
    /// Get or set a value indicating whether the facade should stop.
    /// </summary>
    bool IsStopRequested { get; set; }
}
