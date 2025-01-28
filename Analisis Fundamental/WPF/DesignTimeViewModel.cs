/******************************************************************/
/***** LLENAR ESTA CLASE CONFORME A LA NECESIDAD DEL PROYECTO *****/
/******************************************************************/
/* Poner las propiedades de cada view para ver información dummy en tiempo de diseño */
using DataAccess;
using Models;
using MvvmFramework;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace WPF
{
    public class DesignTimeViewModel
    {
        #region Bootstrapper para ViewModel en Design-Time (no Ejecution-Time)
        [SuppressMessage("CodeQuality", "IDE0052:Remove unread private members",
                         Justification = "No se usa PERO es necesario para inicializar el IoC container")]
        private readonly BootStrp.Bootstrapper bootstrapper = new();
        #endregion

        #region FIELDS
        private readonly IRepository<Empresa, string> repository;
        #endregion

        #region PROPIEDADES para el ShellView
        public object CurrentViewModel { get; } = IoC.Get<MainViewModel>();
        public string Version { get; }= $"Version: YYYY.MM.DD.01";
        #endregion
        #region COMMANDS para el ShellView
        public RelayCommand<Type>? ViewNavigatorCommand { get; }
        #endregion

        #region PROPIEDADES para EmpresasView y ReportesView
        public ObservableCollection<Empresa?> Empresas { get; set; }
        public Empresa? SelectedEmpresa { get; set; }
        #endregion
        #region COMMANDS para EmpresasView y ReportesView
        public RelayCommand? AgregarCommand { get; }
        public RelayCommand? ActualizarCommand { get; }
        public RelayCommand? EliminarCommand { get; }
        public RelayCommand? LimpiarCommand { get; }
        #endregion

        #region PROPIEDADES para EmpresasView
        public Empresa Empresa { get; private set; } = new Empresa() { ID = "AMZN", Nombre = "Amazon", Moneda = "JPY4" };
        #endregion
        
        #region PROPIEDADES para ReportesView
        public Reporte? SelectedReporte { get; set; }
        public Referencia? SelectedReferencia { get; set; }
        public DateTime SelectedFecha { get; set; } = DateTime.Today;
        public Reporte? ReporteActual { get; set; }
        public Reporte? ReporteTA { get; set; }
        public Reporte? ReporteAA { get; set; }
        public List<Multiplo> Multiplos { get; } = new() { new Multiplo("Ninguno", 1), new Multiplo("En Miles", 1000), new Multiplo("En Millones", 1000000) };
        public Multiplo SelectedMultiplo { get; set; }

        public bool IsAnual { get; set; }
        public double ReporteActualCostoVentas { get; set; }
        public double ReporteActualGastosOp { get; set; }
        public double ReporteActualGastosFin { get; set; }
        #endregion
        #region COMMANDS para ReportesView
        public RelayCommand? IsAnualCommand { get; }
        public RelayCommand? MultiploCommand { get; }
        public RelayCommand? FindRefCommand { get; }
        public RelayCommand? PasteAndNextCommand { get; }
        public RelayCommand? PasteAndCloseCommand { get; }
        public RelayCommand? FindStockCommand { get; }
        #endregion

        #region PROPIEDADES para InfoPorAnoView
        public Reporte? Reporte1 { get; set; }
        public Reporte? Reporte2 { get; set; }
        public Reporte? Reporte3 { get; set; }
        public Reporte? Reporte4 { get; set; }
        public Reporte? Reporte5 { get; set; }
        public Reporte? Reporte6 { get; set; }
        public Reporte? Reporte7 { get; set; }
        public Reporte? Reporte8 { get; set; }
        public Reporte? Reporte9 { get; set; }
        #endregion

        #region CONSTRUCTOR
        public DesignTimeViewModel()
        {
            repository = new DesignTimeRepository();

            #region Inicializar propiedades para EmpresasView y ReportesView
            Empresas = new(repository.GetAllCompanies());
            SelectedEmpresa = Empresas[0];
            #endregion

            #region Inicializar propiedades para ReportesView
            ReporteActual = SelectedEmpresa!.Reportes[0];
            ReporteTA = SelectedEmpresa!.Reportes[1];
            ReporteAA = SelectedEmpresa!.Reportes[4];

            SelectedReporte = ReporteActual;
            SelectedReferencia = SelectedEmpresa!.Referencias[0];
            SelectedMultiplo = Multiplos[2];
            
            ReporteActualCostoVentas = ReporteActual.CostoVentas;
            ReporteActualGastosOp = ReporteActual.GastosOp;
            ReporteActualGastosFin = ReporteActual.GastosFin;
            IsAnual = ReporteActual.TrimestreReporte == 4;
            #endregion

            #region Inicializar propiedades para InfoPorAnoView
            Reporte1 = ReporteTA;
            Reporte2 = SelectedEmpresa!.Reportes[2];
            Reporte3 = SelectedEmpresa!.Reportes[3];
            //Reporte4 = ReporteAA;
            //Reporte5 = ReporteAA;
            //Reporte6 = ReporteAA;
            //Reporte7 = ReporteAA;
            //Reporte8 = ReporteAA;
            //Reporte9 = ReporteAA;
            #endregion
        }
        #endregion

        
    }
}