using Models;
using System;
using System.Collections.Generic;
using System.Linq;
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
    /// Interaction logic for TrimestreLabel.xaml
    /// </summary>
    public partial class TrimestreLabel : UserControl
    {
        /// <summary>
        /// Propiedad que contiene el numero del trimestre
        /// </summary>
        public string NumTrimestre
        {
            get => (string)GetValue(NumTrimestreProperty);
            set => SetValue(NumTrimestreProperty, value);
        }
        public static readonly DependencyProperty NumTrimestreProperty =
            DependencyProperty.Register("NumTrimestre",
                                        typeof(string),
                                        typeof(TrimestreLabel),
                                        new PropertyMetadata(string.Empty));

        /// <summary>
        /// Propiedad que contiene el año del trimestre
        /// </summary>
        public string Ano
        {
            get => (string)GetValue(AnoProperty);
            set => SetValue(AnoProperty, value);
        }
        public static readonly DependencyProperty AnoProperty =
            DependencyProperty.Register("Ano",
                                        typeof(string),
                                        typeof(TrimestreLabel),
                                        new PropertyMetadata(string.Empty));

        //public Reporte Reporte
        //{
        //    get => (Reporte)GetValue(ReporteProperty);
        //    set => SetValue(ReporteProperty, value);
        //}
        //public static readonly DependencyProperty ReporteProperty =
        //    DependencyProperty.Register("Reporte", typeof(Reporte), typeof(TrimestreLabel), new PropertyMetadata(null));

        public TrimestreLabel()
        {
            InitializeComponent();
        }
    }
}
