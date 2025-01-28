using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;

// BIEN DOCUMENTADO Y ENTENDIDO
/* Clases necesarias para usar esta:
 * - AssemblySource             OK
 */

namespace MvvmFramework
{
    public static class ViewModelLocator
    {
        #region DEPENDECY PROPERTY AutoWireViewModelProperty
        public static bool GetAutoWireViewModel(DependencyObject obj)
        {
            return (bool)obj.GetValue(AutoWireViewModelProperty);
        }
        public static void SetAutoWireViewModel(DependencyObject obj, bool value)
        {
            obj.SetValue(AutoWireViewModelProperty, value);
        }

        // Using a DependencyProperty as the backing store for AutoWireViewModel.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AutoWireViewModelProperty =
            DependencyProperty.RegisterAttached("AutoWireViewModel", typeof(bool), typeof(ViewModelLocator), new PropertyMetadata(false, AutoWireViewModelChanged));
        #endregion

        /// <summary>
        /// Automaticamente liga el actual View con su respectivo ViewModel.
        /// Este se ejecuta cada vez que la propiedad AutoAttachViewModel cambia.
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void AutoWireViewModelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            // Unfortunately, there is no meaningful way to automatically attach a view model in the design mode using this way of connection ViewModels to Views.
            if (Execution.InDesignMode) { return; }

            Type viewType = d.GetType();
            string viewModelName = viewType.FullName.Replace("View", "ViewModel");

            #region CODIGO PARA BUSCAR EL VIEW_MODEL DESEADO EN LOS ASSAMBLY
            List<Type> allExportedTypes = new();
            foreach (Assembly assembly in AssemblySource.Instance)
            {
                allExportedTypes.AddRange(assembly.GetExportedTypes());
            }
            Type viewModelType = allExportedTypes.Single(x => x.FullName == viewModelName);
            #endregion

            ((FrameworkElement)d).DataContext = IoC.GetInstance(viewModelType, null);
        }
    }
}
