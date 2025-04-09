//using CardBooster.Core.Commands;
//using CardBooster.Core.Commands.Authentication;
//using CardBooster.Core.Commands.Boosters;
//using CardBooster.Core.Models;
//using CardBooster.Core.Queries;
//using CardBooster.Core.Queries.Users;
//using CardBooster.Core.Services;
//using CardBooster.Infrastructure.Command.Authentication;
//using CardBooster.Infrastructure.Database;
//using CardBooster.Infrastructure.Handler;
//using CardBooster.Infrastructure.Queries.Users;
//using CardBooster.Infrastructure.Repository;
//using CardBooster.Infrastructure.Security;
//using CardBooster.Infrastructure.Security.Interfaces;
//using Microsoft.AspNetCore.Authentication.JwtBearer;
//using Microsoft.IdentityModel.Tokens;
//using System.Text;

//namespace Card.API.Configuration
//{
//    public static class DependencyInjection
//    {
//        public static int GetUserCardQueryHandler { get; private set; }

//        public static IServiceCollection AddCardApi(this IServiceCollection services, IConfiguration configuration)
//        {
//            services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));
//            services.AddSingleton<IPasswordHasher, PasswordHasher>();
//            services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();
//            services.AddSingleton<IAuthDb, AuthDb>();

//            services.AddScoped<ICommandAsyncHandler<LoginUserCommand>, LoginUserCommandHandler>();
//            services.AddScoped<ICommandAsyncHandler<OpenBoosterCommand>, OpenBoosterCommandHandler>();
//            services.AddScoped<ICommandAsyncHandler<LoginUserCommand>, LoginUserCommandHandler>();
//            services.AddScoped<IQueryAsyncHandler<GetUserByEmailQuery, User>, GetUserByEmailQueryHandler>();
//            services.AddScoped<IQueryAsyncHandler<GetUserCardQuery, List<Cards>>, GetUserCardsQueryHandler>();

//           var jwtSettings = configuration.GetSection("JwtSettings").Get<JwtSettings>();
//            services.AddAuthentication(option => {
//                option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
//                option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
//            })
//                .AddJwtBearer(option =>
//                {
//                    option.TokenValidationParameters = new TokenValidationParameters
//                    {
//                        ValidateIssuer = true,
//                        ValidateAudience = true,
//                        ValidateLifetime = true,
//                        ValidateIssuerSigningKey = true,
//                        ValidIssuer = jwtSettings.Issuer,
//                        ValidAudience = jwtSettings.Audience,
//                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey))
//                    };
//                });

//            return services;
//        }
//    }
//}
