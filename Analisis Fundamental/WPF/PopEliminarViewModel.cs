using MvvmFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WPF
{
    public class PopEliminarViewModel : Screen
    {
        #region PROPIEDADES
        /// <summary>
        /// Nombre del elemento a eliminar
        /// </summary>
        public string Elemento { get; set; }
        #endregion
        #region CONSTRUCTORES
        /// <summary>
        /// Cuadro de dialogo para confirmar si se desea eliminar un elemento o no
        /// </summary>
        /// <param name="elemento">Nombre o descripción del elemento a eliminar</param>
        public PopEliminarViewModel(string elemento)
        {
            // Inicializar los comandos
            OKCommand = new RelayCommand(OK_Execute);
            CancelCommand = new RelayCommand(Cancel_Execute);

            // Inicializar Propiedades o Fields
            Elemento = elemento;
        }
        #endregion

        #region COMMANDS
        public RelayCommand OKCommand { get; }
        /// <summary>
        /// Guarda los cambios y cierra la ventana
        /// </summary>
        private void OK_Execute() { Close(true); }

        public RelayCommand CancelCommand { get; }
        /// <summary>
        /// No guarda los cambios y cierra la ventana
        /// </summary>
        private void Cancel_Execute() { Close(false); }
        #endregion
    }
}
