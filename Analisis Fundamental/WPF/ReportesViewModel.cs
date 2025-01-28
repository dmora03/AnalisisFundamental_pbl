using DataAccess;
using Models;
using MvvmFramework;
using SimulateMouseKeyboard;
using StockFromYahoo;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace WPF
{
    public class Multiplo
    {
        /// <summary>
        /// Nombre a visualizar del multiplo
        /// </summary>
        public string Nombre { get; set; } = "";

        /// <summary>
        /// Valor del multiplo
        /// </summary>
        public int Valor { get; set; }

        /// <summary>
        /// Crea una nueva instancia inicializando sus propiedades
        /// </summary>
        /// <param name="nombre">Nombre a visualizar</param>
        /// <param name="valor">Valor del multiplo</param>
        public Multiplo(string nombre, int valor)
        {
            Nombre = nombre;
            Valor = valor;
        }
    }

    public class ReportesViewModel : Validatable
    {
        #region FIELDS
        private readonly IWindowManager windowManager;
        private readonly IRepository<Empresa, string> repository;

        private bool disabledReporteActual_PropertyChanged;
        private bool isReporteActualChanged;
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
                _ = ReporteActualATrimestral();

                if (SetProperty(ref selectedEmpresa, value, ReporteActual_PropertyChanged)
                    && SelectedEmpresa is not null)
                {
                    Properties.Settings.Default.EmpresaID = selectedEmpresa?.ID;

                    #region Ordenar los Items para que no esten en el orden original del XML
                    RecargarReportes();
                    RecargarReferencias();
                    #endregion

                    Limpiar_Execute();
                    SelectedReferencia = selectedEmpresa?.Referencias.FirstOrDefault();
                }
            }
        }
        private Empresa? selectedEmpresa;

        /// <summary>
        /// El reporte seleccionado para editar o eliminar
        /// </summary>
        public Reporte? SelectedReporte
        {
            private get => selectedReporte;
            set
            {
                if (SetProperty(ref selectedReporte, value))
                {
                    ReporteActualATrimestral();
                    ActualizarReportes();
                }
            }
        }
        private Reporte? selectedReporte;

        /// <summary>
        /// El conjunto de referencias seleccionados para mostrar o usar con REGEX
        /// </summary>
        public Referencia? SelectedReferencia
        {
            private get => selectedReferencia;
            set => SetProperty(ref selectedReferencia, value, ReporteActual_PropertyChanged);
        }
        private Referencia? selectedReferencia;

        /// <summary>
        /// Fecha seleccionada SOLO para crear un nuevo reporte
        /// </summary>
        public DateTime SelectedFecha
        {
            private get => selectedFecha;
            set => SetProperty(ref selectedFecha, value);
        }
        private DateTime selectedFecha = DateTime.Today;

        /// <summary>
        /// Reporte del trimestre a modificar o crear
        /// </summary>
        public Reporte? ReporteActual
        {
            get => reporteActual;
            private set => SetProperty(ref reporteActual, value, ReporteActual_PropertyChanged);
        }
        private Reporte? reporteActual;

        /// <summary>
        /// Reporte del trimestre anterior al <see cref="SelectedReporte"/>
        /// </summary>
        public Reporte? ReporteTA
        {
            get => reporteTA;
            private set => SetProperty(ref reporteTA, value);
        }
        private Reporte? reporteTA;

        /// <summary>
        /// Reporte del trimestre del año anterior al <see cref="SelectedReporte"/>
        /// </summary>
        public Reporte? ReporteAA
        {
            get => reporteAA;
            private set => SetProperty(ref reporteAA, value);
        }
        private Reporte? reporteAA;

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
                //if (selectedMultiplo != value)
                //{
                //    selectedMultiplo = value;
                //    Properties.Settings.Default.Multiplo = selectedMultiplo?.Valor ?? 1;
                //    bool tempFlag = isReporteActualChanged;
                //    ActualizarReportes(true);
                //    isReporteActualChanged = tempFlag;
                //}
                if (SetProperty(ref selectedMultiplo, value))
                {
                    Properties.Settings.Default.Multiplo = selectedMultiplo?.Valor ?? 1;
                    bool tempFlag = isReporteActualChanged;
                    ActualizarReportes(true);
                    isReporteActualChanged = tempFlag;
                }
            }
        }
        private Multiplo selectedMultiplo = null!;

        /// <summary>
        /// Indica si el reporte en edicion es Anual (True) o Trimestral (False)
        /// </summary>
        public bool IsAnual
        {
            get => isAnual;
            set => SetProperty(ref isAnual, value);
        }
        private bool isAnual;

        /// <summary>
        /// Costos de ventas del reporte actual
        /// </summary>
        public double ReporteActualCostoVentas
        {
            get => ReporteActual is not null ? reporteActualCostoVentas / ReporteActual.Factor : 0;
            set
            {
                if (ReporteActual is not null && SelectedReferencia is not null &&
                    SetProperty(ref reporteActualCostoVentas, value * ReporteActual.Factor))
                {
                    if (SelectedReferencia.CostoVentasAuto)
                    { ReporteActual.UBruta = ReporteActual.Ingresos - ReporteActualCostoVentas; }
                    else
                    { ActualizarSeccionUOperativa(); }
                }
            }
        }
        private double reporteActualCostoVentas;

        /// <summary>
        /// Gastos Operativos del reporte actual
        /// </summary>
        public double ReporteActualGastosOp
        {
            get => ReporteActual is not null ? reporteActualGastosOp / ReporteActual.Factor : 0;
            set
            {
                if (ReporteActual is not null && SelectedReferencia is not null &&
                    SetProperty(ref reporteActualGastosOp, value * ReporteActual.Factor))
                {
                    if (SelectedReferencia.GastosOpAuto)
                    { ReporteActual.UOperativa = ReporteActual.UBruta - ReporteActualGastosOp; }
                    else
                    { ActualizarSeccionUAImpuestos(); }
                }
            }
        }
        private double reporteActualGastosOp;

        /// <summary>
        /// Gastos financieros del reporte actual
        /// </summary>
        public double ReporteActualGastosFin
        {
            get => ReporteActual is not null ? reporteActualGastosFin / ReporteActual.Factor : 0;
            set
            {
                if (ReporteActual is not null && SelectedReferencia is not null &&
                    SetProperty(ref reporteActualGastosFin, value * ReporteActual.Factor))
                {
                    if (SelectedReferencia.GastosFinAuto)
                    { ReporteActual.UAImpuestos = ReporteActual.UOperativa + ReporteActualGastosFin; }
                    else
                    { /* ACTUALIZAR SECCION ninguna */ }
                }
            }
        }
        private double reporteActualGastosFin;
        #endregion

        #region CONSTRUCTOR
        public ReportesViewModel(IWindowManager windowManager, IRepository<Empresa, string> repository)
        {
            this.windowManager = windowManager;
            this.repository = repository;

            // Inicializar los comandos
            IsAnualCommand = new(IsAnual_Execute, IsAnual_CanExecute);
            AgregarCommand = new(Agregar_Execute, Agregar_CanExecute);
            ActualizarCommand = new(Actualizar_Execute, Actualizar_CanExecute);
            EliminarCommand = new(Eliminar_Execute, Eliminar_CanExecute);
            LimpiarCommand = new(Limpiar_Execute, Limpiar_CanExecute);
            MultiploCommand = new(Multiplo_Execute);
            FindRefCommand = new(FindRef_Execute);
            PasteAndNextCommand = new(PasteAndNext_Execute);
            PasteAndCloseCommand = new(PasteAndClose_Execute);
            FindStockCommand = new(FindStock_Execute);

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
        public RelayCommand IsAnualCommand { get; }
        private bool IsAnual_CanExecute()
        {
            return ReporteActual?.TrimestreReporte == 4
                && SelectedEmpresa!.Reportes.FirstOrDefault(r => r.ID == ReporteActual.TrimestreIDOffset(-1)) is not null
                && SelectedEmpresa.Reportes.FirstOrDefault(r => r.ID == ReporteActual.TrimestreIDOffset(-2)) is not null
                && SelectedEmpresa.Reportes.FirstOrDefault(r => r.ID == ReporteActual.TrimestreIDOffset(-3)) is not null;
        }
        /// <summary>
        /// Activa o desactiva la bandera para definir si el reporte actual es trimestral o anual
        /// </summary>
        private void IsAnual_Execute()
        {
            Reporte trimFiscal_1 = SelectedEmpresa!.Reportes.First(r => r.ID == ReporteActual!.TrimestreIDOffset(-3));
            Reporte trimFiscal_2 = SelectedEmpresa!.Reportes.First(r => r.ID == ReporteActual!.TrimestreIDOffset(-2));
            Reporte trimFiscal_3 = SelectedEmpresa!.Reportes.First(r => r.ID == ReporteActual!.TrimestreIDOffset(-1));

            trimFiscal_1.Factor = reporteActual!.Factor;
            trimFiscal_2.Factor = reporteActual.Factor;
            trimFiscal_3.Factor = reporteActual.Factor;

            disabledReporteActual_PropertyChanged = true;
            if (IsAnual)
            {
                ReporteActual!.Ingresos = ReporteActual.Ingresos + trimFiscal_3.Ingresos + trimFiscal_2.Ingresos + trimFiscal_1.Ingresos;
                ReporteActual.IngresosAA = ReporteActual.IngresosAA + trimFiscal_3.IngresosAA + trimFiscal_2.IngresosAA + trimFiscal_1.IngresosAA;
                reporteActualCostoVentas = (ReporteActualCostoVentas + trimFiscal_3.CostoVentas + trimFiscal_2.CostoVentas + trimFiscal_1.CostoVentas) * ReporteActual.Factor;
                ReporteActual.UBruta = ReporteActual.UBruta + trimFiscal_3.UBruta + trimFiscal_2.UBruta + trimFiscal_1.UBruta;
                reporteActualGastosOp = (ReporteActualGastosOp + trimFiscal_3.GastosOp + trimFiscal_2.GastosOp + trimFiscal_1.GastosOp) * ReporteActual.Factor;
                ReporteActual.UOperativa = ReporteActual.UOperativa + trimFiscal_3.UOperativa + trimFiscal_2.UOperativa + trimFiscal_1.UOperativa;
                reporteActualGastosFin = (ReporteActualGastosFin + trimFiscal_3.GastosFin + trimFiscal_2.GastosFin + trimFiscal_1.GastosFin) * ReporteActual.Factor;
                ReporteActual.UAImpuestos = ReporteActual.UAImpuestos + trimFiscal_3.UAImpuestos + trimFiscal_2.UAImpuestos + trimFiscal_1.UAImpuestos;
                ReporteActual.UNeta = ReporteActual.UNeta + trimFiscal_3.UNeta + trimFiscal_2.UNeta + trimFiscal_1.UNeta;
                ReporteActual.UAccionistas = ReporteActual.UAccionistas + trimFiscal_3.UAccionistas + trimFiscal_2.UAccionistas + trimFiscal_1.UAccionistas;
                ReporteActual.UAccionistasAA = ReporteActual.UAccionistasAA + trimFiscal_3.UAccionistasAA + trimFiscal_2.UAccionistasAA + trimFiscal_1.UAccionistasAA;

                ReporteActual.IngresosProp = ReporteActual.IngresosProp + trimFiscal_3.IngresosProp + trimFiscal_2.IngresosProp + trimFiscal_1.IngresosProp;
                ReporteActual.NOI = ReporteActual.NOI + trimFiscal_3.NOI + trimFiscal_2.NOI + trimFiscal_1.NOI;
                ReporteActual.FFO = ReporteActual.FFO + trimFiscal_3.FFO + trimFiscal_2.FFO + trimFiscal_1.FFO;
                ReporteActual.AFFO = ReporteActual.AFFO + trimFiscal_3.AFFO + trimFiscal_2.AFFO + trimFiscal_1.AFFO;
            }
            else
            {
                ReporteActual!.Ingresos = ReporteActual.Ingresos - trimFiscal_3.Ingresos - trimFiscal_2.Ingresos - trimFiscal_1.Ingresos;
                ReporteActual.IngresosAA = ReporteActual.IngresosAA - trimFiscal_3.IngresosAA - trimFiscal_2.IngresosAA - trimFiscal_1.IngresosAA;
                reporteActualCostoVentas = (ReporteActualCostoVentas - trimFiscal_3.CostoVentas - trimFiscal_2.CostoVentas - trimFiscal_1.CostoVentas) * ReporteActual.Factor;
                ReporteActual.UBruta = ReporteActual.UBruta - trimFiscal_3.UBruta - trimFiscal_2.UBruta - trimFiscal_1.UBruta;
                reporteActualGastosOp = (ReporteActualGastosOp - trimFiscal_3.GastosOp - trimFiscal_2.GastosOp - trimFiscal_1.GastosOp) * ReporteActual.Factor;
                ReporteActual.UOperativa = ReporteActual.UOperativa - trimFiscal_3.UOperativa - trimFiscal_2.UOperativa - trimFiscal_1.UOperativa;
                reporteActualGastosFin = (ReporteActualGastosFin - trimFiscal_3.GastosFin - trimFiscal_2.GastosFin - trimFiscal_1.GastosFin) * ReporteActual.Factor;
                ReporteActual.UAImpuestos = ReporteActual.UAImpuestos - trimFiscal_3.UAImpuestos - trimFiscal_2.UAImpuestos - trimFiscal_1.UAImpuestos;
                ReporteActual.UNeta = ReporteActual.UNeta - trimFiscal_3.UNeta - trimFiscal_2.UNeta - trimFiscal_1.UNeta;
                ReporteActual.UAccionistas = ReporteActual.UAccionistas - trimFiscal_3.UAccionistas - trimFiscal_2.UAccionistas - trimFiscal_1.UAccionistas;
                ReporteActual.UAccionistasAA = ReporteActual.UAccionistasAA - trimFiscal_3.UAccionistasAA - trimFiscal_2.UAccionistasAA - trimFiscal_1.UAccionistasAA;

                ReporteActual.IngresosProp = ReporteActual.IngresosProp - trimFiscal_3.IngresosProp - trimFiscal_2.IngresosProp - trimFiscal_1.IngresosProp;
                ReporteActual.NOI = ReporteActual.NOI - trimFiscal_3.NOI - trimFiscal_2.NOI - trimFiscal_1.NOI;
                ReporteActual.FFO = ReporteActual.FFO - trimFiscal_3.FFO - trimFiscal_2.FFO - trimFiscal_1.FFO;
                ReporteActual.AFFO = ReporteActual.AFFO - trimFiscal_3.AFFO - trimFiscal_2.AFFO - trimFiscal_1.AFFO;
            }
            OnPropertyChange(nameof(ReporteActualCostoVentas));
            OnPropertyChange(nameof(ReporteActualGastosOp));
            OnPropertyChange(nameof(ReporteActualGastosFin));
            disabledReporteActual_PropertyChanged = false;
        }

        public RelayCommand AgregarCommand { get; }
        private bool Agregar_CanExecute()
        {
            return SelectedEmpresa is not null
                && ReporteActual is not null
                && isReporteActualChanged
                && !ReporteActual.HasErrors
                && repository.GetReport(SelectedEmpresa.ID, ReporteActual.ID) is null;
        }
        /// <summary>
        /// Agrega un nuevo reporte a la empresa seleccionada
        /// </summary>
        private void Agregar_Execute()
        {
            // Se actualizan los comentarios y las referencias
            repository.UpdateCompany(SelectedEmpresa!);
            if (SelectedReferencia is not null)
            { repository.UpdateReferencia(SelectedEmpresa!.ID, SelectedReferencia); }

            // Se agrega el reporte
            //TODO: Tal vez no es necesaria esta validacion, xq no permite activar ANUAL cuando no hay trimestres suficientes
            if (!ReporteActualATrimestral()) { throw new NotImplementedException(); }
            repository.AddReport(SelectedEmpresa!.ID, ReporteActual!);
            repository.SubmitChanges();

            // Se actualiza la lista de reportes
            RecargarReportes();

            // Se limpia el reporte Actual
            Limpiar_Execute();
        }

        public RelayCommand ActualizarCommand { get; }
        private bool Actualizar_CanExecute()
        {
            return SelectedEmpresa is not null
                && ReporteActual is not null
                && isReporteActualChanged
                && !ReporteActual.HasErrors
                && repository.GetReport(SelectedEmpresa.ID, ReporteActual.ID) is not null;
        }
        /// <summary>
        /// Actualiza la información del Reporte
        /// </summary>
        private void Actualizar_Execute()
        {
            // Se actualizan los comentarios y las referencias
            repository.UpdateCompany(SelectedEmpresa!);
            if (SelectedReferencia is not null)
            { repository.UpdateReferencia(SelectedEmpresa!.ID, SelectedReferencia); }

            // Se actualiza el reporte
            //TODO: Tal vez no es necesaria esta validacion, xq no permite activar ANUAL cuando no hay trimestres suficientes
            if (!ReporteActualATrimestral()) { throw new NotImplementedException(); }
            repository.UpdateReport(SelectedEmpresa!.ID, ReporteActual!);
            repository.SubmitChanges();

            // Se resetea la bandera
            isReporteActualChanged = false;

            // Se actualiza la lista de reportes
            string id = ReporteActual!.ID;
            RecargarReportes();
            SelectedReporte = SelectedEmpresa.Reportes.FirstOrDefault(r => r.ID == id);
        }

        public RelayCommand EliminarCommand { get; }
        private bool Eliminar_CanExecute()
        {
            return SelectedEmpresa is not null
                && ReporteActual is not null
                && repository.GetReport(SelectedEmpresa.ID, ReporteActual.ID) is not null;
        }
        /// <summary>
        /// Elimina el reporte
        /// </summary>
        private void Eliminar_Execute()
        {
            ConfigurationManager.AppSettings.Set("elemento", $"{SelectedEmpresa!.ID}: {ReporteActual!.ID}");
            if (windowManager.ShowDialog(IoC.Get<PopEliminarViewModel>()).GetValueOrDefault())
            {
                // Se elmina el reporte
                repository.DeleteReport(SelectedEmpresa!.ID, ReporteActual!.ID);
                repository.SubmitChanges();
                // Se actualiza la lista de reportes
                RecargarReportes();
                ReporteActual = null;
                // Se limpia el reporte Actual
                Limpiar_Execute();
            }
        }

        public RelayCommand LimpiarCommand { get; }
        private bool Limpiar_CanExecute()
        {
            return SelectedEmpresa is not null
                && (SelectedReporte is not null || isReporteActualChanged);
        }
        /// <summary>
        /// Limpia los campos del formulario
        /// </summary>
        private void Limpiar_Execute()
        {
            // ActualizarComentarios
            SelectedEmpresa!.Comentarios = repository.GetCompany(SelectedEmpresa.ID)!.Comentarios;

            // ActualizarReferencias
            //if (SelectedReferencia is not null){ SelectedReferencia = repository.GetReferencia(SelectedEmpresa!.ID, SelectedReferencia.ID); }

            // ActualizarReporte
            //TODO: Tal vez no es necesaria esta validacion, xq no permite activar ANUAL cuando no hay trimestres suficientes
            if (!ReporteActualATrimestral()) { throw new NotImplementedException(); }

            SelectedFecha = EncontrarFecha();
            SelectedReporte = null;
            ActualizarReportes();
        }

        public RelayCommand<int> MultiploCommand { get; }
        /// <summary>
        /// Cambia al valor del multiplo
        /// </summary>
        /// <param name="obj">El valor del multiplo que se desea usar</param>
        private void Multiplo_Execute(int valor)
        {
            SelectedMultiplo = Multiplos.FirstOrDefault(e => e.Valor == valor) ?? Multiplos[0];
        }

        public RelayCommand<string> FindRefCommand { get; }
        /// <summary>
        /// Cambia de ventana y busca el valor de referencia en esta
        /// </summary>
        /// <param name="obj">Texto completo de referencia para buscar</param>
        private void FindRef_Execute(string obj)
        {
            int retardo = 100;

            obj ??= "";

            int pStart = obj.IndexOf('[') + 1;
            //if (pStart == -1) { pStart = 0; }
            int pEnd = obj.IndexOf(']', pStart + 1);
            //if (pEnd == -1) { pEnd = obj.Length; }


            // Logica para Ignorar delimitadores anidados
            // Ej: Esto es un [ejemplo de [delimitadores] anidados], saludos
            int tempStart = obj.IndexOf('[', pStart);
            while (tempStart > -1 && tempStart < pEnd)
            {
                tempStart = obj.IndexOf('[', tempStart + 1);
                pEnd = obj.IndexOf(']', pEnd + 1);
            }

            if (pEnd == -1) { pEnd = obj.Length; }
            string textoBuscar = obj[pStart..pEnd];

            // Resetea el teclado por si el usuario sigue presionando una tecla
            SimKeyboard.Reset();

            // Alt + TAB
            SimKeyboard.Press(Key.LeftAlt);
            SimKeyboard.Press(Key.Tab);
            SimKeyboard.Release(Key.Tab);
            SimKeyboard.Release(Key.LeftAlt);
            Thread.Sleep(retardo);

            // ESC
            SimKeyboard.Press(Key.Escape);
            SimKeyboard.Release(Key.Escape);
            Thread.Sleep(retardo);

            // Ctrl + F
            SimKeyboard.Press(Key.LeftCtrl);
            SimKeyboard.Press(Key.F);
            SimKeyboard.Release(Key.F);
            SimKeyboard.Release(Key.LeftCtrl);
            Thread.Sleep(retardo);

            // Escribir el texto a buscar
            //SimKeyboard.Type(textoBuscar);    // Los caracteres con acento no los escribe
            //string tempClipboard = Clipboard.GetText();
            Clipboard.SetText(textoBuscar);
            SimKeyboard.Press(Key.LeftCtrl);
            SimKeyboard.Press(Key.V);
            SimKeyboard.Release(Key.V);
            SimKeyboard.Release(Key.LeftCtrl);
            //Thread.Sleep(retardo);
            //Clipboard.SetText(tempClipboard);

            // Enter -> Buscar
            SimKeyboard.Press(Key.Enter);
            SimKeyboard.Release(Key.Enter);
        }

        public RelayCommand PasteAndNextCommand { get; }
        /// <summary>
        /// Pega lo del portapapeles, pasa al siguiente campo y simula Ctrl+F
        /// </summary>
        private void PasteAndNext_Execute()
        {
            int retardo = 100;

            // Resetea el teclado por si el usuario sigue presionando una tecla
            SimKeyboard.Reset();

            // Ctrl + V
            SimKeyboard.Press(Key.LeftCtrl);
            SimKeyboard.Press(Key.V);
            SimKeyboard.Release(Key.V);
            SimKeyboard.Release(Key.LeftCtrl);
            Thread.Sleep(retardo);

            // TAB
            SimKeyboard.Press(Key.Tab);
            SimKeyboard.Release(Key.Tab);
            Thread.Sleep(retardo);

            // Ctrl + F
            SimKeyboard.Press(Key.LeftCtrl);
            SimKeyboard.Press(Key.F);
            SimKeyboard.Release(Key.F);
            SimKeyboard.Release(Key.LeftCtrl);
        }

        public RelayCommand PasteAndCloseCommand { get; }
        /// <summary>
        /// Pega lo del portapapeles, y cierra la venta web de yahoo que se abrio
        /// </summary>
        private void PasteAndClose_Execute()
        {
            int retardo = 100;

            // Resetea el teclado por si el usuario sigue presionando una tecla
            SimKeyboard.Reset();

            // Ctrl + V
            SimKeyboard.Press(Key.LeftCtrl);
            SimKeyboard.Press(Key.V);
            SimKeyboard.Release(Key.V);
            SimKeyboard.Release(Key.LeftCtrl);
            Thread.Sleep(retardo);

            // Alt + TAB
            SimKeyboard.Press(Key.LeftAlt);
            SimKeyboard.Press(Key.Tab);
            SimKeyboard.Release(Key.Tab);
            SimKeyboard.Release(Key.LeftAlt);
            Thread.Sleep(retardo);

            // Ctrl + F4
            SimKeyboard.Press(Key.LeftCtrl);
            SimKeyboard.Press(Key.F4);
            SimKeyboard.Release(Key.F4);
            SimKeyboard.Release(Key.LeftCtrl);
        }

        public RelayCommand<string> FindStockCommand { get; }
        /// <summary>
        /// Busca el valor de la accion al final del trimestre del reporte
        /// </summary>
        /// <param name="obj">Simbolo en Yahoo finance y fecha del reporte</param>
        private void FindStock_Execute(string obj)
        {
            #region Abre la pagina de yahoo con el historial deseado
            long difNetYahooSeconds = 62135510400;
            long oneDayYahoo = 86400;

            string id = SelectedEmpresa!.YahooID;
            DateTime dateTime = ReporteActual!.Fecha;

            long yahooDate = (dateTime.Ticks / 10000000) - difNetYahooSeconds;
            long yahooDatePr1 = yahooDate - (oneDayYahoo * 7);
            string uriPrecio = $"https://finance.yahoo.com/quote/{id}/history?period1={yahooDatePr1}&period2={yahooDate}&interval=1d&filter=history&frequency=1d&includeAdjustedClose=true";

            // for .NET Core you need to add UseShellExecute = true see https://docs.microsoft.com/dotnet/api/system.diagnostics.processstartinfo.useshellexecute#property-value
            _ = Process.Start(new ProcessStartInfo(uriPrecio) { UseShellExecute = true });          //ABRE LA PAGINA DEL HISTORIAL DE PRECIOS
            #endregion


            #region Obtener el precio de la API de yahoo
            ReporteActual.Precio = StockData.GetStockCloseAt(SelectedEmpresa!.YahooID, ReporteActual!.Fecha).Result;
            #endregion
        }
        #endregion

        #region EVENTOS DE WPF
        public void OnViewSelectedDateChanged() { ActualizarReportes(); }
        #endregion

        #region METODOS privados
        /// <summary>
        /// Se ejecuta cada vez que una propiedad de <see cref="ReporteActual"/> es modificada
        /// </summary>
        /// <param name="sender">El componente <see cref="ReporteActual"/></param>
        /// <param name="e">Propiedad que se modificó del componente <see cref="ReporteActual"/></param>
        private void ReporteActual_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (disabledReporteActual_PropertyChanged) { return; }

            isReporteActualChanged = true;
            switch (e.PropertyName)
            {
                case nameof(Reporte.PasivosT):
                    if (ReporteActual!.Capital == 0)
                    {
                        disabledReporteActual_PropertyChanged = true;
                        ReporteActual.Capital = ReporteActual.ActivosT - ReporteActual.PasivosT;
                        disabledReporteActual_PropertyChanged = false;
                    }
                    break;
                case nameof(Reporte.Capital):
                    if (ReporteActual!.PasivosT == 0)
                    {
                        disabledReporteActual_PropertyChanged = true;
                        ReporteActual.PasivosT = ReporteActual.ActivosT - ReporteActual.Capital;
                        disabledReporteActual_PropertyChanged = false;
                    }
                    break;
                case nameof(Reporte.FlujoOperaciones) or
                     nameof(Reporte.FlujoInversiones) or
                     nameof(Reporte.FlujoFinanciamiento) or
                     nameof(Reporte.EfectivoAlInicio):
                    if (ReporteActual!.EfectivoAlFinal == 0)
                    {
                        if (ReporteActual.FlujoOperaciones == 0 ||
                            ReporteActual.FlujoInversiones == 0 ||
                            ReporteActual.FlujoFinanciamiento == 0 ||
                            ReporteActual.EfectivoAlInicio == 0 )
                        { break; }
                        disabledReporteActual_PropertyChanged = true;
                        ReporteActual.EfectivoAlFinal = ReporteActual.EfectivoAlInicio + ReporteActual.FlujoOperaciones + ReporteActual.FlujoInversiones + ReporteActual.FlujoFinanciamiento;
                        disabledReporteActual_PropertyChanged = false;
                    }
                    break;
                case nameof(Reporte.Ingresos):
                    if (ReporteActual!.IngresosProp is 0 or null)
                    {
                        disabledReporteActual_PropertyChanged = true;
                        ReporteActual.IngresosProp = ReporteActual.Ingresos;
                        disabledReporteActual_PropertyChanged = false;
                    }
                    ActualizarSeccionUBruta();
                    break;
                case nameof(Reporte.UBruta):
                    if (SelectedReferencia!.CostoVentasAuto)
                    { ActualizarSeccionUOperativa(); }
                    else
                    { ReporteActualCostoVentas = ReporteActual!.CostoVentas; }
                    break;
                case nameof(Reporte.UOperativa):
                    if (SelectedReferencia!.GastosOpAuto)
                    { ActualizarSeccionUAImpuestos(); }
                    else
                    { ReporteActualGastosOp = ReporteActual!.GastosOp; }
                    break;
                case nameof(Reporte.UAImpuestos):
                    if (SelectedReferencia!.GastosFinAuto)
                    { /* ACTUALIZAR SECCION ninguna */ }
                    else
                    { ReporteActualGastosFin = ReporteActual!.GastosFin; }
                    break;
                default:
                    break;
            }
    }

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
        /// Elimina la lista actual de <see cref="Referencia"/> y carga una nueva y ordenada
        /// </summary>
        private void RecargarReferencias()
        {
            List<Referencia>? sortedRefList = SelectedEmpresa!.Referencias.OrderBy(r => r.ID).ToList();
            SelectedEmpresa.Referencias.Clear();
            foreach (Referencia referencia in sortedRefList)
            { SelectedEmpresa.Referencias.Add(referencia); }
        }

        /// <summary>
        /// Actualiza los 3 reportes trimestrales (Actual, Trimestre anterior y Año anterior)
        /// </summary>
        /// <param name="keepReporteActual">Indica si se desea consevar la información del Reporte Actual
        /// o substituir por uno NUEVO o por el SelectedReporte</param>
        private void ActualizarReportes(bool keepReporteActual = false)
        {
            Reporte? reporteTemp;
            bool isNewReport = false;
            if (SelectedEmpresa is null) { return; }
            if (keepReporteActual == false || ReporteActual is null)
            {
                if (SelectedReporte is null)
                {
                    int trimestre = (SelectedFecha.Month - 1) / 3 + 1;
                    reporteTemp = repository.GetReport(SelectedEmpresa.ID, $"{SelectedFecha.Year}_{trimestre}");

                    if (reporteTemp is null)
                    {
                        reporteTemp = new Reporte() { Fecha = SelectedFecha };
                        isNewReport = true;
                    }
                    else
                    {
                        SelectedReporte = SelectedEmpresa.Reportes.FirstOrDefault(r => r.ID == reporteTemp.ID);
                        return;
                    }
                }
                else
                { reporteTemp = SelectedReporte; }

                ReporteActual = reporteTemp;
            }
            // Cargar la informacion del reporte temporal al Reporte Actual
            //ReporteActual = reporteTemp;
            disabledReporteActual_PropertyChanged = true;
            ReporteActual.Factor = SelectedMultiplo.Valor;

            // Cargar la informacion del reporte del trimestre anterior
            Reporte? reporteAnterior = SelectedEmpresa?.Reportes.FirstOrDefault(r => r.ID == ReporteActual.TrimestreIDOffset(-1));
            ReporteTA = reporteAnterior is null ? new Reporte() : reporteAnterior;
            ReporteTA.Factor = SelectedMultiplo.Valor;

            // Cargar la informacion del reporte del año anterior
            reporteAnterior = SelectedEmpresa?.Reportes.FirstOrDefault(r => r.ID == ReporteActual.TrimestreIDOffset(-4));
            ReporteAA = reporteAnterior is null ? new Reporte() : reporteAnterior;
            ReporteAA.Factor = SelectedMultiplo.Valor;

            if (isNewReport)
            {
                ReporteActual.TrimestreReporte = ReporteTA.TrimestreReporte <= 0 ? 0 : ReporteTA.TrimestreReporte >= 4 ? 1 : ReporteTA.TrimestreReporte + 1;
                ReporteActual.NumUnidadesAA = ReporteAA.NumUnidades;
                ReporteActual.IngresosAA = ReporteAA.Ingresos;
                ReporteActual.UAccionistasAA = ReporteAA.UAccionistas;
                ReporteActual.CapitalAA = ReporteAA.Capital;

                ReporteActual.EfectivoAlInicio = ReporteTA.TrimestreReporte == 4
                                                 ? ReporteTA.EfectivoAlFinal
                                                 : ReporteTA.EfectivoAlInicio;

                reporteActualCostoVentas = 0;
                reporteActualGastosOp = 0;
                reporteActualGastosFin = 0;
            }
            else
            {
                reporteActualCostoVentas = ReporteActual.CostoVentas * ReporteActual.Factor;
                reporteActualGastosOp = ReporteActual.GastosOp * ReporteActual.Factor;
                reporteActualGastosFin = ReporteActual.GastosFin * ReporteActual.Factor;
            }
            OnPropertyChange(nameof(ReporteActualCostoVentas));
            OnPropertyChange(nameof(ReporteActualGastosOp));
            OnPropertyChange(nameof(ReporteActualGastosFin));

            isReporteActualChanged = false;
            disabledReporteActual_PropertyChanged = false;
        }

        /// <summary>
        /// Encuentra una fecha donde aun no existe un reporte trimestral
        /// </summary>
        /// <returns>Fecha donde no existe un reporte trimestral</returns>
        private DateTime EncontrarFecha()
        {
            IEnumerable<Reporte>? ids = selectedEmpresa?.Reportes.OrderBy(r => r.Fecha);
            //if (ids is null || ids.Any() == false) { return DateTime.Today; }
            if (ids is null || ids.Any() == false) { return new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.DaysInMonth(DateTime.Today.Year, DateTime.Today.Month)); }

            DateTime fechaAnterior = ids.First().Fecha;
            foreach (Reporte? item in ids)
            {
                int difTrim = (item.Ano * 4) - (fechaAnterior.Year * 4) +
                               item.TrimestreNatural - (((fechaAnterior.Month - 1) / 3) + 1);

                //if (difTrim > 1) { return fechaAnterior.AddMonths(3); }
                if (difTrim > 1) { break; }
                fechaAnterior = item.Fecha;
            }

            //return fechaAnterior.AddMonths(3);
            fechaAnterior = fechaAnterior.AddMonths(3);
            return new DateTime(fechaAnterior.Year, fechaAnterior.Month, DateTime.DaysInMonth(fechaAnterior.Year, fechaAnterior.Month));
        }

        /// <summary>
        /// Convierte los datos del <see cref="ReporteActual"/> de datos Anuales a trimestrales si la bandera <see cref="IsAnual"/> es TRUE
        /// </summary>
        /// <returns>TRUE si se pudo hacer la conversion o no hubo necesidad, FALSE si no hay reportes sufucientes para convertir de Anual a Trimestral</returns>
        private bool ReporteActualATrimestral()
        {
            if (!IsAnual || ReporteActual is null) { return true; }

            Reporte? trimFiscal_1 = SelectedEmpresa!.Reportes.FirstOrDefault(r => r.ID == ReporteActual.TrimestreIDOffset(-3));
            Reporte? trimFiscal_2 = SelectedEmpresa.Reportes.FirstOrDefault(r => r.ID == ReporteActual.TrimestreIDOffset(-2));
            Reporte? trimFiscal_3 = SelectedEmpresa.Reportes.FirstOrDefault(r => r.ID == ReporteActual.TrimestreIDOffset(-1));

            if (trimFiscal_1 is null || trimFiscal_2 is null || trimFiscal_3 is null) { return false; }

            ReporteActual!.Ingresos = ReporteActual.Ingresos - trimFiscal_3.Ingresos - trimFiscal_2.Ingresos - trimFiscal_1.Ingresos;
            ReporteActual.IngresosAA = ReporteActual.IngresosAA - trimFiscal_3.IngresosAA - trimFiscal_2.IngresosAA - trimFiscal_1.IngresosAA;
            ReporteActual.UBruta = ReporteActual.UBruta - trimFiscal_3.UBruta - trimFiscal_2.UBruta - trimFiscal_1.UBruta;
            ReporteActual.UOperativa = ReporteActual.UOperativa - trimFiscal_3.UOperativa - trimFiscal_2.UOperativa - trimFiscal_1.UOperativa;
            ReporteActual.UAImpuestos = ReporteActual.UAImpuestos - trimFiscal_3.UAImpuestos - trimFiscal_2.UAImpuestos - trimFiscal_1.UAImpuestos;
            ReporteActual.UNeta = ReporteActual.UNeta - trimFiscal_3.UNeta - trimFiscal_2.UNeta - trimFiscal_1.UNeta;
            ReporteActual.UAccionistas = ReporteActual.UAccionistas - trimFiscal_3.UAccionistas - trimFiscal_2.UAccionistas - trimFiscal_1.UAccionistas;
            ReporteActual.UAccionistasAA = ReporteActual.UAccionistasAA - trimFiscal_3.UAccionistasAA - trimFiscal_2.UAccionistasAA - trimFiscal_1.UAccionistasAA;

            ReporteActual.IngresosProp = ReporteActual.IngresosProp - trimFiscal_3.IngresosProp - trimFiscal_2.IngresosProp - trimFiscal_1.IngresosProp;
            ReporteActual.NOI = ReporteActual.NOI - trimFiscal_3.NOI - trimFiscal_2.NOI - trimFiscal_1.NOI;
            ReporteActual.FFO = ReporteActual.FFO - trimFiscal_3.FFO - trimFiscal_2.FFO - trimFiscal_1.FFO;
            ReporteActual.AFFO = ReporteActual.AFFO - trimFiscal_3.AFFO - trimFiscal_2.AFFO - trimFiscal_1.AFFO;

            IsAnual = false;
            return true;
        }

        /// <summary>
        /// Actualiza el campo de CostoVentas o UBruta 
        /// dependiendo del valor de la bandera CostoVentasAuto
        /// </summary>
        private void ActualizarSeccionUBruta()
        {
            if (SelectedReferencia!.CostoVentasAuto)
            { ReporteActual!.UBruta = ReporteActual.Ingresos - ReporteActualCostoVentas; }
            else
            { ReporteActualCostoVentas = ReporteActual!.CostoVentas; }
        }

        /// <summary>
        /// Actualiza el campo de GastosOp o UOperativa 
        /// dependiendo del valor de la bandera CostoVentasAuto
        /// </summary>
        private void ActualizarSeccionUOperativa()
        {
            if (SelectedReferencia!.GastosOpAuto)
            { ReporteActual!.UOperativa = ReporteActual.UBruta - ReporteActualGastosOp; }
            else
            { ReporteActualGastosOp = ReporteActual!.GastosOp; }
        }

        /// <summary>
        /// Actualiza el campo de GastosFin o UAImpuestos
        /// dependiendo del valor de la bandera CostoVentasAuto
        /// </summary>
        private void ActualizarSeccionUAImpuestos()
        {
            if (SelectedReferencia!.GastosFinAuto)
            { ReporteActual!.UAImpuestos = ReporteActual.UOperativa + ReporteActualGastosFin; }
            else
            { ReporteActualGastosFin = ReporteActual!.GastosFin; }
        }
        #endregion
    }
}