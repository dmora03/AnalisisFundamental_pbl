/******************************************************************************/
/********* LLENAR ESTA INTERFAZ CONFORME A LA NECESIDAD DEL PROYECTO **********/
/* Renombrar o agregar propiedades basicas que existiran en todos los modelos */
/******************************************************************************/


namespace Models
{
    /// <summary>
    /// Define las propiedades y métodos que debe de contener una Entidad
    /// </summary>
    public interface IEntity
    {
        string ID { get; set; }
    }
}