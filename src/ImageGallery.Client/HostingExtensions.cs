using System.IdentityModel.Tokens.Jwt;
using IdentityModel;
using ImageGallery.Client.HttpHandlers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
namespace ImageGallery.Client;

public static class HostingExtensions
{
    public static void ConfigureServices(this WebApplicationBuilder builder)
    {
        JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

        builder.Services.AddControllersWithViews()
             .AddJsonOptions(opts => opts.JsonSerializerOptions.PropertyNamingPolicy = null);

        builder.Services.AddAuthorization(authorizationOptions =>
        {
            authorizationOptions.AddPolicy(
                "CanOrderFrame",
                policyBuilder =>
                {
                    policyBuilder.RequireAuthenticatedUser();
                    policyBuilder.RequireClaim("country", "be");
                    policyBuilder.RequireClaim("subscriptionlevel", "PayingUser");
                });
        });

        builder.Services.AddHttpContextAccessor();

        builder.Services.AddTransient<BearerTokenHandler>();

        // create an HttpClient used for accessing the API
        builder.Services.AddHttpClient("APIClient", client =>
        {
            client.BaseAddress = new Uri("https://localhost:44366/");
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add(HeaderNames.Accept, "application/json");
        }).AddHttpMessageHandler<BearerTokenHandler>();

        // create an HttpClient used for accessing the IDP
        builder.Services.AddHttpClient("IDPClient", client =>
        {
            client.BaseAddress = new Uri("https://localhost:44318/");
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add(HeaderNames.Accept, "application/json");
        });

        builder.Services.AddAuthentication(options =>
        {
            options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
        })
        .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
        {
            options.AccessDeniedPath = "/Authorization/AccessDenied";
        })
        .AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, options =>
        {
            // See https://github.com/dotnet/aspnetcore/blob/v6.0.2/src/Security/Authentication/OpenIdConnect/src/OpenIdConnectOptions.cs for default options
            options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            options.Authority = "https://localhost:44318";
            options.ClientId = "imagegalleryclient";
            options.ResponseType = "code";
            options.Scope.Add("address");
            options.Scope.Add("roles");
            options.Scope.Add("imagegalleryapi.read");
            options.Scope.Add("subscriptionlevel");
            options.Scope.Add("country");
            options.ClaimActions.DeleteClaim("sid");
            options.ClaimActions.DeleteClaim("idp");
            options.ClaimActions.DeleteClaim("auth_time");
            options.ClaimActions.MapUniqueJsonKey("role", "role");
            options.ClaimActions.MapUniqueJsonKey("subscriptionlevel", "subscriptionlevel");
            options.ClaimActions.MapUniqueJsonKey("country", "country");
            options.SaveTokens = true;
            options.ClientSecret = "secret";
            options.GetClaimsFromUserInfoEndpoint = true;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                NameClaimType = JwtClaimTypes.GivenName,
                RoleClaimType = JwtClaimTypes.Role
            };
        });
    }
}
