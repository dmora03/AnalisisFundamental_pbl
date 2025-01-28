using System;
using System.Globalization;
using System.Windows.Data;

namespace WPF.Converters
{
    /// <summary>
    /// Si el texto (DataType) dado es de algun tipo de "NA" regresa TRUE
    /// Creado por: David Mora Barreto
    /// Creado el:  8/17/2022 4:26:16 PM
    /// </summary>
    internal class IsReferenciaNAConverter : BaseConverter, IValueConverter
    {
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
            string? texto = value?.ToString()?.ToUpper(culture);
            return texto is "NA"
                         or "~NA~"
                         or "~ NA ~" 
                         or "NO APLICA"
                         or "~NO APLICA~"
                         or "~ NO APLICA ~";
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
                                    Converter={conv:IsNARefConverter}, ConverterParameter=$$$}"/>
 */