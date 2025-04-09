using CardBooster.Core.Commands;
using CardBooster.Core.Commands.Authentication;
using CardBooster.Core.Models;
using CardBooster.Core.Results;
using CardBooster.Infrastructure.Repository;
using CardBooster.Infrastructure.Security.Interfaces;   
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;


namespace CardBooster.Infrastructure.Handler
{
    public class LoginUserCommandHandler :ICommandAsyncHandler<LoginUserCommand>
    {

        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly IAuthDb _authDb;
        private readonly IPasswordHasher _passwordHasher;

        public LoginUserCommandHandler(
            IJwtTokenGenerator jwtTokenGenerator, 
            IAuthDb authDb, 
            IPasswordHasher passwordHasher)
        {
            _jwtTokenGenerator = jwtTokenGenerator;
            _authDb = authDb;
            _passwordHasher = passwordHasher;
        }



        public async Task<Result> ExecuteAsync(LoginUserCommand command)        
        {
            try
            {
                using var connection = _authDb.CreateConnection();
                await connection.OpenAsync();

                User? user = null;

                using (var cmd = connection.CreateCommand())
                {
                    cmd.CommandText = "SELECT IdUser, Name, Email, Password, Role FROM Users WHERE Email = @Email";
                    cmd.Parameters.Add(new SqlParameter("@Email", command.Email));

                    using var reader = await cmd.ExecuteReaderAsync();
                    if (await reader.ReadAsync())
                    {
                        user = new User
                        {
                            IdUser = reader.GetInt32(0),
                            Username = reader.GetString(1),
                            Email = reader.GetString(2),
                            Password = reader.GetString(3),
                            Role = reader.GetString(4)
                        };
                    }
                }
                if (user == null)
                {
                    return Result.Failure("Email ou mot de passe incorrect");
                }

                if (!_passwordHasher.VerifyPassword(user.Password, command.Password))
                {
                    return Result.Failure("Email ou mot de passe incorrect");
                }

                var token = _jwtTokenGenerator.GenerateToken(user);

                var AuthenticationResult = new AuthenticationResult
                {
                    Success = true,
                    Token = token
                };

                return Result.Success();


            }

            catch (SqlException ex)
            {
                return Result.Failure(HandleSqlException(ex));
            }

            catch (Exception ex)
            {
                return Result.Failure("Une erreur s'est produite lors de la connexion", ex);
            }
        }

      

        private string HandleSqlException(SqlException ex)
        {
            return ex.Number switch
            {
                4060 => "Impossible de se connecter à la base de données. Vérifiez les informations d'identification.",
                18456 => "Erreur d'authentification à la base de données.",
                _ => $"Erreur de base de données : {ex.Message}"
            };
        }
    }
}
