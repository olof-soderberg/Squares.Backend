using System.Drawing;
using Squares.Domain.Models;
using Squares.Domain.Repositories;

namespace Squares.Domain.Services;

public class SquareService : ISquareService
{
    private static readonly Random rand = new Random();
    private const int rgbMaxValue = 255;
    private ISquareRepository squareRepository;

    public SquareService(ISquareRepository squareRepository)
    {
        this.squareRepository = squareRepository;
    }

    public async ValueTask<IEnumerable<Square>> GetAllSquares(CancellationToken ct)
        => (await squareRepository.GetAllSquares(ct));

    public async ValueTask DeleteAllSquares(CancellationToken ct)
        => await squareRepository.DeleteAllSquares(ct);

    public IAsyncEnumerable<Square> GetAllSquaresAsyncStream(CancellationToken ct)
        => squareRepository.GetAllSquaresAsyncStream(ct);

    public async ValueTask<Square> SaveNewSquare(CancellationToken ct)
    {
        try
        {
            var squares = await squareRepository.GetAllSquares(ct);
            var lastSquare = squares.OrderByDescending(x => x.Position).FirstOrDefault();

            var color = GetRandomColorHexString();

            while(lastSquare?.Color == color)
            {
                color = GetRandomColorHexString();
            }

            var square = new Square
            (
                Color: color,
                Position: squares.Count()
            );

            var newSquare = await squareRepository.SaveNewSquare(square, ct);
            return newSquare;
        }
        catch (Exception ex) when (ex is not OperationCanceledException)
        {
            // Log exception here if you have a logger
            throw new InvalidOperationException($"Error saving new square: {ex.Message}", ex);
        }
    }

    private string GetRandomColorHexString()
    {
        var color = Color.FromArgb(rand.Next(rgbMaxValue), rand.Next(rgbMaxValue), rand.Next(rgbMaxValue));
        return ColorTranslator.ToHtml(color);
    }
}
