using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WPF.Behaviors
{
    /// <summary>
    /// Behavior que permite fijar el Foco en un objeto dado despues de dar click a un botón
    /// SI el objeto dado es el mismo boton, entonces el foco se fija en el elemento que tenia el foco antes de dar click al boton
    /// [SOLO para <see cref="Button"/>].
    /// </summary>
    public class FocusBehavior
    {
        #region Attached Dependency Property
        public static Control GetElementToFocus(Button obj)
        {
            return (Control)obj.GetValue(ElementToFocusProperty);
        }

        public static void SetElementToFocus(Button obj, Control value)
        {
            obj.SetValue(ElementToFocusProperty, value);
        }

        // Using a DependencyProperty as the backing store for ElementToFocus.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ElementToFocusProperty =
            DependencyProperty.RegisterAttached("ElementToFocus",
                                                typeof(Control),
                                                typeof(FocusBehavior),
                                                new PropertyMetadata(null, ElementToFocusPropertyChanged));
        #endregion

        /// <summary>
        /// Suscribe un método ANONIMO al evento 'Click', donde fija el foco al objeto dado en la propiedad ElementToFocus
        /// </summary>
        /// <param name="d">Objeto que cambio la propiedad</param>
        /// <param name="e">Propiedad ElementToFocus que contiene el objeto que recibirá el foco</param>
        private static void ElementToFocusPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Button button)
            {
                // Si la propiedad se cambia varias veces, hay que prevenir suscribir el metodo al evento varias veces
                // por lo tanto antes de suscribirlo se descuscribe
                if (button == e.NewValue)
                {
                    button.PreviewMouseDown -= Button_PreviewMouseDown;
                    button.PreviewMouseDown += Button_PreviewMouseDown;
                }
                button.Click -= Button_Click;
                button.Click += Button_Click;
            }
        }
        private static void Button_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            SetElementToFocus((Button)sender, (Control)Keyboard.FocusedElement);
        }

        private static void Button_Click(object sender, RoutedEventArgs e)
        {
            Control control = GetElementToFocus((Button)sender);
            _ = control?.Focus();
        }
    }
}

/* COMO USAR

    1.- Definir el namespace
            xmlns:be="clr-namespace:WPF.Behaviors"

    2.- Usarlo como una porpiedad extra
            <Button be:FocusBehavior.ElementToFocus="{Binding ElementName=FechaDelTrimestre_dtp}"/>
            <Button be:FocusBehavior.ElementToFocus="{Binding  Mode=OneWay, RelativeSource={RelativeSource Self}}"/>
            <DatePicker x:Name="FechaDelTrimestre_dtp"/>
 */