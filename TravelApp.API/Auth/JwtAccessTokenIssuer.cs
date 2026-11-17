using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using TravelApp.BusinessLogic.Auth;
using TravelApp.BusinessLogic.Handlers.Accounts.Login;

namespace TravelApp.API.Auth
{
    /// <summary>Issues signed JWT access tokens for verified accounts.</summary>
    public sealed class JwtAccessTokenIssuer : IAccessTokenIssuer
    {
        private readonly SigningCredentials _signingCredentials;
        private readonly IConfiguration _configuration;

        /// <summary>Initializes a new JwtAccessTokenIssuer instance.</summary>
        public JwtAccessTokenIssuer(SigningCredentials signingCredentials, IConfiguration configuration)
        {
            _signingCredentials = signingCredentials;
            _configuration = configuration;
        }

        /// <inheritdoc />
        public LoginResponse Issue(VerifiedLoginAccount account)
        {
            List<Claim> claims =
            [
                new Claim(JwtRegisteredClaimNames.Sub, account.AccountId),
                new Claim(ClaimTypes.NameIdentifier, account.TravelUserId.ToString(CultureInfo.InvariantCulture)),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            ];

            foreach (string role in account.Roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            DateTimeOffset now = DateTimeOffset.UtcNow;
            DateTimeOffset expiresAt = now.AddMinutes(15);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                notBefore: now.UtcDateTime,
                expires: expiresAt.UtcDateTime,
                signingCredentials: _signingCredentials);

            return new LoginResponse
            {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
                ExpiresAtUtc = expiresAt
            };
        }
    }
}