using CardBooster.Core.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardBooster.Core.Commands
{
    public interface ICommandHandler<TCommand>
        where TCommand : ICommandDefinition
    {
        Result Execute(TCommand command);
    }
}
