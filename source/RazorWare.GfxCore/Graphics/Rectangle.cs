
namespace RazorWare.GfxCore.Graphics;

/// <summary>
/// A rectangle.
/// </summary>
public struct Rectangle
{
    /// <summary>
    /// The location of the rectangle.
    /// </summary>
    public Vector Location { get; init; }
    /// <summary>
    /// The size of the rectangle.
    /// </summary>
    public Vector Size { get; init; }
    /// <summary>
    /// The X-coordinate of the rectangle.
    /// </summary>
    public float X => Location.X;
    /// <summary>
    /// The Y-coordinate of the rectangle.
    /// </summary>
    public float Y => Location.Y;
    /// <summary>
    /// The width of the rectangle.
    /// </summary>
    public float Width => Size.X;
    /// <summary>
    /// The height of the rectangle.
    /// </summary>
    public float Height => Size.Y;

    /// <summary>
    /// Creates a new rectangle.
    /// </summary>
    /// <param name="x">The x-coordinate of the rectangle.</param>
    /// <param name="y">The y-coordinate of the rectangle.</param>
    /// <param name="width">The width of the rectangle.</param>
    /// <param name="height">The height of the rectangle.</param>
    public Rectangle(float x, float y, float width, float height)
        : this(new Vector(x, y), new Vector(width, height)) { }
    /// <summary>
    /// Creates a new rectangle.
    /// </summary>
    /// <param name="location">The location of the rectangle.</param>
    /// <param name="size">The size of the rectangle.</param>
    public Rectangle(Vector location, Vector size)
    {
        Location = location;
        Size = size;
    }

    /// <summary>
    /// Converts a <see cref="Vector"/> tuple to a <see cref="Rectangle"/>.
    /// </summary>
    /// <param name="rect">The rectangle.</param>
    public static implicit operator Rectangle((Vector location, Vector size) rect)
        => new Rectangle(rect.location, rect.size);
}
