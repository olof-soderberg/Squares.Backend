using System.Drawing;
using Squares.Domain.Helpers;
using Squares.Domain.Models;
using Squares.Domain.Repositories;

namespace Squares.Domain.Services;

public class SquareService : ISquareService
{
    private readonly ISquareRepository squareRepository;
    private readonly IColorGenerator colorGenerator;

    public SquareService(ISquareRepository squareRepository, IColorGenerator colorGenerator)
    {
        this.squareRepository = squareRepository;
        this.colorGenerator = colorGenerator;
    }

    public async ValueTask<IEnumerable<Square>> GetAllSquares(CancellationToken ct)
        => (await squareRepository.GetAllSquares(ct));

    public async ValueTask DeleteAllSquares(CancellationToken ct)
        => await squareRepository.DeleteAllSquares(ct);

    public IAsyncEnumerable<Square> GetAllSquaresAsyncStream(CancellationToken ct)
        => squareRepository.GetAllSquaresAsyncStream(ct);

    public async ValueTask<Square> SaveNewSquare(CancellationToken ct)
    {
        var position = 0;
        var color = colorGenerator.GetRandomColorHexString();

        var lastSquare = await squareRepository.GetLastSquare(ct);
        if(lastSquare != null)
        {
            position = lastSquare.Position + 1;

            while (lastSquare?.Color == color)
            {
                color = colorGenerator.GetRandomColorHexString();
            }
        }

        var square = new Square
        (
            Color: color,
            Position: position
        );

        var newSquare = await squareRepository.SaveNewSquare(square, ct);
        return newSquare;
    }
}
