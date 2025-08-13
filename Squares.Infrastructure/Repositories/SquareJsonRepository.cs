using Squares.Domain.Models;
using Squares.Domain.Repositories;
using System.Runtime.CompilerServices;
using System.Text.Json;

namespace Squares.Infrastructure.Repositories;

public class SquareJsonRepository : ISquareRepository
{
    private const string filePath = "squares.json";
    private readonly List<Square> squares = new List<Square>();

    public SquareJsonRepository()
    {
        if (File.Exists(filePath))
        {
            try
            {
                var json = File.ReadAllText(filePath);
                var deserializedSquares = JsonSerializer.Deserialize<List<Square>>(json);
                if(deserializedSquares is not null)
                {
                    squares = deserializedSquares;
                };
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error loading squares: {ex.Message}");
            }
        }
    }

    public async ValueTask<IEnumerable<Square>> GetAllSquares(CancellationToken ct)
    {
        using FileStream stream = File.OpenRead(filePath);
        return await JsonSerializer.DeserializeAsync<List<Square>>(stream, cancellationToken: ct) ?? Enumerable.Empty<Square>();
    }

    public ValueTask<Square> GetLastSquare(CancellationToken ct)
        => ValueTask.FromResult(squares.OrderByDescending(x => x.Position).First());

    public async IAsyncEnumerable<Square> GetAllSquaresAsyncStream([EnumeratorCancellation] CancellationToken ct)
    {
        if (!File.Exists("squares.json"))
            yield break;

        await using var stream = File.OpenRead("squares.json");
        var squaresEnumerable = JsonSerializer.DeserializeAsyncEnumerable<Square>(stream, cancellationToken: ct);
        if (squaresEnumerable != null)
        {
            await foreach (var square in squaresEnumerable)
            {
                ct.ThrowIfCancellationRequested();
                if (square != null)
                {
                    yield return square;
                }
            }
        }
    }

    public async ValueTask<Square> SaveNewSquare(Square square, CancellationToken ct)
    {
        squares.Add(new Square
        (
            Color: square.Color,
            Position: square.Position
        ));

        await using FileStream stream = File.Create(filePath);
        await JsonSerializer.SerializeAsync(stream, squares, cancellationToken: ct);
        return square;
    }

    public async ValueTask DeleteAllSquares(CancellationToken ct)
    {
        await using FileStream stream = File.Create(filePath);
        await JsonSerializer.SerializeAsync(stream, Enumerable.Empty<Square>(), cancellationToken: ct);
    }
}
