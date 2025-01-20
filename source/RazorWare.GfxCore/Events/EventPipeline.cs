
using System.Collections.Concurrent;

namespace RazorWare.GfxCore.Events;

/// <summary>
/// The event pipeline.
/// </summary>
public class EventPipeline : IEventPipeline
{
    private static readonly Lazy<EventPipeline> _INSTANCE = new(() => new EventPipeline());

    private readonly ConcurrentDictionary<Type, List<Delegate>> _events = new();

    /// <summary>
    /// The event pipeline instance.
    /// </summary>
    public static IEventPipeline Instance => _INSTANCE.Value;

    /// <summary>
    /// Get the name of the event pipeline.
    /// </summary>
    public string Name => "EventPipeline";

    //  private constructor
    private EventPipeline() { }

    /// <summary>
    /// Subscribe to an event.
    /// </summary>
    /// <typeparam name="TEvent">The event type</typeparam>
    /// <param name="handler">The event handler</param>
    public void Subscribe<TEvent>(Action<TEvent> handler) where TEvent : GfxEvent
    {
        _events.AddOrUpdate(typeof(TEvent), new List<Delegate> { handler },
        (_, list) =>
        {
            list.Add(handler);
            return list;
        });
    }
    /// <summary>
    /// Unsubscribe from an event.
    /// </summary>
    /// <typeparam name="TEvent">The event type</typeparam>
    /// <param name="handler">The event handler</param>
    public void Unsubscribe<TEvent>(Action<TEvent> handler) where TEvent : GfxEvent
    {
        if (_events.TryGetValue(typeof(TEvent), out var list))
        {
            list.Remove(handler);
        }
    }
    /// <summary>
    /// Raise an event.
    /// </summary>
    /// <typeparam name="TEvent">The event type</typeparam>
    /// <param name="event">The event</param>
    public void Raise<TEvent>(TEvent @event) where TEvent : GfxEvent
    {
        if (_events.TryGetValue(typeof(TEvent), out var list))
        {
            foreach (var handler in list)
            {
                ((Action<TEvent>)handler)(@event);
            }

            if (@event.AutoUnsub)
            {
                _events.Remove(typeof(TEvent), out var _);
            }
        }
    }
}
