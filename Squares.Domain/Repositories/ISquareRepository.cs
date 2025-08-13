using Squares.Domain.Models;

namespace Squares.Domain.Repositories;

public interface ISquareRepository
{
    ValueTask<IEnumerable<Square>> GetAllSquares(CancellationToken ct);
    ValueTask<Square> GetLastSquare(CancellationToken ct);
    IAsyncEnumerable<Square> GetAllSquaresAsyncStream(CancellationToken ct);
    ValueTask<Square> SaveNewSquare(Square square, CancellationToken ct);
    ValueTask DeleteAllSquares(CancellationToken ct);
}
