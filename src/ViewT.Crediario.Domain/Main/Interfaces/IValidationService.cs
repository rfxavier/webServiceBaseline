using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewT.Crediario.Domain.Core.Commands;

namespace ViewT.Crediario.Domain.Main.Interfaces
{
    public interface IValidationService
    {
        bool Validate(ICommand command);
    }
}
