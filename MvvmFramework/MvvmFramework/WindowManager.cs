using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;
// BIEN DOCUMENTADO Y ENTENDIDO
/* Clases necesarias para usar esta:
 * - AssemblySource       OK
 */

namespace MvvmFramework
{

    internal interface IViewAware
    {
        Window View { get; set; }
    }

    /// <summary>
    /// Interfaz que da la posibilidad de poder abrir ventanas 
    /// </summary>
    public interface IWindowManager
    {
        /// <summary>
        /// Abre una ventana y regresa solo cuando se cierra la ventana recién abierta.
        /// La ventana es el View del ViewModel deseado
        /// </summary>
        /// <param name="rootModel">Instancia del ViewModel</param>
        /// <returns>Valor que especifica si la actividad se aceptó (verdadero) o se canceló (falso).</returns>
        bool? ShowDialog(object rootModel);

        /// <summary>
        /// Abre una ventana y regresa sin esperar a que se cierre la ventana recién abierta.
        /// La ventana es el View del ViewModel deseado
        /// </summary>
        /// <param name="rootModel">Instancia del ViewModel</param>
        void ShowWindow(object rootModel);
    }

    /// <summary>
    /// Clase que implementa la interfaz <see cref="IWindowManager"/> para poder abrir ventanas
    /// </summary>
    public class WindowManager : IWindowManager
    {
        // NO DOCUMENTAR: La documentación la hereda de la interface
        public bool? ShowDialog(object rootModel)
        {
            return CreateWindow(rootModel, true).ShowDialog();
        }

        // NO DOCUMENTAR: La documentación la hereda de la interface
        public void ShowWindow(object rootModel)
        {
            CreateWindow(rootModel, false).Show();
        }


        protected virtual Window CreateWindow(object rootModel, bool isDialog)
        {
            Window window = LocateWindow(rootModel);
            var view = EnsureWindow(rootModel, window, isDialog);

            return view;
        }

        /// <summary>
        /// Se infiere el nombre del VIEW en base a la intancia VIEW MODEL dada.
        /// </summary>
        /// <param name="rootModel">Instancia del ViewModel</param>
        /// <returns></returns>
        private static Window LocateWindow(object rootModel)
        {
            Type viewModelType = rootModel.GetType();
            string viewFullName = viewModelType.FullName.Replace("ViewModel", "View");

            #region CODIGO PARA BUSCAR EL VIEW DESEADO EN LOS ASSAMBLY
            List<Type> allExportedTypes = new();
            foreach (Assembly assembly in AssemblySource.Instance)
            {
                allExportedTypes.AddRange(assembly.GetExportedTypes());
            }
            Type viewType = allExportedTypes.FirstOrDefault(x => x.FullName == viewFullName)            // Si NO es NULL guarda el View encontrado
                            ?? ViewModelMappings.Mappings.First(x => x.Key == viewModelType).Value;     // Si es NULL busca el View en la clase de Mapeos especiales
            #endregion

            object view = Activator.CreateInstance(viewType);
            Window window = (Window)view;
            window.DataContext = rootModel;

            if (rootModel is IViewAware viewAware)
            {
                viewAware.View = window;
            }

            return window;
        }

        protected virtual Window EnsureWindow(object model, object view, bool isDialog)
        {
            if (view is not Window window)
            {
                window = new Window
                {
                    Content = view,
                    SizeToContent = SizeToContent.WidthAndHeight
                };
                var owner = InferOwnerOf(window);
                if (owner != null)
                {
                    window.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                    window.Owner = owner;
                }
                else
                {
                    window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                }
            }
            else
            {
                var owner = InferOwnerOf(window);
                if (owner != null && isDialog)
                {
                    window.Owner = owner;
                }
            }

            return window;
        }

        protected virtual Window InferOwnerOf(Window window)
        {
            if (Application.Current == null)
            {
                return null;
            }

            var active = Application.Current.Windows.OfType<Window>()
                .Where(x => x.IsActive)
                .FirstOrDefault();
            active ??= Application.Current.MainWindow;
            return active == window ? null : active;
        }
    }
}

/********** EJEMPLO DE COMO USAR LA CLASE **********
-----------------------------------------------------------------------------------------------------
--> EN EL VIEW MODEL DONDE SE ABRIRÁN NUEVAS VENTANAS
    Pasar al constructor el IWindowManager y usar la variable interna para mostrar una ventana nueva
-----------------------------------------------------------------------------------------------------
    private readonly IWindowManager windowManager;
    public MainViewModel(IWindowManager windowManager)
    {
        this.windowManager = windowManager;
    }
    private void ShowDialogMethod()
    {
        windowManager.ShowDialog(IoC.Get<DialogViewModel>());
    }
-----------------------------------------------------------------------------------------------------
--> EN EL VIEW MODEL QUE FUNCIONA COMO VENTANA NUEVA
    Debe heredar de la clase SCREEN y usar el metodo Close() para cuando se quiera cerrar la ventana 
-----------------------------------------------------------------------------------------------------
    public class DialogViewModel : Screen
    {
        private void Accept() { Close(true); }
        private void Cancel() { Close(false); }
    }
/**/