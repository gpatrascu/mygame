// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.Models;
using System.Collections.Generic;
using System.Security.Claims;
using IdentityServer4;
using IdentityServer4.Test;

namespace Games.IdentityProvider
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> IdentityResources =>
            new IdentityResource[]
            { 
                new IdentityResources.OpenId(),
                new IdentityResources.Email(),
                new IdentityResources.Profile(),
                new IdentityResources.Phone(),
                new IdentityResources.Address()
            };

        public static IEnumerable<ApiScope> ApiScopes =>
            new []
            {
                new ApiScope("mygame", "My Game"),
            };

        public static IEnumerable<Client> Clients =>
            new[]
            {
                new Client()
                {
                    ClientId = "mygame",
                    ClientName = "My Game",
                    ClientSecrets = new [] {new Secret("secret".Sha256())},
                    AllowedGrantTypes = new[] { GrantType.AuthorizationCode, GrantType.ClientCredentials,
                        GrantType.ResourceOwnerPassword },
                    RequireClientSecret = false,
                    RedirectUris =           { "http://localhost:3000/signin-oidc" },
                    PostLogoutRedirectUris = { "http://localhost:3000/signout-oidc", "http://localhost:3000" },
                    AllowedCorsOrigins =     { "http://localhost:3000" },
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Email,
                        "mygame"
                    },
                    AlwaysSendClientClaims = true,
                },
            };

        public static IEnumerable<ApiResource> ApiResources => new ApiResource[]
        {
            new ApiResource("mygame", "My Game"), 
        };

        public static IEnumerable<TestUser> Users => new[]
        {
            CreateTestUser("george.patrascu@yahoo.com", "George Patrascu"),
            CreateTestUser("valentina.patrascu@yahoo.com", "Valentina Patrascu"),
            CreateTestUser("grogu.patrascu@yahoo.com", "Grogu Patrascu"),
            CreateTestUser("georgiana.moldovan@yahoo.com", "Georgiana Moldovan"),
            CreateTestUser("iosif.moldovan@yahoo.com", "Iosif Moldovan"),
        };

        private static TestUser CreateTestUser(string mail, string name)
        {
            return new TestUser()
            {
                SubjectId = mail,
                Username = mail,
                Password = "password", 
                
                Claims = new List<Claim>()
                {
                    new ("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name", name)
                }
            };
        }
    }
}