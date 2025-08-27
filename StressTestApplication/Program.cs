using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;

var services = new ServiceCollection();
services.AddHttpClient();
var serviceProvider = services.BuildServiceProvider();
var clientFactory = serviceProvider.GetRequiredService<IHttpClientFactory>();

Console.WriteLine("Press any button to begin..");
Console.ReadLine();

var stopwatch = Stopwatch.StartNew();

var iterations = 100;
List<Task> tasks = new();

for (int i = 0; i < iterations; i++)
{
    tasks.Add(DoWork(i));
}

await Task.WhenAll(tasks);

async Task DoWork(int i)
{
    HttpClient client = clientFactory.CreateClient();
    await client.GetAsync("http://localhost:7280/squares");
    await client.PostAsync("http://localhost:7280/squares", default);
    Console.WriteLine(i);
}

stopwatch.Stop();
Console.WriteLine($"Completed {iterations} iterations in {stopwatch.ElapsedMilliseconds} ms");
Console.ReadLine();