using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WPF.Behaviors
{
    /// <summary>
    /// Comportamiento (Behaviour) que permite o no al usuario ingresar solo dígitos.
    /// [ONLY for <see cref="TextBox"/>].
    /// </summary>
    public static class DigitsOnlyBehavior
    {
        #region Attached Dependency Property
        public static bool GetIsDigitOnly(TextBox obj)
        {
            return (bool)obj.GetValue(IsDigitOnlyProperty);
        }

        public static void SetIsDigitOnly(TextBox obj, bool value)
        {
            obj.SetValue(IsDigitOnlyProperty, value);
        }

        // Using a DependencyProperty as the backing store for IsDigitOnly.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsDigitOnlyProperty =
            DependencyProperty.RegisterAttached("IsDigitOnly", typeof(bool), typeof(DigitsOnlyBehavior), new PropertyMetadata(false, OnIsDigitOnlyChanged));
        #endregion

        /// <summary>
        /// Suscribe el método <see cref="BlockNonDigitCharacters"/> al evento 'PreviewTextInput '
        /// si la propiedad IsDigitOnly cambia a TRUE, si cambia a FALSE la desuscribe.
        /// </summary>
        /// <param name="d">Objeto que cambio la propiedad</param>
        /// <param name="e">Propiedad IsDigitOnly</param>
        private static void OnIsDigitOnlyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            // ignoring error checking
            TextBox? textBox = (TextBox)d;
            bool isDigitOnly = (bool)e.NewValue;

            if (isDigitOnly)
            {
                textBox.PreviewTextInput += BlockNonDigitCharacters;
            }
            else
            {
                textBox.PreviewTextInput -= BlockNonDigitCharacters;
            }
        }

        /// <summary>
        /// Bloquea el input del usuario si el input NO es un digito
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void BlockNonDigitCharacters(object sender, TextCompositionEventArgs e)
        {
            e.Handled = e.Text.Any(ch => !(char.IsDigit(ch)
                                || ch == '.'
                                || ch == '-'
                                || ch == '+'
                                || ch == '*'
                                || ch == '/'));
        }
    }
}

/* COMO USAR

    1.- Definir el namespace
            xmlns:be="clr-namespace:WPF.Behaviors"

    2.- Usarlo como una porpiedad extra
            <TextBox be:DigitsOnlyBehavior.IsDigitOnly="True"/>
 */