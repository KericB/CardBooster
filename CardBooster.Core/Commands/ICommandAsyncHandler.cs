using CardBooster.Core.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardBooster.Core.Commands
{
    public interface ICommandAsyncHandler<TCommand> 
        where TCommand : ICommandDefinition
    {
        Task<Result> ExecuteAsync(TCommand command);
    }
   
}
