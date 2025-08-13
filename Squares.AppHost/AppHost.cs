var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.Squares_Api>("squares-api");

builder.Build().Run();
