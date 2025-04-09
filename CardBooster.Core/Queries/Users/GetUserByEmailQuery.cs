using CardBooster.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardBooster.Core.Queries.Users
{
    public class GetUserByEmailQuery : IQueryDefinition
    {
        public string Email { get; set; } = string.Empty;
    }
}
