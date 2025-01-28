using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace WPF.Converters
{
    /// <summary>
    /// Convierte una lista de datos en una figura poligonal (grafica lineal)
    /// Creado Por: David Mora Barreto
    /// Creado El:  9/23/2022 5:30:28 PM
    /// </summary>
    internal class PoligonMultiConverter : BaseConverter, IMultiValueConverter
    {
        /// <summary>
        /// Convierte una serie de valores a una representación visual del los datos
        /// </summary>
        /// <param name="values">Serie de valores</param>
        /// <param name="targetType">El Tipo de objeto (Representación visual) al que se desea convertir la serie de valores</param>
        /// <param name="parameter"></param>
        /// <param name="culture">Configuración regional (Nombre de la referencia cultural, Sistema de escritura, Calendario utilizado, Orden de clasificación de las cadenas y Formato de fechas y números).</param>
        /// <returns>El DataType convertido al objeto de representación visual</returns>
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            PointCollection points = new();
            if (values.Length == 3 &&
                values[0] is IReadOnlyList<double?> dataPoints &&
                values[1] is double width &&
                values[2] is double height)
            {
                double porcentajeMargen = 0.0;
                double maxLevel = height * porcentajeMargen;
                double minLevel = height - maxLevel;
                double nullLevel;

                int round = 2;
                double maxValue = Math.Round(dataPoints.Where(i => double.IsFinite(i.GetValueOrDefault())).Max() ?? 0, round);
                double minValue = Math.Round(dataPoints.Where(i => double.IsFinite(i.GetValueOrDefault())).Min() ?? 0, round);

                if (minValue == maxValue)
                {
                    // Dibuja una X en el area grafica
                    points.Add(new Point(0, 0));
                    points.Add(new Point(width, height));
                    points.Add(new Point(width / 2, height / 2));
                    points.Add(new Point(width, 0));
                    points.Add(new Point(width / 2, height / 2));
                    points.Add(new Point(0, height));
                    points.Add(new Point(width / 2, height / 2));
                    return points;
                }
                else if (minValue >= 0)
                {
                    // Dibuja una linea horizontal en la parte baja del area grafica
                    nullLevel = height;
                    points.Add(new Point(width, nullLevel));
                    points.Add(new Point(0, nullLevel));
                }
                else
                {
                    // Dibuja una linea horizontal en la parte proporcional correspondiente del area grafica
                    nullLevel = Interpolacion(0, minValue, maxValue, minLevel, maxLevel);
                    points.Add(new Point(width, nullLevel));
                    points.Add(new Point(0, nullLevel));
                }

                double step = width / (dataPoints.Count - 1);
                double position = 0;
                foreach (double? value in dataPoints)
                {
                    if (value is null || !double.IsFinite(value.GetValueOrDefault()))
                    {
                        if (position > 0) { points.Add(new Point(position - step, nullLevel)); }
                        if (position < width) { points.Add(new Point(position + step, nullLevel)); }
                    }
                    else
                    {
                        double valueRound = Math.Round(value.GetValueOrDefault(), round);
                        points.Add(new Point(position, Interpolacion(valueRound, minValue, maxValue, minLevel, maxLevel)));

                        double X = height * 0.075;
                        points.Add(new Point(position, Interpolacion(valueRound, minValue, maxValue, minLevel, maxLevel) - X));
                        points.Add(new Point(position, Interpolacion(valueRound, minValue, maxValue, minLevel, maxLevel) + X));
                        points.Add(new Point(position, Interpolacion(valueRound, minValue, maxValue, minLevel, maxLevel)));
                        points.Add(new Point(position + X, Interpolacion(valueRound, minValue, maxValue, minLevel, maxLevel)));
                        points.Add(new Point(position - X, Interpolacion(valueRound, minValue, maxValue, minLevel, maxLevel)));
                        points.Add(new Point(position, Interpolacion(valueRound, minValue, maxValue, minLevel, maxLevel)));
                    }
                    position += step;
                }
            }
            return points;
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

        /// <summary>
        /// Interpola el Valor de entrada para devolver el Nivel correspondiente
        /// </summary>
        /// <param name="inputVal">Valor para poner en la grafica</param>
        /// <param name="minVal">Valor minimo en la grafica</param>
        /// <param name="maxVal">Valor maximo en la grafica</param>
        /// <param name="minLevel">Nivel minimo para trazar la grafica</param>
        /// <param name="maxLevel">Nivel maximo para trazar la grafica</param>
        /// <returns>Nivel o Coordenada de la gráfica</returns>
        private static double Interpolacion(double inputVal, double minVal, double maxVal, double minLevel, double maxLevel)
        {
            return maxLevel + ((inputVal - maxVal) * ((minLevel - maxLevel) / (minVal - maxVal)));
        }
    }
}

/* COMO USAR EL CONVERTIDOR

    1.- Definir el namespace
            xmlns:conv="clr-namespace:LineChart"

    2.- Use it in binding
            <Canvas ClipToBounds="True">
                <Polygon Stroke="LightBlue" Fill="AliceBlue">
                    <Polygon.Resources>
                        <Style TargetType="Polygon">
                            <Setter Property="Points">
                                <Setter.Value>
                                    <MultiBinding Converter="{StaticResource PoligonMultiConverter}">
                                        <Binding Path="Values"/>
                                        <Binding Path="ActualWidth" RelativeSource="{RelativeSource AncestorType=Canvas}"/>
                                        <Binding Path="ActualHeight" RelativeSource="{RelativeSource AncestorType=Canvas}"/>
                                    </MultiBinding>
                                </Setter.Value>
                            </Setter>
                        </Style>
                    </Polygon.Resources>
                </Polygon>
            </Canvas>
 */