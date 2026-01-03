using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniNavigator_UI.Service
{
    public interface IValidationService
    {
        bool ValidateSingleValue(string value, Type type, out string error);
    }
}
