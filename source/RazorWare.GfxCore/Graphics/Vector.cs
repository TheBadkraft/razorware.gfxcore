using Vec2 = OpenTK.Mathematics.Vector2;

namespace RazorWare.GfxCore.Graphics;

/// <summary>
/// A 2D vector
/// </summary>
public struct Vector : IEquatable<Vector>
{
    /// <summary>
    /// The X component of the vector
    /// </summary>
    public float X { get; init; }
    /// <summary>
    /// The Y component of the vector
    /// </summary>
    public float Y { get; init; }

    /// <summary>
    /// Creates a new instance of <see cref="RazorWare.GfxCore.Graphics.Vector"/>
    /// </summary>
    /// <param name="x">The x component</param>
    /// <param name="y">The y component</param>
    public Vector(float x, float y)
    {
        X = x;
        Y = y;
    }

    /// <summary>
    /// Converts a tuple to a <see cref="RazorWare.GfxCore.Graphics.Vector"/>
    /// </summary>
    /// <param name="vector">The vector to add</param>
    /// <returns>The sum of the two vectors</returns>
    public static implicit operator Vector((float x, float y) vector) => new(vector.x, vector.y);
    /// <summary>
    /// Converts an <see cref="OpenTK.Mathematics.Vector2"/> to a <see cref="RazorWare.GfxCore.Graphics.Vector"/>
    /// </summary>
    /// <param name="vector">The vector to convert</param>
    /// <returns>The converted vector</returns>
    public static implicit operator Vector(Vec2 vector) => new(vector.X, vector.Y);
    /// <summary>
    /// Converts a <see cref="RazorWare.GfxCore.Graphics.Vector"/> to an <see cref="OpenTK.Mathematics.Vector2"/>
    /// </summary>
    /// <param name="vector">The vector to convert</param>
    /// <returns>The converted vector</returns>
    public static implicit operator Vec2(Vector vector) => new(vector.X, vector.Y);
    /// <summary>
    /// Adds two vectors together
    /// </summary>
    /// <param name="v1">The first vector</param>
    /// <param name="v2">The second vector</param>
    /// <returns>The sum of the two vectors</returns>
    public static Vector operator +(Vector v1, Vector v2) => new(v1.X + v2.X, v1.Y + v2.Y);
    /// <summary>
    /// Subtracts two vectors
    /// </summary>
    /// <param name="v1">The first vector</param>
    /// <param name="v2">The second vector</param>
    /// <returns>The difference of the two vectors</returns>
    public static Vector operator -(Vector v1, Vector v2) => new(v1.X - v2.X, v1.Y - v2.Y);
    /// <summary>
    /// Multiplies a vector by a scalar
    /// </summary>
    /// <param name="v">The vector</param>
    /// <param name="scalar">The scalar</param>
    /// <returns>The product of the vector and the scalar</returns>
    public static Vector operator *(Vector v, float scalar) => Scale(v, scalar);

    /*
       TODO: make these extension methods
   */
    /// <summary>
    /// Calculates the magnitude the vector
    /// </summary>
    /// <param name="v">The vector</param>
    /// <returns>The magnitude of the vector</returns>
    public static float Magnitude(Vector v) => (float)Math.Sqrt(v.X * v.X + v.Y * v.Y);
    /// <summary>
    /// Calculates the dot product of two vectors
    /// </summary>
    /// <param name="v1">The first vector</param>
    /// <param name="v2">The second vector</param>
    /// <returns>The dot product of the two vectors</returns>
    public static float Dot(Vector v1, Vector v2) => v1.X * v2.X + v1.Y * v2.Y;
    /// <summary>
    /// Calculates the dot product of a vector with itself
    /// </summary>
    /// <param name="v">The vector</param>
    /// <returns>The dot product of the vector with itself</returns>
    public static float Dot(Vector v) => Dot(v, v);
    /// <summary>
    /// Normalizes a vector
    /// </summary>
    /// <param name="v">The vector</param>
    /// <returns>The normalized vector</returns>
    public static Vector Normalize(Vector v) => v * (1f / Magnitude(v));
    /// <summary>
    /// Scales a vector by a scalar
    /// </summary>
    /// <param name="v">The vector</param>
    /// <param name="scalar">The scalar</param>
    /// <returns>The scaled vector</returns>
    public static Vector Scale(Vector v, float scalar) => new(v.X * scalar, v.Y * scalar);

    public bool Equals(Vector other) => X == other.X && Y == other.Y;
    public override bool Equals(object? obj) => obj is Vector other && Equals(other);
    public override int GetHashCode() => HashCode.Combine(X, Y);
    public override string ToString() => $"({X}, {Y})";
}
