using System;

namespace MiniNavigator_UI.Service
{
    /// <summary>
    /// Сервис для валидации значения с типом
    /// </summary>
    public class ValidationService : IValidationService
    {
        public bool ValidateSingleValue(string value, Type type, out string error)
        {
            if(type == null) throw new ArgumentNullException(nameof(type), "Тип атрибута не задан");
            if(value == null) throw new ArgumentNullException(nameof(value), "Значение атрибута равно null");

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
