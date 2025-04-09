using BCrypt.Net;
using CardBooster.Core.Commands;
using CardBooster.Core.Results;
using CardBooster.Infrastructure.Database;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardBooster.Core.Models.AddUsers
{
    public class AddUserHandler : ICommandHandler<AddUser>
    {

        private readonly AuthDb _authDb;

        public AddUserHandler(AuthDb authDb)
        {
            _authDb = authDb;
        }
        public Result Execute(AddUser command)
        {
            try
            {
                using (var connection = _authDb.CreateConnection())
                {
                    var query = "INSERT INTO Users (Name, Password, Email) VALUES (@Name, @Password, @Email)";
                    using (var cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@Name", command.Name);
                        cmd.Parameters.AddWithValue("@Password", HashPassword(command.Password));
                        cmd.Parameters.AddWithValue("@Email", command.Email);
                        connection.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Failure("Failed to add user", ex);
            }
        }

        private string HashPassword(string password) 
        { 
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

    }
}
