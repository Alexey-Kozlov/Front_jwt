using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography.X509Certificates;
using Microsoft.IdentityModel.Tokens;
using System.Globalization;

namespace Front_jwt.Services
{
    public static class TokenGenerator
    {
        public static string CreateClientAuthJwt(string CertificatePath, string CertificatePassword, string AuthUrl)
        {
            //получаем запрос из приватного ключа сертификата. Публичный ключ зарегестрирован в 
            //классе ClientStore на сервере. Т.е. аутентификация клиента происходит по ключам сертификата
            var tokenHandler = new JwtSecurityTokenHandler { TokenLifetimeInMinutes = 1 };

            var securityToken = tokenHandler.CreateJwtSecurityToken(
                // iss совпадает с client_id
                issuer: "Test_jwt",
                // http://ws-pc-70:5001/connect/token
                audience: $"{AuthUrl}/connect/token",
                //audience: $"http://ws-pc-70:5007",
                // sub - это  client_id нашего приложения
                subject: new ClaimsIdentity(
                  new List<Claim> {
                      new Claim("sub", "Test_jwt"), 
                      //необходим всегда новый id для каждого обращения
                      new Claim("jti", Guid.NewGuid().ToString())
                  }),
                //время жизни токена - минутв
                expires: DateTime.UtcNow.AddSeconds(60),
                // подпись приватным ключом (RS256 для IdentityServer)
                signingCredentials: new SigningCredentials(
                  new X509SecurityKey(new X509Certificate2(CertificatePath, CertificatePassword)), "RS256")
            );
            return tokenHandler.WriteToken(securityToken);
        }
    }
}
