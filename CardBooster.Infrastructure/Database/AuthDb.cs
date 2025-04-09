using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using CardBooster.Infrastructure.Repository;
using System.Data.Common;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;


namespace CardBooster.Infrastructure.Database
{
    public class AuthDb : IAuthDb
    {
        private readonly string _connectionString;

        public AuthDb(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("Default");
        }

        public SqlConnection CreateConnection()
        {
            return new SqlConnection(_connectionString);
        }
    }
        
}
