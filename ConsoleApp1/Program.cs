// See https://aka.ms/new-console-template for more information
using Squares.Api.Controllers;
using Squares.Domain.Services;
using Squares.Infrastructure.Repositories;

var controller = new SquaresController(new SquareService(new SquareRepository()));

Console.WriteLine("Press any button to begin..");
Console.ReadLine();

await foreach(var square in controller.GetSquaresAsyncStream(default))
{
    Console.WriteLine($"Square Color: {square.Color}");
}


Console.ReadLine();