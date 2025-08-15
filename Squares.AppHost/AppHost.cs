var builder = DistributedApplication.CreateBuilder(args);

var api = builder.AddProject<Projects.Squares_Api>("squares-api");

builder.Build().Run();
