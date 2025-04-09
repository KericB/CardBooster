using CardBooster.Core.Commands;
using CardBooster.Core.Commands.Boosters;
using CardBooster.Core.Models;
using CardBooster.Core.Queries;
using CardBooster.Core.Queries.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardBooster.Core.Repositories
{
    public interface IOpenBoosterRepo : 
        IQueryAsyncHandler<GetUserCardQuery, User>,
        ICommandAsyncHandler<OpenBoosterCommand>,
        IQueryAsyncHandler<GetUserCardQuery, List<Cards>>
    {

    }
}
