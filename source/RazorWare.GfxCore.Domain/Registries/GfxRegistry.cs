using System.Collections;

namespace RazorWare.GfxCore.Registries;

/// <summary>
/// The base registry class
/// </summary>
/// <typeparam name="TRegItem">The registry type</typeparam>
public abstract class GfxRegistry<TRegItem> : IRegistry<TRegItem>
{
    private readonly Dictionary<RegistryKey, TRegItem> _registry = new();

    /// <summary>
    /// Get the registry name
    /// </summary>
    public string Name { get; protected init; }
    public Type Type { get; protected init; }

    protected GfxRegistry(string name)
    {
        Name = name;
        Type = typeof(TRegItem);
    }

    /// <summary>
    /// Register a registry item
    /// </summary>
    /// <typeparam name="T">Interface type</typeparam>
    /// <param name="type">Item type</param>
    /// <param name="regItem">The item</param>
    /// <param name="identifier">Item identifier</param>
    /// <param name="tags">Item tags</param>
    /// <returns>The registry item</returns>
    /// <exception cref="ArgumentException"></exception>
    public virtual T Register<T>(Type type, object regItem, string identifier = null, params string[] tags) where T : class
    {
        if (!(regItem is TRegItem item))
        {
            throw new ArgumentException($"{regItem.GetType().Name} is not a valid registry item ({typeof(TRegItem).Name}).");
        }

        var key = new RegistryKey(identifier, tags)
        {
            InterfaceType = typeof(T),
            ObjectType = type
        };

        if (!_registry.TryAdd(key, item))
        {
            throw new ArgumentException($"Registry key [{key}] already exists.");
        }

        return regItem as T;
    }
    /// <summary>
    /// Try to resolve registry item by type
    /// </summary>
    /// <param name="type">The registry type</param>
    /// <param name="registry">The registry</param>
    /// <returns>TRUE if the registry is found, FALSE otherwise</returns>
    public virtual bool TryResolve(Type type, out TRegItem registry)
    {
        registry = default;

        if (TryResolveKey(type, out var key))
        {
            return _registry.TryGetValue(key, out registry);
        }

        return false;
    }

    /// <summary>
    /// Get the registry enumerator
    /// </summary>
    /// <returns>The registry enumerator</returns>
    public IEnumerator<TRegItem> GetEnumerator()
    {
        foreach (var val in _registry.Values)
        {
            yield return val;
        }
    }
    /// <summary>
    /// Get the registry's object enumerator
    /// </summary>
    /// <returns>The registry enumerator</returns>
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    /// <summary>
    /// Try to resolve a registry key
    /// </summary>
    /// <param name="type"></param>
    /// <param name="key"></param>
    /// <returns></returns>
    protected bool TryResolveKey(Type type, out RegistryKey key)
    {
        key = _registry.Keys
            .Where(k => k.ObjectType == type || k.InterfaceType == type)
            .FirstOrDefault();

        return key != null;
    }
    /// <summary>
    /// Try to add a registry item
    /// </summary>
    /// <param name="key">The registry key</param>
    /// <param name="item">The registry item</param>
    /// <returns>TRUE if the item was added, FALSE otherwise</returns>
    protected bool TryAdd(RegistryKey key, TRegItem item)
    {
        return _registry.TryAdd(key, item);
    }
    /// <summary>
    /// Get a registry item
    /// </summary>
    /// <param name="key">The registry key</param>
    /// <returns>The registry item</returns>
    protected TRegItem Get(RegistryKey key)
    {
        return _registry[key];
    }

    /// <summary>
    /// The registry key for resolving registry items
    /// </summary>
    protected class RegistryKey
    {
        private readonly List<string> _tags = new();
        private readonly string _identifier;

        /// <summary>
        /// The interface type
        /// </summary>
        public Type InterfaceType { get; init; }
        /// <summary>
        /// The object type
        /// </summary>
        public Type ObjectType { get; init; }
        /// <summary>
        /// The registry item identifier
        /// </summary>
        public string Identifier => _identifier ?? ObjectType.Name;
        /// <summary>
        /// The registry item tags
        /// </summary>
        public IEnumerable<string> Tags => _tags;

        public RegistryKey(string identifier = null, params string[] tags)
        {
            _identifier = identifier;
            _tags.AddRange(tags);
        }

        #region overrides
        /// <summary>
        /// Determine object equality
        /// </summary>
        /// <param name="obj">The object to compare</param>
        /// <returns>TRUE if the objects are equal, FALSE otherwise</returns>
        public override bool Equals(object obj)
        {
            if (obj is RegistryKey key)
            {
                return key.InterfaceType == InterfaceType &&
                    key.ObjectType == ObjectType &&
                    key.Identifier == Identifier &&
                    key.Tags.SequenceEqual(Tags);
            }

            return false;
        }
        /// <summary>
        /// Get the object hash code
        /// </summary>
        /// <returns>The object hash code</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                var hash = 17;
                hash = hash * 23 + InterfaceType.GetHashCode();
                hash = hash * 23 + ObjectType.GetHashCode();
                hash = hash * 23 + Identifier.GetHashCode();

                foreach (var tag in Tags)
                {
                    hash = hash * 23 + tag.GetHashCode();
                }

                return hash;
            }
        }
        /// <summary>
        /// Get the object as a string
        /// </summary>
        /// <returns>The object as a string</returns>
        public override string ToString()
        {
            var tags = string.Join(", ", Tags);
            var tagStr = string.IsNullOrEmpty(tags) ? "" : $"[{tags}]";
            var idStr = InterfaceType.Name == Identifier ? "" : $" ({Identifier})";

            return $"{ObjectType.Name} : {InterfaceType.Name} {idStr} {tagStr}";
        }
        #endregion overrides
    }
}