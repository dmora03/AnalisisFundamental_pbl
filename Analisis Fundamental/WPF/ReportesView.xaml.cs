using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace WPF
{
    /// <summary>
    /// Interaction logic for ReportesView.xaml
    /// </summary>
    public partial class ReportesView : UserControl
    {
        public ReportesView()
        {
            InitializeComponent();
        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            long difNetYahooSeconds = 62135510400;
            long oneDayYahoo = 86400;

            string id = e.Uri.OriginalString;
            DateTime dateTime = (DateTime)((FrameworkContentElement)sender).Tag;

            long yahooDate = (dateTime.Ticks / 10000000) - difNetYahooSeconds;
            long yahooDatePr1 = yahooDate - (oneDayYahoo * 7);
            long yahooDateDiv1 = yahooDate - (oneDayYahoo * 366);
            long yahooDateDiv2 = yahooDate + (oneDayYahoo * 62);
            string uriPrecio = $"https://finance.yahoo.com/quote/{id}/history?period1={yahooDatePr1}&period2={yahooDate}&interval=1d&filter=history&frequency=1d&includeAdjustedClose=true";
            string uriDividendo = $"https://finance.yahoo.com/quote/{id}/history?period1={yahooDateDiv1}&period2={yahooDateDiv2}&interval=capitalGain%7Cdiv%7Csplit&filter=div&frequency=1d&includeAdjustedClose=true";

            // for .NET Core you need to add UseShellExecute = true see https://docs.microsoft.com/dotnet/api/system.diagnostics.processstartinfo.useshellexecute#property-value
            //_ = Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            //_ = Process.Start(new ProcessStartInfo(uriDividendo) { UseShellExecute = true });     //ABRE LA PAGIA DE DIVIDENDO
            _ = Process.Start(new ProcessStartInfo(uriPrecio) { UseShellExecute = true });          //ABRE LA PAGINA DEL HISTORIAL DE PRECIOS
            e.Handled = true;
            _ = Precio_txt.Focus();
        }

        private void CommandBinding_Executed(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
        {

        }
    }
}
