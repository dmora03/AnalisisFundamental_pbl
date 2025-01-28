using MvvmFramework;
using System.Xml.Serialization;

namespace Models
{
    public class Concepto : Validatable
    {
        [XmlAttribute]
        public string Ref
        {
            get => @ref;
            set =>SetProperty(ref @ref, value);
        }
        private string @ref = "";

        [XmlText]
        public long Value
        {
            get => value;
            set => SetProperty(ref this.value, value);
        }
        private long value;
    }

    public class ConceptoOpcional : Concepto
    {
        [XmlAttribute]
        public bool Directo
        {
            get => directo;
            set => SetProperty(ref directo, value);
        }
        private bool directo = true;
    }
}
