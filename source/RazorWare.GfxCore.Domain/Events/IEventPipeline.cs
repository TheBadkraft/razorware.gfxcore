using RazorWare.GfxCore.Extensibility;

namespace RazorWare.GfxCore.Events;

/// <summary>
/// The event pipeline interface.
/// </summary>
public interface IEventPipeline : IGfxCommandTarget
{
    /// <summary>
    /// Subscribe to an event.
    /// </summary>
    /// <typeparam name="TEvent">The event type</typeparam>
    /// <param name="handler">The event handler</param>
    void Subscribe<TEvent>(Action<TEvent> handler) where TEvent : GfxEvent;
    /// <summary>
    /// Unsubscribe from an event.
    /// </summary>
    /// <typeparam name="TEvent">The event type</typeparam>
    /// <param name="handler">The event handler</param>
    void Unsubscribe<TEvent>(Action<TEvent> handler) where TEvent : GfxEvent;
    /// <summary>
    /// Raise an event.
    /// </summary>
    /// <typeparam name="TEvent">The event type</typeparam>
    /// <param name="event">The event</param>
    void Raise<TEvent>(TEvent @event) where TEvent : GfxEvent;
}