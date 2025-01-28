using System.Windows;
// BIEN DOCUMENTADO Y ENTENDIDO
/*
Clases necesarias para usar esta:
- WindowManager
 */

namespace MvvmFramework
{
    /// <summary>
    /// Clase que encapsula el método Close que puede establecer o no el 
    /// DialogResult y cierra la ventana a través de la interfaz IViewAware.
    /// </summary>
    public class Screen : IViewAware
    {
        public Window View { get; set; }

        /// <summary>
        /// Cierra la ventana estableciendo el DialogResult con el valor dado
        /// SOLO si la ventana se abrio como Dialog, de lo contrario se ignora el parametro
        /// </summary>
        /// <param name="result">Valor a pasar al DialogResult</param>
        public void Close(bool? result = null)
        {
            try { View.DialogResult = result; }
            catch (System.InvalidOperationException) { }
            finally { View.Close(); }
        }
    }
}