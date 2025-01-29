var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.MS_OcelotGateway>("http");

builder.AddProject<Projects.MS_IdentityServer>("SelfHost");

builder.AddProject<Projects.MS_Catalog>("ms-catalog");

builder.AddProject<Projects.MS_Basket>("ms-basket");

builder.AddProject<Projects.MS_Discount>("ms-discount");

builder.AddProject<Projects.MS_Comment>("ms-comment");

builder.Build().Run();
