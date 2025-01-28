using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WPF.Components
{
    /// <summary>
    /// Interaction logic for PoligonChart.xaml
    /// </summary>
    public partial class PoligonChart : UserControl
    {
        #region DependencyProperty Values
        public IReadOnlyList<double?> Values
        {
            get => (IReadOnlyList<double?>)GetValue(ValuesProperty);
            set => SetValue(ValuesProperty, value);
        }
        public static readonly DependencyProperty ValuesProperty =
            DependencyProperty.Register("Values",
                                        typeof(IReadOnlyList<double?>),
                                        typeof(PoligonChart),
                                        new PropertyMetadata(new List<double?> { 0, 0, 0, 0 }, OnValuesChanged));

        private static void OnValuesChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is PoligonChart poligonChart)
            {
                poligonChart.Canvas_SizeChanged(poligonChart, null);
            }
        }
        #endregion

        public PoligonChart()
        {
            InitializeComponent();
        }

        private void Canvas_SizeChanged(object sender, SizeChangedEventArgs? e)
        {
            Polygon grafica = new();
            PointCollection points = new();
            CanvasArea.Children.Clear();
            _ = CanvasArea.Children.Add(grafica);

            double diametro = CanvasArea.ActualHeight * 0.1;    //MAX 7, MIN 2.5
            if (diametro > 7) { diametro = 7; }       // Max Value is 7
            else if (diametro < 2.5) { diametro = 2.5; }  // Min Value is 2.5

            double margen = diametro / 2;
            double maxLevelY = margen;
            double minLevelY = CanvasArea.ActualHeight - margen;
            double maxLevelX = CanvasArea.ActualWidth - margen;
            double minLevelX = margen;
            double nullLevel;

            int round = 2;
            double maxValue = Math.Round(Values.Where(i => double.IsFinite(i.GetValueOrDefault())).Max() ?? 0, round);
            double minValue = Math.Round(Values.Where(i => double.IsFinite(i.GetValueOrDefault())).Min() ?? 0, round);

            if (minValue == maxValue)
            {
                // Dibuja una X en el area grafica
                points.Add(new Point(minLevelX, maxLevelY));
                points.Add(new Point(maxLevelX, minLevelY));
                points.Add(new Point(maxLevelX / 2, minLevelY / 2));
                points.Add(new Point(maxLevelX, maxLevelY));
                points.Add(new Point(maxLevelX / 2, minLevelY / 2));
                points.Add(new Point(minLevelX, minLevelY));
                points.Add(new Point(maxLevelX / 2, minLevelY / 2));

                grafica.Points = points;
                return;
            }
            else if (minValue >= 0)
            {
                // Dibuja una linea horizontal en la parte baja del area grafica
                nullLevel = minLevelY;
                points.Add(new Point(maxLevelX, nullLevel));
                points.Add(new Point(minLevelX, nullLevel));
            }
            else
            {
                // Dibuja una linea horizontal en la parte proporcional correspondiente del area grafica
                nullLevel = Interpolacion(0, minValue, maxValue, minLevelY, maxLevelY);
                points.Add(new Point(maxLevelX, nullLevel));
                points.Add(new Point(minLevelX, nullLevel));
            }

            double stepX = (maxLevelX - minLevelX) / (Values.Count - 1);
            double levelX = minLevelX;
            foreach (double? value in Values)
            {
                if (value is null || !double.IsFinite(value.GetValueOrDefault()))
                {   // Si no hay valor:
                    // Y es el segundo punto, traza una linea vertical del punto anterior a la linea neutral
                    if (levelX > minLevelX) { points.Add(new Point(levelX - stepX, nullLevel)); }
                    // Y no es el ultimo punto, pone el siguiente punto en el paso siguiente al nivel de la linea neutral
                    if (levelX < maxLevelX) { points.Add(new Point(levelX + stepX, nullLevel)); }

                    Line line = new()
                    {
                        X1 = levelX,
                        Y1 = nullLevel - (diametro / 2),
                        X2 = levelX,
                        Y2 = nullLevel + (diametro / 2)
                    };
                    _ = CanvasArea.Children.Add(line);
                }
                else
                {
                    double valueRound = Math.Round(value.GetValueOrDefault(), round);
                    double levelY = Interpolacion(valueRound, minValue, maxValue, minLevelY, maxLevelY);
                    points.Add(new Point(levelX, levelY));

                    Ellipse circle = new()
                    {
                        Width = diametro,
                        Height = diametro
                    };
                    Canvas.SetLeft(circle, levelX - (circle.Width / 2));
                    Canvas.SetTop(circle, levelY - (circle.Height / 2));
                    _ = CanvasArea.Children.Add(circle);
                }
                levelX += stepX;
            }
            grafica.Points = points;
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
