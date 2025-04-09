using CardBooster.Core.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardBooster.Core.Queries
{
    public interface IQueryHandler <TQuery, TResult>
        where TQuery : IQueryDefinitionResult<TResult>
    {
        Result<TResult> Execute(TQuery query);
    }
   
   
}
