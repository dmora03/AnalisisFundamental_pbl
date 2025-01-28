using MvvmFramework;
using System.Configuration;
using System.Windows.Input;
using WPF.EventMessages;

namespace WPF
{
    public class MainViewModel
    {
        #region FIELDS
        private readonly IEventAggregator aggregator;
        #endregion

        #region PROPIEDADES
        public int SampleNumber { get; set; } = 99;
        #endregion

        #region CONSTRUCTORES
        public MainViewModel(IEventAggregator aggregator)
        {
            this.aggregator = aggregator;

            // Inicializar los comandos
            SampleCommand = new RelayCommand(Sample_Execute, Sample_CanExecute);
        }
        #endregion

        #region COMMANDS
        public RelayCommand SampleCommand { get; }
        private bool Sample_CanExecute()
        {
            return true; // Siempre se podra ejecutar
        }
        /// <summary>
        /// Comando de ejemplo
        /// </summary>
        private void Sample_Execute()
        {
            // Para pasar un parametro primitivo se agrega el dato de este al AppSettings bajo el nombre del parametro
            ConfigurationManager.AppSettings.Set("primitiveValue", SampleNumber.ToString());
            aggregator.Publish(new SwitchToVm(typeof(SampleViewModel)));
        }
        #endregion
    }
}
