
namespace RazorWare.GfxCore.Extensibility;

/// <summary>
/// A singleton class instance similar to <see cref="System.Lazy{T}"/>. 
/// </summary>
/// <typeparam name="T">The type of the singleton instance.</typeparam>
public class Singleton<T> where T : class
{
    private static object _lock = new object();

    private readonly Func<T> _factory;
    private T _instance;

    public T Value
    {
        get
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = _factory();
                    }
                }
            }

            return _instance;
        }
    }

    public Singleton(Func<T> factory)
    {
        _factory = factory;
    }
}
