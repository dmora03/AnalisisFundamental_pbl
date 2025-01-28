using MvvmFramework;
using System;
using System.Diagnostics;
using WPF.EventMessages;

namespace WPF
{
    public class ShellViewModel : Validatable, IHandle<SwitchToVm>
    {

        #region PROPIEDADES
        /// <summary>
        /// Contiene la vista que actualmente se esta mostrando
        /// </summary>
        public object CurrentViewModel
        {
            get => currentViewModel;
            set => SetProperty(ref currentViewModel, value);
        }
        private object currentViewModel = null!;

        /// <summary>
        /// Contiene la version de la aplicación
        /// </summary>
        public string Version
        {
            get => version;
            set => SetProperty(ref version, value);
        }
        private string version = null!;
        #endregion

        #region CONSTRUCTORES
        public ShellViewModel(IEventAggregator aggregator)
        {
            aggregator.Subscribe(this);
            CurrentViewModel = IoC.Get<MainViewModel>();

            // Inicializar los comandos
            ViewNavigatorCommand = new(ViewNavigator_Execute);

            // Inicializar Propiedades o Fields
            string fn = Environment.CurrentDirectory + @"\WPF.exe";
            FileVersionInfo fv = FileVersionInfo.GetVersionInfo(fn);
            Version = $"Version: {fv.FileVersion}";
        }
        #endregion

        #region Implementacion de la Interace IHandle
        public void Handle(SwitchToVm message)
        {
            ViewNavigator_Execute(message.ViewModel);
        }
        #endregion

        #region COMMANDS

        public RelayCommand<Type> ViewNavigatorCommand { get; }
        /// <summary>
        /// Actualiza o cambia la vista que se muestra en el area principal del ShellView
        /// </summary>
        /// <param name="viewModel">ViewModel que se mostrara en el area principal del ShellView</param>
        private void ViewNavigator_Execute(Type viewModel)
        {
            if (viewModel is null) { throw new ArgumentNullException(nameof(viewModel), "Se paso NULL como ViewModel a mostrar"); }
            CurrentViewModel = IoC.Get(viewModel);
        }
        #endregion

        #region EVENTOS DE WPF
        /// <summary>
        /// Evento lanzado al cerrar la aplicación
        /// </summary>
        public void OnClosing(object sender, System.ComponentModel.CancelEventArgs e)
        { Properties.Settings.Default.Save(); }
        #endregion
    }
}
