using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WPF.Behaviors
{
    /// <summary>
    /// Behaviour que permite seleccionar el texto entre los delimitadores especificados, las siguientes opciones estan disponibles:
    /// "{x:Null}" => Deshabilita el bahavior
    /// "" => Selecciona TODO el texto del TextBox
    /// Si es UN delimitador => Selecciona del primer delimitador al segundo encontrado o del principio al delimitador
    /// Si son DOS delimitadores => Selecciona del primer delimitador al segundo, o del principio al segundo delimitador o del primer delimitador al final
    /// [SOLO para <see cref="TextBox"/>].
    /// </summary>
    public class SelectBetweenOnFocusBehavior
    {
        #region Attached Dependency Property
        public static string GetDelimiters(TextBox obj)
        {
            return (string)obj.GetValue(DelimitersProperty);
        }

        public static void SetDelimiters(TextBox obj, string value)
        {
            obj.SetValue(DelimitersProperty, value);
        }

        // Using a DependencyProperty as the backing store for Delimiters.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DelimitersProperty =
            DependencyProperty.RegisterAttached("Delimiters", typeof(string), typeof(SelectBetweenOnFocusBehavior), new PropertyMetadata(null, OnDelemitersChanged));
        #endregion

        /// <summary>
        /// Suscribe unos métodos al ciertos eventos cuando
        /// la propiedad Delimiters cambia  a TRUE, si cambia a FALSE los desuscribe.
        /// </summary>
        /// <param name="d">Objeto que cambio la propiedad</param>
        /// <param name="e">Propiedad Enable</param>
        private static void OnDelemitersChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not TextBox textBox) { return; }

            if (e.NewValue is not string) { return; }
            else if (e.NewValue is not null)
            {
                textBox.GotFocus += SelectAll;
                textBox.PreviewMouseDown += IgnoreMouseButton;
            }
            else
            {
                textBox.GotFocus -= SelectAll;
                textBox.PreviewMouseDown -= IgnoreMouseButton;
            }
        }


        /// <summary>
        /// Selecciona el texto que esta entre los delimitadores
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void SelectAll(object sender, RoutedEventArgs e)
        {
            if (sender is not TextBox txtBox) { return; }
            char[] arr = GetDelimiters((TextBox)sender).ToCharArray();

            if (arr.Length == 0)
            {
                txtBox.SelectAll();
                return;
            }

            string str = txtBox.Text;
            int start = str.IndexOf(arr[0]);
            int end;
            if (arr.Length == 1)
            {
                end = str.IndexOf(arr[0], start + 1);
                if (end < 0)
                {
                    end = start;
                    start = -1;
                }
            }
            else
            {
                end = str.IndexOf(arr[1], start + 1);
                if (end < 0 && start > -1)
                {
                    end = str.Length;
                }

                // Logica para Ignorar delimitadores dentro de limitadores
                // Ej: Esto es un [ejemplo de [delimitadores] anidados], saludos
                int tempStart = str.IndexOf(arr[0], start + 1);
                while (tempStart > -1 && tempStart < end)
                {
                    tempStart = str.IndexOf(arr[0], tempStart + 1);
                    end = str.IndexOf(arr[1], end + 1);
                }
            }

            if (end < 0) { return; }
            txtBox.SelectionStart = start + 1;
            txtBox.SelectionLength = end - start - 1;
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
            <TextBox be:SelectBetweenOnFocusBehavior.Delimiters="{x:Null}"/>
            <TextBox be:SelectBetweenOnFocusBehavior.Delimiters="|"/>
            <TextBox be:SelectBetweenOnFocusBehavior.Delimiters="[]"/>
            <TextBox be:SelectBetweenOnFocusBehavior.Delimiters="{}"/> !!!!!!NO USAR {}
 */