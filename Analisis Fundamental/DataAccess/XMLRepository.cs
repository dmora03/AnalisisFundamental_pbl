using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace DataAccess
{
    //class XMLRepository<T, TKey> : IRepository<T, TKey> where T : class, IEntity

    /// <summary>
    /// Clase con metodos para consultar o modificar el XML
    /// </summary>
    public class XMLRepository : IRepository<Empresa, string>
    {
        /// <summary>
        /// Constantes de los Nombres de nodos y atributos del XML
        /// </summary>
        private struct XML
        {
            public const string LST_Reportes = "Reportes";
            public const string LST_Referencias = "Referencias";

            public const string NDO_Empresa = "Empresa";
            public const string NDO_Reporte = "Reporte";
            public const string NDO_Referencia = "Referencia";

            public const string ATR_IdEmpresa = "ID";
            public const string ATR_IdReporte = "ID";
            public const string ATR_IdReferencia = "ID";

            public const string ATR_NombreEmpresa = "Nombre";
            public const string ATR_MonedaEmpresa = "Moneda";
            public const string ATR_YahooID = "YahooID";
            public const string NDO_ComentariosEmpresa = "Comentarios";
        }

        private readonly XDocument doc;
        private readonly string xmlFilePath;

        /// <summary>
        /// Inicializa una nueva intancia de la clase <see cref="XMLRepository"/> con un XML dado
        /// </summary>
        /// <param name="xmlFilePath">Ruta del XML a ser cargado</param>
        public XMLRepository(string xmlFilePath)
        {
            this.xmlFilePath = xmlFilePath;
            this.doc = XDocument.Load(xmlFilePath, LoadOptions.None);
        }

        #region GET
        /// <summary>
        /// Obtiene todas las empresas registadas
        /// </summary>
        /// <returns>Una colleccion de todas las Empresa</returns>
        public IEnumerable<Empresa> GetAllCompanies()
        {
            return doc.Descendants(XML.NDO_Empresa).
                       Select(element => element.FromXElement<Empresa>()).
                       OrderBy(e => e?.ID)!;
                       //.Cast<Empresa>();
        }

        /// <summary>
        /// Obtiene la empresa que tenga el ID especificado
        /// </summary>
        /// <param name="idEmpresa">El ID de la empresa a encontrar</param>
        /// <returns>La <see cref="Empresa"/> con el ID deseado</returns>
        public Empresa? GetCompany(string idEmpresa)
        {
            return GetCompanyById(idEmpresa)?.FromXElement<Empresa>();
        }

        /// <summary>
        /// Obtiene todos los reportes de una empresa especificada
        /// </summary>
        /// <param name="idEmpresa">El ID de la empresa de la que se quiere obtener sus reportes</param>
        /// <returns>Una colección de todos los reportes de la empresa especificada</returns>
        public IEnumerable<Reporte> GetAllReports(string idEmpresa)
        {
            return GetCompanyById(idEmpresa)?.Descendants(XML.NDO_Reporte)
                                             .Select(e => e.FromXElement<Reporte>())
                                             .OrderByDescending(r => r?.ID)!//.Cast<Reporte>()
                   ?? Enumerable.Empty<Reporte>();
        }

        /// <summary>
        /// Obtiene el Reporte especificado por el ID del reporte y el ID de la empresa deseada
        /// si no existe regresa NULL
        /// </summary>
        /// <param name="idEmpresa">ID de la empresa donde esta el reporte</param>
        /// <param name="idReporte">ID del reporte deseado</param>
        /// <returns>El <see cref="Reporte"/> deseado o <see cref="Nullable"/> si no existe</returns>
        public Reporte? GetReport(string idEmpresa, string idReporte)
        {
            return GetReportById(idEmpresa, idReporte)?.FromXElement<Reporte>();
        }

        /// <summary>
        /// Obtiene la Referencia especificada por el ID de la referencia y el ID de la empresa deseada
        /// si no existe regresa NULL
        /// </summary>
        /// <param name="idEmpresa">ID de la empresa donde esta el reporte</param>
        /// <param name="idReferencia">ID de la referencia deseada</param>
        /// <returns>La <see cref="Referencia"/> deseada o <see cref="Nullable"/> si no existe</returns>
        public Referencia? GetReferencia(string idEmpresa, string idReferencia)
        {
            return GetReferenciaById(idEmpresa, idReferencia)?.FromXElement<Referencia>();
        }

        /// <summary>
        /// Obtiene el <see cref="XElement"/> de una empresa que tenga el ID especificado
        /// </summary>
        /// <param name="id">El ID de la empresa a encontrar</param>
        /// <returns>El <see cref="XElement"/> de la Empresa con el ID deseado</returns>
        /// <exception cref="InvalidOperationException">Lanzado cuando hay 2 o mas elementos con el mismo ID</exception>
        private XElement? GetCompanyById(string id)
        {
            return doc.Descendants(XML.NDO_Empresa)
                .SingleOrDefault(element => (string?)element.Attribute(XML.ATR_IdEmpresa) == id);
        }

        /// <summary>
        /// Obtiene el <see cref="XElement"/> del reporte de una empresa especificada
        /// </summary>
        /// <param name="idEmpresa">El ID de la empresa</param>
        /// <param name="idReporte">El ID del reporte a encontrar</param>
        /// <returns>El <see cref="XElement"/> del Reporte de la empresa indicada</returns>
        /// <exception cref="InvalidOperationException">Lanzado cuando hay 2 o mas elementos con el mismo ID</exception>
        private XElement? GetReportById(string idEmpresa, string idReporte)
        {
            return GetCompanyById(idEmpresa)?
                .Descendants(XML.NDO_Reporte)
                .SingleOrDefault(e => (string?)e.Attribute(XML.ATR_IdReporte) == idReporte);
        }

        /// <summary>
        /// Obtiene el <see cref="XElement"/> de la referencia de una empresa especificada
        /// </summary>
        /// <param name="idEmpresa">El ID de la empresa</param>
        /// <param name="idReferencia">El ID de la referencia a encontrar</param>
        /// <returns>El <see cref="XElement"/> de la Referencia de la empresa indicada</returns>
        /// <exception cref="InvalidOperationException">Lanzado cuando hay 2 o mas elementos con el mismo ID</exception>
        private XElement? GetReferenciaById(string idEmpresa, string idReferencia)
        {
            return GetCompanyById(idEmpresa)?
                .Descendants(XML.NDO_Referencia)
                .SingleOrDefault(e => (string?)e.Attribute(XML.ATR_IdReferencia) == idReferencia);
        }
        #endregion
        #region ADD
        /// <summary>
        /// Agrega la empresa especificada
        /// </summary>
        /// <param name="empresa">La empresa que se agregará</param>
        /// <exception cref="ArgumentException">Lanzado cuando la empresa a agregar ya existe</exception>
        public void AddCompany(Empresa empresa)
        {
            if (GetCompanyById(empresa.ID) is null)
            {
                doc.Root!.Add(empresa.ToXElement<Empresa>());
            }
            else
            {
                throw new ArgumentException($"La empresa {empresa.ID} ya existe.", nameof(empresa));
            }
        }

        /// <summary>
        /// Agrega un reporte a una empresa especificada
        /// </summary>
        /// <param name="idEmpresa">El ID de la empresa que se le agregarea el reporte</param>
        /// <param name="reporte">El reporte que se agregará</param>
        /// <exception cref="ArgumentException">Lanzado cuando la empresa no existe o el reporte a agregar ya existe</exception>
        public void AddReport(string idEmpresa, Reporte reporte)
        {
            XElement? company = GetCompanyById(idEmpresa)?.Element(XML.LST_Reportes);
            if (company == null) { throw new ArgumentException($"La empresa {idEmpresa} NO esta registrada o no se encontro el nodo {XML.LST_Reportes}.", nameof(idEmpresa)); }

            XElement? exist = company.Descendants(XML.NDO_Reporte)
                .FirstOrDefault(e => (string?)e.Attribute(XML.ATR_IdReporte) == reporte.ID);
            if (exist == null)
            {
                int fctr = reporte.Factor;
                reporte.Factor = 1;
                UpdateAnnualData(idEmpresa, reporte);
                company.Add(reporte.ToXElement<Reporte>());
                reporte.Factor = fctr;
            }
            else
            {
                throw new ArgumentException($"El reporte {reporte.ID} ya existe.", nameof(reporte));
            }
        }

        /// <summary>
        /// Agrega una referencia a una empresa especificada
        /// </summary>
        /// <param name="idEmpresa">El ID de la empresa que se le agregara la referencia</param>
        /// <param name="referencia">La referencia que se agregará</param>
        /// <exception cref="ArgumentException">Lanzado cuando la empresa no existe o la referencia a agregar ya existe</exception>
        public void AddReferencia(string idEmpresa, Referencia referencia)
        {
            XElement? company = GetCompanyById(idEmpresa)?.Element(XML.LST_Referencias);
            if (company == null) { throw new ArgumentException($"La empresa {idEmpresa} NO esta registrada o no se encontro el nodo {XML.LST_Referencias}.", nameof(idEmpresa)); }

            XElement? exist = company.Descendants(XML.NDO_Referencia)
                .FirstOrDefault(e => (string?)e.Attribute(XML.ATR_IdReferencia) == referencia.ID);
            if (exist == null)
            {
                company.Add(referencia.ToXElement<Referencia>());
            }
            else
            {
                throw new ArgumentException($"La referencia {referencia.ID} ya existe.", nameof(referencia));
            }
        }
        #endregion
        #region DELETE
        /// <summary>
        /// Eliminar la empresa con el ID especificado
        /// </summary>
        /// <param name="idEmpresa">ID de la empresa a eliminar</param>
        public void DeleteCompany(string idEmpresa)
        {
            GetCompanyById(idEmpresa)?
                .Remove();
        }

        /// <summary>
        /// Elimina un reporte de una empresa especificada
        /// </summary>
        /// <param name="idEmpresa">ID de la empresa</param>
        /// <param name="idReporte">ID del reporte a eliminar</param>
        public void DeleteReport(string idEmpresa, string idReporte)
        {
            XElement? xReporte = GetReportById(idEmpresa, idReporte);
            if (xReporte is null) { return; }

            Reporte reporte = xReporte.FromXElement<Reporte>()!;
            Reporte? repTD = GetReport(idEmpresa, reporte.TrimestreIDOffset(1));
            if (repTD is not null)
            {
                ClearAnnualData(repTD);
                UpdateAnnualDataXML(idEmpresa, repTD);
            }
            repTD = GetReport(idEmpresa, reporte.TrimestreIDOffset(2));
            if (repTD is not null)
            {
                ClearAnnualData(repTD);
                UpdateAnnualDataXML(idEmpresa, repTD);
            }
            repTD = GetReport(idEmpresa, reporte.TrimestreIDOffset(3));
            if (repTD is not null)
            {
                ClearAnnualData(repTD);
                UpdateAnnualDataXML(idEmpresa, repTD);
            }

            xReporte.Remove();
        }

        /// <summary>
        /// Elimina una referencia de una empresa especificada
        /// </summary>
        /// <param name="idEmpresa">ID de la empresa</param>
        /// <param name="idReferencia">ID de la referencia a eliminar</param>
        public void DeleteReferencia(string idEmpresa, string idReferencia)
        {
            GetReferenciaById(idEmpresa, idReferencia)?
                .Remove();
        }
        #endregion
        #region UPDATE
        /// <summary>
        /// Actualiza SOLO el Nombre, la Moneda y los Comentarios de una empresa
        /// </summary>
        /// <param name="empresa">La empresa que se actualizará en la base de datos</param>
        /// <exception cref="ArgumentException">Lanzado cuando la empresa a actualizar NO existe</exception>
        public void UpdateCompany(Empresa empresa)
        {
            XElement? xEmpresa = GetCompanyById(empresa.ID);
            if (xEmpresa is null)
            {
                throw new ArgumentException($"La empresa {empresa.ID} NO existe, por lo que no es posible actualizarla", nameof(empresa));
            }
            else
            {
                //xEmpresa.ReplaceWith(empresa.ToXElement<Empresa>());
                xEmpresa.Attribute(XML.ATR_NombreEmpresa)!.Value = empresa.Nombre;
                xEmpresa.Attribute(XML.ATR_MonedaEmpresa)!.Value = empresa.Moneda;
                xEmpresa.Attribute(XML.ATR_YahooID)!.Value = empresa.YahooID;
                xEmpresa.Element(XML.NDO_ComentariosEmpresa)!.Value = empresa.Comentarios;
            }
        }

        /// <summary>
        /// Actualiza un reporte dado en la empresa especificada
        /// </summary>
        /// <param name="idEmpresa">El ID de la empresa</param>
        /// <param name="reporte">El reporte que se actualizará en la base de datos</param>
        /// <exception cref="ArgumentException">Lanzado cuando el reporte a actualizar NO existe o la empresa NO existe</exception>
        public void UpdateReport(string idEmpresa, Reporte reporte)
        {
            XElement? xReporte = GetReportById(idEmpresa, reporte.ID);
            if (xReporte is null)
            {
                throw new ArgumentException($"El Reporte {reporte.ID} de la empresa {idEmpresa} NO existe, por lo que no es posible actualizarlo", nameof(reporte));
            }
            else
            {
                int fctr = reporte.Factor;
                reporte.Factor = 1;
                UpdateAnnualData(idEmpresa, reporte);
                xReporte.ReplaceWith(reporte.ToXElement<Reporte>());
                reporte.Factor = fctr;
            }
        }

        /// <summary>
        /// Actualiza una referencia dada en la empresa especificada
        /// </summary>
        /// <param name="idEmpresa">El ID de la empresa</param>
        /// <param name="referencia">La referencia que se actualizará en la base de datos</param>
        /// <exception cref="ArgumentException">Lanzado cuando la referencia a actualizar NO existe o la empresa NO existe</exception>
        public void UpdateReferencia(string idEmpresa, Referencia referencia)
        {
            XElement? xReferencia = GetReferenciaById(idEmpresa, referencia.ID);
            if (xReferencia is null)
            {
                throw new ArgumentException($"La Referencia {referencia.ID} de la empresa {idEmpresa} NO existe, por lo que no es posible actualizarlo", nameof(referencia));
            }
            else
            {
                xReferencia.ReplaceWith(referencia.ToXElement<Referencia>());
            }
        }
        #endregion

        /// <summary>
        /// Guarda todos los cambios hechos
        /// </summary>
        public void SubmitChanges() => SubmitChanges(xmlFilePath);

        /// <summary>
        /// Guardar los cambios en un archivo distinto
        /// </summary>
        /// <param name="filePath">Path donde se guardara el nuevo archivo</param>
        public void SubmitChanges(string filePath)
        {
            doc.Save(filePath);
        }


        #region Actualizar Datos Anuales Acumulativos
        /// <summary>
        /// Actualiza SOLO los datos Anuales del reporte dado y sus 3 reportes siguientes dependientes (solo si hay)
        /// y Si el reporte dado es el primer Trimestre fiscal, actualiza el Efectivo al Inicio de los 3 reportes siguientes tambien
        /// </summary>
        /// <param name="idEmpresa">El ID de la empresa</param>
        /// <param name="reporte">El reporte que se actualizará</param>
        private void UpdateAnnualData(string idEmpresa, Reporte reporte)
        {
            Reporte? rep3TA = GetReport(idEmpresa, reporte.TrimestreIDOffset(-3));
            Reporte? rep2TA = GetReport(idEmpresa, reporte.TrimestreIDOffset(-2));
            Reporte? rep1TA = GetReport(idEmpresa, reporte.TrimestreIDOffset(-1));

            Reporte? rep1TD = GetReport(idEmpresa, reporte.TrimestreIDOffset(1));
            Reporte? rep2TD = GetReport(idEmpresa, reporte.TrimestreIDOffset(2));
            Reporte? rep3TD = GetReport(idEmpresa, reporte.TrimestreIDOffset(3));

            Reporte? rep1AnoDespues = GetReport(idEmpresa, reporte.TrimestreIDOffset(4));

            if (rep1TA is null || rep2TA is null || rep3TA is null) { ClearAnnualData(reporte); }
            else { SumAnnualData(rep3TA, rep2TA, rep1TA, reporte); }

            if (rep1TD is not null)
            {
                if (rep1TA is null || rep2TA is null) { ClearAnnualData(rep1TD); }
                else { SumAnnualData(rep2TA, rep1TA, reporte, rep1TD); }

                if (reporte.TrimestreReporte == 1)
                { rep1TD.EfectivoAlInicio = reporte.EfectivoAlInicio; }

                UpdateAnnualDataXML(idEmpresa, rep1TD);
            }

            if (rep2TD is not null)
            {
                if (rep1TA is null || rep1TD is null) { ClearAnnualData(rep2TD); }
                else { SumAnnualData(rep1TA, reporte, rep1TD, rep2TD); }

                if (reporte.TrimestreReporte == 1)
                { rep2TD.EfectivoAlInicio = reporte.EfectivoAlInicio; }

                UpdateAnnualDataXML(idEmpresa, rep2TD);
            }

            if (rep3TD is not null)
            {
                if (rep1TD is null || rep2TD is null) { ClearAnnualData(rep3TD); }
                else { SumAnnualData(reporte, rep1TD, rep2TD, rep3TD); }

                if (reporte.TrimestreReporte == 1)
                { rep3TD.EfectivoAlInicio = reporte.EfectivoAlInicio; }

                UpdateAnnualDataXML(idEmpresa, rep3TD);
            }

            if (rep1AnoDespues is not null)
            {
                rep1AnoDespues.NumUnidadesAA = reporte.NumUnidades;
                rep1AnoDespues.IngresosAA = reporte.Ingresos;
                rep1AnoDespues.UAccionistasAA = reporte.UAccionistas;
                rep1AnoDespues.CapitalAA = reporte.Capital;

                UpdateAnnualDataXML(idEmpresa, rep1AnoDespues);
            }
        }

        /// <summary>
        /// Actualiza el reporte dado en el XML, sin verificar si existe o no
        /// </summary>
        /// <param name="idEmpresa">El ID de la empresa</param>
        /// <param name="reporte">El reporte que se actualizará en la base de datos</param>
        private void UpdateAnnualDataXML(string idEmpresa, Reporte reporte)
        {
            XElement? xReporte = GetReportById(idEmpresa, reporte.ID);
            xReporte!.ReplaceWith(reporte.ToXElement<Reporte>());
        }

        /// <summary>
        /// Suma los 4 trimestres dados y guarda el dato Annual en el cuarto trimestre
        /// </summary>
        /// <param name="trim1">Primer trimestre</param>
        /// <param name="trim2">Segundo trimestre</param>
        /// <param name="trim3">Tercer trimestre</param>
        /// <param name="trim4Annual">Cuarto trimestre que contrendra el valor acumulado anual</param>
        private static void SumAnnualData(Reporte trim1, Reporte trim2, Reporte trim3, Reporte trim4Annual)
        {
            trim4Annual.IngresosAcumulados = trim1.Ingresos + trim2.Ingresos + trim3.Ingresos + trim4Annual.Ingresos;
            trim4Annual.IngresosAAAcumulados = trim1.IngresosAA + trim2.IngresosAA + trim3.IngresosAA + trim4Annual.IngresosAA;
            trim4Annual.UBrutaAcumulados = trim1.UBruta + trim2.UBruta + trim3.UBruta + trim4Annual.UBruta;
            trim4Annual.UOperativaAcumulados = trim1.UOperativa + trim2.UOperativa + trim3.UOperativa + trim4Annual.UOperativa;
            trim4Annual.UAImpuestosAcumulados = trim1.UAImpuestos + trim2.UAImpuestos + trim3.UAImpuestos + trim4Annual.UAImpuestos;
            trim4Annual.UNetaAcumulados = trim1.UNeta + trim2.UNeta + trim3.UNeta + trim4Annual.UNeta;
            trim4Annual.UAccionistasAcumulados = trim1.UAccionistas + trim2.UAccionistas + trim3.UAccionistas + trim4Annual.UAccionistas;
            trim4Annual.UAccionistasAAAcumulados = trim1.UAccionistasAA + trim2.UAccionistasAA + trim3.UAccionistasAA + trim4Annual.UAccionistasAA;
            
            trim4Annual.IngresosPropAcumulados = trim1.IngresosProp + trim2.IngresosProp + trim3.IngresosProp + trim4Annual.IngresosProp;
            trim4Annual.NOIAcumulados = trim1.NOI + trim2.NOI + trim3.NOI + trim4Annual.NOI;
            trim4Annual.FFOAcumulados= trim1.FFO + trim2.FFO + trim3.FFO + trim4Annual.FFO;
            trim4Annual.AFFOAcumulados = trim1.AFFO + trim2.AFFO + trim3.AFFO + trim4Annual.AFFO;

            trim4Annual.DividendosAcumulados= trim1.Dividendo + trim2.Dividendo + trim3.Dividendo + trim4Annual.Dividendo;
        }

        /// <summary>
        /// Borra toda la informacion Annual acumulada del reporte deseado
        /// </summary>
        /// <param name="reporte">Reporte que se le borrara la informacion Anual acumulada</param>
        private static void ClearAnnualData(Reporte reporte)
        {
            reporte.IngresosAcumulados = null;
            reporte.IngresosAAAcumulados = null;
            reporte.UBrutaAcumulados = null;
            reporte.UOperativaAcumulados = null;
            reporte.UAImpuestosAcumulados = null;
            reporte.UNetaAcumulados = null;
            reporte.UAccionistasAcumulados = null;
            reporte.UAccionistasAAAcumulados = null;

            reporte.IngresosPropAcumulados = null;
            reporte.NOIAcumulados = null;
            reporte.FFOAcumulados = null;
            reporte.AFFOAcumulados = null;

            reporte.DividendosAcumulados = null;
        }
        #endregion
    }
}