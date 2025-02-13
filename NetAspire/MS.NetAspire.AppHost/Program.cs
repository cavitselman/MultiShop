var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.MS_OcelotGateway>("ms-ocelotgateway");

builder.AddProject<Projects.MS_IdentityServer>("SelfHost");

builder.AddProject<Projects.MS_Catalog>("ms-catalog");

builder.AddProject<Projects.MS_Basket>("ms-basket");

builder.AddProject<Projects.MS_Discount>("ms-discount");

builder.AddProject<Projects.MS_Comment>("ms-comment");

builder.AddProject<Projects.MS_Order_WebApi>("ms-order-webapi");

builder.AddProject<Projects.MS_Message>("ms-message");

builder.AddProject<Projects.MS_Cargo_WebApi>("ms-cargo-webapi");

builder.AddProject<Projects.MS_SignalRRealTimeApi>("ms-signalrrealtimeapi");

builder.AddProject<Projects.MS_Images>("ms-images");

builder.Build().Run();
