using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Security.Cryptography;
using TravelApp.API.Auth;
using TravelApp.BusinessLogic.Auth;
using TravelApp.DataAccess.Auth;
using TravelApp.DataAccess.SQLRepositories.EntityFramework;

namespace TravelApp.API.DependencyInjection
{
    /// <summary>Configures authentication and authorization for TravelApp.</summary>
    public static class AuthenticationExtensions
    {
        /// <summary>Registers local account verification and JWT authentication.</summary>
        /// <param name="builder">The builder used to configure the API.</param>
        public static void AddTravelAppAuthentication(this WebApplicationBuilder builder)
        {
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddScoped<ICurrentUser, CurrentUser>();

            string connectionString = builder.Configuration["ConnectionStrings:TravelDb"] ?? throw new InvalidOperationException("ConnectionStrings:TravelDb mangler.");

            builder.Services.AddDbContext<TravelAppContext>(options => options.UseSqlServer(connectionString));

            builder.Services.AddScoped<IPasswordHasher<UserAccount>, PasswordHasher<UserAccount>>();
            builder.Services.AddScoped<ILoginAccountVerifier, SqlLoginAccountVerifier>();
            builder.Services.AddScoped<IAccessTokenIssuer, JwtAccessTokenIssuer>();

            if (builder.Environment.IsDevelopment())
            {
                builder.Configuration["Jwt:Issuer"] ??= "TravelApp.API";
                builder.Configuration["Jwt:Audience"] ??= "TravelApp.Client";
                builder.Configuration["Jwt:SigningKey"] ??= Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));
            }

            string jwtIssuer = builder.Configuration["Jwt:Issuer"] ?? throw new InvalidOperationException("Jwt:Issuer mangler.");

            string jwtAudience = builder.Configuration["Jwt:Audience"] ?? throw new InvalidOperationException("Jwt:Audience mangler.");

            string encodedSigningKey = builder.Configuration["Jwt:SigningKey"] ?? throw new InvalidOperationException("Jwt:SigningKey mangler.");

            byte[] signingKeyBytes = Convert.FromBase64String(encodedSigningKey);

            if (signingKeyBytes.Length < 32)
            {
                throw new InvalidOperationException("JWT-signeringsnøglen skal være mindst 32 bytes.");
            }

            var signingKey = new SymmetricSecurityKey(signingKeyBytes);

            builder.Services.AddSingleton(new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256));

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
                {
                    options.MapInboundClaims = false;

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = jwtIssuer,
                        ValidateAudience = true,
                        ValidAudience = jwtAudience,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = signingKey,
                        ValidAlgorithms = [SecurityAlgorithms.HmacSha256],
                        ValidateLifetime = true,
                        RequireExpirationTime = true,
                        RequireSignedTokens = true,
                        RoleClaimType = ClaimTypes.Role,
                        ClockSkew = TimeSpan.FromSeconds(30)
                    };
                });

            builder.Services.AddAuthorization();
        }
    }
}