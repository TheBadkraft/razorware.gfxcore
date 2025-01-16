
namespace RazorWare.GfxCore.Registries;

public class RegistryManager : GfxRegistry<IRegistry>
{
    public RegistryManager() : base("Registries") { }


    public T Resolve<T>() where T : class, IRegistry
    {
        T registry = default;

        if (TryResolveKey(typeof(T), out var key))
        {
            registry = Get(key) as T;
        }
        else
        {
            throw new ArgumentException($"Registry key [{key}] not found.");
        }

        return registry;
    }
}
