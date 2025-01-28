using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;

// BIEN DOCUMENTADO Y ENTENDIDO
/* Clases necesarias para usar esta:
 * - Ninguna
 */

namespace MvvmFramework
{
    /// <summary>
    /// Notifica a los clientes que un valor de propiedad ha cambiado y 
    /// tambien la valida en función de los atributos que tiene la propiedad
    /// </summary>
    public class Validatable : INotifyPropertyChanged, INotifyDataErrorInfo
    {
        /// <summary>
        /// Contiene la lista de errores para cada nombre de propiedad.
        /// </summary>
        private readonly Dictionary<string, List<string>> errors = new();

        #region INotifyPropertyChanged
        /// <summary>
        /// Ocurre cuando se cambia una propiedad del componente.
        /// Asignale el método que se ejecutará cada vez que una propiedad se modifique
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        #endregion

        #region INotifyDataErrorInfo
        /// <summary>
        /// Regresa Verdadero si la entidad tiene errores de validación; de lo contrario, falso.
        /// </summary>
        public bool HasErrors => errors.Count > 0;

        /// <summary>
        /// Ocurre cuando los errores de validación han cambiado para una propiedad o para toda la entidad.
        /// </summary>
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged = delegate { };

        /// <summary>
        /// Obtiene los errores de validación para una propiedad especificada o para toda la entidad.
        /// </summary>
        /// <param name="propertyName">Nombre de la propiedad para la que se recuperarán los errores de validación; o NULL o EMPTY, para recuperar errores a nivel de entidad.</param>
        /// <returns>Los errores de validación de la propiedad o entidad</returns>
        public IEnumerable GetErrors(string propertyName)
        {
            return string.IsNullOrEmpty(propertyName)
                ? errors.Values                         // Si NULL o EMPTY regresa TODOS los errores de la entidad
                : errors.ContainsKey(propertyName)
                    ? errors[propertyName]              // Si 'propertyName' tiene errores los regresa
                    : null;                             // Si 'propertyName' NO tiene errores regesa NULL
        }
        #endregion

        /// <summary>
        /// Notifica a los clientes que el valor de la propiedad dada ha cambiado
        /// </summary>
        /// <param name="propertyName">Nombre de la propiedad</param>
        protected virtual void OnPropertyChange(string propertyName)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));

            object value = GetType().GetProperty(propertyName).GetValue(this, null);
            ValidatableProperty(propertyName,value);
        }

        /// <summary>
        /// Si la variable Miebro realmente cambió, entonces la actualiza, 
        /// notifica a los clientes del cambio, valida el nuevo dato y regresa TRUE;
        /// de lo contrario solo regresa FALSE
        /// </summary>
        /// <typeparam name="T">El Tipo de elemento de la propiedad</typeparam>
        /// <param name="member">La variable Miembro de la propiedad</param>
        /// <param name="val">El nuevo valor de la propiedad</param>
        /// <param name="propertyName">Nombre de la propiedad, si se omite, el nombre de propiedad del llamador será usado como argumento.</param>
        /// <returns>TRUE si el valor cambió, de lo contrario FALSE</returns>
        protected virtual bool SetProperty<T>(ref T member, T val, [CallerMemberName] string propertyName = null)
        {
            if (Equals(member, val)) { return false; }
            member = val;
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));

            ValidatableProperty(propertyName, val);
            return true;
        }

        /// <summary>
        /// Si la variable Miebro realmente cambió, entonces la actualiza, 
        /// notifica a los clientes del cambio, liga un método al PropertyChangedEventHandler, 
        /// valida el nuevo dato y regresa TRUE; de lo contrario solo regresa FALSE
        /// </summary>
        /// <typeparam name="T">El Tipo de elemento de la propiedad</typeparam>
        /// <param name="member">La variable Miembro de la propiedad</param>
        /// <param name="val">El nuevo valor de la propiedad</param>
        /// <param name="propertyChangedAction">Método que se ejecutará cada vez que una propiedad de esta propiedad cambie</param>
        /// <param name="propertyName">Nombre de la propiedad, si se omite, el nombre de propiedad del llamador será usado como argumento.</param>
        /// <returns>TRUE si el valor cambió, de lo contrario FALSE</returns>
        protected virtual bool SetProperty<T>(ref T member, T val, PropertyChangedEventHandler propertyChangedAction,
                                              [CallerMemberName] string propertyName = null) where T : Validatable
        {
            if (Equals(member, val)) { return false; }
            if (member is not null) { member.PropertyChanged -= propertyChangedAction; }
            member = val;
            if (member is not null) { member.PropertyChanged += propertyChangedAction; }
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));

            ValidatableProperty(propertyName, val);
            return true;
        }

        /// <summary>
        /// Si la variable Miebro realmente cambió, entonces la actualiza, 
        /// notifica a los clientes del cambio, valida el nuevo dato y regresa TRUE;
        /// de lo contrario solo regresa FALSE.
        /// **BUBBLING se refiere que Notificará si alguna propiedad de esta propiedad cambió,
        /// provocando que las notificaciones suban hasta la ultima capa.
        /// Se siguire inicializar el Set the la propiedad en el contructor, para que el Bubbling funcione desde que se contruye el objeto
        /// </summary>
        /// <typeparam name="T">El Tipo de elemento de la propiedad, este DEBE heredar la clase <see cref="Validatable"/></typeparam>
        /// <param name="member">La variable Miembro de la propiedad</param>
        /// <param name="val">El nuevo valor de la propiedad</param>
        /// <param name="propertyName">Nombre de la propiedad, si se omite, el nombre de propiedad del llamador será usado como argumento.</param>
        /// <returns>TRUE si el valor cambió, de lo contrario FALSE</returns>
        protected virtual bool SetPropertyBubbling<T>(ref T member, T val,
                                                      [CallerMemberName] string propertyName = null) where T : Validatable
        {
            if (Equals(member, val)) { return false; }
            if (member is not null) { member.PropertyChanged -= Member_PropertyChanged; }
            member = val;
            if (member is not null) { member.PropertyChanged += Member_PropertyChanged; }

            void Member_PropertyChanged(object sender, PropertyChangedEventArgs e)
            { OnPropertyChange(propertyName); }
            
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));

            ValidatableProperty(propertyName, val);
            return true;
        }

        /// <summary>
        /// Valida el valor de la propiedad en función de los atributos que tiene
        /// </summary>
        /// <typeparam name="T">Tipo de elemento del valor de la propiedad</typeparam>
        /// <param name="propertyName">Nombre de la Propiedad</param>
        /// <param name="value">Valor de la propiedad</param>
        private void ValidatableProperty<T>(string propertyName, T value)
        {
            List<ValidationResult> results = new();
            ValidationContext context = new(this) { MemberName = propertyName };
            _ = Validator.TryValidateProperty(value, context, results);

            if (results.Any())
            {
                errors[propertyName] = results.Select(c => c.ErrorMessage).ToList();
            }
            else
            {
                _ = errors.Remove(propertyName);
            }
            ErrorsChanged(this, new DataErrorsChangedEventArgs(propertyName));
        }
    }
}
/*EJEMPLO DE COMO USAR LA CLASE/
   public class TestClass : Validatable
   {
       public string ID
       {
           get => id;
           set => SetProperty(ref id, value);
       }
       private string id;

       public string Nombre
       {
           get => nombre;
           set => SetProperty(ref nombre, value);
       }
       private string nombre;
   }/**/