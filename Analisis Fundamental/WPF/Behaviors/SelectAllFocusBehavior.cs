using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WPF.Behaviors
{
    /// <summary>
    /// Behaviour que permite seleccionar todo el texto ser seleccionado el objeto.
    /// [SOLO para <see cref="TextBox"/> y <see cref="PasswordBox"/>].
    /// </summary>
    public static class SelectAllFocusBehavior
    {
        #region Attached Dependency Property
        public static bool GetEnable(DependencyObject obj)
        {
            return (bool)obj.GetValue(EnableProperty);
        }

        public static void SetEnable(DependencyObject obj, bool value)
        {
            obj.SetValue(EnableProperty, value);
        }

        // Using a DependencyProperty as the backing store for Enable.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty EnableProperty =
            DependencyProperty.RegisterAttached("Enable",
                                                typeof(bool),
                                                typeof(SelectAllFocusBehavior),
                                                new PropertyMetadata(false, OnEnableChanged));
        #endregion

        /// <summary>
        /// Suscribe unos métodos al ciertos eventos cuando
        /// la propiedad Enable cambia a TRUE, si cambia a FALSE los desuscribe.
        /// </summary>
        /// <param name="d">Objeto que cambio la propiedad</param>
        /// <param name="e">Propiedad Enable</param>
        private static void OnEnableChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not FrameworkElement frameworkElement)
            { return; }

            if (e.NewValue is not bool enable)
            { return; }

            if (enable)
            {
                frameworkElement.GotFocus += SelectAll;
                frameworkElement.PreviewMouseDown += IgnoreMouseButton;
            }
            else
            {
                frameworkElement.GotFocus -= SelectAll;
                frameworkElement.PreviewMouseDown -= IgnoreMouseButton;
            }
        }

        /// <summary>
        /// Selecciona todo el texto
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void SelectAll(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox tbox)
            { tbox.SelectAll(); }
            else if (sender is PasswordBox pbox)
            { pbox.SelectAll(); }
        }

        /// <summary>
        /// Si el objeto aún no esta enfocado, dale el foco y detiene el procesamiento
        /// posterior del evento del click del raton
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void IgnoreMouseButton(object sender, MouseButtonEventArgs e)
        {
            if (sender is not FrameworkElement frameworkElement || frameworkElement.IsKeyboardFocusWithin)
            { return; }

            _ = frameworkElement.Focus();
            e.Handled = true;
        }
    }
}

/* COMO USAR

    1.- Definir el namespace
            xmlns:be="clr-namespace:WPF.Behaviors"

    2.- Usarlo como una porpiedad extra
            <TextBox be:SelectAllFocusBehavior.Enable="True"/>
 */