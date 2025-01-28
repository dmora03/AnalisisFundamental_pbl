using System;
using System.Diagnostics;
using System.Windows.Input;

// BIEN DOCUMENTADO Y ENTENDIDO
/* Clases necesarias para usar esta:
 * - Ninguna
 */

namespace MvvmFramework
{
    /// <summary>
    /// Implementación generica reutilizable de la interfaz ICommand SIN parámetros de entrada
    /// </summary>
    public class RelayCommand : ICommand
    {
        /// <summary>
		/// Encapsula un método que no tiene parámetros y devuelve un valor del tipo bool
		/// </summary>
		private readonly Func<bool> canExecute;
        /// <summary>
        /// Encapsula un método que no tiene parámetros y no devuelve un valor.
        /// </summary>
        private readonly Action execute;
        /// <summary>
        /// Bandera que indica se deja que el <see cref="CommandManager"/> decida cuando ejecutar el CanExecute (TRUE), o si se hace de forma manual (FALSE)
        /// </summary>
        private readonly bool enableRequerySuggested;

        #region CONSTRUCTOR
        /// <summary>
        /// Inicializa una nueva instancia de la clase RelayCommand con el método especificado.
        /// </summary>
        /// <param name="execute">Método que se llamará cuando se invoca el comando</param>
        public RelayCommand(Action execute) : this(execute, null, false) { }

        /// <summary>
		/// Inicializa una nueva instancia de la clase RelayCommand con el método y verificador especificados.
		/// </summary>
		/// <param name="execute">Método que se llamará cuando se invoca el comando</param>
		/// <param name="canExecute">Método que determina si el comando se puede ejecutar en su estado actual</param>
        /// <param name="enableRequerySuggested">True si se deja que el <see cref="CommandManager"/> decida cuando ejecutar el CanExecute, False si se hara de forma manual</param>
        public RelayCommand(Action execute, Func<bool> canExecute, bool enableRequerySuggested = true)
        {
            this.execute = execute ?? throw new ArgumentNullException(nameof(execute));
            this.canExecute = canExecute;
            this.enableRequerySuggested = enableRequerySuggested;
        }
        #endregion

        #region ICommand
        /// <summary>
        /// Ocurre cuando ocurren cambios que afectan si el comando debe ejecutarse o no.
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add
            {
                if (enableRequerySuggested) { CommandManager.RequerySuggested += value; }
                else { _canExecuteChanged += value; }
            }
            remove
            {
                if (enableRequerySuggested) { CommandManager.RequerySuggested -= value; }
                else { _canExecuteChanged -= value; }
            }
        }
        private EventHandler _canExecuteChanged;

        /// <summary>
		/// Define el método que determina si el comando se puede ejecutar en su estado actual.
		/// </summary>
		/// <param name="parameter">Datos utilizados por el comando. Si no requiere datos, este objeto se puede establecer en NULL</param>
		/// <returns>Verdadero si este comando se puede ejecutar; de lo contrario, falso.</returns>
        [DebuggerStepThrough]
        public bool CanExecute(object parameter) { return canExecute == null || canExecute(); }

        /// <summary>
		/// Define el método que se llamará cuando se invoca el comando.
		/// </summary>
		/// <param name="parameter">Datos utilizados por el comando. Si no requiere datos, este objeto se puede establecer en NULL</param>
        public void Execute(object parameter) { execute(); }
        #endregion

        /// <summary>
        /// Provoca que el evento <see cref="CanExecuteChanged"/> sea lanzado
        /// </summary>
        public void RiseCanExecuteChanged()
        {
            _canExecuteChanged(this, EventArgs.Empty);
        }
    }

    /// <summary>
    /// Implementación generica reutilizable de la interfaz ICommand CON UN parámetro de entrada
    /// </summary>
    /// <typeparam name="T">El tipo de parametro de entrada que recibira el comando</typeparam>
    public class RelayCommand<T> : ICommand
    {
        /// <summary>
		/// Encapsula un método que tiene un parámetro del tipo T y devuelve un valor del tipo bool
		/// </summary>
		private readonly Func<T, bool> canExecute;
        /// <summary>
        /// Encapsula un método que tiene un solo parámetro del tipo T y no devuelve un valor.
        /// </summary>
        private readonly Action<T> execute;
        /// <summary>
        /// Bandera que indica se deja que el <see cref="CommandManager"/> decida cuando ejecutar el CanExecute (TRUE), o si se hace de forma manual (FALSE)
        /// </summary>
        private readonly bool enableRequerySuggested;

        #region CONSTRUCTOR
        /// <summary>
		/// Inicializa una nueva instancia de la clase RelayCommand con el método especificado.
		/// </summary>
		/// <param name="execute">Método, con un parámetro del tipo <typeparamref name="T"/>, que se llamará cuando se invoca el comando</param>
        public RelayCommand(Action<T> execute) : this(execute, null, false) { }

        /// <summary>
        /// Inicializa una nueva instancia de la clase RelayCommand con el método y verificador especificados.
        /// </summary>
        /// <param name="execute">Método, con un parámetro del tipo <typeparamref name="T"/>, que se llamará cuando se invoca el comando</param>
        /// <param name="canExecute">Método, con un parámetro del tipo <typeparamref name="T"/>, que determina si el comando se puede ejecutar en su estado actual</param>
        /// <param name="enableRequerySuggested">True si se deja que el <see cref="CommandManager"/> decida cuando ejecutar el CanExecute, False si se hara de forma manual</param>
        public RelayCommand(Action<T> execute, Func<T, bool> canExecute, bool enableRequerySuggested = true)
        {
            this.execute = execute ?? throw new ArgumentNullException(nameof(execute));
            this.canExecute = canExecute;
            this.enableRequerySuggested = enableRequerySuggested;
        }
        #endregion

        #region ICommand
        /// <summary>
		/// Ocurre cuando ocurren cambios que afectan si el comando debe ejecutarse o no.
		/// </summary>
        public event EventHandler CanExecuteChanged
        {
            add
            {
                if (enableRequerySuggested) { CommandManager.RequerySuggested += value; }
                else { _canExecuteChanged += value; }
            }
            remove
            {
                if (enableRequerySuggested) { CommandManager.RequerySuggested -= value; }
                else { _canExecuteChanged -= value; }
            }
        }
        private EventHandler _canExecuteChanged;

        /// <summary>
		/// Define el método que determina si el comando se puede ejecutar en su estado actual.
		/// </summary>
		/// <param name="parameter">Datos utilizados por el comando del tipo <typeparamref name="T"/>. Si no requiere datos, este objeto se puede establecer en NULL</param>
		/// <returns>Verdadero si este comando se puede ejecutar; de lo contrario, falso.</returns>
        [DebuggerStepThrough]
        public bool CanExecute(object parameter) { return canExecute == null || canExecute((T)parameter); }

        /// <summary>
		/// Define el método que se llamará cuando se invoca el comando.
		/// </summary>
		/// <param name="parameter">Datos utilizados por el comando del tipo <typeparamref name="T"/>. Si no requiere datos, este objeto se puede establecer en NULL</param>
        public void Execute(object parameter) { execute((T)parameter); }
        #endregion

        /// <summary>
        /// Provoca que el evento <see cref="CanExecuteChanged"/> sea lanzado
        /// </summary>
        public void RiseCanExecuteChanged()
        {
            _canExecuteChanged(this, EventArgs.Empty);
        }
    }
}
