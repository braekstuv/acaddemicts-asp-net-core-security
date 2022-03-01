// Copyright (c) Duende Software. All rights reserved.
// See LICENSE in the project root for license information.


using System.Security.Claims;
using Duende.IdentityServer.Test;

namespace Kaika.IDP;

public class TestUsers
{
    public static List<TestUser> Users = new List<TestUser>
    {
        new TestUser{
            SubjectId = "b6f7a7b1-f403-4434-8dac-8495ddd1f36e",
            Username = "Frank",
            Password = "password",
            Claims = new List<Claim>{
                new Claim("given_name", "Frank"),
                new Claim("family_name", "Underwood"),
                new Claim("address", "Main Road 1"),
            }
        },
        new TestUser{
            SubjectId = "b99748a7-ee9c-4e05-8c71-784dda2d2e87",
            Username = "Claire",
            Password = "password",
            Claims = new List<Claim>{
                new Claim("given_name", "Claire"),
                new Claim("family_name", "Underwood"),
                new Claim("address", "Big Street 2"),
            }
        },
    };
}
