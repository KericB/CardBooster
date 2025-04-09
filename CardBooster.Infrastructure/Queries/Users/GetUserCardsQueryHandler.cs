using CardBooster.Core.Models;
using CardBooster.Core.Queries;
using CardBooster.Core.Queries.Users;
using CardBooster.Core.Results;
using CardBooster.Infrastructure.Repository;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardBooster.Infrastructure.Queries.Users
{
    public class GetUserCardsQueryHandler : IQueryAsyncHandler<GetUserCardQuery, List<Cards>>
    {
        private readonly IAuthDb _authDb;

        public GetUserCardsQueryHandler(IAuthDb authDb)
        {
            _authDb = authDb;
        }

        public async Task<Result<List<Cards>>> ExecuteAsync(GetUserCardQuery query)
        {
            try
            {
                using var connection = _authDb.CreateConnection();
                await connection.OpenAsync();

                var cards = new List<Cards>();

                using var cmd = connection.CreateCommand();
                cmd.CommandText = @"
                SELECT c.IdCard,
                    c.Name,
                    r.Name as Rarity,
                    c.IdRarity,
                    FROM CARDS c 
                    JOIN BoosterCards bc ON c.IdCard = bc.IdCard
                    JOIN Booster b ON bc.IdBooster = b.IdBooster
                    JOIN Rarity r ON c.IdRarity = r.Id  

                    WHERE b.IdUser = @IdUser";

                cmd.Parameters.Add(new SqlParameter("@UserId", SqlDbType.Int) { Value = query.UserId });

                using var reader = await cmd.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    var card = new Cards
                    {
                        IdCard = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        Rarity = reader.GetString(2),
                        IdRarity = reader.GetString(3),

                    };
                }
                return Result<List<Cards>>.Success(cards);
            }
            catch (SqlException ex)
            {
                return Result<List<Cards>>.Failure(HandleSqlException(ex));
            }
            catch (Exception ex) 
            { 
                return Result<List<Cards>>.Failure("Une erreur est survenue lors de la récupération des cartes.", ex);
            }
        }

        private string HandleSqlException(SqlException ex)
        {
            return ex.Number switch
            {
                4060 => "Impossible de se connecter à la base de données.",
                208 => "Table ou vue non trouvée.",
                _ => $"Erreur SQL inconnue.{ex.Message}"
            };
        }
    }
}
