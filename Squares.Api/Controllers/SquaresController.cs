using Microsoft.AspNetCore.Mvc;
using Squares.Api.DTOs;
using Squares.Domain.Services;
using System.Runtime.CompilerServices;

namespace Squares.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class SquaresController : ControllerBase
{
    private ISquareService _repository;

    public SquaresController(ISquareService repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<SquareDto>>> GetSquares(CancellationToken ct)
    {
        var squares = await _repository.GetAllSquares(ct);
        var squareDtos = squares.Select(s => new SquareDto(s.Color, s.Position));
        return Ok(squareDtos);
    }

    [HttpGet("stream")]
    public async IAsyncEnumerable<SquareDto> GetSquaresAsyncStream([EnumeratorCancellation] CancellationToken ct)
    {
        await foreach (var square in _repository.GetAllSquaresAsyncStream(ct))
        {
            await Task.Delay(500, ct); // Simulate some delay
            yield return new SquareDto(square.Color, square.Position);
        }
    }

    [HttpPost]
    public async Task<ActionResult<SquareDto>> SaveNewSquare(CancellationToken ct)
    {
        var square = await _repository.SaveNewSquare(ct);
        var squareDto = new SquareDto(square.Color, square.Position);
        return Ok(squareDto);
    }

    [HttpDelete]
    public async Task<IActionResult> ClearAllSquares(CancellationToken ct)
    {
        await _repository.DeleteAllSquares(ct);
        return NoContent();
    }
}
