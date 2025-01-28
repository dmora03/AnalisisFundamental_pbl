using System;
using System.Globalization;
using System.Windows.Data;

namespace WPF.Converters
{
    /// <summary>
    /// Si el valor (DataType) es menor al parametro dado regresa "1", de lo contario "0"
    /// Creado por: David Mora Barreto
    /// Creado el:  8/31/2022 12:01:52 PM
    /// </summary>
    internal class IsLessThanConverter : BaseConverter, IValueConverter
    {
        /// <summary>
        /// Si el valor (DataType) es menor al parametro dado regresa "1", de lo contario "0"
        /// Si el valor o parametro no son datos numericos, regresara NULL
        /// </summary>
        /// <param name="value">El valor del DataType</param>
        /// <param name="targetType">El Tipo de objeto (Representación visual) al que se desea convertir el DataType</param>
        /// <param name="parameter"></param>
        /// <param name="culture">Configuración regional (Nombre de la referencia cultural, Sistema de escritura, Calendario utilizado, Orden de clasificación de las cadenas y Formato de fechas y números).</param>
        /// <returns>El DataType convertido al objeto de representación visual</returns>
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return double.TryParse(value?.ToString(), out double valueDouble) == false ||
                   double.TryParse(parameter?.ToString(), out double paramDouble) == false
                ? null
                : valueDouble < paramDouble ? "1" : "0";




            //string[]? paramArr = parameter?.ToString()?.Split(',');
            //if (paramArr is null || paramArr.Length != 3) { return 0; /*throw new ArgumentException(null, nameof(parameter));*/ }
            //if (double.TryParse(value?.ToString(), out double valueDouble) == false) { return 0; }

            //_ = double.TryParse(paramArr[0], out double valADouble);
            //_ = double.TryParse(paramArr[1], out double valBDouble);
            //_ = double.TryParse(paramArr[2], out double invrDouble);


            //int returnValue = 0;
            //if (valueDouble < valADouble)
            //{
            //    returnValue = -1;
            //}
            //else if (valueDouble > valBDouble)
            //{
            //    returnValue = 1;
            //}

            //if (invrDouble == 1) { returnValue *= -1; }
            //return returnValue;

        }

        /// <summary>
        /// Convierte la representación visual del dato a un DataType deseado
        /// </summary>
        /// <param name="value">Representación visual del dato</param>
        /// <param name="targetType">El DataType al que se desea convertir la reprsentación visual del dato</param>
        /// <param name="parameter"></param>
        /// <param name="culture">Configuración regional (Nombre de la referencia cultural, Sistema de escritura, Calendario utilizado, Orden de clasificación de las cadenas y Formato de fechas y números).</param>
        /// <returns>El dato visual convertido al DataType objetivo</returns>
        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
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
                                    Converter={conv:IsLessThanConverter}, ConverterParameter=$$$}"/>
 */