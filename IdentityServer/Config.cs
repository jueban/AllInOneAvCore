using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;

namespace IdentityServer
{
    public static class Config
    {
        public static List<TestUser> Users
        {
            get
            {
                var address = new
                {
                    street_address = "One Hacker Way",
                    locality = "Heidelberg",
                    postal_code = 69118,
                    country = "Germany"
                };

                return new List<TestUser>
                {
                  new TestUser
                  {
                    SubjectId = "007",
                    Username = "admin",
                    Password = "admin",
                    Claims =
                    {
                      new Claim(JwtClaimTypes.Name, "Admin"),
                      new Claim(JwtClaimTypes.GivenName, "Admin"),
                      new Claim(JwtClaimTypes.FamilyName, "Admin"),
                      new Claim(JwtClaimTypes.Email, "Admin@Admin.com"),
                      new Claim(JwtClaimTypes.EmailVerified, "true", ClaimValueTypes.Boolean),
                      new Claim(JwtClaimTypes.Role, "admin"),
                      new Claim(JwtClaimTypes.WebSite, "http://Admin.com"),
                      new Claim(JwtClaimTypes.Address, JsonSerializer.Serialize(address),
                        IdentityServerConstants.ClaimValueTypes.Json)
                    }
                  }
                };
            }
        }

        public static IEnumerable<IdentityResource> IdentityResources =>
          new[]
          {
        new IdentityResources.OpenId(),
        new IdentityResources.Profile(),
        new IdentityResource
        {
          Name = "role",
          UserClaims = new List<string> {"role"}
        }
          };

        public static IEnumerable<ApiScope> ApiScopes =>
          new[]
          {
        new ApiScope("api.read"),
        new ApiScope("api.write"),
          };
        public static IEnumerable<ApiResource> ApiResources => new[]
        {
      new ApiResource("api")
      {
        Scopes = new List<string> {"api.read", "api.write"},
        ApiSecrets = new List<Secret> {new Secret("ScopeSecret".Sha256())},
        UserClaims = new List<string> {"role"}
      }
    };

        public static IEnumerable<Client> Clients =>
          new[]
          {
        new Client
        {
          ClientId = "hangfire",
          ClientName = "hangfire",

          AllowedGrantTypes = GrantTypes.ClientCredentials,
          ClientSecrets = {new Secret("pa$$w0rd".Sha256())},

          AllowedScopes = {"api.read", "api.write"}
        },

        new Client
        {
          ClientId = "mvc",
          ClientSecrets = {new Secret("pa$$w0rd".Sha256())},

          AllowedGrantTypes = GrantTypes.Code,

          RedirectUris = {"https://localhost:20020/signin-oidc"},
          FrontChannelLogoutUri = "https://localhost:20020/signout-oidc",
          PostLogoutRedirectUris = {"https://localhost:20020/signout-callback-oidc"},

          AllowOfflineAccess = true,
          AllowedScopes = {"openid", "profile", "api.read"},
          RequirePkce = true,
          RequireConsent = true,
          AllowPlainTextPkce = false
        },
          };
    }
}
