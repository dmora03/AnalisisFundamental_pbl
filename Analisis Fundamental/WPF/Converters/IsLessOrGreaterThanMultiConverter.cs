using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace WPF.Converters
{
    /// <summary>
    /// Regresa -1, 1 o 0 dependiendo si el valor de entrada es menor al Limite A, Mayor al Limite B o ninguno de los anteriores, respectivamente
    /// Creado por: David Mora Barreto
    /// Creado El:  9/27/2022 11:11:10 AM
    /// </summary>
    internal class IsLessOrGreaterThanMultiConverter : BaseConverter, IMultiValueConverter
    {
        /// <summary>
        /// Convierte una serie de valores a una representación visual del los datos
        /// </summary>
        /// <param name="values">Serie de valores</param>
        /// <param name="targetType">El Tipo de objeto (Representación visual) al que se desea convertir la serie de valores</param>
        /// <param name="parameter"></param>
        /// <param name="culture">Configuración regional (Nombre de la referencia cultural, Sistema de escritura, Calendario utilizado, Orden de clasificación de las cadenas y Formato de fechas y números).</param>
        /// <returns>El DataType convertido al objeto de representación visual</returns>
        public object Convert(object?[] values, Type targetType, object? parameter, CultureInfo culture)
        {
            string? valor = values[0]?.ToString();
            string? limites = values[1]?.ToString();

            if (valor is null || limites is null || values[1] == DependencyProperty.UnsetValue) { return 0; }

            string[] paramArr = limites.Split(',');
            if (paramArr.Length != 3) { throw new ArgumentException("No se pasaron los 3 parámetros necesarios al convertidor", nameof(values)); }
            if (double.TryParse(valor, out double valIn) == false) { return "0"; }

            bool isAvailableA = double.TryParse(paramArr[0], out double valA);
            bool isAvailableB = double.TryParse(paramArr[1], out double valB);
            if (!isAvailableA && !isAvailableB) { throw new ArgumentException("Falta por lo menos uno de los siguientes limites A y/o B", nameof(values)); }

            _ = double.TryParse(paramArr[2], out double invrDouble);
            invrDouble = invrDouble == 1 ? -1 : 1;

            if (valIn < valA && (isAvailableA || !isAvailableB))
            { return (-1 * invrDouble).ToString(culture); }

            if (valIn > valB && (!isAvailableA || isAvailableB))
            { return (1 * invrDouble).ToString(culture); }

            return "0";
        }

        /// <summary>
        /// Convierte la representación visual del dato a una serie de valores
        /// </summary>
        /// <param name="value">Representación visual del dato</param>
        /// <param name="targetTypes">Los DataTypes de los valores que se desean obtener de la reprsentación visual del dato</param>
        /// <param name="parameter"></param>
        /// <param name="culture">Configuración regional (Nombre de la referencia cultural, Sistema de escritura, Calendario utilizado, Orden de clasificación de las cadenas y Formato de fechas y números).</param>
        /// <returns>El dato visual convertido al DataType objetivo</returns>
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
                        <MultiBinding Converter="{conv:IsLessOrGreaterThanMultiConverter}">
                            <Binding Path="Text" ElementName="TextBox"/>
                            <Binding Path="Text" ElementName="TextBox1"/>
                        </MultiBinding>
                     </TextBlock.Text>
                </TextBlock>
            </StackPanel>
 */