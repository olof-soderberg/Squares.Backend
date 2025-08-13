using Squares.Domain.Models;

namespace Squares.Domain.Services;

public interface ISquareService
{
    ValueTask<IEnumerable<Square>> GetAllSquares(CancellationToken ct);
    IAsyncEnumerable<Square> GetAllSquaresAsyncStream(CancellationToken ct);
    ValueTask<Square> SaveNewSquare(CancellationToken ct);
    ValueTask DeleteAllSquares(CancellationToken ct);
}
