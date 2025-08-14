namespace Squares.Api.DTOs;

/// <summary>
/// Represents a square with a color and position.
/// </summary>
public sealed record SquareDto
{
    /// <summary>
    /// Gets the color of the square.
    /// </summary>
    /// <example>#FF1177</example>
    public string Color { get; init; }

    /// <summary>
    /// Gets the position of the square.
    /// </summary>
    /// <example>1</example>
    public int Position { get; init; }

    /// <summary>
    /// Initializes a new instance of the <see cref="SquareDto"/> record.
    /// </summary>
    /// <param name="Color">The color of the square.</param>
    /// <param name="Position">The position of the square.</param>
    public SquareDto(string Color, int Position)
    {
        this.Color = Color;
        this.Position = Position;
    }
}