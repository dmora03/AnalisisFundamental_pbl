/*********************************************************************/
/***** MODIFICAR ESTA CLASE CONFORME A LA NECESIDAD DEL PROYECTO *****/
/*        Hay que agregar metodos o renombrar los existentes         */
/*********************************************************************/
using Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace DataAccess
{
    public class DesignTimeRepository : IRepository<Empresa, string>
    {
        private readonly ObservableCollection<Empresa> empresas;

        /// <summary>
        /// Inicializa una nueva intancia de la clase <see cref="DesignTimeRepository"/> con datos Dummy
        /// </summary>
        public DesignTimeRepository()
        {
            //INICIALIZACION
            ObservableCollection<Referencia> referencias = new()
            {
                new Referencia()
                {
                    ID = "Ref1",
                    AccionesCir = "AccionesCir",
                    ActivosC = "ActivosC",
                    ActivosT = "ActivosT",
                    Capital = "Capital",
                    CostoVentasAuto = true,
                    Deuda = "Deuda",
                    Dividendo = "Dividendo",
                    Efectivo = "Efectivo",
                    EfectivoAlFinal = "EfectivoAlFinal",
                    EfectivoAlInicio = "EfectivoAlInicio",
                    FlujoFinanciamiento = "FlujoFinanciamiento",
                    FlujoInversiones = "FlujoInversiones",
                    FlujoOperaciones = "FlujoOperaciones",
                    GastosFinAuto = false,
                    GastosOpAuto = true,
                    Ingresos = "Ingresos",
                    NumUnidades = "NumUnidades",
                    PasivosC = "PasivosC",
                    PasivosT = "PasivosT",
                    UAccionistas = "UAccionistas",
                    UAImpuestos = "UAImpuestos",
                    UBruta = "UBruta",
                    UNeta = "UNeta",
                    UOperativa = "UOperativa",
                },
                new Referencia() { ID = "Ref2" },
                new Referencia() { ID = "Ref3" },
                new Referencia() { ID = "Ref4" },
                new Referencia() { ID = "Ref5" },
                new Referencia() { ID = "Ref6" },
                new Referencia() { ID = "Ref7" },
                new Referencia() { ID = "Ref8" },
                new Referencia() { ID = "Ref9" }
            };

            ObservableCollection<Reporte> reportes = new()
            {
                new Reporte()
                {
                    Fecha = new(2020, 9, 30),
                    TrimestreReporte = 3,
                    AccionesCir = 497801569,
                    ActivosC = 526560000,
                    ActivosT = 20925753000,
                    Capital = 12104003000,
                    CapitalAA = 10585774000,
                    Deuda = 8453483000,
                    Dividendo = 0.1530,
                    Efectivo = 141913000.00,
                    EfectivoAlFinal = 0,
                    EfectivoAlInicio = 0,
                    FlujoFinanciamiento = 0,
                    FlujoInversiones = 0,
                    FlujoOperaciones = 0,
                    Ingresos = 321293000.00,
                    IngresosAA = 377457000.00,
                    NumUnidades = 782486.00,
                    NumUnidadesAA = 782486.00,
                    PasivosC = 230789000,
                    PasivosT = 8821750000.00,
                    Precio = 6.00,
                    UAccionistas = 65899000.00,
                    UAccionistasAA = 140421000.00,
                    UAImpuestos = 65899000.00,
                    UBruta = 321293000.00,
                    UNeta = 65899000.00,
                    UOperativa = 227330000.00,
                    DividendosAcumulados = 0.5511,
                    IngresosAAAcumulados = -1472717000.00,
                    IngresosAcumulados = 1374324000.00,
                    UAccionistasAAAcumulados = 1092768000.00,
                    UAccionistasAcumulados = 1925085000.00,
                    UAImpuestosAcumulados = 1925085000.00,
                    UBrutaAcumulados = 1374324000.00,
                    UNetaAcumulados = 1925085000.00,
                    UOperativaAcumulados = 982649000.00
                },
                new Reporte() 
                {
                    Fecha = new(2020, 6, 30),
                    TrimestreReporte = 2,
                    AccionesCir = 492673364,
                    ActivosC = 452362000,
                    ActivosT = 20789360000.00,
                    Capital = 12057897000.00,
                    CapitalAA = 10598945000.00,
                    Deuda = 8383096000.00,
                    Dividendo = 0.1327,
                    Efectivo = 169260000.00,
                    EfectivoAlFinal = 0,
                    EfectivoAlInicio = 0,
                    FlujoFinanciamiento = 0,
                    FlujoInversiones = 0,
                    FlujoOperaciones = 0,
                    Ingresos = 293032000.00,
                    IngresosAA = 375816000.00,
                    NumUnidades = 782486.00,
                    NumUnidadesAA = 782486.00,
                    PasivosC = 189476000,
                    PasivosT = 8731463000.00,
                    Precio = 6.00,
                    UAccionistas = 247583000.00,
                    UAccionistasAA = 132736000.00,
                    UAImpuestos = 247583000.00,
                    UBruta = 293032000.00,
                    UNeta = 247583000.00,
                    UOperativa = 212504000.00,
                    DividendosAcumulados = 0,
                    IngresosAAAcumulados = 0,
                    IngresosAcumulados = 0,
                    UAccionistasAAAcumulados = 0,
                    UAccionistasAcumulados = 0,
                    UAImpuestosAcumulados = 0,
                    UBrutaAcumulados = 0,
                    UNetaAcumulados = 0,
                    UOperativaAcumulados = 0
                },
                new Reporte()
                {
                    Fecha = new(2020, 3, 30),
                    TrimestreReporte = 1,
                    AccionesCir = 485374094,
                    ActivosC = 335218000,
                    ActivosT = 20621826000.00,
                    Capital = 11837510000.00,
                    CapitalAA = 10585550620.00,
                    Deuda = 8254958000.00,
                    Dividendo = 0.1327,
                    Efectivo = 115040000.00,
                    EfectivoAlFinal = 0,
                    EfectivoAlInicio = 0,
                    FlujoFinanciamiento = 0,
                    FlujoInversiones = 0,
                    FlujoOperaciones = 0,
                    Ingresos = 369954000.00,
                    IngresosAA = 358622000.00,
                    NumUnidades = 782486.00,
                    NumUnidadesAA = 782332.00,
                    PasivosC = 3087163000,
                    PasivosT = 8784316000.00,
                    Precio = 8.30,
                    UAccionistas = 100263000.00,
                    UAccionistasAA = 125887000.00,
                    UAImpuestos = 100263000.00,
                    UBruta = 369954000.00,
                    UNeta = 100263000.00,
                    UOperativa = 264333000.00,
                    DividendosAcumulados = 0,
                    IngresosAAAcumulados = 0,
                    IngresosAcumulados = 0,
                    UAccionistasAAAcumulados = 0,
                    UAccionistasAcumulados = 0,
                    UAImpuestosAcumulados = 0,
                    UBrutaAcumulados = 0,
                    UNetaAcumulados = 0,
                    UOperativaAcumulados = 0
                },
                new Reporte()
                {
                    Fecha = new(2019, 12, 31),
                    TrimestreReporte = 4,
                    AccionesCir = 444990729,
                    ActivosC = 348899000,
                    ActivosT = 20375620000.00,
                    Capital = 11544002000.00,
                    CapitalAA = 10657556000.00,
                    Deuda = 8112849000.00,
                    Dividendo = 0.1327,
                    Efectivo = 94822000.00,
                    EfectivoAlFinal = 0,
                    EfectivoAlInicio = 0,
                    FlujoFinanciamiento = 0,
                    FlujoInversiones = 0,
                    FlujoOperaciones = 0,
                    Ingresos = 390045000.00,
                    IngresosAA = 360822000.00,
                    NumUnidades = 782486.00,
                    NumUnidadesAA = 779422.00,
                    PasivosC = 3305474000,
                    PasivosT = 8831618000.00,
                    Precio = 9.45,
                    UAccionistas = 1511340000.00,
                    UAccionistasAA = 693724000.00,
                    UAImpuestos = 1511340000.00,
                    UBruta = 390045000.00,
                    UNeta = 1511340000.00,
                    UOperativa = 278482000.00,
                    DividendosAcumulados = 0,
                    IngresosAAAcumulados = 0,
                    IngresosAcumulados = 0,
                    UAccionistasAAAcumulados = 0,
                    UAccionistasAcumulados = 0,
                    UAImpuestosAcumulados = 0,
                    UBrutaAcumulados = 0,
                    UNetaAcumulados = 0,
                    UOperativaAcumulados = 0
                },
                new Reporte() { Fecha = new(2021, 9, 30) },
                new Reporte() { Fecha = new(2021, 6, 30) },
            };

            empresas = new ObservableCollection<Empresa>()
            {
                new Empresa(){ID="0DMB", Nombre="DTR David Mora Barreto", Moneda="MXN", Comentarios="Hola como estan?", Referencias=referencias, Reportes=reportes},
                new Empresa(){ID="0OSC", Nombre="DTR Oscar", Moneda="USD", Comentarios="Yo bien y tu?", Referencias=referencias, Reportes=reportes},
                new Empresa(){ID="0DAN", Nombre="DTR Daniel", Moneda="EUR", Comentarios="A todo dar", Referencias=referencias, Reportes=reportes},
                new Empresa(){ID="1DMB", Nombre="DTR David Mora Barreto", Moneda="MXN", Comentarios="Hola como estan?"},
                new Empresa(){ID="1OSC", Nombre="DTR Oscar", Moneda="USD", Comentarios="Yo bien y tu?"},
                new Empresa(){ID="1DAN", Nombre="DTR Daniel", Moneda="EUR", Comentarios="A todo dar"},
                new Empresa(){ID="2DMB", Nombre="DTR David Mora Barreto", Moneda="MXN", Comentarios="Hola como estan?"},
                new Empresa(){ID="2OSC", Nombre="DTR Oscar", Moneda="USD", Comentarios="Yo bien y tu?"},
                new Empresa(){ID="2DAN", Nombre="DTR Daniel", Moneda="EUR", Comentarios="A todo dar"},
                new Empresa(){ID="3DMB", Nombre="DTR David Mora Barreto", Moneda="MXN", Comentarios="Hola como estan?"},
                new Empresa(){ID="3OSC", Nombre="DTR Oscar", Moneda="USD", Comentarios="Yo bien y tu?"},
                new Empresa(){ID="3DAN", Nombre="DTR Daniel", Moneda="EUR", Comentarios="A todo dar"},
                new Empresa(){ID="4DMB", Nombre="DTR David Mora Barreto", Moneda="MXN", Comentarios="Hola como estan?"},
                new Empresa(){ID="4OSC", Nombre="DTR Oscar", Moneda="USD", Comentarios="Yo bien y tu?"},
                new Empresa(){ID="4DAN", Nombre="DTR Daniel", Moneda="EUR", Comentarios="A todo dar"},
                new Empresa(){ID="5DMB", Nombre="DTR David Mora Barreto", Moneda="MXN", Comentarios="Hola como estan?"},
                new Empresa(){ID="5OSC", Nombre="DTR Oscar", Moneda="USD", Comentarios="Yo bien y tu?"},
                new Empresa(){ID="5DAN", Nombre="DTR Daniel", Moneda="EUR", Comentarios="A todo dar"},
                new Empresa(){ID="6DMB", Nombre="DTR David Mora Barreto", Moneda="MXN", Comentarios="Hola como estan?"},
                new Empresa(){ID="6OSC", Nombre="DTR Oscar", Moneda="USD", Comentarios="Yo bien y tu?"},
                new Empresa(){ID="6DAN", Nombre="DTR Daniel", Moneda="EUR", Comentarios="A todo dar"},
                new Empresa(){ID="7DMB", Nombre="DTR David Mora Barreto", Moneda="MXN", Comentarios="Hola como estan?"},
                new Empresa(){ID="7OSC", Nombre="DTR Oscar", Moneda="USD", Comentarios="Yo bien y tu?"},
                new Empresa(){ID="7DAN", Nombre="DTR Daniel", Moneda="EUR", Comentarios="A todo dar"},
                new Empresa(){ID="8DMB", Nombre="DTR David Mora Barreto", Moneda="MXN", Comentarios="Hola como estan?"},
                new Empresa(){ID="8OSC", Nombre="DTR Oscar", Moneda="USD", Comentarios="Yo bien y tu?"},
                new Empresa(){ID="8DAN", Nombre="DTR Daniel", Moneda="EUR", Comentarios="A todo dar"},
                new Empresa(){ID="9DMB", Nombre="DTR David Mora Barreto", Moneda="MXN", Comentarios="Hola como estan?"},
                new Empresa(){ID="9OSC", Nombre="DTR Oscar", Moneda="USD", Comentarios="Yo bien y tu?"},
                new Empresa(){ID="9DAN", Nombre="DTR Daniel", Moneda="EUR", Comentarios="A todo dar"},
            };
        }

        #region IREPOSITORY
        #region GET
        public IEnumerable<Empresa> GetAllCompanies()
        { return empresas.OrderBy(e => e.ID); }
        public Empresa? GetCompany(string idEmpresa)
        { return empresas.FirstOrDefault(e => e.ID == idEmpresa); }
        public IEnumerable<Reporte> GetAllReports(string idEmpresa)
        { return GetCompany(idEmpresa)?.Reportes ?? Enumerable.Empty<Reporte>(); }
        public Reporte? GetReport(string idEmpresa, string idReporte)
        { return GetAllReports(idEmpresa)?.FirstOrDefault(r => r?.ID == idReporte); }
        public Referencia? GetReferencia(string idEmpresa, string idReferencia)
        { return GetCompany(idEmpresa)?.Referencias.FirstOrDefault(r => r?.ID == idReferencia); }
        #endregion

        #region ADD (NotImplementedException)
        public void AddCompany(Empresa empresa) { throw new NotImplementedException(); }
        public void AddReport(string idEmpresa, Reporte reporte) { throw new NotImplementedException(); }
        public void AddReferencia(string idEmpresa, Referencia referencia) { throw new NotImplementedException(); }
        #endregion

        #region DELETE (NotImplementedException)
        public void DeleteCompany(string idEmpresa) { throw new NotImplementedException(); }
        public void DeleteReport(string idEmpresa, string idReporte) { throw new NotImplementedException(); }
        public void DeleteReferencia(string idEmpresa, string idReferencia) { throw new NotImplementedException(); }
        #endregion

        #region UPDATE (NotImplementedException)
        public void UpdateCompany(Empresa empresa) { throw new NotImplementedException(); }
        public void UpdateReport(string idEmpresa, Reporte reporte) { throw new NotImplementedException(); }
        public void UpdateReferencia(string idEmpresa, Referencia referencia) { throw new NotImplementedException(); }
        #endregion

        public void SubmitChanges() { throw new NotImplementedException(); }
        #endregion
    }
}