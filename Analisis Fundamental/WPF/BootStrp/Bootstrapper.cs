/******************************************************************/
/***** LLENAR ESTA CLASE CONFORME A LA NECESIDAD DEL PROYECTO *****/
/******************************************************************/
using Castle.Core;
using Castle.Facilities.TypedFactory;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using DataAccess;
using Models;
using MvvmFramework;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace WPF.BootStrp
{
    /// <summary>
    /// Clase que inicializa el IoC (Arrancador=Bootstraper)
    /// </summary>
    public class Bootstrapper : BootstrapperBase
    {
        private readonly WindsorContainer container = new();

        /* Si todos los Views y ViewModels estan en el ensamblado inicial o de arranque
         * se puede dejar este metodo como esta, si hay en otros ensamblados entonces
         * agregar todos donde hayan Views y ViewModels
         * 
         * EJEMPLO:
           return new[]
            {
                typeof (MainViewModel).Assembly,
                typeof (MainView).Assembly
            };
         */
        protected override IEnumerable<Assembly> SelectAssemblies()
        {
            return base.SelectAssemblies();
        }

        /* Si existe uno o mas Views y ViewModels que no siguen el nombre estandar y
         * necesitan ser mapeados, se mapearían en este método;
         * de lo contrario se dejaria este metodo como esta
         * 
         * EJEMPLO:
           return new Dictionary<Type, Type>()
            {
                {typeof (AddStudentViewModel), typeof (AddEditStudentView)},
                {typeof (EditStudentViewModel), typeof (AddEditStudentView)}
            };
         */
        protected override Dictionary<Type, Type> SpecialViewModelMappings()
        {
            return base.SpecialViewModelMappings();
        }

        /* Actualizar este método para configurar el contenedor IoC para el modo en Ejecución */
        protected override void ConfigureForRuntime()
        {
            //Para pasar parametros primitivos por medio del "ConfigurationManager.AppSettings.Set"
            container.Kernel.Resolver.AddSubResolver(new AppSettingsConvention());

            _ = container.Register(
                                   Component.For<IRepository<Empresa, string>>().
                                         ImplementedBy<XMLRepository>().
                                         DependsOn(Dependency.OnValue<string>(AppContext.BaseDirectory + @"XML\EmpresasDB.xml")),
                                   Component.For<IEventAggregator>().
                                             ImplementedBy<EventAggregator>().
                                             LifestyleSingleton(),
                                   Component.For<IWindowManager>().
                                             ImplementedBy<WindowManager>().
                                             LifestyleSingleton()
                                  );
            RegisterViewModels();
            _ = container.AddFacility<TypedFactoryFacility>();
        }

        /* Actualizar este método para configurar el contenedor IoC para el modo en Diseño */
        protected override void ConfigureForDesignTime()
        {
            //Para pasar parametros primitivos por medio del "ConfigurationManager.AppSettings.Set"
            container.Kernel.Resolver.AddSubResolver(new AppSettingsConvention());

            _ = container.Register(
                                   Component.For<IRepository<Empresa, string>>()
                                            .ImplementedBy<DesignTimeRepository>(),
                                   Component.For<IEventAggregator>().
                                             ImplementedBy<EventAggregator>().
                                             LifestyleSingleton(),
                                   Component.For<IWindowManager>().
                                             ImplementedBy<WindowManager>().
                                             LifestyleSingleton()
                                  );
            RegisterViewModels();
            _ = container.AddFacility<TypedFactoryFacility>();
        }

        /*NO MODIFICAR, codificado para funcionar con Castle.Windsor*/
        private void RegisterViewModels()
        {
            _ = container.Register(
                                   Classes.FromAssembly(typeof(MainViewModel).Assembly).
                                   Where(x => x.Name.EndsWith("ViewModel", StringComparison.Ordinal)).
                                   Configure(x => x.LifeStyle.Is(LifestyleType.Transient)));
        }

        /*NO MODIFICAR, codificado para funcionar con Castle.Windsor*/
        protected override object GetInstance(Type service, string key)
        {
            return string.IsNullOrWhiteSpace(key)
                ? container.Resolve(service)
                : container.Resolve(key, service);
        }
    }
}
