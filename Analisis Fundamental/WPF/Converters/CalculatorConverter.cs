using System;
using System.Globalization;
using System.Windows.Data;

namespace WPF.Converters
{
    /// <summary>
    /// Evalua la expresion matematica en el string y regresa el resultado numérico
    /// Creado Por: David Mora Barreto
    /// Creado El:  7/28/2022 9:38:08 AM
    /// </summary>
    internal class CalculatorConverter : BaseConverter, IValueConverter
    {
        //private readonly System.Diagnostics.Stopwatch watch = new(); //Solo para pruebas de rendimiento

        /// <summary>
        /// Convierte un DataType a una representación visual del dato
        /// </summary>
        /// <param name="value">El valor del DataType</param>
        /// <param name="targetType">El Tipo de objeto (Representación visual) al que se desea convertir el DataType</param>
        /// <param name="parameter"></param>
        /// <param name="culture">Configuración regional (Nombre de la referencia cultural, Sistema de escritura, Calendario utilizado, Orden de clasificación de las cadenas y Formato de fechas y números).</param>
        /// <returns>El DataType convertido al objeto de representación visual</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string? format = parameter?.ToString();
            return format is null ? value : string.Format(culture, "{0:" + format + "}", value);
            //return $"{value:c}";
            //{}{0:c}
            //{}{0:#,0.#####}
            //return format.Replace("{0:", $"{{{value}:");
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
            if (value is null) { return null; }
            //watch.Reset();
            //watch.Start();    //Para medir tiempo de ejecucion de codigo

            //TODO: Cambiar por un mejor convertidor o codigo mas eficiente
            string? outputData = value.ToString();
            try
            {
                if (!double.TryParse(outputData, NumberStyles.Currency, culture, out _))
                {
                    outputData = outputData?.Replace("$", "");
                    outputData = outputData?.Replace(",", "");
                    using System.Data.DataTable? dt = new();
                    outputData = dt.Compute(outputData, null).ToString();
                }
            }
            catch { }
            finally { }

            //watch.Stop();
            //System.Diagnostics.Debug.Print(watch.Elapsed.TotalMilliseconds + " ms");

            return outputData!;
        }
    }
}

/* COMO USAR EL CONVERTIDOR

    1.- Definir el namespace
            xmlns:conv="clr-namespace:WPF.Converters"

    2.- Use it in binding
            <TextBox Text="{Binding ElementName=txt, Path=Height, 
                                    Converter={conv:CalculatorConverter}, ConverterParameter=$$$}"/>
 */