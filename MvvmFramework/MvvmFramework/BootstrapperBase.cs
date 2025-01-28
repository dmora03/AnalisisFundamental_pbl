using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;

// BIEN DOCUMENTADO Y ENTENDIDO
/*
Clases necesarias para usar esta:
- Execution             OK
- AssemblySource        OK
- IoC                   OK
- ViewModelMappings     OK
 */
namespace MvvmFramework
{
    /// <summary>
    /// Clase abstracta que se utiliza para trasladar la responsabilidad 
    /// de la configuración inicial de la aplicación a la raíz de la aplicación
    /// </summary>
    public abstract class BootstrapperBase
    {
        #region CONSTRUCTOR
        /// <summary>
        /// Inicia el Arrancador (Bootstrapper)
        /// - En App.xaml en el nodo XML Application agregamos el evento Startup y 
        ///   desde este evento instanciamos esta clase. Ej. '_ = new BootStrp.Bootstrapper();'
        /// - Tambien en los ViewModels para Desgin-Time se agrega una instancia como field 
        ///   para poder acceder al IoC en tiempo de diseño. Ej. 'private readonly BootStrp.Bootstrapper bootstrapper = new();'
        /// </summary>
        public BootstrapperBase()
        {
            AssemblySource.Instance.AddRange(SelectAssemblies());
            ViewModelMappings.Mappings = SpecialViewModelMappings();

            if (Execution.InDesignMode) { ConfigureForDesignTime(); }
            else { ConfigureForRuntime(); }

            IoC.GetInstance = GetInstance;
        }
        #endregion

        /// <summary>
        /// Método que usa un "Inversion of Control container" (Ej. Castle.Windsor)
        /// para crear la instancia de un tipo especificado y opcionalmente una llave dada
        /// </summary>
        /// <param name="service">Tipo de objeto a obtener</param>
        /// <param name="key">Llave a buscar???</param>
        /// <returns>Instancia del tipo deseado</returns>
        protected abstract object GetInstance(Type service, string key);


        /// <summary>
        /// Lista de <see cref="Assembly"/> donde estan los Views y ViewModels
        /// </summary>
        /// <returns>Lista de <see cref="Assembly"/> donde estan los Views y ViewModels</returns>
        protected virtual IEnumerable<Assembly> SelectAssemblies()
        {
            if (Execution.InDesignMode)
            {
                AppDomain appDomain = AppDomain.CurrentDomain;
                Assembly[] assemblies = appDomain.GetType()
                    .GetMethod("GetAssemblies")
                    .Invoke(appDomain, null) as Assembly[] ?? Array.Empty<Assembly>();

                Assembly applicationAssembly = assemblies.LastOrDefault(ContainsApplicationClass);
                return applicationAssembly == null ? Array.Empty<Assembly>() : new[] { applicationAssembly };
            }
            Assembly entryAssembly = Assembly.GetEntryAssembly();
            return entryAssembly == null ? Array.Empty<Assembly>() : new[] { entryAssembly };
        }

        /// <summary>
        /// Diccionario para mapear los ViewModels con sus Views, los cuales no cumplen con el
        /// nombre estandar para ser mapeados automaticamente por el ViewModelLocator.
        /// <para>
        /// Ejemplo:
        /// <br>(Key=AddStudentViewModel, Value=AddEditStudentView)</br>
        /// <br>(Key=EditStudentViewModel, Value=AddEditStudentView)</br>
        /// </para>
        /// </summary>
        /// <returns>Diccionario con los pares Key=ViewModel, Value=View</returns>
        protected virtual Dictionary<Type, Type> SpecialViewModelMappings() { return null; }

        /// <summary>
        /// Metodo responsable de configurar el contenedor IoC para el modo en Ejecución
        /// </summary>
        protected virtual void ConfigureForRuntime() { }
        /// <summary>
        /// Metodo responsable de configurar el contenedor IoC para el modo en Diseño
        /// </summary>
        protected virtual void ConfigureForDesignTime() { }




        /// <summary>
        /// Regresa TRUE si el <see cref="Assembly"/> dado contiene la classe <see cref="Application"/>, de lo contrario FALSE
        /// </summary>
        /// <param name="assembly">Ensamblado donde se buscara la clase <see cref="Application"/></param>
        /// <returns>True si contien la clase, de lo contrario False</returns>
        private static bool ContainsApplicationClass(Assembly assembly)
        {
            bool containsApp = false;

            try
            {
                containsApp = assembly.EntryPoint != null &&
                              assembly.GetExportedTypes().Any(t => t.IsSubclassOf(typeof(Application)));
            }
            catch { }
            return containsApp;
        }
    }
}