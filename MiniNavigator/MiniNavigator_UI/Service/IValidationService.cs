using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniNavigator_UI.Service
{
    public interface IValidationService
    {
        bool ValidateTypesForRow(DataRow row, DataTable table, out string errorMessage);
    }
}
