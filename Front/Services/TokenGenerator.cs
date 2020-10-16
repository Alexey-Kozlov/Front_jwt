using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography.X509Certificates;
using Microsoft.IdentityModel.Tokens;

namespace Front.Services
{
    public static class TokenGenerator
    {
        public static string CreateClientAuthJwt()
        {
            // set exp to 5 minutes
            var tokenHandler = new JwtSecurityTokenHandler { TokenLifetimeInMinutes = 5 };

            var securityToken = tokenHandler.CreateJwtSecurityToken(
                // iss must be the client_id of our application
                issuer: "Test_jwt",
                // aud must be the identity provider (token endpoint)
                audience: "https://ws-pc-70:5001/connect/token",
                // sub must be the client_id of our application
                subject: new ClaimsIdentity(
                  new List<Claim> { 
                      new Claim("sub", "Test_jwt"), 
                      new Claim("jti", Guid.NewGuid().ToString()) 
                  }),
                
                // sign with the private key (using RS256 for IdentityServer)
                signingCredentials: new SigningCredentials(
                  new X509SecurityKey(new X509Certificate2(@"C:\Certificates\WsCert.pfx")), "RS256")
            );

            return tokenHandler.WriteToken(securityToken);
        }
    }
}
