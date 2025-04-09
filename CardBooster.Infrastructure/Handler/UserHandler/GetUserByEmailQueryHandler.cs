using CardBooster.Core.Models;
using CardBooster.Core.Results;
using CardBooster.Infrastructure.Repository;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CardBooster.Core.Queries.Users
{
    public class GetUserByEmailQueryHandler : IQueryAsyncHandler<GetUserByEmailQuery, User>
    {
        private readonly IAuthDb _authDb;
        public GetUserByEmailQueryHandler(IAuthDb authDb)
        {
            _authDb = authDb;
        }

      
        public async Task<Result<User>> ExecuteAsync(GetUserByEmailQuery query)
        {
            try
            {
                using var connection = _authDb.CreateConnection();
                await connection.OpenAsync();

                using var cmd = connection.CreateCommand();
                cmd.CommandText = "SELECT IdUser, Username, Password, Email, Role FROM Users WHERE Email = @Email";
                cmd.Parameters.Add(new SqlParameter("@Email", SqlDbType.NVarChar) { Value = query.Email });

                using var reader = await cmd.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                {
                    var user = new User
                    {
                        IdUser = reader.GetInt32(0),
                        Username = reader.GetString(1),
                        Password = reader.GetString(2),
                        Email = reader.GetString(3),
                        Role = reader.GetString(4)
                    };
                    return Result<User>.Success(user);
                }
                else
                {
                    return Result<User>.Failure("Aucun utilisateur trouvé avec cet Email");
                }
            }

            catch (SqlException ex)
            {
                return Result<User>.Failure(HandleSqlException(ex));
            }
            catch (Exception ex)
            {
                return Result<User>.Failure("Une erreur est survenue lors de la recherche de l'utilisateur.", ex);
            }
        }


        private string HandleSqlException(SqlException ex)
        {
            return ex.Number switch
            {
                4060 => "Erreur d'authentification de la base de données.",
                208 => "La table User n'existe pas.",
                _ => $"Une erreur SQL est survenue.{ex.Message}",
            };
        }
    }
}
