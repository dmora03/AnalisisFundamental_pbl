using System;

namespace WPF.EventMessages
{
    /// <summary>
    /// Clase usada como mensaje en el <see cref="MvvmFramework.IEventAggregator"/> para indicar cual ViewModel es el activo
    /// </summary>
    public class SwitchToVm
    {
        /// <summary>
        /// ViewModel actualmente activo o visible
        /// </summary>
        public Type ViewModel { get; private set; }

        /// <summary>
        /// Constructor de la clase
        /// </summary>
        /// <param name="viewModel">ViewModel que sera el activo o visible</param>
        public SwitchToVm(Type viewModel) { ViewModel = viewModel ?? throw new ArgumentNullException(nameof(viewModel)); }
    }
}
