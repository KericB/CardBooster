using CardBooster.Core.Commands;
using CardBooster.Core.Queries;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardBooster.Infrastructure.Repository
{
    public interface IAuthDb
    {
        SqlConnection CreateConnection();
    }
}
