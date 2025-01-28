using MvvmFramework;
using System;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Models
{
    [XmlRoot()]
    public class Referencia : Validatable, IEntity
    {
        #region ATRIBUTOS XML
        [XmlAttribute]
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        //[RegularExpression(@"^[^\s]+$", ErrorMessage = "{0} no puede contener espacios")]
        public string ID
        {
            get => iD;
            set => SetProperty(ref iD, value);
        }
        private string iD = "";
        #endregion

        #region ELEMENTOS XML
        [XmlElement("NumeroDeUnidades")]
        public string? NumUnidades
        {
            get => numero;
            set => SetProperty(ref numero, value);
        }
        private string? numero;

        [XmlElement]
        public string? Ingresos
        {
            get => ventas;
            set => SetProperty(ref ventas, value);
        }
        private string? ventas;

        [XmlElement("CostoDeVentasAutocalculado")]
        public bool CostoVentasAuto
        {
            get => costoVentasAuto;
            set => SetProperty(ref costoVentasAuto, value);
        }
        private bool costoVentasAuto = true;

        [XmlElement("UtilidadBruta")]
        public string? UBruta
        {
            get => uBruta;
            set => SetProperty(ref uBruta, value);
        }
        private string? uBruta;

        [XmlElement("GastosOperativosAutocalculado")]
        public bool GastosOpAuto
        {
            get => gastosOpAuto;
            set => SetProperty(ref gastosOpAuto, value);
        }
        private bool gastosOpAuto = true;

        [XmlElement("UtilidadOperativa")]
        public string? UOperativa
        {
            get => uOperativa;
            set => SetProperty(ref uOperativa, value);
        }
        private string? uOperativa;

        [XmlElement("GastosFinancierosAutocalculado")]
        public bool GastosFinAuto
        {
            get => gastosFinAuto;
            set => SetProperty(ref gastosFinAuto, value);
        }
        private bool gastosFinAuto = true;

        [XmlElement("UtilidadAntesDeImpuestos")]
        public string? UAImpuestos
        {
            get => uAImpuestos;
            set => SetProperty(ref uAImpuestos, value);
        }
        private string? uAImpuestos;

        [XmlElement("UtilidadNeta")]
        public string? UNeta
        {
            get => uNeta;
            set => SetProperty(ref uNeta, value);
        }
        private string? uNeta;

        [XmlElement("UtilidadParaAccionistas")]
        public string? UAccionistas
        {
            get => uAccionistas;
            set => SetProperty(ref uAccionistas, value);
        }
        private string? uAccionistas;

        [XmlElement("IngresosProp")]
        public string? IngresosProp
        {
            get => ingresosProp;
            set => SetProperty(ref ingresosProp, value);
        }
        private string? ingresosProp;

        [XmlElement("NOI")]
        public string? NOI
        {
            get => noi;
            set => SetProperty(ref noi, value);
        }
        private string? noi;

        [XmlElement("FFO")]
        public string? FFO
        {
            get => ffo;
            set => SetProperty(ref ffo, value);
        }
        private string? ffo;

        [XmlElement("AFFO")]
        public string? AFFO
        {
            get => affo;
            set => SetProperty(ref affo, value);
        }
        private string? affo;

        [XmlElement]
        public string? Efectivo
        {
            get => efectivo;
            set => SetProperty(ref efectivo, value);
        }
        private string? efectivo;

        [XmlElement]
        public string? Deuda
        {
            get => deuda;
            set => SetProperty(ref deuda, value);
        }
        private string? deuda;

        [XmlElement("ActivosCirculantes")]
        public string? ActivosC
        {
            get => activosC;
            set => SetProperty(ref activosC, value);
        }
        private string? activosC;

        [XmlElement("PasivosCirculantes")]
        public string? PasivosC
        {
            get => pasivosC;
            set => SetProperty(ref pasivosC, value);
        }
        private string? pasivosC;

        [XmlElement("ActivosTotales")]
        public string? ActivosT
        {
            get => activosT;
            set => SetProperty(ref activosT, value);
        }
        private string? activosT;

        [XmlElement("PasivosTotales")]
        public string? PasivosT
        {
            get => pasivosT;
            set => SetProperty(ref pasivosT, value);
        }
        private string? pasivosT;

        [XmlElement]
        public string? Capital
        {
            get => capital;
            set => SetProperty(ref capital, value);
        }
        private string? capital;

        [XmlElement("FlujoDeOperaciones")]
        public string? FlujoOperaciones
        {
            get => deOperaciones;
            set => SetProperty(ref deOperaciones, value);
        }
        private string? deOperaciones;

        [XmlElement("FlujoDeInversiones")]
        public string? FlujoInversiones
        {
            get => deInversiones;
            set => SetProperty(ref deInversiones, value);
        }
        private string? deInversiones;

        [XmlElement("FlujoDeFinanciamiento")]
        public string? FlujoFinanciamiento
        {
            get => deFinanciamiento;
            set => SetProperty(ref deFinanciamiento, value);
        }
        private string? deFinanciamiento;

        [XmlElement("EfectivoAlInicioDelAno")]
        public string? EfectivoAlInicio
        {
            get => efectivoAlInicio;
            set => SetProperty(ref efectivoAlInicio, value);
        }
        private string? efectivoAlInicio;

        [XmlElement("EfectivoAlFinalDelAno")]
        public string? EfectivoAlFinal
        {
            get => efectivoAlFinal;
            set => SetProperty(ref efectivoAlFinal, value);
        }
        private string? efectivoAlFinal;

        [XmlElement("AccionesEnCirculacion")]
        public string? AccionesCir
        {
            get => circulacion;
            set => SetProperty(ref circulacion, value);
        }
        private string? circulacion;

        [XmlElement]
        public string? Dividendo
        {
            get => dividendo;
            set => SetProperty(ref dividendo, value);
        }
        private string? dividendo;
        #endregion
    }
}



