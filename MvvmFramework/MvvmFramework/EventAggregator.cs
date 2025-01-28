using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
// BIEN DOCUMENTADO Y ENTENDIDO
/*
Clases necesarias para usar esta:
- Ninguna
 */
namespace MvvmFramework
{
    /// <summary>
    /// Una interfaz de marcador para las clases que se suscriben a los mensajes.
    /// <br>A marker interface for classes that subscribe to messages.</br>
    /// </summary>
    public interface IHandle { }

    /// <summary>
    /// Denota una clase que puede manejar un tipo particular de mensaje.
    /// <br>Denotes a class which can handle a particular type of message.</br>
    /// </summary>
    /// <typeparam name = "TMessage">El tipo de mensaje a manejar.</typeparam>
    public interface IHandle<TMessage> : IHandle
    {
        //don't use contravariance here
        /// <summary>
        /// Maneja el mensaje.
        /// <br>Handles the message.</br>
        /// </summary>
        /// <param name = "message">El mensaje.</param>
        void Handle(TMessage message);
    }

    /// <summary>
    /// Habilita la publicación y la suscripción a eventos sin conexión directa.
    /// <br>Enables loosely-coupled publication of and subscription to events.</br>
    /// </summary>
    public interface IEventAggregator
    {
        /// <summary>
        /// Busca los controladores suscritos para verificar si tenemos un controlador para el tipo de mensaje proporcionado.
        /// <br>Searches the subscribed handlers to check if we have a handler for the message type supplied.</br>
        /// </summary>
        /// <param name="messageType">El tipo de mensaje a buscar</param>
        /// <returns>TRUE si algun controlador(handler) fue encotrado, de lo contrario FASLE.</returns>
        bool HandlerExistsFor(Type messageType);

        /// <summary>
        /// Suscribe una instancia a todos los eventos declarados mediante implementaciones de <see cref = "IHandle{T}" />
        /// <br>Subscribes an instance to all events declared through implementations of <see cref = "IHandle{T}" /></br>
        /// </summary>
        /// <param name = "subscriber">La instancia para suscribirse a la publicación de eventos.</param>
        void Subscribe(object subscriber);

        /// <summary>
        /// Da de baja la instancia de todos los eventos.
        /// </summary>
        /// <param name = "subscriber">La instancia para darse de baja.</param>
        void Unsubscribe(object subscriber);

        /// <summary>
        /// Publica un mensaje.
        /// </summary>
        /// <param name = "message">La instancia del mensaje.
        ///                         <br>The message instance</br>.</param>
        /// <param name = "marshal">Permite que el editor proporcione un contador de hilos personalizado para la publicación del mensaje.
        ///                         <br>Allows the publisher to provide a custom thread marshaller for the message publication.</br></param>
        void Publish(object message, Action<System.Action> marshal);

        /// <summary>
        /// Publica un mensaje.
        /// </summary>
        /// <param name = "message">La instancia del mensaje.
        ///                         <br>The message instance</br>.</param>
        /// <remarks>
        /// Esto no organiza la publicación en ningún hilo especial de forma predeterminada.
        /// <br>Does not marshall the the publication to any special thread by default.</br>
        /// </remarks>
        void Publish(object message);
    }

    /// <summary>
    /// Habilita la publicación y la suscripción a eventos sin conexión directa.
    /// <br>Enables loosely-coupled publication of and subscription to events.</br>
    /// </summary>
    public class EventAggregator : IEventAggregator
    {
        private readonly List<Handler> handlers = new();

        /// <summary>
        /// Processing of handler results on publication thread.
        /// </summary>
        private static readonly Action<object, object> HandlerResultProcessing = (target, result) => { };

        /// <summary>
        /// The default thread marshaller used for publication;
        /// </summary>
        private static readonly Action<Action> DefaultPublicationThreadMarshaller = action => action();
        /// <summary>
        /// Gets or sets the default publication thread marshaller.
        /// </summary>
        public Action<Action> PublicationThreadMarshaller { get; set; }
        
        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref = "EventAggregator" />.
        /// </summary>
        public EventAggregator()
        {
            PublicationThreadMarshaller = DefaultPublicationThreadMarshaller;
        }

        // NO DOCUMENTAR: La documentación la hereda de la interface
        public bool HandlerExistsFor(Type messageType)
        {
            return handlers.Any(handler => handler.Handles(messageType) & !handler.IsDead);
        }

        // NO DOCUMENTAR: La documentación la hereda de la interface
        public virtual void Subscribe(object subscriber)
        {
            if (subscriber == null)
            {
                throw new ArgumentNullException(nameof(subscriber));
            }
            lock (handlers)
            {
                if (handlers.Any(x => x.Matches(subscriber)))
                {
                    return;
                }

                handlers.Add(new Handler(subscriber));
            }
        }

        // NO DOCUMENTAR: La documentación la hereda de la interface
        public virtual void Unsubscribe(object subscriber)
        {
            if (subscriber == null)
            {
                throw new ArgumentNullException(nameof(subscriber));
            }
            lock (handlers)
            {
                var found = handlers.FirstOrDefault(x => x.Matches(subscriber));

                if (found != null)
                {
                    handlers.Remove(found);
                }
            }
        }

        // NO DOCUMENTAR: La documentación la hereda de la interface
        public virtual void Publish(object message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }
            Publish(message, PublicationThreadMarshaller);
        }

        // NO DOCUMENTAR: La documentación la hereda de la interface
        public virtual void Publish(object message, Action<System.Action> marshal)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }
            if (marshal == null)
            {
                throw new ArgumentNullException(nameof(marshal));
            }

            Handler[] toNotify;
            lock (handlers)
            {
                toNotify = handlers.ToArray();
            }

            marshal(() => {
                var messageType = message.GetType();

                var dead = toNotify
                    .Where(handler => !handler.Handle(messageType, message))
                    .ToList();

                if (dead.Any())
                {
                    lock (handlers)
                    {
                        dead.Apply(x => handlers.Remove(x));
                    }
                }
            });
        }

        private class Handler
        {
            private readonly WeakReference reference;
            private readonly Dictionary<Type, MethodInfo> supportedHandlers = new();

            public bool IsDead
            {
                get { return reference.Target == null; }
            }

            public Handler(object handler)
            {
                reference = new WeakReference(handler);

                IEnumerable<Type> interfaces = handler.GetType().GetInterfaces()
                    .Where(x => typeof(IHandle).IsAssignableFrom(x) && x.IsGenericType);

                foreach (Type @interface in interfaces)
                {
                    Type type = @interface.GetGenericArguments()[0];
                    MethodInfo method = @interface.GetMethod("Handle");
                    supportedHandlers[type] = method;
                }
            }

            public bool Matches(object instance)
            {
                return reference.Target == instance;
            }

            public bool Handle(Type messageType, object message)
            {
                object target = reference.Target;
                if (target == null)
                {
                    return false;
                }

                foreach (KeyValuePair<Type, MethodInfo> pair in supportedHandlers)
                {
                    if (pair.Key.IsAssignableFrom(messageType))
                    {
                        object result = pair.Value.Invoke(target, new[] { message });
                        if (result != null)
                        {
                            HandlerResultProcessing(target, result);
                        }
                    }
                }

                return true;
            }

            public bool Handles(Type messageType)
            {
                return supportedHandlers.Any(pair => pair.Key.IsAssignableFrom(messageType));
            }
        }
    }

    /// <summary>
    /// Extension methods for <see cref="IEnumerable{T}"/>
    /// </summary>
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Applies the action to each element in the list.
        /// </summary>
        /// <typeparam name="T">The enumerable item's type.</typeparam>
        /// <param name="enumerable">The elements to enumerate.</param>
        /// <param name="action">The action to apply to each item in the list.</param>
        public static void Apply<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            foreach (var item in enumerable)
            {
                action(item);
            }
        }
    }
}

/*EJEMPLO DE COMO USAR EL EVENT AGREGATOR/
======================================== EN EL VIEW MODEL ===============================================================
public class MainViewModel
{
        private readonly IEventAggregator aggregator;

        public MainViewModel(IEventAggregator aggregator)
        {
            this.aggregator = aggregator;                           // Cargar el IEventAgregator a la variable local
        }

        private void ReloadStudents() 
        {
            aggregator.Publish(new StudentsLoadedMessage());        // Lanzar el evento con el messageObject
        }
    }

    public class StudentsLoadedMessage {}                           // Crear el messageObject especifico para cada situacion
                                                                       La clase puede estar vacia, tal como este ejemplo 
=========================================================================================================================
======================================== EN EL VIEW =====================================================================
public partial class MainView : IHandle<StudentsLoadedMessage>      // Heredar del IHandle<messageObject>
{
    public MainView()
    {
        var aggregator = IoC.Get<IEventAggregator>();               // Suscribir la clase al IEventAgregator
        aggregator.Subscribe(this);                                    Nota: no es necesario tener un FIELD en la clase
    }

    public void Handle(StudentsLoadedMessage message)               // Evento lanzado cuando se lanza el messageObject
    {
        StudentsLoadedText.Text = "Loading Finished";
    }
}
=========================================================================================================================
/**/

