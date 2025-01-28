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

namespace WPF
{
    /// <summary>
    /// Interaction logic for InfoPorAnoView.xaml
    /// </summary>
    public partial class InfoPorAnoView : UserControl
    {
        public InfoPorAnoView()
        {
            InitializeComponent();
        }

        private void ScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            Encabezado_sv.ScrollToHorizontalOffset(e.HorizontalOffset);
        }

        private void Columnas_Click(object sender, RoutedEventArgs e)
        {
            bool flag = (sender as Button)?.Content.ToString() == "Todas";

            EnableColAno1.IsChecked = flag;
            EnableColAno2.IsChecked = flag;
            EnableColAno3.IsChecked = flag;
            EnableColAno4.IsChecked = flag;
            EnableColAno5.IsChecked = flag;
            EnableColAno6.IsChecked = flag;
            EnableColAno7.IsChecked = flag;
            EnableColAno8.IsChecked = flag;
            EnableColAno9.IsChecked = flag;
        }

        private void RowUnidades_Click(object sender, RoutedEventArgs e)
        {
            bool flag = (sender as Button)?.Content.ToString() == "Todas";

            EnableRowNumUnidades.IsChecked = flag;
            EnableRowCrecimientoUnidades.IsChecked = flag;
            EnableRowCrecimientoUnidadesP.IsChecked = flag;
        }

        private void RowEstResult_Click(object sender, RoutedEventArgs e)
        {
            bool flag = (sender as Button)?.Content.ToString() == "Todas";

            EnableRowIngresos.IsChecked = flag;
            EnableRowIngUnidad.IsChecked = flag;
            EnableRowCrecIngP.IsChecked = flag;
            EnableRowCostoVentas.IsChecked = flag;
            EnableRowUBruta.IsChecked = flag;
            EnableRowMargenBruto.IsChecked = flag;
            EnableRowGastosOp.IsChecked = flag;
            EnableRowUOperativa.IsChecked = flag;
            EnableRowMargenOp.IsChecked = flag;
            EnableRowGastosFin.IsChecked = flag;
            EnableRowUAImpuestos.IsChecked = flag;
            EnableRowImpuestos.IsChecked = flag;
            EnableRowUNeta.IsChecked = flag;
            EnableRowUAccionistas.IsChecked = flag;
            EnableRowCrecUtP.IsChecked = flag;
            EnableRowMargenNeto.IsChecked = flag;
        }

        private void RowFibras_Click(object sender, RoutedEventArgs e)
        {
            bool flag = (sender as Button)?.Content.ToString() == "Todas";

            EnableRowIngresosProp.IsChecked = flag;
            EnableRowNOI.IsChecked = flag;
            EnableRowNOIAccion.IsChecked = flag;
            EnableRowMargenNOI.IsChecked = flag;
            EnableRowFFO.IsChecked = flag;
            EnableRowFFOAccion.IsChecked = flag;
            EnableRowMargenFFO.IsChecked = flag;
            EnableRowAFFO.IsChecked = flag;
            EnableRowAFFOAccion.IsChecked = flag;
            EnableRowMargenAFFO.IsChecked = flag;
        }

        private void RowSitFin_Click(object sender, RoutedEventArgs e)
        {
            bool flag = (sender as Button)?.Content.ToString() == "Todas";

            EnableRowEfectivo.IsChecked = flag;
            EnableRowDeuda.IsChecked = flag;
            EnableRowActivosC.IsChecked = flag;
            EnableRowPasivosC.IsChecked = flag;
            EnableRowActivosT.IsChecked = flag;
            EnableRowPasivosT.IsChecked = flag;
            EnableRowCapital.IsChecked = flag;
            EnableRowCrecCapP.IsChecked = flag;
            EnableRowACPC.IsChecked = flag;
            EnableRowATPT.IsChecked = flag;
            EnableRowPTAT.IsChecked = flag;
            PAcidoEnableRow.IsChecked = flag;
            EnableRowEfDeuda.IsChecked = flag;
            EnableRowEfCapital.IsChecked = flag;
            EnableRowDeudaAT.IsChecked = flag;
            EnableRowDeudaCapital.IsChecked = flag;
        }

        private void RowFlujoEf_Click(object sender, RoutedEventArgs e)
        {
            bool flag = (sender as Button)?.Content.ToString() == "Todas";

            EnableRowEfInicio.IsChecked = flag;
            EnableRowFOperaciones.IsChecked = flag;
            EnableRowFInversiones.IsChecked = flag;
            EnableRowFFinanciamiento.IsChecked = flag;
            EnableRowEfFinal.IsChecked = flag;
        }

        private void RowAcciones_Click(object sender, RoutedEventArgs e)
        {
            bool flag = (sender as Button)?.Content.ToString() == "Todas";

            EnableRowAccCirc.IsChecked = flag;
            EnableRowPrecio.IsChecked = flag;
            EnableRowCapMerc.IsChecked = flag;
            EnableRowUPA.IsChecked = flag;
            EnableRowPU.IsChecked = flag;
            EnableRowValLibro.IsChecked = flag;
            EnableRowPVL.IsChecked = flag;
            EnableRowDividendos.IsChecked = flag;
            EnableRowRendDividendos.IsChecked = flag;
        }
    }
}
