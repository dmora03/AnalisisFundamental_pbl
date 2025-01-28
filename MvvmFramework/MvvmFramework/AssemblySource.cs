using System.Collections.Generic;
using System.Reflection;

// BIEN DOCUMENTADO Y ENTENDIDO
/* Clases necesarias para usar esta:
 * - Ninguna
 */

namespace MvvmFramework
{
    /// <summary>
    /// Clase Singleton que contiene una lista de <see cref="Assembly"/> donde estan los Views y Viewsmodels
    /// </summary>
    public static class AssemblySource
    {
        /// <summary>
        /// Instancia Singleton de <see cref="AssemblySource"/> 
        /// que contiene una lista de <see cref="Assembly"/> donde estan los Views y ViewModels
        /// </summary>
        public static readonly List<Assembly> Instance = new();
    }
}