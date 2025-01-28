using System;
using System.Globalization;
using System.Windows.Data;

namespace WPF.Converters
{
    /// <summary>
    /// Regresa -1, 1 o 0 dependiendo si el valor de entrada es menor al Limite A, Mayor al Limite B o ninguno de los anteriores, respectivamente
    /// Creado por: David Mora Barreto
    /// Creado el:  9/8/2022 5:57:16 PM
    /// </summary>
    internal class IsLessOrGreaterThanConverter : BaseConverter, IValueConverter
    {
        /// <summary>
        /// Regresa -1, 1 o 0 dependiendo si el valor de entrada es menor al Limite A, Mayor al Limite B o ninguno de los anteriores, respectivamente
        /// </summary>
        /// <param name="value">El valor del DataType</param>
        /// <param name="targetType">El Tipo de objeto (Representación visual) al que se desea convertir el DataType</param>
        /// <param name="converterParameter"></param>
        /// <param name="culture">Configuración regional (Nombre de la referencia cultural, Sistema de escritura, Calendario utilizado, Orden de clasificación de las cadenas y Formato de fechas y números).</param>
        /// <returns>El DataType convertido al objeto de representación visual</returns>
        public object? Convert(object? value, Type targetType, object? converterParameter, CultureInfo culture)
        {
            string[]? paramArr = converterParameter?.ToString()?.Split(',');
            if (paramArr is null || paramArr.Length != 3) { throw new ArgumentException("No se pasaron los 3 parámetros necesarios al convertidor", nameof(converterParameter)); }
            if (double.TryParse(value?.ToString(), out double valIn) == false) { return "0"; }

            bool isAvailableA = double.TryParse(paramArr[0], out double valA);
            bool isAvailableB = double.TryParse(paramArr[1], out double valB);
            if (!isAvailableA && !isAvailableB) { throw new ArgumentException("Falta por lo menos uno de los siguientes limites A y/o B", nameof(converterParameter)); }

            _ = double.TryParse(paramArr[2], out double invrDouble);
            invrDouble = invrDouble == 1 ? -1 : 1;

            if (valIn < valA && (isAvailableA || !isAvailableB))
            { return (-1 * invrDouble).ToString(culture); }

            if (valIn > valB && (!isAvailableA || isAvailableB))
            { return (1 * invrDouble).ToString(culture); }

            return "0";
        }

        /// <summary>
        /// Convierte la representación visual del dato a un DataType deseado
        /// </summary>
        /// <param name="value">Representación visual del dato</param>
        /// <param name="targetType">El DataType al que se desea convertir la reprsentación visual del dato</param>
        /// <param name="parameter"></param>
        /// <param name="culture">Configuración regional (Nombre de la referencia cultural, Sistema de escritura, Calendario utilizado, Orden de clasificación de las cadenas y Formato de fechas y números).</param>
        /// <returns>El dato visual convertido al DataType objetivo</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();    // The convert back is not supported for this convertion class
        }
    }
}

/* COMO USAR EL CONVERTIDOR

    1.- Definir el namespace
            xmlns:conv="clr-namespace:WPF.Converters"

    2.- Usarlo en el binding
            <TextBox Text="{Binding ElementName=txt, Path=Height, 
                                    Converter={conv:IsGreaterThanConverter}, ConverterParameter=$$$}"/>
 */