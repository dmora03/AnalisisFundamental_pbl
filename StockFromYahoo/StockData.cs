using System;
using System.Linq;
using System.Threading.Tasks;
using YahooFinanceApi;

namespace StockFromYahoo
{
    public class StockData
    {
        public static async Task<int> GetStockData(string symbol, DateTime startDate, DateTime endDate)
        {
            // Por seguridad se usa un TRY en tareas asincronas,
            // por si la tarea ayncrona falla o nunca termina de ejecutarse
            try
            {
                var historic_data = await Yahoo.GetHistoricalAsync(symbol, startDate, endDate);
                var security = await Yahoo.Symbols(symbol).Fields(Field.LongName).QueryAsync();
                var ticker = security[symbol];
                var companyName = ticker[Field.LongName];
                for (int i = 0; i < historic_data.Count; i++)
                {
                    Console.WriteLine($"{companyName} Closing price on: " +
                        $"{historic_data.ElementAt(i).DateTime.Month}/{historic_data.ElementAt(i).DateTime.Day}/{historic_data.ElementAt(i).DateTime.Year}: " +
                        $"${Math.Round(historic_data.ElementAt(i).Close, 2)}");
                }
                return 1;
            }
            catch (Exception)
            {
                //Console.WriteLine("Failed to get symbol:" + symbol);
                return 0;
            }
        }


        /// <summary>
        /// Get a sepecyfied stock at what Close at a specified date
        /// </summary>
        /// <param name="symbol">Symbol of the stock to search</param>
        /// <param name="date">Date to get the Close value of the day</param>
        /// <returns>The Close value</returns>
        public static async Task<double> GetStockCloseAt(string symbol, DateTime date)
        {
            // Artificial delay to show responsiveness. (No se porque sin esto no funciona en WPF)
            await Task.Delay(TimeSpan.FromMilliseconds(1)).ConfigureAwait(false);
            // Fin de codigo extra para que funcione en WPF

            // Por seguridad se usa un TRY en tareas asincronas,
            // por si la tarea ayncrona falla o nunca termina de ejecutarse
            try
            {
                var historic_data = await Yahoo.GetHistoricalAsync(symbol, date.AddDays(-7), date.AddDays(1)).ConfigureAwait(false);
                return (double)historic_data[historic_data.Count - 1].Close;
            }
            catch (Exception ex) { return -1; }
        }
    }
}
