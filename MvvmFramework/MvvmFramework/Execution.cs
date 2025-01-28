using System.ComponentModel;
using System.Windows;

// BIEN DOCUMENTADO Y ENTENDIDO
/* Clases necesarias para usar esta:
 * - Ninguna
 */

namespace MvvmFramework
{
    public static class Execution
    {
        /// <summary>
        /// Regresa TRUE si se esta ejecutando en modo Diseño, de lo contrario FALSE
        /// </summary>
        public static bool InDesignMode
        {
            get
            {
                if (inDesignMode is null)
                {
                    DependencyProperty prop = DesignerProperties.IsInDesignModeProperty;
                    inDesignMode = (bool)DependencyPropertyDescriptor.FromProperty(prop, typeof(FrameworkElement)).Metadata.DefaultValue;

                    // OPCION2
                    //inDesignMode = DesignerProperties.GetIsInDesignMode(new System.Windows.DependencyObject());
                }
                return inDesignMode.GetValueOrDefault(false);
            }
        }
        private static bool? inDesignMode;
    }
}