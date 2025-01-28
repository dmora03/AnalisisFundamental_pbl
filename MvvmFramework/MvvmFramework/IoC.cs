using System;
using System.Collections.Generic;
using System.Linq;

// INCOMPLETO: Field-BuildUp, y Para que son las llaves?
/*
Clases necesarias para usar esta:
- Ninguna
 */
namespace MvvmFramework
{
    /// <summary>
    /// Clase definida para brindar la capacidad de usar cualquier contenedor IoC (Inversion of Control) que se desee.
    /// </summary>
    public static class IoC
    {
        #region FIELDS
        /// <summary>
        /// ???Passes an existing instance to the IoC container to enable dependencies to be injected.
        /// </summary>
        internal static Action<object> BuildUp =
            instance => { throw new InvalidOperationException("IoC.BuildUp is not initialized."); };

        /// <summary>
        /// Encapsula un metodo que regresa una instancia del contenedor de un tipo dado y opcionalmente una llave dada
        /// <para>
        /// Parametros:
        /// <br>-> <see cref="Type"/>: Tipo a resolver</br>
        /// <br>-> <see cref="string"/>: Lave a buscar</br>
        /// </para>
        /// <para>
        /// Regresa:
        /// <br>-> <see cref="object"/>: Instancia encontrada en el contenedor</br>
        /// </para></summary>
        internal static Func<Type, string, object> GetInstance =
            (service, key) => { throw new InvalidOperationException("IoC.GetInstance is not initialized."); };

        /// <summary>
        /// Encapsula un metodo que regresa todas las intancias de un tipo en particular
        /// <para>
        /// Parametros:
        /// <br>-> <see cref="Type"/>: Tipo que se busca</br>
        /// </para>
        /// <para>
        /// Regresa:
        /// <br>-> <see cref="IEnumerable{object}"/>: Todas las intancias del tipo deseado</br>
        /// </para>
        /// </summary>
        internal static Func<Type, IEnumerable<object>> GetAllInstances =
            service => { throw new InvalidOperationException("IoC.GetAllInstances is not initialized."); };
        #endregion

        #region METODS
        /// <summary>
        /// Obtiene una intancia del contenedor en base a un tipo dado y opcionalemente a una llave dada.
        /// </summary>
        /// <typeparam name="T">El Tipo a resolver.</typeparam>
        /// <param name="key">La Key a buscar???.</param>
        /// <returns>La instancia resuelta.</returns>
        public static T Get<T>(string key = null)
        {
            return (T)GetInstance(typeof(T), key);
        }

        /// <summary>
        /// Obtiene una instancia del contenedor en base a un Tipo dado y opcionalmente a una llave dada.
        /// </summary>
        /// <param name="type">El Type a resolver.</param>
        /// <param name="key">La Key a buscar???.</param>
        /// <returns>La instancia.</returns>
        public static object Get(Type type, string key = null)
        {
            return GetInstance(type, key);
        }

        /// <summary>
        /// Obtiene todas las intancias de un Tipo en parituclar.
        /// </summary>
        /// <typeparam name="T">El Tipo a resolver.</typeparam>
        /// <returns>Las instancias resueltas.</returns>
        public static IEnumerable<T> GetAll<T>()
        {
            return GetAllInstances(typeof(T)).Cast<T>();
        }
        #endregion
    }
}