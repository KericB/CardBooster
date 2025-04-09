using CardBooster.Core.Queries.Users;
using CardBooster.Core.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardBooster.Core.Queries
{
    public interface IQueryAsyncHandler<TQuery, TResult>
        where TQuery : IQueryDefinition
    {
        Task<Result<TResult>> ExecuteAsync(TQuery query);
    }
}
