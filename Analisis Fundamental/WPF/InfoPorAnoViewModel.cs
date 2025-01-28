using DataAccess;
using Models;
using MvvmFramework;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace WPF
{
    public class InfoPorAnoViewModel : Validatable
    {
        #region FIELDS
        private readonly IRepository<Empresa, string> repository;
        #endregion

        #region PROPIEDADES
        /// <summary>
        /// Lista de todas las empresas disponibles
        /// </summary>
        public ObservableCollection<Empresa> Empresas { get; } = new ObservableCollection<Empresa>();

        /// <summary>
        /// La empresa seleccionada de la lista
        /// </summary>
        public Empresa? SelectedEmpresa
        {
            private get => selectedEmpresa;
            set
            {
                if (SetProperty(ref selectedEmpresa, value))
                {
                    Properties.Settings.Default.EmpresaID = selectedEmpresa?.ID;
                    RecargarReportes();     // Ordenar los Items para que no esten en el orden original del XML
                    SelectedReporte = (selectedEmpresa?.Reportes.Count) > 0 ? selectedEmpresa?.Reportes[0] : null;
                }
            }
        }
        private Empresa? selectedEmpresa;

        /// <summary>
        /// El reporte seleccionado
        /// </summary>
        public Reporte? SelectedReporte
        {
            private get => selectedReporte;
            set
            {
                if (SetProperty(ref selectedReporte, value))
                { ActualizarReportes(); }
            }
        }
        private Reporte? selectedReporte;

        /// <summary>
        /// Reporte de un año anterior al seleccionado
        /// </summary>
        public Reporte? Reporte1
        {
            get => reporte1;
            set => SetProperty(ref reporte1, value);
        }
        private Reporte? reporte1 = null!;

        /// <summary>
        /// Reporte de un año anterior al seleccionado
        /// </summary>
        public Reporte? Reporte2
        {
            get => reporte2;
            set => SetProperty(ref reporte2, value);
        }
        private Reporte? reporte2 = null!;

        /// <summary>
        /// Reporte de un año anterior al seleccionado
        /// </summary>
        public Reporte? Reporte3
        {
            get => reporte3;
            set => SetProperty(ref reporte3, value);
        }
        private Reporte? reporte3 = null!;

        /// <summary>
        /// Reporte de un año anterior al seleccionado
        /// </summary>
        public Reporte? Reporte4
        {
            get => reporte4;
            set => SetProperty(ref reporte4, value);
        }
        private Reporte? reporte4 = null!;

        /// <summary>
        /// Reporte de un año anterior al seleccionado
        /// </summary>
        public Reporte? Reporte5
        {
            get => reporte5;
            set => SetProperty(ref reporte5, value);
        }
        private Reporte? reporte5 = null!;

        /// <summary>
        /// Reporte de un año anterior al seleccionado
        /// </summary>
        public Reporte? Reporte6
        {
            get => reporte6;
            set => SetProperty(ref reporte6, value);
        }
        private Reporte? reporte6 = null!;

        /// <summary>
        /// Reporte de un año anterior al seleccionado
        /// </summary>
        public Reporte? Reporte7
        {
            get => reporte7;
            set => SetProperty(ref reporte7, value);
        }
        private Reporte? reporte7 = null!;

        /// <summary>
        /// Reporte de un año anterior al seleccionado
        /// </summary>
        public Reporte? Reporte8
        {
            get => reporte8;
            set => SetProperty(ref reporte8, value);
        }
        private Reporte? reporte8 = null!;

        /// <summary>
        /// Reporte de un año anterior al seleccionado
        /// </summary>
        public Reporte? Reporte9
        {
            get => reporte9;
            set => SetProperty(ref reporte9, value);
        }
        private Reporte? reporte9 = null!;

        /// <summary>
        /// Lista de los Multiplos disponibles para seleccionar
        /// </summary>
        public List<Multiplo> Multiplos { get; } = new();

        /// <summary>
        /// El Multiplo seleccionado de la lista de multiplos disponibles
        /// </summary>
        public Multiplo SelectedMultiplo
        {
            private get => selectedMultiplo;
            set
            {
                if (SetProperty(ref selectedMultiplo, value))
                {
                    Properties.Settings.Default.Multiplo = selectedMultiplo?.Valor ?? 1;
                    //bool tempFlag = isReporteActualChanged;
                    ActualizarReportes();
                    //isReporteActualChanged = tempFlag;
                }
            }
        }
        private Multiplo selectedMultiplo = null!;

        #region Propiedades para graficas
        public List<double?> NumUnidades => new()
        {
            Reporte8?.NumUnidadesAA,
            Reporte7?.NumUnidadesAA,
            Reporte6?.NumUnidadesAA,
            Reporte5?.NumUnidadesAA,
            Reporte4?.NumUnidadesAA,
            Reporte3?.NumUnidadesAA,
            Reporte2?.NumUnidadesAA,
            Reporte1?.NumUnidadesAA,
            SelectedReporte?.NumUnidadesAA,
            SelectedReporte?.NumUnidades
        };
        public List<double?> CrecimientoUnidades => new()
        {
            Reporte9?.CrecimientoUnidades,
            Reporte8?.CrecimientoUnidades,
            Reporte7?.CrecimientoUnidades,
            Reporte6?.CrecimientoUnidades,
            Reporte5?.CrecimientoUnidades,
            Reporte4?.CrecimientoUnidades,
            Reporte3?.CrecimientoUnidades,
            Reporte2?.CrecimientoUnidades,
            Reporte1?.CrecimientoUnidades,
            SelectedReporte?.CrecimientoUnidades
        };
        public List<double?> CrecimientoUnidadesP => new()
        {
            Reporte9?.CrecimientoUnidadesP,
            Reporte8?.CrecimientoUnidadesP,
            Reporte7?.CrecimientoUnidadesP,
            Reporte6?.CrecimientoUnidadesP,
            Reporte5?.CrecimientoUnidadesP,
            Reporte4?.CrecimientoUnidadesP,
            Reporte3?.CrecimientoUnidadesP,
            Reporte2?.CrecimientoUnidadesP,
            Reporte1?.CrecimientoUnidadesP,
            SelectedReporte?.CrecimientoUnidadesP
        };
        public List<double?> IngresosAcumulados => new()
        {
            Reporte8?.IngresosAAAcumulados,
            Reporte7?.IngresosAAAcumulados,
            Reporte6?.IngresosAAAcumulados,
            Reporte5?.IngresosAAAcumulados,
            Reporte4?.IngresosAAAcumulados,
            Reporte3?.IngresosAAAcumulados,
            Reporte2?.IngresosAAAcumulados,
            Reporte1?.IngresosAAAcumulados,
            SelectedReporte?.IngresosAAAcumulados,
            SelectedReporte?.IngresosAcumulados
        };
        public List<double?> IngresosUnidad => new()
        {
            Reporte9?.IngresosUnidad,
            Reporte8?.IngresosUnidad,
            Reporte7?.IngresosUnidad,
            Reporte6?.IngresosUnidad,
            Reporte5?.IngresosUnidad,
            Reporte4?.IngresosUnidad,
            Reporte3?.IngresosUnidad,
            Reporte2?.IngresosUnidad,
            Reporte1?.IngresosUnidad,
            SelectedReporte?.IngresosUnidad
        };
        public List<double?> CrecimientoIngresosP => new()
        {
            Reporte9?.CrecimientoIngresosP,
            Reporte8?.CrecimientoIngresosP,
            Reporte7?.CrecimientoIngresosP,
            Reporte6?.CrecimientoIngresosP,
            Reporte5?.CrecimientoIngresosP,
            Reporte4?.CrecimientoIngresosP,
            Reporte3?.CrecimientoIngresosP,
            Reporte2?.CrecimientoIngresosP,
            Reporte1?.CrecimientoIngresosP,
            SelectedReporte?.CrecimientoIngresosP
        };
        public List<double?> CostoVentasAcumulados => new()
        {
            Reporte9?.CostoVentasAcumulados,
            Reporte8?.CostoVentasAcumulados,
            Reporte7?.CostoVentasAcumulados,
            Reporte6?.CostoVentasAcumulados,
            Reporte5?.CostoVentasAcumulados,
            Reporte4?.CostoVentasAcumulados,
            Reporte3?.CostoVentasAcumulados,
            Reporte2?.CostoVentasAcumulados,
            Reporte1?.CostoVentasAcumulados,
            SelectedReporte?.CostoVentasAcumulados
        };
        public List<double?> UBrutaAcumulados => new()
        {
            Reporte9?.UBrutaAcumulados,
            Reporte8?.UBrutaAcumulados,
            Reporte7?.UBrutaAcumulados,
            Reporte6?.UBrutaAcumulados,
            Reporte5?.UBrutaAcumulados,
            Reporte4?.UBrutaAcumulados,
            Reporte3?.UBrutaAcumulados,
            Reporte2?.UBrutaAcumulados,
            Reporte1?.UBrutaAcumulados,
            SelectedReporte?.UBrutaAcumulados
        };
        public List<double?> MargenBruto => new()
        {
            Reporte9?.MargenBruto,
            Reporte8?.MargenBruto,
            Reporte7?.MargenBruto,
            Reporte6?.MargenBruto,
            Reporte5?.MargenBruto,
            Reporte4?.MargenBruto,
            Reporte3?.MargenBruto,
            Reporte2?.MargenBruto,
            Reporte1?.MargenBruto,
            SelectedReporte?.MargenBruto
        };
        public List<double?> GastosOpAcumulados => new()
        {
            Reporte9?.GastosOpAcumulados,
            Reporte8?.GastosOpAcumulados,
            Reporte7?.GastosOpAcumulados,
            Reporte6?.GastosOpAcumulados,
            Reporte5?.GastosOpAcumulados,
            Reporte4?.GastosOpAcumulados,
            Reporte3?.GastosOpAcumulados,
            Reporte2?.GastosOpAcumulados,
            Reporte1?.GastosOpAcumulados,
            SelectedReporte?.GastosOpAcumulados
        };
        public List<double?> UOperativaAcumulados => new()
        {
            Reporte9?.UOperativaAcumulados,
            Reporte8?.UOperativaAcumulados,
            Reporte7?.UOperativaAcumulados,
            Reporte6?.UOperativaAcumulados,
            Reporte5?.UOperativaAcumulados,
            Reporte4?.UOperativaAcumulados,
            Reporte3?.UOperativaAcumulados,
            Reporte2?.UOperativaAcumulados,
            Reporte1?.UOperativaAcumulados,
            SelectedReporte?.UOperativaAcumulados
        };
        public List<double?> MargenOperativo => new()
        {
            Reporte9?.MargenOperativo,
            Reporte8?.MargenOperativo,
            Reporte7?.MargenOperativo,
            Reporte6?.MargenOperativo,
            Reporte5?.MargenOperativo,
            Reporte4?.MargenOperativo,
            Reporte3?.MargenOperativo,
            Reporte2?.MargenOperativo,
            Reporte1?.MargenOperativo,
            SelectedReporte?.MargenOperativo
        };
        public List<double?> GastosFinAcumulados => new()
        {
            Reporte9?.GastosFinAcumulados,
            Reporte8?.GastosFinAcumulados,
            Reporte7?.GastosFinAcumulados,
            Reporte6?.GastosFinAcumulados,
            Reporte5?.GastosFinAcumulados,
            Reporte4?.GastosFinAcumulados,
            Reporte3?.GastosFinAcumulados,
            Reporte2?.GastosFinAcumulados,
            Reporte1?.GastosFinAcumulados,
            SelectedReporte?.GastosFinAcumulados
        };
        public List<double?> UAImpuestosAcumulados => new()
        {
            Reporte9?.UAImpuestosAcumulados,
            Reporte8?.UAImpuestosAcumulados,
            Reporte7?.UAImpuestosAcumulados,
            Reporte6?.UAImpuestosAcumulados,
            Reporte5?.UAImpuestosAcumulados,
            Reporte4?.UAImpuestosAcumulados,
            Reporte3?.UAImpuestosAcumulados,
            Reporte2?.UAImpuestosAcumulados,
            Reporte1?.UAImpuestosAcumulados,
            SelectedReporte?.UAImpuestosAcumulados
        };
        public List<double?> ImpuestosAcumulados => new()
        {
            Reporte9?.ImpuestosAcumulados,
            Reporte8?.ImpuestosAcumulados,
            Reporte7?.ImpuestosAcumulados,
            Reporte6?.ImpuestosAcumulados,
            Reporte5?.ImpuestosAcumulados,
            Reporte4?.ImpuestosAcumulados,
            Reporte3?.ImpuestosAcumulados,
            Reporte2?.ImpuestosAcumulados,
            Reporte1?.ImpuestosAcumulados,
            SelectedReporte?.ImpuestosAcumulados
        };
        public List<double?> UNetaAcumulados => new()
        {
            Reporte9?.UNetaAcumulados,
            Reporte8?.UNetaAcumulados,
            Reporte7?.UNetaAcumulados,
            Reporte6?.UNetaAcumulados,
            Reporte5?.UNetaAcumulados,
            Reporte4?.UNetaAcumulados,
            Reporte3?.UNetaAcumulados,
            Reporte2?.UNetaAcumulados,
            Reporte1?.UNetaAcumulados,
            SelectedReporte?.UNetaAcumulados
        };
        public List<double?> UAccionistasAcumulados => new()
        {
            Reporte8?.UAccionistasAAAcumulados,
            Reporte7?.UAccionistasAAAcumulados,
            Reporte6?.UAccionistasAAAcumulados,
            Reporte5?.UAccionistasAAAcumulados,
            Reporte4?.UAccionistasAAAcumulados,
            Reporte3?.UAccionistasAAAcumulados,
            Reporte2?.UAccionistasAAAcumulados,
            Reporte1?.UAccionistasAAAcumulados,
            SelectedReporte?.UAccionistasAAAcumulados,
            SelectedReporte?.UAccionistasAcumulados
        };
        public List<double?> CrecimientoUtilidadesP => new()
        {
            Reporte9?.CrecimientoUtilidadesP,
            Reporte8?.CrecimientoUtilidadesP,
            Reporte7?.CrecimientoUtilidadesP,
            Reporte6?.CrecimientoUtilidadesP,
            Reporte5?.CrecimientoUtilidadesP,
            Reporte4?.CrecimientoUtilidadesP,
            Reporte3?.CrecimientoUtilidadesP,
            Reporte2?.CrecimientoUtilidadesP,
            Reporte1?.CrecimientoUtilidadesP,
            SelectedReporte?.CrecimientoUtilidadesP
        };
        public List<double?> MargenNeto => new()
        {
            Reporte9?.MargenNeto,
            Reporte8?.MargenNeto,
            Reporte7?.MargenNeto,
            Reporte6?.MargenNeto,
            Reporte5?.MargenNeto,
            Reporte4?.MargenNeto,
            Reporte3?.MargenNeto,
            Reporte2?.MargenNeto,
            Reporte1?.MargenNeto,
            SelectedReporte?.MargenNeto
        };
        public List<double?> IngresosPropAcumulados => new()
        {
            Reporte9?.IngresosPropAcumulados,
            Reporte8?.IngresosPropAcumulados,
            Reporte7?.IngresosPropAcumulados,
            Reporte6?.IngresosPropAcumulados,
            Reporte5?.IngresosPropAcumulados,
            Reporte4?.IngresosPropAcumulados,
            Reporte3?.IngresosPropAcumulados,
            Reporte2?.IngresosPropAcumulados,
            Reporte1?.IngresosPropAcumulados,
            SelectedReporte?.IngresosPropAcumulados
        };
        public List<double?> NOIAcumulados => new()
        {
            Reporte9?.NOIAcumulados,
            Reporte8?.NOIAcumulados,
            Reporte7?.NOIAcumulados,
            Reporte6?.NOIAcumulados,
            Reporte5?.NOIAcumulados,
            Reporte4?.NOIAcumulados,
            Reporte3?.NOIAcumulados,
            Reporte2?.NOIAcumulados,
            Reporte1?.NOIAcumulados,
            SelectedReporte?.NOIAcumulados
        };
        public List<double?> NOIAccion => new()
        {
            Reporte9?.NOIAccion,
            Reporte8?.NOIAccion,
            Reporte7?.NOIAccion,
            Reporte6?.NOIAccion,
            Reporte5?.NOIAccion,
            Reporte4?.NOIAccion,
            Reporte3?.NOIAccion,
            Reporte2?.NOIAccion,
            Reporte1?.NOIAccion,
            SelectedReporte?.NOIAccion
        };
        public List<double?> MargenNOI => new()
        {
            Reporte9?.MargenNOI,
            Reporte8?.MargenNOI,
            Reporte7?.MargenNOI,
            Reporte6?.MargenNOI,
            Reporte5?.MargenNOI,
            Reporte4?.MargenNOI,
            Reporte3?.MargenNOI,
            Reporte2?.MargenNOI,
            Reporte1?.MargenNOI,
            SelectedReporte?.MargenNOI
        };
        public List<double?> FFOAcumulados => new()
        {
            Reporte9?.FFOAcumulados,
            Reporte8?.FFOAcumulados,
            Reporte7?.FFOAcumulados,
            Reporte6?.FFOAcumulados,
            Reporte5?.FFOAcumulados,
            Reporte4?.FFOAcumulados,
            Reporte3?.FFOAcumulados,
            Reporte2?.FFOAcumulados,
            Reporte1?.FFOAcumulados,
            SelectedReporte?.FFOAcumulados
        };
        public List<double?> FFOAccion => new()
        {
            Reporte9?.FFOAccion,
            Reporte8?.FFOAccion,
            Reporte7?.FFOAccion,
            Reporte6?.FFOAccion,
            Reporte5?.FFOAccion,
            Reporte4?.FFOAccion,
            Reporte3?.FFOAccion,
            Reporte2?.FFOAccion,
            Reporte1?.FFOAccion,
            SelectedReporte?.FFOAccion
        };
        public List<double?> MargenFFO => new()
        {
            Reporte9?.MargenFFO,
            Reporte8?.MargenFFO,
            Reporte7?.MargenFFO,
            Reporte6?.MargenFFO,
            Reporte5?.MargenFFO,
            Reporte4?.MargenFFO,
            Reporte3?.MargenFFO,
            Reporte2?.MargenFFO,
            Reporte1?.MargenFFO,
            SelectedReporte?.MargenFFO
        };
        public List<double?> AFFOAcumulados => new()
        {
            Reporte9?.AFFOAcumulados,
            Reporte8?.AFFOAcumulados,
            Reporte7?.AFFOAcumulados,
            Reporte6?.AFFOAcumulados,
            Reporte5?.AFFOAcumulados,
            Reporte4?.AFFOAcumulados,
            Reporte3?.AFFOAcumulados,
            Reporte2?.AFFOAcumulados,
            Reporte1?.AFFOAcumulados,
            SelectedReporte?.AFFOAcumulados
        };
        public List<double?> AFFOAccion => new()
        {
            Reporte9?.AFFOAccion,
            Reporte8?.AFFOAccion,
            Reporte7?.AFFOAccion,
            Reporte6?.AFFOAccion,
            Reporte5?.AFFOAccion,
            Reporte4?.AFFOAccion,
            Reporte3?.AFFOAccion,
            Reporte2?.AFFOAccion,
            Reporte1?.AFFOAccion,
            SelectedReporte?.AFFOAccion
        };
        public List<double?> MargenAFFO => new()
        {
            Reporte9?.MargenAFFO,
            Reporte8?.MargenAFFO,
            Reporte7?.MargenAFFO,
            Reporte6?.MargenAFFO,
            Reporte5?.MargenAFFO,
            Reporte4?.MargenAFFO,
            Reporte3?.MargenAFFO,
            Reporte2?.MargenAFFO,
            Reporte1?.MargenAFFO,
            SelectedReporte?.MargenAFFO
        };
        public List<double?> Efectivo => new()
        {
            Reporte9?.Efectivo,
            Reporte8?.Efectivo,
            Reporte7?.Efectivo,
            Reporte6?.Efectivo,
            Reporte5?.Efectivo,
            Reporte4?.Efectivo,
            Reporte3?.Efectivo,
            Reporte2?.Efectivo,
            Reporte1?.Efectivo,
            SelectedReporte?.Efectivo
        };
        public List<double?> Deuda => new()
        {
            Reporte9?.Deuda,
            Reporte8?.Deuda,
            Reporte7?.Deuda,
            Reporte6?.Deuda,
            Reporte5?.Deuda,
            Reporte4?.Deuda,
            Reporte3?.Deuda,
            Reporte2?.Deuda,
            Reporte1?.Deuda,
            SelectedReporte?.Deuda
        };
        public List<double?> ActivosC => new()
        {
            Reporte9?.ActivosC,
            Reporte8?.ActivosC,
            Reporte7?.ActivosC,
            Reporte6?.ActivosC,
            Reporte5?.ActivosC,
            Reporte4?.ActivosC,
            Reporte3?.ActivosC,
            Reporte2?.ActivosC,
            Reporte1?.ActivosC,
            SelectedReporte?.ActivosC
        };
        public List<double?> PasivosC => new()
        {
            Reporte9?.PasivosC,
            Reporte8?.PasivosC,
            Reporte7?.PasivosC,
            Reporte6?.PasivosC,
            Reporte5?.PasivosC,
            Reporte4?.PasivosC,
            Reporte3?.PasivosC,
            Reporte2?.PasivosC,
            Reporte1?.PasivosC,
            SelectedReporte?.PasivosC
        };
        public List<double?> ActivosT => new()
        {
            Reporte9?.ActivosT,
            Reporte8?.ActivosT,
            Reporte7?.ActivosT,
            Reporte6?.ActivosT,
            Reporte5?.ActivosT,
            Reporte4?.ActivosT,
            Reporte3?.ActivosT,
            Reporte2?.ActivosT,
            Reporte1?.ActivosT,
            SelectedReporte?.ActivosT
        };
        public List<double?> PasivosT => new()
        {
            Reporte9?.PasivosT,
            Reporte8?.PasivosT,
            Reporte7?.PasivosT,
            Reporte6?.PasivosT,
            Reporte5?.PasivosT,
            Reporte4?.PasivosT,
            Reporte3?.PasivosT,
            Reporte2?.PasivosT,
            Reporte1?.PasivosT,
            SelectedReporte?.PasivosT
        };
        public List<double?> Capital => new()
        {
            Reporte8?.CapitalAA,
            Reporte7?.CapitalAA,
            Reporte6?.CapitalAA,
            Reporte5?.CapitalAA,
            Reporte4?.CapitalAA,
            Reporte3?.CapitalAA,
            Reporte2?.CapitalAA,
            Reporte1?.CapitalAA,
            SelectedReporte?.CapitalAA,
            SelectedReporte?.Capital
        };
        public List<double?> CrecimientoCapitalP => new()
        {
            Reporte9?.CrecimientoCapitalP,
            Reporte8?.CrecimientoCapitalP,
            Reporte7?.CrecimientoCapitalP,
            Reporte6?.CrecimientoCapitalP,
            Reporte5?.CrecimientoCapitalP,
            Reporte4?.CrecimientoCapitalP,
            Reporte3?.CrecimientoCapitalP,
            Reporte2?.CrecimientoCapitalP,
            Reporte1?.CrecimientoCapitalP,
            SelectedReporte?.CrecimientoCapitalP
        };
        public List<double?> ActCPasvC => new()
        {
            Reporte9?.ActCPasvC,
            Reporte8?.ActCPasvC,
            Reporte7?.ActCPasvC,
            Reporte6?.ActCPasvC,
            Reporte5?.ActCPasvC,
            Reporte4?.ActCPasvC,
            Reporte3?.ActCPasvC,
            Reporte2?.ActCPasvC,
            Reporte1?.ActCPasvC,
            SelectedReporte?.ActCPasvC
        };
        public List<double?> ActTPasvT => new()
        {
            Reporte9?.ActTPasvT,
            Reporte8?.ActTPasvT,
            Reporte7?.ActTPasvT,
            Reporte6?.ActTPasvT,
            Reporte5?.ActTPasvT,
            Reporte4?.ActTPasvT,
            Reporte3?.ActTPasvT,
            Reporte2?.ActTPasvT,
            Reporte1?.ActTPasvT,
            SelectedReporte?.ActTPasvT
        };
        public List<double?> PasvTActT => new()
        {
            Reporte9?.PasvTActT,
            Reporte8?.PasvTActT,
            Reporte7?.PasvTActT,
            Reporte6?.PasvTActT,
            Reporte5?.PasvTActT,
            Reporte4?.PasvTActT,
            Reporte3?.PasvTActT,
            Reporte2?.PasvTActT,
            Reporte1?.PasvTActT,
            SelectedReporte?.PasvTActT
        };
        public List<double?> PruebaAcido => new()
        {
            Reporte9?.PruebaAcido,
            Reporte8?.PruebaAcido,
            Reporte7?.PruebaAcido,
            Reporte6?.PruebaAcido,
            Reporte5?.PruebaAcido,
            Reporte4?.PruebaAcido,
            Reporte3?.PruebaAcido,
            Reporte2?.PruebaAcido,
            Reporte1?.PruebaAcido,
            SelectedReporte?.PruebaAcido
        };
        public List<double?> EfectivoDeuda => new()
        {
            Reporte9?.EfectivoDeuda,
            Reporte8?.EfectivoDeuda,
            Reporte7?.EfectivoDeuda,
            Reporte6?.EfectivoDeuda,
            Reporte5?.EfectivoDeuda,
            Reporte4?.EfectivoDeuda,
            Reporte3?.EfectivoDeuda,
            Reporte2?.EfectivoDeuda,
            Reporte1?.EfectivoDeuda,
            SelectedReporte?.EfectivoDeuda
        };
        public List<double?> EfectivoCapital => new()
        {
            Reporte9?.EfectivoCapital,
            Reporte8?.EfectivoCapital,
            Reporte7?.EfectivoCapital,
            Reporte6?.EfectivoCapital,
            Reporte5?.EfectivoCapital,
            Reporte4?.EfectivoCapital,
            Reporte3?.EfectivoCapital,
            Reporte2?.EfectivoCapital,
            Reporte1?.EfectivoCapital,
            SelectedReporte?.EfectivoCapital
        };
        public List<double?> DeudaActivosT => new()
        {
            Reporte9?.DeudaActivosT,
            Reporte8?.DeudaActivosT,
            Reporte7?.DeudaActivosT,
            Reporte6?.DeudaActivosT,
            Reporte5?.DeudaActivosT,
            Reporte4?.DeudaActivosT,
            Reporte3?.DeudaActivosT,
            Reporte2?.DeudaActivosT,
            Reporte1?.DeudaActivosT,
            SelectedReporte?.DeudaActivosT
        };
        public List<double?> DeudaCapital => new()
        {
            Reporte9?.DeudaCapital,
            Reporte8?.DeudaCapital,
            Reporte7?.DeudaCapital,
            Reporte6?.DeudaCapital,
            Reporte5?.DeudaCapital,
            Reporte4?.DeudaCapital,
            Reporte3?.DeudaCapital,
            Reporte2?.DeudaCapital,
            Reporte1?.DeudaCapital,
            SelectedReporte?.DeudaCapital
        };
        public List<double?> FlujoOperaciones => new()
        {
            Reporte9?.FlujoOperaciones,
            Reporte8?.FlujoOperaciones,
            Reporte7?.FlujoOperaciones,
            Reporte6?.FlujoOperaciones,
            Reporte5?.FlujoOperaciones,
            Reporte4?.FlujoOperaciones,
            Reporte3?.FlujoOperaciones,
            Reporte2?.FlujoOperaciones,
            Reporte1?.FlujoOperaciones,
            SelectedReporte?.FlujoOperaciones
        };
        public List<double?> FlujoInversiones => new()
        {
            Reporte9?.FlujoInversiones,
            Reporte8?.FlujoInversiones,
            Reporte7?.FlujoInversiones,
            Reporte6?.FlujoInversiones,
            Reporte5?.FlujoInversiones,
            Reporte4?.FlujoInversiones,
            Reporte3?.FlujoInversiones,
            Reporte2?.FlujoInversiones,
            Reporte1?.FlujoInversiones,
            SelectedReporte?.FlujoInversiones
        };
        public List<double?> FlujoFinanciamiento => new()
        {
            Reporte9?.FlujoFinanciamiento,
            Reporte8?.FlujoFinanciamiento,
            Reporte7?.FlujoFinanciamiento,
            Reporte6?.FlujoFinanciamiento,
            Reporte5?.FlujoFinanciamiento,
            Reporte4?.FlujoFinanciamiento,
            Reporte3?.FlujoFinanciamiento,
            Reporte2?.FlujoFinanciamiento,
            Reporte1?.FlujoFinanciamiento,
            SelectedReporte?.FlujoFinanciamiento
        };
        public List<double?> EfectivoAlInicio => new()
        {
            Reporte9?.EfectivoAlInicio,
            Reporte8?.EfectivoAlInicio,
            Reporte7?.EfectivoAlInicio,
            Reporte6?.EfectivoAlInicio,
            Reporte5?.EfectivoAlInicio,
            Reporte4?.EfectivoAlInicio,
            Reporte3?.EfectivoAlInicio,
            Reporte2?.EfectivoAlInicio,
            Reporte1?.EfectivoAlInicio,
            SelectedReporte?.EfectivoAlInicio
        };
        public List<double?> EfectivoAlFinal => new()
        {
            Reporte9?.EfectivoAlFinal,
            Reporte8?.EfectivoAlFinal,
            Reporte7?.EfectivoAlFinal,
            Reporte6?.EfectivoAlFinal,
            Reporte5?.EfectivoAlFinal,
            Reporte4?.EfectivoAlFinal,
            Reporte3?.EfectivoAlFinal,
            Reporte2?.EfectivoAlFinal,
            Reporte1?.EfectivoAlFinal,
            SelectedReporte?.EfectivoAlFinal
        };
        public List<double?> AccionesCir => new()
        {
            Reporte9?.AccionesCir,
            Reporte8?.AccionesCir,
            Reporte7?.AccionesCir,
            Reporte6?.AccionesCir,
            Reporte5?.AccionesCir,
            Reporte4?.AccionesCir,
            Reporte3?.AccionesCir,
            Reporte2?.AccionesCir,
            Reporte1?.AccionesCir,
            SelectedReporte?.AccionesCir
        };
        public List<double?> Precio => new()
        {
            Reporte9?.Precio,
            Reporte8?.Precio,
            Reporte7?.Precio,
            Reporte6?.Precio,
            Reporte5?.Precio,
            Reporte4?.Precio,
            Reporte3?.Precio,
            Reporte2?.Precio,
            Reporte1?.Precio,
            SelectedReporte?.Precio
        };
        public List<double?> CapitalizaciónMercado => new()
        {
            Reporte9?.CapitalizaciónMercado,
            Reporte8?.CapitalizaciónMercado,
            Reporte7?.CapitalizaciónMercado,
            Reporte6?.CapitalizaciónMercado,
            Reporte5?.CapitalizaciónMercado,
            Reporte4?.CapitalizaciónMercado,
            Reporte3?.CapitalizaciónMercado,
            Reporte2?.CapitalizaciónMercado,
            Reporte1?.CapitalizaciónMercado,
            SelectedReporte?.CapitalizaciónMercado
        };
        public List<double?> UPA => new()
        {
            Reporte9?.UPA,
            Reporte8?.UPA,
            Reporte7?.UPA,
            Reporte6?.UPA,
            Reporte5?.UPA,
            Reporte4?.UPA,
            Reporte3?.UPA,
            Reporte2?.UPA,
            Reporte1?.UPA,
            SelectedReporte?.UPA
        };
        public List<double?> PU => new()
        {
            Reporte9?.PU,
            Reporte8?.PU,
            Reporte7?.PU,
            Reporte6?.PU,
            Reporte5?.PU,
            Reporte4?.PU,
            Reporte3?.PU,
            Reporte2?.PU,
            Reporte1?.PU,
            SelectedReporte?.PU
        };
        public List<double?> ValorLibro => new()
        {
            Reporte9?.ValorLibro,
            Reporte8?.ValorLibro,
            Reporte7?.ValorLibro,
            Reporte6?.ValorLibro,
            Reporte5?.ValorLibro,
            Reporte4?.ValorLibro,
            Reporte3?.ValorLibro,
            Reporte2?.ValorLibro,
            Reporte1?.ValorLibro,
            SelectedReporte?.ValorLibro
        };
        public List<double?> PrecioValorLibro => new()
        {
            Reporte9?.PrecioValorLibro,
            Reporte8?.PrecioValorLibro,
            Reporte7?.PrecioValorLibro,
            Reporte6?.PrecioValorLibro,
            Reporte5?.PrecioValorLibro,
            Reporte4?.PrecioValorLibro,
            Reporte3?.PrecioValorLibro,
            Reporte2?.PrecioValorLibro,
            Reporte1?.PrecioValorLibro,
            SelectedReporte?.PrecioValorLibro
        };
        public List<double?> DividendosAcumulados => new()
        {
            Reporte9?.DividendosAcumulados,
            Reporte8?.DividendosAcumulados,
            Reporte7?.DividendosAcumulados,
            Reporte6?.DividendosAcumulados,
            Reporte5?.DividendosAcumulados,
            Reporte4?.DividendosAcumulados,
            Reporte3?.DividendosAcumulados,
            Reporte2?.DividendosAcumulados,
            Reporte1?.DividendosAcumulados,
            SelectedReporte?.DividendosAcumulados
        };
        public List<double?> RentabilidadAnDividendo => new()
        {
            Reporte9?.RentabilidadAnDividendo,
            Reporte8?.RentabilidadAnDividendo,
            Reporte7?.RentabilidadAnDividendo,
            Reporte6?.RentabilidadAnDividendo,
            Reporte5?.RentabilidadAnDividendo,
            Reporte4?.RentabilidadAnDividendo,
            Reporte3?.RentabilidadAnDividendo,
            Reporte2?.RentabilidadAnDividendo,
            Reporte1?.RentabilidadAnDividendo,
            SelectedReporte?.RentabilidadAnDividendo
        };
        #endregion
#endregion

        #region CONSTRUCTOR
        public InfoPorAnoViewModel(IRepository<Empresa, string> repository)
        {
            this.repository = repository;

            // Inicializar los comandos
            MultiploCommand = new(Multiplo_Execute);

            // Inicializar Propiedades o Fields
            Multiplos.Add(new Multiplo("Ninguno", 1));
            Multiplos.Add(new Multiplo("En Miles", 1000));
            Multiplos.Add(new Multiplo("En Millones", 1000000));
            //SelectedMultiplo = Multiplos[0];
            SelectedMultiplo = Multiplos.FirstOrDefault(e => e.Valor == Properties.Settings.Default.Multiplo) ?? Multiplos[0];
            RecargarEmpresas();

            SelectedEmpresa = Empresas.FirstOrDefault(e => e.ID == Properties.Settings.Default.EmpresaID);
        }
        #endregion

        #region COMMANDS
        public RelayCommand<int> MultiploCommand { get; }
        /// <summary>
        /// Cambia al valor del multiplo
        /// </summary>
        /// <param name="obj">El valor del multiplo que se desea usar</param>
        private void Multiplo_Execute(int valor)
        {
            SelectedMultiplo = Multiplos.FirstOrDefault(e => e.Valor == valor) ?? Multiplos[0];
        }
        #endregion

        #region METODOS privados
        /// <summary>
        /// Elimina la lista actual de <see cref="Empresa"/> y carga una nueva desde la base de datos
        /// </summary>
        private void RecargarEmpresas()
        {
            Empresas.Clear();
            foreach (Empresa empresa in repository.GetAllCompanies())
            { Empresas.Add(empresa); }
        }

        /// <summary>
        /// Elimina la lista actual de <see cref="Reporte"/> y carga una nueva desde la base de datos
        /// </summary>
        private void RecargarReportes()
        {
            SelectedEmpresa!.Reportes.Clear();  //Ya se verifico que SelectedEmpresa no sea NULL
            foreach (Reporte reporte in repository.GetAllReports(SelectedEmpresa.ID))
            { SelectedEmpresa.Reportes.Add(reporte); }
        }

        /// <summary>
        /// Actualiza todos los reportes en base al reporte y multiplo seleccionado
        /// </summary>
        private void ActualizarReportes()
        {
            if (SelectedEmpresa is null) { return; }
            if (SelectedReporte is null)
            {
                Reporte1 = null;
                Reporte2 = null;
                Reporte3 = null;
                Reporte4 = null;
                Reporte5 = null;
                Reporte6 = null;
                Reporte7 = null;
                Reporte8 = null;
                Reporte9 = null;
            }
            else
            {
                Reporte1 = SelectedEmpresa?.Reportes.FirstOrDefault(r => r.ID == SelectedReporte.TrimestreIDOffset(-1 * 4));
                Reporte2 = SelectedEmpresa?.Reportes.FirstOrDefault(r => r.ID == SelectedReporte.TrimestreIDOffset(-2 * 4));
                Reporte3 = SelectedEmpresa?.Reportes.FirstOrDefault(r => r.ID == SelectedReporte.TrimestreIDOffset(-3 * 4));
                Reporte4 = SelectedEmpresa?.Reportes.FirstOrDefault(r => r.ID == SelectedReporte.TrimestreIDOffset(-4 * 4));
                Reporte5 = SelectedEmpresa?.Reportes.FirstOrDefault(r => r.ID == SelectedReporte.TrimestreIDOffset(-5 * 4));
                Reporte6 = SelectedEmpresa?.Reportes.FirstOrDefault(r => r.ID == SelectedReporte.TrimestreIDOffset(-6 * 4));
                Reporte7 = SelectedEmpresa?.Reportes.FirstOrDefault(r => r.ID == SelectedReporte.TrimestreIDOffset(-7 * 4));
                Reporte8 = SelectedEmpresa?.Reportes.FirstOrDefault(r => r.ID == SelectedReporte.TrimestreIDOffset(-8 * 4));
                Reporte9 = SelectedEmpresa?.Reportes.FirstOrDefault(r => r.ID == SelectedReporte.TrimestreIDOffset(-9 * 4));

                SelectedReporte.Factor = SelectedMultiplo.Valor;
                if (Reporte1 is not null) { Reporte1.Factor = SelectedMultiplo.Valor; }
                if (Reporte2 is not null) { Reporte2.Factor = SelectedMultiplo.Valor; }
                if (Reporte3 is not null) { Reporte3.Factor = SelectedMultiplo.Valor; }
                if (Reporte4 is not null) { Reporte4.Factor = SelectedMultiplo.Valor; }
                if (Reporte5 is not null) { Reporte5.Factor = SelectedMultiplo.Valor; }
                if (Reporte6 is not null) { Reporte6.Factor = SelectedMultiplo.Valor; }
                if (Reporte7 is not null) { Reporte7.Factor = SelectedMultiplo.Valor; }
                if (Reporte8 is not null) { Reporte8.Factor = SelectedMultiplo.Valor; }
                if (Reporte9 is not null) { Reporte9.Factor = SelectedMultiplo.Valor; }
            }

            OnPropertyChange(nameof(NumUnidades));
            OnPropertyChange(nameof(CrecimientoUnidades));
            OnPropertyChange(nameof(CrecimientoUnidadesP));
            OnPropertyChange(nameof(IngresosAcumulados));
            OnPropertyChange(nameof(IngresosUnidad));
            OnPropertyChange(nameof(CrecimientoIngresosP));
            OnPropertyChange(nameof(CostoVentasAcumulados));
            OnPropertyChange(nameof(UBrutaAcumulados));
            OnPropertyChange(nameof(MargenBruto));
            OnPropertyChange(nameof(GastosOpAcumulados));
            OnPropertyChange(nameof(UOperativaAcumulados));
            OnPropertyChange(nameof(MargenOperativo));
            OnPropertyChange(nameof(GastosFinAcumulados));
            OnPropertyChange(nameof(UAImpuestosAcumulados));
            OnPropertyChange(nameof(ImpuestosAcumulados));
            OnPropertyChange(nameof(UNetaAcumulados));
            OnPropertyChange(nameof(UAccionistasAcumulados));
            OnPropertyChange(nameof(CrecimientoUtilidadesP));
            OnPropertyChange(nameof(MargenNeto));
            OnPropertyChange(nameof(IngresosPropAcumulados));
            OnPropertyChange(nameof(NOIAcumulados));
            OnPropertyChange(nameof(NOIAccion));
            OnPropertyChange(nameof(MargenNOI));
            OnPropertyChange(nameof(FFOAcumulados));
            OnPropertyChange(nameof(FFOAccion));
            OnPropertyChange(nameof(MargenFFO));
            OnPropertyChange(nameof(AFFOAcumulados));
            OnPropertyChange(nameof(AFFOAccion));
            OnPropertyChange(nameof(MargenAFFO));
            OnPropertyChange(nameof(Efectivo));
            OnPropertyChange(nameof(ActivosC));
            OnPropertyChange(nameof(PasivosC));
            OnPropertyChange(nameof(ActivosT));
            OnPropertyChange(nameof(PasivosT));
            OnPropertyChange(nameof(Capital));
            OnPropertyChange(nameof(CrecimientoCapitalP));
            OnPropertyChange(nameof(ActCPasvC));
            OnPropertyChange(nameof(ActTPasvT));
            OnPropertyChange(nameof(PasvTActT));
            OnPropertyChange(nameof(PruebaAcido));
            OnPropertyChange(nameof(Deuda));
            OnPropertyChange(nameof(EfectivoDeuda));
            OnPropertyChange(nameof(EfectivoCapital));
            OnPropertyChange(nameof(DeudaActivosT));
            OnPropertyChange(nameof(DeudaCapital));
            OnPropertyChange(nameof(FlujoOperaciones));
            OnPropertyChange(nameof(FlujoInversiones));
            OnPropertyChange(nameof(FlujoFinanciamiento));
            OnPropertyChange(nameof(EfectivoAlInicio));
            OnPropertyChange(nameof(EfectivoAlFinal));
            OnPropertyChange(nameof(AccionesCir));
            OnPropertyChange(nameof(Precio));
            OnPropertyChange(nameof(CapitalizaciónMercado));
            OnPropertyChange(nameof(UPA));
            OnPropertyChange(nameof(PU));
            OnPropertyChange(nameof(ValorLibro));
            OnPropertyChange(nameof(PrecioValorLibro));
            OnPropertyChange(nameof(DividendosAcumulados));
            OnPropertyChange(nameof(RentabilidadAnDividendo));
        }
        #endregion
    }
}

/* COMO PASAR PARAMETROS PRIMITIVOS A ESTA CLASE
    1.- Desde la clase que se instanciara esta clase (InfoPorAnoViewModel), 
        Agregar el Dato Primitivo al AppSettings con el nombre llave igual al nombre del parametro
            ConfigurationManager.AppSettings.Set("primitiveValue", "123");

    2.- Crear la intancia con el IoC o el publish del Aggregator
            aggregator.Publish(new SwitchToVm(typeof(InfoPorAnoViewModel)));
 */