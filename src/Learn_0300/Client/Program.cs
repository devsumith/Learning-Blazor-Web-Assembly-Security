using Microsoft.Extensions.DependencyInjection;

var builder =
	Microsoft.AspNetCore.Components.WebAssembly
	.Hosting.WebAssemblyHostBuilder.CreateDefault(args: args);

builder.RootComponents.Add<Client.App>(selector: "#app");

builder.RootComponents.Add<Microsoft.AspNetCore
	.Components.Web.HeadOutlet>(selector: "head::after");

// New
builder.Services.AddOptions();

// New
// If we do not have Policy!
//builder.Services.AddAuthorizationCore();

// New
// If we have Policy!
builder.Services.AddAuthorizationCore(options =>
{
	options.AddPolicy(name: "CanBuy",
		configurePolicy: policy => policy.RequireClaim(claimType: "Over21"));

	options.AddPolicy(name: "CanDelete",
		configurePolicy: policy => policy.RequireRole(roles: "Administrator"));
});

// New
builder.Services.AddScoped
	<Microsoft.AspNetCore.Components
	.Authorization.AuthenticationStateProvider,
	Infrastructure.CustomAuthenticationStateProvider>();

builder.Services.AddScoped
	(sp => new System.Net.Http.HttpClient
	{
		BaseAddress = new System.Uri
			(uriString: builder.HostEnvironment.BaseAddress),
	});

await builder.Build().RunAsync();