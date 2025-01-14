using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace WebApiApp.Helpers
{
    public class JwtHelper
    {
        private readonly IConfiguration _config;

        public JwtHelper(IConfiguration configuration)
        {
            _config = configuration;
        }

        public string CreateAccessToken(string sub)
        {
            return EncodeToken(
                tokenType: "access",
                lifetime: _config.GetValue<int>("JWT:ACCESS_TOKEN_LIFETIME"),
                subject: sub
            );
        }

        public string CreateRefreshToken(string sub)
        {
            return EncodeToken(
                tokenType: "refresh",
                lifetime: _config.GetValue<int>("JWT:REFRESH_TOKEN_LIFETIME"),
                subject: sub
            );
        }

        private string EncodeToken(string tokenType, int lifetime, string subject)
        {
            var sysNow = TimeHelper.Now();
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, subject),
                new Claim("type", tokenType),
                new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString())
            };
            var userClaimsIdentity = new ClaimsIdentity(claims);

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetValue<string>("JWT:SECRET_KEY")));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = userClaimsIdentity,
                Expires = sysNow.AddMinutes(lifetime),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}