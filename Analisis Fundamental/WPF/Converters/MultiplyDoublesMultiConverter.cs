using System;
using System.Globalization;
using System.Windows.Data;

namespace WPF.Converters
{
    /// <summary>
    /// Multiplica todos los valores de entrada
    /// Creado Por: David Mora Barreto
    /// Creado El:  7/7/2022 3:55:42 PM
    /// </summary>
    internal class MultiplyDoublesMultiConverter : BaseConverter, IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            double result = 1.0;
            foreach (object item in values)
            {
                if (item is double @double)
                { result *= @double; }
            }
            //for (int i = 0; i < values.Length; i++)
            //{
            //    if (values[i] is double)
            //        result *= (double)values[i];
            //}

            return result;
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
                        <MultiBinding Converter="{conv:MultiplyDoublesMultiConverter}">
                            <Binding Path="Text" ElementName="TextBox"/>
                            <Binding Path="Text" ElementName="TextBox1"/>
                        </MultiBinding>
                     </TextBlock.Text>
                </TextBlock>
            </StackPanel>
 */