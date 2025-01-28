using System;
using System.Collections.Generic;

// BIEN DOCUMENTADO Y ENTENDIDO
/*
Clases necesarias para usar esta:
- Ninguna
 */
namespace MvvmFramework
{
    /// <summary>
    /// Clase para mapear los ViewModels con sus Views los cuales no cumplen con el 
    /// nombre estandar para ser mapeados automaticamente con el ViewModelLocator
    /// </summary>
    public static class ViewModelMappings
    {
        /// <summary>
        /// Un <see cref="Dictionary{TKey, TValue}"/> donde
        /// TKey es un ViewModel y el TValue es el View que le corresponde
        /// </summary>
        public static Dictionary<Type, Type> Mappings { get; set; }
    }
}