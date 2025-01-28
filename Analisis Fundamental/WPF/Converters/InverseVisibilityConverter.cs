using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace WPF.Converters
{
    /// <summary>
    /// Invierte el valor de Visibilidad.
    /// Si es Visible regresa Collapsed, de otra manera regresa Visible
    /// Creado por: David Mora Barreto
    /// Creado el:  9/1/2022 7:16:47 PM
    /// </summary>
    internal class InverseVisibilityConverter : BaseConverter, IValueConverter
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
            return targetType == typeof(Visibility)
                ? (Visibility)value == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible
                : throw new InvalidOperationException("The target must be a Visibility type");
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
                                    Converter={conv:InverseVisibilityConverter}, ConverterParameter=$$$}"/>
 */