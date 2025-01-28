using System;
using System.Reflection;

namespace WPF.ExtensionMethods
{
    /// <summary>
    /// Una clase estática para funciones de tipo reflexión.
    /// </summary>
    public static class Reflection
    {
        /// <summary>
        /// Extensión para 'Objeto' que copia las propiedades a un objeto de destino.
        /// </summary>
        /// <param name="source">El 'Objeto' fuente.</param>
        /// <param name="destination">El 'Objeto' destino.</param>
        public static void CopyPropertiesTo(this object source, object destination)
        {
            if (source is null) { throw new ArgumentNullException(nameof(source)); }
            if (destination is null) { throw new ArgumentNullException(nameof(destination)); }

            // Getting the Types of the objects
            Type typeDest = destination.GetType();
            Type typeSrc = source.GetType();

            // Iterate the Properties of the source instance and  
            // populate them from their desination counterparts  
            PropertyInfo[] srcProps = typeSrc.GetProperties();
            foreach (PropertyInfo srcProp in srcProps)
            {
                if (!srcProp.CanRead)
                { continue; }

                PropertyInfo? targetProperty = typeDest.GetProperty(srcProp.Name);
                if (targetProperty is null)
                { continue; }
                if (!targetProperty.CanWrite)
                { continue; }
                if (targetProperty.GetSetMethod(true) is not null && targetProperty.GetSetMethod(true)!.IsPrivate)
                { continue; }
                if ((targetProperty.GetSetMethod()?.Attributes & MethodAttributes.Static) != 0)
                { continue; }
                if (!targetProperty.PropertyType.IsAssignableFrom(srcProp.PropertyType))
                { continue; }

                // Passed all tests, lets set the value
                targetProperty.SetValue(destination, srcProp.GetValue(source, null), null);
            }
        }
    }
}
