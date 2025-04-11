// DependencyInjection.cs
namespace CardBooster.API.Configuration;

using CardBooster.Core.Commands;
using CardBooster.Core.Commands.Authentication;
using CardBooster.Core.Commands.Boosters;
using CardBooster.Core.Models;
using CardBooster.Core.Queries;
using CardBooster.Core.Queries.Users;
using CardBooster.Core.Services;
using CardBooster.Infrastructure.Command.Authentication;
using CardBooster.Infrastructure.Database;
using CardBooster.Infrastructure.Handler;
using CardBooster.Infrastructure.Queries.Users;
using CardBooster.Infrastructure.Repository;
using CardBooster.Infrastructure.Security;
using CardBooster.Infrastructure.Security.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

public static class DependencyInjection
{
    public static IServiceCollection AddCardBoosterServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Configuration JWT
        services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));

        // Sécurité
        services.AddSingleton<IPasswordHasher, PasswordHasher>();
        services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();

        // Base de données
        services.AddSingleton<IAuthDb, AuthDb>();

        // Commands
        services.AddScoped<ICommandAsyncHandler<RegisterUserCommand>, RegisterUserCommandHandler>();
        services.AddScoped<ICommandAsyncHandler<LoginUserCommand>, LoginUserCommandHandler>();
        services.AddScoped<ICommandAsyncHandler<OpenBoosterCommand>, OpenBoosterCommandHandler>();

        // Queries
        services.AddScoped<IQueryAsyncHandler<GetUserByEmailQuery, User>, GetUserByEmailQueryHandler>();
        services.AddScoped<IQueryAsyncHandler<GetUserCardQuery, List<Cards>>, GetUserCardsQueryHandler>();

        // Configuration JWT Bearer
        var jwtSettings = configuration.GetSection("JwtSettings").Get<JwtSettings>();
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtSettings.Issuer,
                ValidAudience = jwtSettings.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey))
            };
        });

        return services;
    }
}