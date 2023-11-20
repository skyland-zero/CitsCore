var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.Cits_IdentityService_Api>("IdentityService");

builder.Build().Run();
