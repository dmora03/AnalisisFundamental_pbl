using MvvmFramework;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Models
{
    [XmlRoot(ElementName = "Empresa")]
    public class Empresa : Validatable, IEntity
    {
        #region ATRIBUTOS XML
        [Display(Name = "Simbolo")]
        [XmlAttribute]
        [Required(ErrorMessage = "{0} es obligatorio")]
        [RegularExpression(@"^[^\s]+$", ErrorMessage = "{0} no puede contener espacios")]
        public string ID
        {
            get => id;
            set => SetProperty(ref id, value);
        }
        private string id = "";

        [Display(Name = "Simbolo en Yahoo Finance")]
        [XmlAttribute]
        [RegularExpression(@"^[^\s]+$", ErrorMessage = "{0} no puede contener espacios")]
        public string YahooID
        {
            get => yahooID;
            set => SetProperty(ref yahooID, value);
        }
        private string yahooID = "";

        [XmlAttribute]
        [Required(ErrorMessage = "{0} es obligatorio")]
        public string Nombre
        {
            get => nombre;
            set => SetProperty(ref nombre, value);
        }
        private string nombre = "";

        [XmlAttribute]
        [Required(ErrorMessage = "{0} es obligatorio")]
        [RegularExpression(@"^[^\s]+$", ErrorMessage = "{0} no puede contener espacios")]
        [StringLength(maximumLength: 3, MinimumLength = 1, ErrorMessage = "{0} no puede tener mas de {1} caracteres")]
        public string Moneda
        {
            get => moneda;
            set => SetProperty(ref moneda, value);
        }
        private string moneda = "";
        #endregion

        #region ELEMENTOS XML
        [XmlElement]
        public string Comentarios
        {
            get => comentarios;
            set => SetProperty(ref comentarios, value);
        }
        private string comentarios = "";

        [XmlArray(ElementName = "Reportes")]
        public ObservableCollection<Reporte> Reportes
        {
            get => reportes;
            set => SetProperty(ref reportes, value);
        }
        private ObservableCollection<Reporte> reportes = new();

        [XmlArray(ElementName = "Referencias")]
        public ObservableCollection<Referencia> Referencias
        {
            get => referencias;
            set => SetProperty(ref referencias, value);
        }
        private ObservableCollection<Referencia> referencias = new();
        #endregion
    }
}