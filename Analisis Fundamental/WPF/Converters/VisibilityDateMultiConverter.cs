using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace WPF.Converters
{
    /// <summary>
    /// Dependiendo si algun objeto de entrada es NULL, regresa el tipo de visibilidad
    /// Creado Por: David Mora Barreto
    /// Creado El:  6/15/2022 8:01:51 AM
    /// </summary>
    internal class VisibilityDateMultiConverter : BaseConverter, IMultiValueConverter
    {
        /// <summary>
        /// En base a si se selecciono una empresa y/o reporte regresa si el objeto debe ser visible o collapsado
        /// </summary>
        /// <param name="values">Son SOLO dos parametros, primero la empresa seleccionada y despues el reporte seleccionado</param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns>Regresa el estado de la propiedad Visibility del objeto</returns>
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            return values[0] is null ? Visibility.Collapsed
                                     : values[1] is not null ? Visibility.Collapsed
                                                             : (object)Visibility.Visible;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();    // The convert back is not supported for this convertion class
        }
    }
}

/* COMO USAR EL CONVERTIDOR

    1.- Definir el namespace
            xmlns:conv="clr-namespace:WPF.Converters"

    2.- Use it in binding
            <StackPanel Orientation="Vertical">
                <TextBox x:Name="TextBox" />
                <TextBox x:Name="TextBox1" />
                <TextBlock >
                    <TextBlock.Text>
                        <MultiBinding Converter="{conv:EnableDateMultiConverter}">
                            <Binding Path="Text" ElementName="TextBox"/>
                            <Binding Path="Text" ElementName="TextBox1"/>
                        </MultiBinding>
                     </TextBlock.Text>
                </TextBlock>
            </StackPanel>
 */