using CardBooster.Core.Commands;
using CardBooster.Core.Commands.Boosters;
using CardBooster.Core.Models;
using CardBooster.Core.Repositories;
using CardBooster.Core.Results;
using CardBooster.Infrastructure.Database;
using CardBooster.Infrastructure.Repository;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CardBooster.Infrastructure.Handler
{
    public class OpenBoosterCommandHandler : ICommandAsyncHandler<OpenBoosterCommand>
    {
        private readonly IAuthDb _authDb;

        public OpenBoosterCommandHandler(IAuthDb authDb)
        {
            _authDb = authDb;
        }

        public async Task<Result<Booster>> ExecuteAsync(OpenBoosterCommand command)
        {
            try
            {
                using var connection = _authDb.CreateConnection();
                await connection.OpenAsync();

                int boosterId;

                using (var CreateBoosterCmd = connection.CreateCommand())
                {
                    CreateBoosterCmd.CommandText = @"
                            INSERT INTO Booster (OpenDateBooster, IdUser)
                            VALUES (GetDate(), @IdUser);
                            SELECT SCOPE_IDENTITY();";

                    CreateBoosterCmd.Parameters.Add(new SqlParameter("@IdUser", SqlDbType.Int)
                    { Value = command.UserId });

                    boosterId = Convert.ToInt32(await CreateBoosterCmd.ExecuteScalarAsync());
                }

                var Cards = new List<Cards>();
                using (var drawCardsCmd = new SqlCommand("sp_DrawBoosterCards", connection))
                {
                    drawCardsCmd.CommandType = CommandType.StoredProcedure;
                    drawCardsCmd.Parameters.Add(new SqlParameter("@BoosterId", SqlDbType.Int) { Value = boosterId });

                    using var Reader = await drawCardsCmd.ExecuteReaderAsync();
                    while (await Reader.ReadAsync())
                    {
                        Cards.Add(new Cards
                        {
                            IdCard = Reader.GetInt32(Reader.GetOrdinal("IdCard")),
                            Rarity = Reader.GetString(Reader.GetOrdinal("Rarity")),
                            Name = Reader.GetString(Reader.GetOrdinal("Name")),
                            ImageUrl = Reader.GetString(Reader.GetOrdinal("ImageUrl")),
                            IdRarity = Reader.GetString(Reader.GetOrdinal("IdRarity"))
                        });
                    }

                    var booster = new Booster
                    {
                        IdBooster = boosterId,
                        OpenDateBooster = DateTime.Now,
                        IdUser = command.UserId,
                        Cards = Cards,
                    };

                    return Result<Booster>.Success(booster);
                }
            }
            catch (SqlException ex)
            {
                return Result<Booster>.Failure(HandleSqlException(ex));
            }

            catch (Exception ex)
            {
                return Result<Booster>.Failure("Une erreur est survenue lors de l'ouverture du booster", ex);
            }
        }

        Task<Result> ICommandAsyncHandler<OpenBoosterCommand>.ExecuteAsync(OpenBoosterCommand command)
        {
            throw new NotImplementedException();
        }

        private string HandleSqlException(SqlException ex) {
            return ex.Number switch 
            {
                547 => "L'utilisateur n'existe pas",
                2627 => "Une erreur de duplication s'est produite.",
                _ => $"Erreur de base de données {ex.Message}",
            };
        }
    }
}
