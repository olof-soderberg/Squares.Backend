using Microsoft.Extensions.DependencyInjection;
using Squares.Api.DTOs;
using System.Text.Json;

var services = new ServiceCollection();
services.AddHttpClient();
var serviceProvider = services.BuildServiceProvider();
var clientFactory = serviceProvider.GetRequiredService<IHttpClientFactory>();

Console.WriteLine("Press any button to begin..");
Console.ReadLine();

var client = clientFactory.CreateClient();
var stream = await client.GetStreamAsync("http://localhost:7280/squares/stream");

var squares = JsonSerializer.DeserializeAsyncEnumerable<SquareDto>(stream, new JsonSerializerOptions
{
    PropertyNameCaseInsensitive = true,
});

await foreach (var square in squares)
{
    Console.WriteLine($"Square Position: {square.Position}, Color: {square.Color}");
}

Console.ReadLine();