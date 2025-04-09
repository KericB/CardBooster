using CardBooster.Core.Commands;
using CardBooster.Core.Commands.Authentication;
using CardBooster.Core.Models;
using CardBooster.Core.Results;
using CardBooster.Infrastructure.Repository;
using CardBooster.Infrastructure.Security.Interfaces;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using System.Data;

namespace CardBooster.Infrastructure.Command.Authentication
{
    public class RegisterUserCommandHandler : ICommandAsyncHandler<RegisterUserCommand>
    {

        public readonly IAuthDb _authDb;
        private readonly IPasswordHasher _passwordHasher;

        public RegisterUserCommandHandler(IAuthDb authDb, IPasswordHasher passwordHasher)
        {
            _authDb = authDb;
            _passwordHasher = passwordHasher;
        }

        public async Task<Result> ExecuteAsync(RegisterUserCommand command)
        {
            try
            {
                {
                    using var connection = _authDb.CreateConnection();
                    await connection.OpenAsync();
                    using (var checkEmailCmd = connection.CreateCommand())
                    {
                        checkEmailCmd.CommandText = "SELECT COUNT(1) FROM Users WHERE Email = @Email";
                        checkEmailCmd.Parameters.Add(new SqlParameter("@Email", SqlDbType.NVarChar) { Value = command.Email });

                        var exists = (int)await checkEmailCmd.ExecuteScalarAsync() > 0;
                        if (exists)
                        {
                            return Result.Failure("Un utilisateur avec cet email existe déjà.");
                        }
                    }

                    string HashedPassword = _passwordHasher.HashPassword(command.Password);

                    using (var insertCmd = connection.CreateCommand())
                    {
                        insertCmd.CommandText = @"INSERT INTO Users (Username, Password, Email, Role)
                                                VALUES (@Username, @Password, @Email, @Role);
                                                SELECT SCOPE_IDENTITY();";
                        insertCmd.Parameters.Add(new SqlParameter("@Username", SqlDbType.NVarChar) { Value = command.Name });
                        insertCmd.Parameters.Add(new SqlParameter("@Password", SqlDbType.NVarChar) { Value = HashedPassword });
                        insertCmd.Parameters.Add(new SqlParameter("@Email", SqlDbType.NVarChar) { Value = command.Email });
                        insertCmd.Parameters.Add(new SqlParameter("@Role", SqlDbType.NVarChar) { Value = command.Role });

                        var userId = await insertCmd.ExecuteScalarAsync();

                        return Result.Success();
                    }
                }


            }

            catch (SqlException ex)
            {
                return HandleSqlException(ex);
            }
            catch (Exception ex)
            {
                return Result.Failure("Une erreur est survenue lors de l'inscription", ex);
            }

            
        }
        private Result HandleSqlException(SqlException ex)
        {
            string errorMessage = ex.Number switch
            {
                2627 => "Un utilisateur avec ces informations existe déjà.",
                2601 => "Un utilisateur avec ces informations existe déjà.",
                _ => $"Une erreur de base de données est survenue:{ex.Message}."
            };
           
                    return Result.Failure(errorMessage, ex);
        }
    }



}
