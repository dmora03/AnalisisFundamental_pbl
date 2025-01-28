using MvvmFramework;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Xml.Serialization;

namespace Models
{
    [XmlRoot(ElementName = "Empresas")]
    public class XMLEmpresas : Validatable
    {
        [XmlElement(ElementName = "Empresa")]
        public ObservableCollection<Empresa> Empresas
        {
            get => empresas;
            set => SetProperty(ref empresas, value);
        }
        private ObservableCollection<Empresa> empresas = new();
    }
}
