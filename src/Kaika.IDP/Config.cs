using Duende.IdentityServer;
using Duende.IdentityServer.Models;

namespace Kaika.IDP;

public static class Config
{
    public static IEnumerable<IdentityResource> IdentityResources =>
        new IdentityResource[]
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
            new IdentityResources.Address(),
            new IdentityResource(
                "roles",
                "Your role(s)",
                new List<string>() { "role" }
            ),
            new IdentityResource(
                "country",
                "The country you're living in",
                new List<string>() { "country" }
            ),
            new IdentityResource(
                "subscriptionlevel",
                "Your subscription level",
                new List<string>() { "subscriptionlevel" }
            )
        };

    public static IEnumerable<ApiResource> ApiResources =>
        new ApiResource[]
            {
                new ApiResource("imagegalleryapi", "Image Gallery API")
                {
                    UserClaims = { "role" },
                    Scopes = { "imagegalleryapi.read" }
                }
            };

    public static IEnumerable<ApiScope> ApiScopes =>
        new ApiScope[]
            {
                new ApiScope("imagegalleryapi.read", "Read Image Gallery API")
            };

    public static IEnumerable<Client> Clients =>
        new Client[]
            {
                new Client
                {
                    ClientName = "Image Gallery",
                    ClientId = "imagegalleryclient",
                    // setting option to true could lead to issues where id token exceeds browser uri length limit
                    // AlwaysIncludeUserClaimsInIdToken = true,
                    AllowedGrantTypes = GrantTypes.Code,
                    RequirePkce = true,
                    RedirectUris = new List<string>
                    {
                        "https://localhost:44389/signin-oidc"
                    },
                    PostLogoutRedirectUris = new List<string>
                    {
                        "https://localhost:44389/signout-callback-oidc"
                    },
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Address,
                        "roles",
                        "imagegalleryapi.read",
                        "country",
                        "subscriptionlevel"
                    },
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },
                    RequireConsent = true
                }
            };
}
