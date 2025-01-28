using System;
using System.Globalization;
using System.Windows.Data;

namespace WPF.Converters
{
    /// <summary>
    /// Invierte el valor booleano
    /// Creado Por: David Mora Barreto
    /// Creado El:  6/21/2022 12:20:52 PM
    /// </summary>
    internal class InverseBooleanConverter : BaseConverter, IValueConverter
    {
        public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return targetType == typeof(bool) ? !(bool)value : throw new InvalidOperationException("The target must be a boolean");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();    // The convert back is not supported for this convertion class
        }
    }
}

/* COMO USAR EL CONVERTIDOR

    1.- Definir el namespace
            xmlns:conv="clr-namespace:WPF.Converters"

    2.- Use it in binding
            <CheckBox x:Name="CostoVentas_chk"/>
            <TextBox IsEnabled="{Binding IsChecked, ElementName=CostoVentas_chk,
                                         Converter={conv:InverseBooleanConverter}}"/>
 */