var builder = DistributedApplication.CreateBuilder(args);

var apiService = builder.AddProject<Projects.AspireAppEmp_ApiService>("apiservice");

builder.AddProject<Projects.AspireAppEmp_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(apiService);

builder.Build().Run();
