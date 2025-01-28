using MvvmFramework;
using System;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Models
{
    [XmlRoot]
    public class Reporte : Validatable, IEntity
    {
        #region ATRIBUTOS XML
        [XmlAttribute]
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [RegularExpression(@"^[^\s]+$", ErrorMessage = "{0} no puede contener espacios")]
        public string ID
        {
            get => iD;
            set => SetProperty(ref iD, value);
        }
        private string iD = "";

        [XmlAttribute]
        [Display(Name = "Trimestre del Reporte")]
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [Range(minimum: 1, maximum: 4, ErrorMessage = "{0} solo puede tener valores del {1} al {2}")]
        public int TrimestreReporte
        {
            get => trimestreReporte;
            set => SetProperty(ref trimestreReporte, value);
        }
        private int trimestreReporte;

        [XmlAttribute]
        [Display(Name = "Trimestre Natural")]
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [Range(minimum: 1, maximum: 4, ErrorMessage = "{0} solo puede tener valores del {1} al {2}")]
        public int TrimestreNatural { get; set; }

        [XmlAttribute]
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [Range(minimum: 1, maximum: 31, ErrorMessage = "{0} solo puede tener valores del {1} al {2}")]
        public int Dia
        {
            get => dia;
            set
            {
                if (SetProperty(ref dia, value))
                {
                    if (dia > DateTime.DaysInMonth(Ano, Mes))
                    {
                        dia = DateTime.DaysInMonth(Ano, Mes);
                    }
                    OnPropertyChange(nameof(Fecha));
                }
            }
        }
        private int dia = 1;

        [XmlAttribute]
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [Range(minimum: 1, maximum: 12, ErrorMessage = "{0} solo puede tener valores del {1} al {2}")]
        public int Mes
        {
            get => mes;
            set
            {
                if (SetProperty(ref mes, value))
                {
                    TrimestreNatural = value switch
                    {
                        <= 3 => 1,
                        <= 6 => 2,
                        <= 9 => 3,
                        _ => 4,
                    };
                    ID = $"{Ano}_{TrimestreNatural}";
                    OnPropertyChange(nameof(TrimestreNatural));
                    OnPropertyChange(nameof(Fecha));
                }
            }
        }
        private int mes = 1;

        [XmlAttribute]
        [Display(Name = "Año")]
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        public int Ano
        {
            get => ano;
            set
            {
                if (SetProperty(ref ano, value))
                {
                    ID = $"{value}_{TrimestreNatural}";
                    OnPropertyChange(nameof(Fecha));
                }
            }
        }
        private int ano = 1;
        #endregion

        #region ELEMENTOS XML
        [XmlElement("NumeroDeUnidades")]
        public double? NumUnidades
        {
            get => numero;
            set => SetProperty(ref numero, value);
        }
        private double? numero;

        [XmlElement("NumeroDeUnidadesAA")]
        public double? NumUnidadesAA
        {
            get => numeroAA;
            set => SetProperty(ref numeroAA, value);
        }
        private double? numeroAA;


        [XmlElement]
        public double Ingresos
        {
            get => ingresos / Factor;
            set => SetProperty(ref ingresos, value * Factor);
        }
        private double ingresos;

        [XmlElement]
        public double IngresosAA
        {
            get => ingresosAA / Factor;
            set => SetProperty(ref ingresosAA, value * Factor);
        }
        private double ingresosAA;

        [XmlElement("UtilidadBruta")]
        public double UBruta
        {
            get => uBruta / Factor;
            set => SetProperty(ref uBruta, value * Factor);
        }
        private double uBruta;

        [XmlElement("UtilidadOperativa")]
        public double UOperativa
        {
            get => uOperativa / Factor;
            set => SetProperty(ref uOperativa, value * Factor);
        }
        private double uOperativa;

        [XmlElement("UtilidadAntesDeImpuestos")]
        public double UAImpuestos
        {
            get => uAImpuestos / Factor;
            set => SetProperty(ref uAImpuestos, value * Factor);
        }
        private double uAImpuestos;

        [XmlElement("UtilidadNeta")]
        public double UNeta
        {
            get => uNeta / Factor;
            set => SetProperty(ref uNeta, value * Factor);
        }
        private double uNeta;

        [XmlElement("UtilidadParaAccionistas")]
        public double UAccionistas
        {
            get => uAccionistas / Factor;
            set => SetProperty(ref uAccionistas, value * Factor);
        }
        private double uAccionistas;

        [XmlElement("UtilidadParaAccionistasAA")]
        public double UAccionistasAA
        {
            get => uAccionistasAA / Factor;
            set => SetProperty(ref uAccionistasAA, value * Factor);
        }
        private double uAccionistasAA;


        [XmlElement("IngresoProp")]
        public double? IngresosProp
        {
            get => ingresosProp / Factor;
            set => SetProperty(ref ingresosProp, value * Factor);
        }
        private double? ingresosProp;
        
        [XmlElement("NOI")]
        public double? NOI
        {
            get => noi / Factor;
            set => SetProperty(ref noi, value * Factor);
        }
        private double? noi;

        [XmlElement("FFO")]
        public double? FFO
        {
            get => ffo / Factor;
            set => SetProperty(ref ffo, value * Factor);
        }
        private double? ffo;

        [XmlElement("AFFO")]
        public double? AFFO
        {
            get => affo / Factor;
            set => SetProperty(ref affo, value * Factor);
        }
        private double? affo;


        [XmlElement]
        public double Efectivo
        {
            get => efectivo / Factor;
            set => SetProperty(ref efectivo, value * Factor);
        }
        private double efectivo;

        [XmlElement]
        public double Deuda
        {
            get => deuda / Factor;
            set => SetProperty(ref deuda, value * Factor);
        }
        private double deuda;

        [XmlElement("ActivosCirculantes")]
        public double ActivosC
        {
            get => activosC / Factor;
            set => SetProperty(ref activosC, value * Factor);
        }
        private double activosC;

        [XmlElement("PasivosCirculantes")]
        public double PasivosC
        {
            get => pasivosC / Factor;
            set => SetProperty(ref pasivosC, value * Factor);
        }
        private double pasivosC;

        [XmlElement("ActivosTotales")]
        public double ActivosT
        {
            get => activosT / Factor;
            set => SetProperty(ref activosT, value * Factor);
        }
        private double activosT;

        [XmlElement("PasivosTotales")]
        public double PasivosT
        {
            get => pasivosT / Factor;
            set => SetProperty(ref pasivosT, value * Factor);
        }
        private double pasivosT;

        [XmlElement]
        public double Capital
        {
            get => capital / Factor;
            set => SetProperty(ref capital, value * Factor);
        }
        private double capital;

        [XmlElement]
        public double CapitalAA
        {
            get => capitalAA / Factor;
            set => SetProperty(ref capitalAA, value * Factor);
        }
        private double capitalAA;

        [XmlElement("FlujoDeOperaciones")]
        public double FlujoOperaciones
        {
            get => flujoOperaciones / Factor;
            set => SetProperty(ref flujoOperaciones, value * Factor);
        }
        private double flujoOperaciones;

        [XmlElement("FlujoDeInversiones")]
        public double FlujoInversiones
        {
            get => flujoInversiones / Factor;
            set => SetProperty(ref flujoInversiones, value * Factor);
        }
        private double flujoInversiones;

        [XmlElement("FlujoDeFinanciamiento")]
        public double FlujoFinanciamiento
        {
            get => flujoFinanciamiento / Factor;
            set => SetProperty(ref flujoFinanciamiento, value * Factor);
        }
        private double flujoFinanciamiento;

        [XmlElement("EfectivoAlInicioDelAno")]
        public double EfectivoAlInicio
        {
            get => efectivoAlInicio / Factor;
            set => SetProperty(ref efectivoAlInicio, value * Factor);
        }
        private double efectivoAlInicio;

        [XmlElement("EfectivoAlFinalDelAno")]
        public double EfectivoAlFinal
        {
            get => efectivoAlFinal / Factor;
            set => SetProperty(ref efectivoAlFinal, value * Factor);
        }
        private double efectivoAlFinal;

        [XmlElement("AccionesEnCirculacion")]
        public double AccionesCir
        {
            get => accionesCir / Factor;
            set => SetProperty(ref accionesCir, value * Factor);
        }
        private double accionesCir;

        [XmlElement("PrecioPorAccion")]
        public double Precio
        {
            get => precio;
            set => SetProperty(ref precio, value);
        }
        private double precio;

        [XmlElement]
        public double Dividendo
        {
            get => dividendo;
            set => SetProperty(ref dividendo, value);
        }
        private double dividendo;
        #endregion

        #region CONSTRUCTORES
        public Reporte()
        {
            Fecha = DateTime.MinValue;     // Con esto se inicializa la Fecha, ID y TrimestreNatural
            TrimestreNatural = 1;
            TrimestreReporte = TrimestreNatural;
        }
        #endregion

        #region Valores acumulados de un año completo
        [XmlElement]
        public double? IngresosAcumulados
        {
            get => ingresosAcumulados / Factor;
            set => SetProperty(ref ingresosAcumulados, value * Factor);
        }
        private double? ingresosAcumulados;

        [XmlElement]
        public double? IngresosAAAcumulados
        {
            get => ingresosAAAcumulados / Factor;
            set => SetProperty(ref ingresosAAAcumulados, value * Factor);
        }
        private double? ingresosAAAcumulados;

        [XmlElement]
        public double? UBrutaAcumulados
        {
            get => uBrutaAcumulados / Factor;
            set => SetProperty(ref uBrutaAcumulados, value * Factor);
        }
        private double? uBrutaAcumulados;

        [XmlElement]
        public double? UOperativaAcumulados
        {
            get => uOperativaAcumulados / Factor;
            set => SetProperty(ref uOperativaAcumulados, value * Factor);
        }
        private double? uOperativaAcumulados;

        [XmlElement]
        public double? UAImpuestosAcumulados
        {
            get => uAImpuestosAcumulados / Factor;
            set => SetProperty(ref uAImpuestosAcumulados, value * Factor);
        }
        private double? uAImpuestosAcumulados;

        [XmlElement]
        public double? UNetaAcumulados
        {
            get => uNetaAcumulados / Factor;
            set => SetProperty(ref uNetaAcumulados, value * Factor);
        }
        private double? uNetaAcumulados;

        [XmlElement]
        public double? UAccionistasAcumulados
        {
            get => uAccionistasAcumulados / Factor;
            set => SetProperty(ref uAccionistasAcumulados, value * Factor);
        }
        private double? uAccionistasAcumulados;

        [XmlElement]
        public double? UAccionistasAAAcumulados
        {
            get => uAccionistasAAAcumulados / Factor;
            set => SetProperty(ref uAccionistasAAAcumulados, value * Factor);
        }
        private double? uAccionistasAAAcumulados;

        [XmlElement()]
        public double? IngresosPropAcumulados
        {
            get => ingresosPropAcumulados / Factor;
            set => SetProperty(ref ingresosPropAcumulados, value * Factor);
        }
        private double? ingresosPropAcumulados;

        [XmlElement]
        public double? NOIAcumulados
        {
            get => noiAcumulados / Factor;
            set => SetProperty(ref noiAcumulados, value * Factor);
        }
        private double? noiAcumulados;

        [XmlElement]
        public double? FFOAcumulados
        {
            get => ffoAcumulados / Factor;
            set => SetProperty(ref ffoAcumulados, value * Factor);
        }
        private double? ffoAcumulados;

        [XmlElement]
        public double? AFFOAcumulados
        {
            get => affoAcumulados / Factor;
            set => SetProperty(ref affoAcumulados, value * Factor);
        }
        private double? affoAcumulados;

        [XmlElement]
        public double? DividendosAcumulados
        {
            get => dividendosAcumulados;
            set => SetProperty(ref dividendosAcumulados, value);
        }
        private double? dividendosAcumulados;
        #endregion

        #region Propiedades Personalizadas
        [XmlIgnore]
        public DateTime Fecha
        {
            get => new(Ano, Mes, Dia);
            set
            {
                /*/
                dia = value.Day;
                OnPropertyChange(nameof(Dia));

                Ano = value.Year;
                Mes = value.Month;
                /**/
                bool notificarFecha = false;
                if (value.Day != Dia)
                {
                    notificarFecha = true;
                    dia = value.Day;
                    OnPropertyChange(nameof(Dia));
                }

                if (value.Year != Ano && value.Month == Mes)
                {
                    notificarFecha = true;
                    ano = value.Year;
                    OnPropertyChange(nameof(Ano));

                    ID = $"{Ano}_{TrimestreNatural}";
                }
                else if (value.Year == Ano && value.Month != Mes)
                {
                    notificarFecha = true;
                    mes = value.Month;
                    OnPropertyChange(nameof(Mes));

                    TrimestreNatural = value.Month switch
                    {
                        <= 3 => 1,
                        <= 6 => 2,
                        <= 9 => 3,
                        _ => 4,
                    };
                    ID = $"{Ano}_{TrimestreNatural}";
                    OnPropertyChange(nameof(TrimestreNatural));
                }
                else if (value.Year != Ano && value.Month != Mes)
                {
                    notificarFecha = true;
                    ano = value.Year;
                    OnPropertyChange(nameof(Ano));
                    mes = value.Month;
                    OnPropertyChange(nameof(Mes));

                    TrimestreNatural = value.Month switch
                    {
                        <= 3 => 1,
                        <= 6 => 2,
                        <= 9 => 3,
                        _ => 4,
                    };
                    ID = $"{Ano}_{TrimestreNatural}";
                    OnPropertyChange(nameof(TrimestreNatural));
                }

                if (notificarFecha) { OnPropertyChange(nameof(Fecha)); }
                /**/
            }
        }

        /// <summary>
        /// Factor por el que se dividira el valor original de ciertas propiedades
        /// o factor por el que se multiplicara cuando se cambie el valor de ciertas propiedades
        /// </summary>
        [XmlIgnore]
        public int Factor
        {
            get => factor;
            set
            {
                if (SetProperty(ref factor, value))
                {
                    OnPropertyChange(nameof(Ingresos));
                    OnPropertyChange(nameof(IngresosAA));
                    OnPropertyChange(nameof(UBruta));
                    OnPropertyChange(nameof(UOperativa));
                    OnPropertyChange(nameof(UAImpuestos));
                    OnPropertyChange(nameof(UNeta));
                    OnPropertyChange(nameof(UAccionistas));
                    OnPropertyChange(nameof(UAccionistasAA));

                    OnPropertyChange(nameof(IngresosProp));
                    OnPropertyChange(nameof(NOI));
                    OnPropertyChange(nameof(FFO));
                    OnPropertyChange(nameof(AFFO));

                    OnPropertyChange(nameof(Efectivo));
                    OnPropertyChange(nameof(Deuda));
                    OnPropertyChange(nameof(ActivosC));
                    OnPropertyChange(nameof(PasivosC));
                    OnPropertyChange(nameof(ActivosT));
                    OnPropertyChange(nameof(PasivosT));
                    OnPropertyChange(nameof(Capital));
                    OnPropertyChange(nameof(CapitalAA));

                    OnPropertyChange(nameof(FlujoOperaciones));
                    OnPropertyChange(nameof(FlujoInversiones));
                    OnPropertyChange(nameof(FlujoFinanciamiento));
                    OnPropertyChange(nameof(EfectivoAlInicio));
                    OnPropertyChange(nameof(EfectivoAlFinal));

                    OnPropertyChange(nameof(AccionesCir));

                    OnPropertyChange(nameof(IngresosAcumulados));
                    OnPropertyChange(nameof(IngresosAAAcumulados));
                    OnPropertyChange(nameof(UBrutaAcumulados));
                    OnPropertyChange(nameof(UOperativaAcumulados));
                    OnPropertyChange(nameof(UAImpuestosAcumulados));
                    OnPropertyChange(nameof(UNetaAcumulados));
                    OnPropertyChange(nameof(UAccionistasAcumulados));
                    OnPropertyChange(nameof(UAccionistasAAAcumulados));

                    OnPropertyChange(nameof(IngresosPropAcumulados));
                    OnPropertyChange(nameof(NOIAcumulados));
                    OnPropertyChange(nameof(FFOAcumulados));
                    OnPropertyChange(nameof(AFFOAcumulados));


                    // PROPIEDADES DE SOLO LECTURA
                    OnPropertyChange(nameof(CostoVentas));
                    OnPropertyChange(nameof(GastosOp));
                    OnPropertyChange(nameof(GastosFin));
                    OnPropertyChange(nameof(CostoVentasAcumulados));
                    OnPropertyChange(nameof(GastosOpAcumulados));
                    OnPropertyChange(nameof(GastosFinAcumulados));
                    OnPropertyChange(nameof(ImpuestosAcumulados));

                    //OnPropertyChange(nameof(CrecimientoUnidades));
                    //OnPropertyChange(nameof(CrecimientoUnidadesP));
                    //OnPropertyChange(nameof(CrecimientoCapitalP));
                    //OnPropertyChange(nameof(ActCPasvC));
                    //OnPropertyChange(nameof(ActTPasvT));
                    //OnPropertyChange(nameof(PasvTActT));
                    //OnPropertyChange(nameof(EfectivoDeuda));
                    //OnPropertyChange(nameof(PruebaAcido));
                    //OnPropertyChange(nameof(EfectivoCapital));
                    //OnPropertyChange(nameof(DeudaActivosT));
                    //OnPropertyChange(nameof(DeudaCapital));
                    OnPropertyChange(nameof(CapitalizaciónMercado));
                    //OnPropertyChange(nameof(ValorLibro));
                    //OnPropertyChange(nameof(PrecioValorLibro));

                    //OnPropertyChange(nameof(CrecimientoIngresosP));
                    //OnPropertyChange(nameof(IngresosUnidad));
                    //OnPropertyChange(nameof(MargenBruto));
                    //OnPropertyChange(nameof(MargenOperativo));
                    //OnPropertyChange(nameof(CrecimientoUtilidadesP));
                    //OnPropertyChange(nameof(MargenNeto));
                    //OnPropertyChange(nameof(MargenNOI));
                    //OnPropertyChange(nameof(MargenFFO));
                    //OnPropertyChange(nameof(MargenAFFO));
                    //OnPropertyChange(nameof(NOIAccion));
                    //OnPropertyChange(nameof(FFOAccion));
                    //OnPropertyChange(nameof(AFFOAccion));
                    //OnPropertyChange(nameof(UPA));
                    //OnPropertyChange(nameof(PU));
                }
            }
        }
        private int factor = 1;
        #endregion

        #region Propiedades de SOLO LECTURA
        /// <summary>
        /// Calcula el valor del Costo de Ventas
        /// </summary>
        [XmlIgnore]
        public double CostoVentas => Ingresos - UBruta;

        /// <summary>
        /// Calcula el valor del Costo de Ventas Acumulados en el año
        /// </summary>
        [XmlIgnore]
        public double? CostoVentasAcumulados => IngresosAcumulados - UBrutaAcumulados;

        /// <summary>
        /// Calcula el valor del Gasto Operativo
        /// </summary>
        [XmlIgnore]
        public double GastosOp => UBruta - UOperativa;

        /// <summary>
        /// Calcula el valor del Gasto Operativo Acumulados en el año
        /// </summary>
        [XmlIgnore]
        public double? GastosOpAcumulados => UBrutaAcumulados - UOperativaAcumulados;

        /// <summary>
        /// Calcula el valor de la Utilidad Antes de Impuestos
        /// </summary>
        [XmlIgnore]
        public double GastosFin => UAImpuestos - UOperativa;

        /// <summary>
        /// Calcula el valor de la Utilidad Antes de Impuestos Acumulados en el año
        /// </summary>
        [XmlIgnore]
        public double? GastosFinAcumulados => UAImpuestosAcumulados - UOperativaAcumulados;

        /// <summary>
        /// Calcula el valor del Impuesto Acumulados en el año
        /// </summary>
        [XmlIgnore]
        public double? ImpuestosAcumulados => UAImpuestosAcumulados - UNetaAcumulados;

        /// <summary>
        /// Calcula el crecimiento de unidades respecto al trimestre del año anterior
        /// </summary>
        [XmlIgnore]
        public double? CrecimientoUnidades => NumUnidades - NumUnidadesAA;

        /// <summary>
        /// Calcula el crecimiento de unidades en porcentaje respecto al trimestre del año anterior
        /// </summary>
        [XmlIgnore]
        public double? CrecimientoUnidadesP => NumUnidadesAA < 0
            ? Math.Abs((NumUnidades.GetValueOrDefault() / NumUnidadesAA.GetValueOrDefault()) - 1)
            : (NumUnidades / NumUnidadesAA) - 1;

        /// <summary>
        /// Calcula el crecimiento de capital en porcentaje respecto al trimestre del año anterior
        /// </summary>
        [XmlIgnore]
        public double CrecimientoCapitalP => CapitalAA < 0
            ? Math.Abs((Capital / CapitalAA) - 1)
            : (Capital / CapitalAA) - 1;

        /// <summary>
        /// Calcula la razón financiera de Activos Cir. / Pasivos Cir.
        /// + Razón financiera que Indica cuantas veces puedes pagar lo que debes +
        /// - Valor<1 la empresa no podrá cubrir sus deudas con los activos circulantes
        /// - Valor>1 brinda mayor seguridad a la empresa y a los inversionistas.
        /// </summary>
        [XmlIgnore]
        public double ActCPasvC => ActivosC / PasivosC;

        /// <summary>
        /// Calcula Activos Tot. / Pasivos Tot.
        /// + Indica que tan lejos esta la empresa de fracasar +
        /// - Valor = 1 significa que esta técnicamente quebrada y ya no vale
        /// - Valor< 1 significa que esta quebrada y debe más de lo que tiene
        /// </summary>
        [XmlIgnore]
        public double ActTPasvT => ActivosT / PasivosT;

        //TODO: Comentario de LTV

        /// <summary>
        /// Calcula Pasivos Tot. / Activos Tot. (LTV)
        /// + ????????????????????????????????????????????? +
        /// </summary>
        [XmlIgnore]
        public double PasvTActT => PasivosT / ActivosT;

        /// <summary>
        /// Calcula Efectivo / Deuda con intereses
        /// + Indica el nivel de endeudamiento con respecto al efectivo +
        /// + Sirve para ver en caso de crisis que tanto se puede pagar si no hay ingresos, recuerda que la
        /// deuda es de corto y largo plazo, no todo se debe pagar de inmediato +
        /// - Ejemplo: Si es 60% significa que solo se puede pagar el 60% de la deuda usando el efectivo
        /// </summary>
        [XmlIgnore]
        public double EfectivoDeuda => Efectivo / Deuda;

        /// <summary>
        /// Calcula la Prueba del Acido
        /// + Indica cuantas veces puede pagar lo que debe usando SOLO el efectivo que tiene +
        /// - Cash Cow: Se les dice así a las empresas que empiezan a juntar dinero pero no saben que
        /// hacer con tanto dinero en efectivo.
        /// Esto no es útil para el inversior ya que estas sólo lo tienen guardado en el banco. 
        /// Lo que pueden hacer es invertirlo para crecer o repartirlo pagando dividendos.
        /// </summary>
        [XmlIgnore]
        public double PruebaAcido => Efectivo / PasivosC;

        /// <summary>
        /// Calcula Efectivo / Capital
        /// + Indica el % de Efectivo Liquido del valor de la empresa +
        /// - Ejemplo: Si es 40%, significa que el 40% de lo que vale la empresa lo tiene en el banco y
        /// el otro 60% esta invertido o trabajandose
        /// </summary>
        [XmlIgnore]
        public double EfectivoCapital => Efectivo / Capital;

        /// <summary>
        /// Calcula Deuda / Activos Totales
        /// </summary>
        [XmlIgnore]
        public double DeudaActivosT => Deuda / ActivosT;

        /// <summary>
        /// Calcula Deuda / Capital
        /// + Indica el % de Deuda del valor de la empresa +
        /// + Se puede ver en que tiene deuda la empresa en el reporte trimestral en
        /// el area de DEUDA(deuda con instituciones financieras) +
        /// - Ejemplo: Si es 30%, significa que la empresa debe el 30% de lo que vale
        /// -> Reuters: Total Debt/Total Equity(Quarterly)
        /// </summary>
        [XmlIgnore]
        public double DeudaCapital => Deuda / Capital;

        /// <summary>
        /// Calcula la Capitalización de Mercado
        /// + Indica el valor de la empresa en la bolsa y nos ayuda a ver que tan grande es la empresa +
        /// </summary>
        [XmlIgnore]
        public double CapitalizaciónMercado => AccionesCir * Precio;

        /// <summary>
        /// Calcula el Valor en Libros por Acción
        /// + Indica cuanto del capital contable es mío por tener una acción +
        /// </summary>
        [XmlIgnore]
        public double ValorLibro => Capital / AccionesCir;

        /// <summary>
        /// Calcula el Precio / Valor en Libros (P/VL)
        /// + Indica cuantas veces pagas la Acción a Valor en LIBROS al adquirir una Acción a Valor MERCADO +
        /// ¡¡¡ Es el segundo principal indicador !!!
        /// - Es como la diferencia del valor de mercado de una casa y el valor catastral
        /// - Un P/VL bajo nos puede sugerir que la empresa cotiza por debajo de su valor real, sin embargo también puede indicar que algo esta mal.
        /// Siempre debe ser comparado contra otras acciones similares y en la misma industria.
        /// </summary>
        [XmlIgnore]
        public double PrecioValorLibro => Precio / ValorLibro;



        /// <summary>
        /// Calcula el crecimiento de Ingresos en porcentaje de año a año
        /// </summary>
        [XmlIgnore]
        public double? CrecimientoIngresosP => IngresosAAAcumulados < 0
            ? Math.Abs((IngresosAcumulados.GetValueOrDefault() / IngresosAAAcumulados.GetValueOrDefault()) - 1)
            : (IngresosAcumulados / IngresosAAAcumulados) - 1;

        /// <summary>
        /// Calcula la cantidad de ingresos en cada sucursal o unidad (En algunas NO APLICA) 
        /// </summary>
        [XmlIgnore]
        public double? IngresosUnidad => ingresosAcumulados / NumUnidades;

        /// <summary>
        /// Calcula lo que se le saca al producto que se vende en porcentaje
        /// ¡¡¡ Primera Metrica de Rentabilidad !!!
        /// </summary>
        [XmlIgnore]
        public double? MargenBruto => UBrutaAcumulados / IngresosAcumulados;

        /// <summary>
        /// Calcula  lo que se le saca al producto, 
        /// tomando en cuenta los gastos incurridos para poder vender el producto en porcentaje
        /// ¡¡¡ Segunda Metrica de Rentabilidad !!!
        /// </summary>
        [XmlIgnore]
        public double? MargenOperativo => UOperativaAcumulados / IngresosAcumulados;

        /// <summary>
        /// Calcula el crecimiento de las Utilidades para accionistas en porcentaje de año a año
        /// </summary>
        [XmlIgnore]
        public double? CrecimientoUtilidadesP => UAccionistasAAAcumulados < 0
            ? Math.Abs((UAccionistasAcumulados.GetValueOrDefault() / UAccionistasAAAcumulados.GetValueOrDefault()) - 1)
            : (UAccionistasAcumulados / UAccionistasAAAcumulados) - 1;

        /// <summary>
        /// Calcula la rentabilidad total del negocio
        /// ¡¡¡ Metrica de Rentabilidad Total: A mayor % es más rentable
        /// porque el accionista se lleva más dinero por cada venta!!!
        /// </summary>
        [XmlIgnore]
        public double? MargenNeto => UAccionistasAcumulados / IngresosAcumulados;


        /// <summary>
        /// Margen del NOI respecto al ingreso
        /// </summary>
        [XmlIgnore]
        public double? MargenNOI => NOIAcumulados / IngresosPropAcumulados;

        /// <summary>
        /// Margen del FFO respecto al ingreso
        /// </summary>
        [XmlIgnore]
        public double? MargenFFO => FFOAcumulados / IngresosPropAcumulados;

        /// <summary>
        /// Margen del AFFO respecto al ingreso
        /// </summary>
        [XmlIgnore]
        public double? MargenAFFO => AFFOAcumulados / IngresosPropAcumulados;

        /// <summary>
        /// Margen del NOI respecto al ingreso
        /// </summary>
        [XmlIgnore]
        public double? NOIAccion => NOIAcumulados / AccionesCir;

        /// <summary>
        /// Margen del FFO respecto al ingreso
        /// </summary>
        [XmlIgnore]
        public double? FFOAccion => FFOAcumulados / AccionesCir;

        /// <summary>
        /// Margen del AFFO respecto al ingreso
        /// </summary>
        [XmlIgnore]
        public double? AFFOAccion => AFFOAcumulados / AccionesCir;

        /// <summary>
        /// Calcula Utilidad por Accion, en ingles EPS
        /// </summary>
        [XmlIgnore]
        public double? UPA => UAccionistasAcumulados / AccionesCir;

        /// <summary>
        /// Calcula el precio de la accion entre la Utilidad por Accion (P/U, en ingles P/E)
        /// + Principal Indicador para determinar si una accion es cara o barata +
        /// ¡¡¡ Es el retono de inversion, se puede interpretar como el número de
        /// años que tardaría en recuperar la inversión) !!!
        /// - Es como si fueran las rentas de una casa
        /// </summary>
        [XmlIgnore]
        public double? PU => Precio / UPA;

        /// <summary>
        /// Rendimiento anual del dividendo (Dividend Yield)
        /// </summary>
        [XmlIgnore]
        public double? RentabilidadAnDividendo => DividendosAcumulados / Precio;
        #endregion
    }
}
