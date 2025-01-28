using MvvmFramework;
using System;
using System.Windows.Input;
using WPF.EventMessages;

namespace WPF
{
    public class SampleViewModel
    {
        #region FIELDS
        private readonly IEventAggregator aggregator;
        private readonly IWindowManager windowManager;
        #endregion

        #region PROPIEDADES
        public string SampleText { get; set; }
        #endregion

        #region CONSTRUCTORES
        public SampleViewModel(IEventAggregator aggregator, IWindowManager windowManager, int primitiveValue)
        {
            this.aggregator = aggregator;
            this.windowManager = windowManager;

            // Inicializar los comandos
            SampleCommand = new RelayCommand(Sample_Execute, Sample_CanExecute);
            ShowWindowCommand = new RelayCommand(ShowWindow_Execute);

            SampleText = $"Muestra la vista Inicial #{primitiveValue}";
        }
        #endregion

        #region COMMANDS
        public RelayCommand SampleCommand { get; }
        private bool Sample_CanExecute()
        {
            return true; // Siempre se podra ejecutar
        }
        /// <summary>
        /// Publica el mensaje para cambiar la vista al MainView 
        /// </summary>
        private void Sample_Execute()
        {
            aggregator.Publish(new SwitchToVm(typeof(MainViewModel)));
        }

        public RelayCommand ShowWindowCommand { get; }
        /// <summary>
        /// Muestra una nueva ventana
        /// </summary>
        private void ShowWindow_Execute()
        {
            _ = windowManager.ShowDialog(IoC.Get<PopEliminarViewModel>());
        }
        #endregion
    }
}
