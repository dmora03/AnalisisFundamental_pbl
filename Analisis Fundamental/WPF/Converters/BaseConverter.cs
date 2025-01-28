using System;
using System.Windows.Markup;

namespace WPF.Converters
{
    /// <summary>
    /// Clase base para las clases del tipo IValueConverter o IMultiValueConverter
    /// para que sus convertidores puedan ser usandos sin ser intanciados en RESOURCES
    /// </summary>
    public abstract class BaseConverter : MarkupExtension
    {
        public override object ProvideValue(IServiceProvider serviceProvider) { return this; }
    }
}
