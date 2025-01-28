using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WPF.Behaviors
{
    /// <summary>
    /// Behavior que al abrir el ComboBox guarda el Elemento que tenia el foco
    /// para después de cerrar el ComboBox fijar el Foco a ese Elemento
    /// </summary>
    public class FocusLastElementBehavior
    {
        #region Attached Dependency Property
        public static bool GetEnable(ComboBox obj)
        {
            return (bool)obj.GetValue(EnableProperty);
        }

        public static void SetEnable(ComboBox obj, bool value)
        {
            obj.SetValue(EnableProperty, value);
        }

        // Using a DependencyProperty as the backing store for Enable.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty EnableProperty =
            DependencyProperty.RegisterAttached("Enable",
                                                typeof(bool),
                                                typeof(FocusLastElementBehavior),
                                                new PropertyMetadata(false,OnEnableChange));
        #endregion

        #region FIELDS
        private static IInputElement? focusedElement;
        #endregion

        /// <summary>
        /// Suscribe un método ANONIMO al evento 'DropDownOpened' y 'DropDownClosed'
        /// </summary>
        /// <param name="d">Objeto que cambio la propiedad</param>
        /// <param name="e">Propiedad Enable que indica si el Behavior esta habilitado o no</param>
        private static void OnEnableChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is not bool enable) { return; }
            if (d is ComboBox comboBox && enable)
            {
                comboBox.DropDownOpened += (s, args) =>
                {
                    focusedElement = Keyboard.FocusedElement;
                };

                comboBox.DropDownClosed += (s, args) =>
                {
                    _ = focusedElement?.Focus();
                };
            }
        }
    }
}

/* COMO USAR

    1.- Definir el namespace
            xmlns:be="clr-namespace:WPF.Behaviors"

    2.- Usarlo como una porpiedad extra
            <TextBox be:FocusLastElementBehavior.Enable="True"/>
 */