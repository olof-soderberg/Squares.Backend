var builder = DistributedApplication.CreateBuilder(args);

var api = builder.AddProject<Projects.Squares_Api>("squares-api");
builder.AddNodeApp("react", "../squares.react")
    .WithReference(api)
    .WaitFor(api);

builder.Build().Run();
