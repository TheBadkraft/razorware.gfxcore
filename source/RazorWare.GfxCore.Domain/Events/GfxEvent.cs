using RazorWare.GfxCore.Registries;

namespace RazorWare.GfxCore.Events;

/// <summary>
/// The base Gfx event class.
/// </summary>
public abstract class GfxEvent : EventArgs
{
    /// <summary>
    /// Get the event sender.
    /// </summary>
    public object Sender { get; init; }
    /// <summary>
    /// Get the event message.
    /// </summary>
    public string Message { get; init; }
    /// <summary>
    /// Determine whether to unsubscribe after the event is raised.
    /// </summary>
    /// <remarks>
    /// [DEFAULT] false
    /// </remarks>
    public bool AutoUnsub { get; init; } = false;
}

/// <summary>
/// Bootstrap Initialized Event
/// </summary>
/// <remarks>
/// This event is raised when the GfxCore bootstrap has been initialized. 
/// Defaults to auto-unsubscribe.
/// </remarks>
public class BootstrapInitializedEvent : GfxEvent
{
    public RegistryManager Registries { get; init; }

    /// <summary>
    /// Initializes a new instance of the <see cref="BootstrapInitializedEvent"/> class.
    /// </summary>
    /// <param name="registries">The registry manager</param>
    public BootstrapInitializedEvent(RegistryManager registries)
    {
        Registries = registries;
        AutoUnsub = true;
    }
}
