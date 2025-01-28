using DataAccess;
using Models;
using MvvmFramework;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;

namespace WPF
{
    public class EmpresasViewModel : Validatable
    {
        #region FIELDS
        private readonly IWindowManager windowManager;
        private readonly IRepository<Empresa, string> repository;

        private bool isEmpresaChanged;
        private readonly Referencia yahooRef = new()
        {
            ID = "Yahoo",
            Ingresos = "[Total Revenue]",
            CostoVentasAuto = true,
            UBruta = "[Cost of Revenue]",
            GastosOpAuto = true,
            UOperativa = "[Operating Expense]",
            GastosFinAuto = false,
            UAImpuestos = "[Pretax Income]",
            UNeta = "[Net Income Including Non-Controlling Interests]",
            UAccionistas = "[Net Income Common Stockholders]",
            IngresosProp = "",
            NOI = "",
            FFO = "",
            AFFO = "",
            Efectivo = "Total Assets => Current Assets => [Cash, Cash Equivalents & Short Term Investments]",
            ActivosC = "Total Assets => [Current Assets]",
            ActivosT = "[Total Assets]",
            Deuda = "[Total Debt]",
            PasivosC = "Total Liabilities Net Minority Interest => [Current Liabilities]",
            PasivosT = "",
            Capital = "[Stockholders' Equity]",
            FlujoOperaciones = "[Cash Flow from Continuing Operating Activities] {Sumar todos los trimestres fiscales del presente año fiscal}",
            FlujoInversiones = "[Cash Flow from Continuing Investing Activities] {Sumar todos los trimestres fiscales del presente año fiscal}",
            FlujoFinanciamiento = "[Cash Flow from Continuing Financing Activities] {Sumar todos los trimestres fiscales del presente año fiscal}",
            EfectivoAlInicio = "[End Cash Position] {Poner el del Cuarto trimestre fiscal del año anterior}",
            EfectivoAlFinal = "[End Cash Position]",
            AccionesCir = "[Shares Outstanding] {Yahoo solo da el dato del ultimo reporte disponible}"
        };
        //private readonly Referencia reutersRef = new()
        //{
        //    ID = "Reuters",
        //    Ingresos = "Total Revenue",
        //    CostoVentasAuto = true,
        //    UBruta = "Cost of Revenue, Total",
        //    GastosOpAuto = true,
        //    UOperativa = "Operating Income + Unusual Expense (Income)",
        //    GastosFinAuto = false,
        //    UAImpuestos = "Net Income Before Taxes",
        //    UNeta = "Net Income After Taxes",
        //    UAccionistas = "Net Income",
        //    IngresosProp = "",
        //    NOI = "",
        //    FFO = "",
        //    AFFO = "",
        //    Efectivo = "Total Assets => Current Assets => Cash, Cash Equivalents & Short Term Investments",
        //    ActivosC = "Total Assets => Current Assets",
        //    ActivosT = "Total Assets",
        //    Deuda = "Total Debt",
        //    PasivosC = "Total Liabilities Net Minority Interest => Current Liabilities",
        //    PasivosT = "",
        //    Capital = "Stockholders' Equity",
        //    FlujoOperaciones = "Cash Flow from Continuing Operating Activities {Sumar todos los trimestres fiscales del presente año fiscal}",
        //    FlujoInversiones = "Cash Flow from Continuing Investing Activities {Sumar todos los trimestres fiscales del presente año fiscal}",
        //    FlujoFinanciamiento = "Cash Flow from Continuing Financing Activities {Sumar todos los trimestres fiscales del presente año fiscal}",
        //    EfectivoAlInicio = "End Cash Position {Poner el del Cuarto trimestre fiscal del año anterior}",
        //    EfectivoAlFinal = "End Cash Position",
        //    AccionesCir = "Shares Outstanding {Yahoo solo da el dato del ultimo reporte disponible}"
        //};
        private readonly Referencia ayudaRef = new()
        {
            ID = "Ω Ayuda",
            Ingresos = "",
            CostoVentasAuto = true,
            UBruta = "Costo de venta: Son todos los gastos asociados a la producción, distribución y difusión de un producto o servicio. " +
            "En otras palabras, todo el dinero gastado en las operaciones de una empresa.",
            GastosOpAuto = true,
            UOperativa = "Gastos Operativos: Son los gastos en el que incurre una empresa para el desarrollo normal de sus actividades diarias, " +
            "por ejemplo: arriendo, salarios, marketing, costos de inventario, compra de suministros, investigación y desarrollo, etc.",
            GastosFinAuto = true,
            UAImpuestos = "Gastos Financieros: Son intereses de obligaciones y bonos, intereses de deudas, intereses por descuento de efectos, " +
            "diferencias de cambio de moneda, y TAMBIEN los gastos generados por las pérdidas de valor de activos financieros.",
            UNeta = "",
            UAccionistas = "No siempre es igual a la UTILIDAD NETA porque a veces existen OPERACIONES DISCONTINUAS, " +
            "estas existen cuando una empresa vende divisiones o parte de ella y se reportan utilidades, por lo que se toman en cuenta en la utilidad neta.",
            IngresosProp = "Son las rentas que se cobraron de las propiedades.",
            NOI = "Ingreso Operativo Neto: Son las rentas cobradas menos los gastos de mantenimiento, seguros y predial.",
            FFO = "Fondos de la Operación: Es el NOI - (Ingresos Financieros, Impuestos y Depresiación y Amortización) - (Gastos o Ingresos por el servicion de Deuda o Intereses)",
            AFFO = "Fondos Ajustados de la Operación: Es el FFO - Las inversiones de capital (CAPEX)",
            Efectivo = "Es el efectivo del que se puede disponer de forma inmediata",
            ActivosC = "Es el dinero líquido o dinero que se puede obtener de forma fácil (en menos de 1 año)",
            ActivosT = "Indica lo que la empresa Tiene",
            Deuda = "Es la suma de todos los prestamos por instituciones financieras de Pasivos Circulantes y Pasivos No Circulantes.",
            PasivosC = "Indica la deuda a Corto Plazo que tiene la empresa (Deudas menores a un año)",
            PasivosT = "Indica lo que la empresa Debe",
            Capital = "Indica el valor de la empresa (Lo que tienes menos lo que debes, Activos - Pasivos). " +
            "Aveces la formula no aplica porque la empresa tiene acciones 'privadas' que NO cotizan en la bolsa y forman parte del capital privado" +
            " (Algunas empresas no están 100 % bursatilizadas).",
            FlujoOperaciones = "",
            FlujoInversiones = "",
            FlujoFinanciamiento = "",
            EfectivoAlInicio = "",
            EfectivoAlFinal = "",
            AccionesCir = "-Se puede encontrar en la página de la empresa o en www.bmv.com.mx [Empresas Listadas->Buscar->Seleccionas la Empresa->Estadísticas de Operación->Acciones en Circulación]. " +
            "- Para empresas de Estados Unidos se puede encontrar en ThinkOrSwim [Pestaña de Trade->All Productos->Simbolo de la Accion->Underlying(Expandir esa Area)->Shares]. " +
            "- Tambien se puede encontrar en Yahoo Finance [Buscas la Empresa->Pestaña Statistics->Share Statistics->Shares Outstanding]."
        };
        #endregion

        #region PROPIEDADES
        /// <summary>
        /// Lista de todas las empresas filtradas por el ID y el Nombre de <see cref="Empresa"/>
        /// </summary>
        public ObservableCollection<Empresa> Empresas { get; } = new ObservableCollection<Empresa>();

        /// <summary>
        /// La empresa seleccionada de la lista
        /// </summary>
        public Empresa? SelectedEmpresa
        {
            get => selectedEmpresa;
            set
            {
                if (SetProperty(ref selectedEmpresa, value))
                {
                    if (value is not null)
                    {
                        Empresa.PropertyChanged -= Empresa_PropertyChange;
                        Empresa.ID = value.ID;
                        Empresa.Nombre = value.Nombre;
                        Empresa.Moneda = value.Moneda;
                        Empresa.YahooID = value.YahooID;
                        Empresa.Comentarios = value.Comentarios;
                        Empresa.PropertyChanged += Empresa_PropertyChange;
                    }
                }
            }
        }
        private Empresa? selectedEmpresa;

        ///// <summary>
        ///// La referencia seleccionada de la lista
        ///// </summary>
        public Referencia? SelectedReferencia
        {
            private get => selectedReferencia;
            set => SetProperty(ref selectedReferencia, value, SelectedReferencia_PropertyChanged);
        }
        private Referencia? selectedReferencia;

        /// <summary>
        /// Empresa a crear, editar o eliminar
        /// </summary>
        public Empresa Empresa { get; } = new();
        #endregion

        #region CONSTRUCTOR
        public EmpresasViewModel(IWindowManager windowManager, IRepository<Empresa, string> repository)
        {
            this.windowManager = windowManager;
            this.repository = repository;

            // Inicializar los comandos
            AgregarCommand = new(Agregar_Execute, Agregar_CanExecute);
            ActualizarCommand = new(Actualizar_Execute, Actualizar_CanExecute);
            EliminarCommand = new(Eliminar_Execute, Eliminar_CanExecute);
            LimpiarCommand = new(Limpiar_Execute, Limpiar_CanExecute);

            // Inicializar Propiedades o Fields
            Empresa.PropertyChanged += Empresa_PropertyChange;
            RecargarEmpresas();
        }
        #endregion

        #region COMMANDS
        public RelayCommand AgregarCommand { get; }
        [DebuggerStepThrough]
        private bool Agregar_CanExecute()
        {
            return !Empresas.Any(e => e.ID == Empresa.ID) && !Empresa.HasErrors &&
                   (Empresa.ID != "") && (Empresa.Nombre != "") && (Empresa.Moneda != "");
        }
        /// <summary>
        /// Agrega una nueva empresa
        /// </summary>
        private void Agregar_Execute()
        {
            GuardarReferencias();
            repository.AddCompany(Empresa);
            repository.SubmitChanges();
            RecargarEmpresas();
            SelectedEmpresa = Empresas.FirstOrDefault(e => e.ID == Empresa.ID);
        }

        public RelayCommand ActualizarCommand { get; }
        [DebuggerStepThrough]
        private bool Actualizar_CanExecute()
        {
            return Empresas.Any(e => e.ID == Empresa.ID) 
                && !Empresa.HasErrors
                && isEmpresaChanged;
        }
        /// <summary>
        /// Actualiza la información de la empresa
        /// </summary>
        private void Actualizar_Execute()
        {
            GuardarReferencias();
            repository.UpdateCompany(Empresa);
            repository.SubmitChanges();
            RecargarEmpresas();
            SelectedEmpresa = Empresas.FirstOrDefault(e => e.ID == Empresa.ID);

            // Se resetea la bandera
            isEmpresaChanged = false;
        }

        public RelayCommand EliminarCommand { get; }
        [DebuggerStepThrough]
        private bool Eliminar_CanExecute()
        { return Empresas.Any(e => e.ID == Empresa.ID); }
        /// <summary>
        /// Elimina la empresa
        /// </summary>
        private void Eliminar_Execute()
        {
            ConfigurationManager.AppSettings.Set("elemento", $"{Empresa.ID}: {Empresa.Nombre}");
            if (windowManager.ShowDialog(IoC.Get<PopEliminarViewModel>()).GetValueOrDefault())
            {
                repository.DeleteCompany(Empresa.ID);
                repository.SubmitChanges();
                Limpiar_Execute();
            }
        }

        public RelayCommand LimpiarCommand { get; }
        [DebuggerStepThrough]
        private bool Limpiar_CanExecute()
        { return Empresa.ID != "" || Empresa.Nombre != "" || Empresa.Moneda != "" || Empresa.Comentarios != ""; }
        /// <summary>
        /// Limpia los campos del formulario
        /// </summary>
        private void Limpiar_Execute()
        {
            Empresa.PropertyChanged -= Empresa_PropertyChange;
            Empresa.Comentarios = "";
            Empresa.Moneda = "";
            Empresa.YahooID = "";
            Empresa.Nombre = "";
            Empresa.PropertyChanged += Empresa_PropertyChange;
            Empresa.ID = "";

            // Se resetea la bandera
            isEmpresaChanged = false;
        }
        #endregion

        #region METODOS PUBLICOS
        #endregion

        #region METODOS privados
        /// <summary>
        /// Se ejecuta cada vez que una propiedad de <see cref="Empresa"/> es modificada
        /// Si el ID o el Nombre es modificado se Recargan las Empresas
        /// </summary>
        /// <param name="sender">El componente <see cref="Empresa"/></param>
        /// <param name="e">Propiedad que se modificó del componente <see cref="Empresa"/></param>
        private void Empresa_PropertyChange(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName is (nameof(Models.Empresa.ID))
                               or (nameof(Models.Empresa.Nombre)))
            {
                RecargarEmpresas();
                Empresa.YahooID = Empresa.ID;
            }
            isEmpresaChanged = true;
        }

        /// <summary>
        /// Se ejecuta cada vez que una propiedad de <see cref="SelectedReferencia"/> es modificada
        /// Si el ID es modificado, verifica que este no exista y si existe le agregar la terminación "_Duplicado"
        /// </summary>
        /// <param name="sender">El componente <see cref="SelectedReferencia"/></param>
        /// <param name="e">Propiedad que se modificó del componente <see cref="SelectedReferencia"/></param>
        private void SelectedReferencia_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName is nameof(Referencia.ID))
            {
                int countRef;
                countRef = SelectedEmpresa!.Referencias.Count(r => r.ID == SelectedReferencia!.ID) +
                           repository.GetCompany(SelectedEmpresa.ID)!.Referencias.Count(r => r.ID == SelectedReferencia!.ID);

                if (countRef > 1)
                { SelectedReferencia!.ID = $"{SelectedReferencia.ID}_Duplicado"; }
            }
            isEmpresaChanged = true;
        }

        /// <summary>
        /// Carga una nueva lista de elementos <see cref="Empresa"/> a la propiedad <see cref="Empresas"/>
        /// </summary>
        /// <param name="nuevaLista">Lista que se cargará</param>
        private void RecargarEmpresas()
        {
            Empresas.Clear();
            foreach (Empresa empresa in EmpresasFiltradas())
            { Empresas.Add(empresa); }
        }

        /// <summary>
        /// Obtiene una lista de las empresas filtradas por el ID o el Nombre de estas
        /// </summary>
        /// <returns>Lista filtrada de las empresas</returns>
        private IEnumerable<Empresa> EmpresasFiltradas()
        {
            IEnumerable<Empresa> empresas = repository.GetAllCompanies();

            string nombre = Empresa.Nombre ?? "";
            string id = Empresa.ID ?? "";
            if (nombre == "" && id == "") { return empresas; }

            Regex regFiltNombre = new(nombre, RegexOptions.IgnoreCase);
            Regex regFiltID = new(id, RegexOptions.IgnoreCase);

            return nombre != "" && id == ""
                ? empresas.Where(s => regFiltNombre.IsMatch(s.Nombre))
                : nombre == "" && id != ""
                ? empresas.Where(s => regFiltID.IsMatch(s.ID))
                : empresas.Where(s => regFiltNombre.IsMatch(s.Nombre) || regFiltID.IsMatch(s.ID));
        }

        /// <summary>
        /// Guarda las referencias en la base de datos si hay una empresa seleccionada, 
        /// de lo contrario se crea una referencia vacia en el objeto <see cref="Empresa"/>
        /// </summary>
        private void GuardarReferencias()
        {
            if (SelectedEmpresa is null)
            {
                Empresa.Referencias.Add(ayudaRef);
                Empresa.Referencias.Add(yahooRef);
                //Empresa.Referencias.Add(reutersRef);
                return;
            }
            
            // Primero Elimina todas las referencias originales
            foreach (Referencia? referencia in repository.GetCompany(SelectedEmpresa.ID)!.Referencias)
            { repository.DeleteReferencia(SelectedEmpresa.ID, referencia.ID); }

            // Ahora agrega las nuevas referencias
            foreach (Referencia referencia in SelectedEmpresa.Referencias)
            {
                if (!string.IsNullOrWhiteSpace(referencia.ID))
                { repository.AddReferencia(SelectedEmpresa.ID, referencia); }
            }

            // Si no se agrego ninguna referencia, entonces se agrega una vacia
            if (repository.GetCompany(SelectedEmpresa.ID)!.Referencias.Count == 0)
            {
                throw new System.NotSupportedException("EmpresasViewModel.cs-Linea 371: En teoria nunca se cae aqui, si sale este error habilitar las siguientes lineas de codigo y quitar este THROW");
                //repository.AddReferencia(SelectedEmpresa.ID, ayudaRef);
                //repository.AddReferencia(SelectedEmpresa.ID, yahooRef);
                //repository.AddReferencia(SelectedEmpresa.ID, reutersRef);
            }
        }
        #endregion
    }
}