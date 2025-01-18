using System;

namespace RazorWare.GfxCore.Extensibility;

public interface IGfxService
{
    string Name { get; }
}

public interface IGfxSystem
{
    string Name { get; }
}

public interface IGfxResource
{
    string Name { get; }
}

public interface IGfxEventSource
{
    string Name { get; }
}

public interface IGfxCommandTarget
{
    string Name { get; }
}