using Microsoft.Extensions.DependencyInjection;
using Squares.Api.Controllers;
using Squares.Domain.Helpers;
using Squares.Domain.Repositories;
using Squares.Domain.Services;
using Squares.Infrastructure.Repositories;

var services = new ServiceCollection();
services.AddScoped<ISquareService, SquareService>();
services.AddSingleton<ISquareRepository, SquareJsonRepository>();
services.AddScoped<IColorGenerator, ColorGenerator>();
services.AddLogging();

var serviceProvider = services.BuildServiceProvider();

var controller = new SquaresController(serviceProvider.GetService<ISquareService>());

Console.WriteLine("Press any button to begin..");
Console.ReadLine();

await foreach(var square in controller.GetSquaresAsyncStream(default))
{
    Console.WriteLine($"Square Color: {square.Color}");
}


Console.ReadLine();