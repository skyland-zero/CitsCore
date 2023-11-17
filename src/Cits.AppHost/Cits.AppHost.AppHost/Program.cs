var builder = DistributedApplication.CreateBuilder(args);



builder.AddProject<Projects.Cits_IdentityService_Api>("cits.identityservice.api");



builder.Build().Run();
