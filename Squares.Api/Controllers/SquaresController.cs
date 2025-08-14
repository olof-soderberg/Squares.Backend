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

    /// <summary>
    /// Fetches squares from memory
    /// </summary>
    /// <param name="ct"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<SquareDto>>> GetSquares(CancellationToken ct)
    {
        var squares = await _repository.GetAllSquares(ct);
        var squareDtos = squares.Select(s => new SquareDto(s.Color, s.Position));
        return Ok(squareDtos);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="ct"></param>
    /// <returns></returns>
    [HttpGet("stream")]
    public async IAsyncEnumerable<SquareDto> GetSquaresAsyncStream([EnumeratorCancellation] CancellationToken ct)
    {
        await foreach (var square in _repository.GetAllSquaresAsyncStream(ct))
        {
            await Task.Delay(500, ct); // Simulate some delay
            yield return new SquareDto(square.Color, square.Position);
        }
    }

    /// <summary>
    /// Creates and returns a new square with different color from the last
    /// </summary>
    /// <param name="ct"></param>
    /// <returns></returns>
    [HttpPost]
    public async ValueTask<ActionResult<SquareDto>> SaveNewSquare(CancellationToken ct)
    {
        var square = await _repository.SaveNewSquare(ct);
        var squareDto = new SquareDto(square.Color, square.Position);
        return Ok(squareDto);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="ct"></param>
    /// <returns></returns>
    [HttpDelete]
    public async ValueTask<IActionResult> ClearAllSquares(CancellationToken ct)
    {
        await _repository.DeleteAllSquares(ct);
        return NoContent();
    }
}
