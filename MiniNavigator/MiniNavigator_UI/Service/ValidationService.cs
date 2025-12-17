using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniNavigator_UI.Service
{
    internal class ValidationService : IValidationService
    {
        /// <summary>
        /// Проверяет соответствие типов для строки DataRow
        /// </summary>
        /// <param name="row">Проверяемая строка</param>
        /// <param name="table">DataTable с колонками и типами</param>
        /// <param name="errorMessage">Сообщение об ошибке</param>
        /// <returns>true, если все значения корректны</returns>
        public bool ValidateTypesForRow(DataRow row, DataTable table, out string errorMessage)
        {
            errorMessage = string.Empty;

            foreach (DataColumn col in table.Columns)
            {
                var value = row[col.ColumnName];

                if (value == DBNull.Value || value == null || string.IsNullOrWhiteSpace(value.ToString()))
                    continue;

                try
                {
                    var converted = Convert.ChangeType(value, col.DataType);
                }
                catch 
                {
                    errorMessage = $"Значение '{value}' у атрибута '{col.ColumnName}' не соответствует типу атрибута: {col.DataType.Name}.";
                    return false;
                }
            }
            
            return true;
        }

        public bool ValidateSingleValue(string value, Type type, out string error)
        {
            error = null;

            bool valid =
                type == typeof(string) ||
                (type == typeof(int) && int.TryParse(value, out _)) ||
                (type == typeof(byte) && byte.TryParse(value, out _)) ||
                (type == typeof(decimal) && decimal.TryParse(value, out _)) ||
                (type == typeof(double) && double.TryParse(value, out _)) ||
                (type == typeof(bool) && bool.TryParse(value, out _)) ||
                (type == typeof(DateTime) && DateTime.TryParse(value, out _));

            if (!valid)
                error = $"Значение '{value}' не соответствует атрибуту с типом {type.Name}";

            return valid;
        }

    }
}
