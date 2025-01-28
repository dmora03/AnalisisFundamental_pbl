using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using DataAccess;
using Models;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;

namespace UnitTests
{
    [TestClass]
    public class XMLRepositoryTest : TestBase
    {                            
        private const string XML_FILE_PATH = @"C:\Users\uid86857\source\repos\Analisis Fundamental\DataAccess\XML\EmpresasDB_TEST.xml";
        private const string XML_FILE_SAVE = @"C:\Users\uid86857\source\repos\Analisis Fundamental\DataAccess\XML\EmpresasDB_SAVED.xml";

        protected XMLRepository repository;

        private static Empresa empresa_full;
        private static Empresa empresa_empty;
        private static Referencia referencia1;
        private static Referencia referencia2;
        private static Reporte reporte_1;
        private static Reporte reporte_2;
        private static Reporte reporte_3;
        private static Reporte reporte_4;

        #region INITIALIZE & CLEANUP CLASS
        /// <summary>
        /// Realiza la inicialización antes de la ejecución de TODAS las pruebas unitarias de esta clase
        /// </summary>
        /// <param name="tc"></param>
        [ClassInitialize()]
        public static void ClassInitialize(TestContext tc)
        {
            tc.WriteLine("In ClassInitialize() method");
            reporte_1 = new()
            {
                ID = "2000_1",
                TrimestreNatural = 1,
                TrimestreReporte = 1,
                Dia = 1,
                Mes = 1,
                Ano = 1000,
                NumUnidades = 100,
                NumUnidadesAA = 100,
                Ingresos = 100,
                IngresosAA = 100,
                UBruta = 100,
                UOperativa = 100,
                UAImpuestos = 100,
                UNeta = 100,
                UAccionistas = 100,
                UAccionistasAA = 100,
                Efectivo = 100,
                Deuda = 100,
                ActivosC = 100,
                PasivosC = 100,
                ActivosT = 100,
                PasivosT = 100,
                Capital = 100,
                CapitalAA = 100,
                FlujoOperaciones = 100,
                FlujoInversiones = 100,
                FlujoFinanciamiento = 100,
                EfectivoAlInicio = 100,
                EfectivoAlFinal = 100,
                AccionesCir = 100,
                Precio = 100,
                Dividendo = 100,
            };
            reporte_2 = new()
            {
                ID = "2000_2",
                TrimestreNatural = 2,
                TrimestreReporte = 2,
                Dia = 2,
                Mes = 2,
                Ano = 2000,
                NumUnidades = 200,
                NumUnidadesAA = 200,
                Ingresos = 200,
                IngresosAA = 200,
                UBruta = 200,
                UOperativa = 200,
                UAImpuestos = 200,
                UNeta = 200,
                UAccionistas = 200,
                UAccionistasAA = 200,
                Efectivo = 200,
                Deuda = 200,
                ActivosC = 200,
                PasivosC = 200,
                ActivosT = 200,
                PasivosT = 200,
                Capital = 200,
                CapitalAA = 200,
                FlujoOperaciones = 200,
                FlujoInversiones = 200,
                FlujoFinanciamiento = 200,
                EfectivoAlInicio = 200,
                EfectivoAlFinal = 200,
                AccionesCir = 200,
                Precio = 200,
                Dividendo = 200,
            };
            reporte_3 = new()
            {
                ID = "2000_3",
                TrimestreNatural = 3,
                TrimestreReporte = 3,
                Dia = 3,
                Mes = 3,
                Ano = 3000,
                NumUnidades = 300,
                NumUnidadesAA = 300,
                Ingresos = 300,
                IngresosAA = 300,
                UBruta = 300,
                UOperativa = 300,
                UAImpuestos = 300,
                UNeta = 300,
                UAccionistas = 300,
                UAccionistasAA = 300,
                Efectivo = 300,
                Deuda = 300,
                ActivosC = 300,
                PasivosC = 300,
                ActivosT = 300,
                PasivosT = 300,
                Capital = 300,
                CapitalAA = 300,
                FlujoOperaciones = 300,
                FlujoInversiones = 300,
                FlujoFinanciamiento = 300,
                EfectivoAlInicio = 300,
                EfectivoAlFinal = 300,
                AccionesCir = 300,
                Precio = 300,
                Dividendo = 300,
            };
            reporte_4 = new()
            {
                ID = "2000_4",
                TrimestreNatural = 4,
                TrimestreReporte = 4,
                Dia = 4,
                Mes = 4,
                Ano = 4000,
                NumUnidades = 400,
                NumUnidadesAA = 400,
                Ingresos = 400,
                IngresosAA = 400,
                UBruta = 400,
                UOperativa = 400,
                UAImpuestos = 400,
                UNeta = 400,
                UAccionistas = 400,
                UAccionistasAA = 400,
                Efectivo = 400,
                Deuda = 400,
                ActivosC = 400,
                PasivosC = 400,
                ActivosT = 400,
                PasivosT = 400,
                Capital = 400,
                CapitalAA = 400,
                FlujoOperaciones = 400,
                FlujoInversiones = 400,
                FlujoFinanciamiento = 400,
                EfectivoAlInicio = 400,
                EfectivoAlFinal = 400,
                AccionesCir = 400,
                Precio = 400,
                Dividendo = 400,
            };

            referencia1 = new()
            {
                ID = "UnoRef",
                NumUnidades = "Ref1",
                Ingresos = "Ref1",
                UBruta = "Ref1",
                UOperativa = "Ref1",
                UAImpuestos = "Ref1",
                UNeta = "Ref1",
                UAccionistas = "Ref1",
                Efectivo = "",
                Deuda = "",
                ActivosC = "",
                PasivosC = "",
                ActivosT = "",
                PasivosT = "",
                Capital = "",
                FlujoOperaciones = "Ref1",
                FlujoInversiones = "Ref1",
                FlujoFinanciamiento = "Ref1",
                EfectivoAlInicio = "Ref1",
                EfectivoAlFinal = "Ref1",
                AccionesCir = "Ref1",
                Dividendo = "Ref1",
                CostoVentasAuto = true,
                GastosFinAuto = true,
                //GastosOpAuto = true
            };
            referencia2 = new()
            {
                ID = "DosRef",
                NumUnidades = "Ref2",
                Ingresos = "",
                UBruta = "",
                UOperativa = "",
                UAImpuestos = "",
                UNeta = "",
                UAccionistas = "",
                Efectivo = "Ref2",
                Deuda = "Ref2",
                ActivosC = "Ref2",
                PasivosC = "Ref2",
                ActivosT = "Ref2",
                PasivosT = "Ref2",
                Capital = "Ref2",
                FlujoOperaciones = "",
                FlujoInversiones = "",
                FlujoFinanciamiento = "",
                EfectivoAlInicio = "",
                EfectivoAlFinal = "",
                //AccionesCir = "",
                //Dividendo = "",
                CostoVentasAuto = false,
                GastosFinAuto = false,
                //GastosOpAuto = false
            };

            empresa_empty = new()
            {
                ID = "EmptyEmp",
                //Nombre = "Empresa de Pruebas",
                //Moneda = "XXX",
                //Comentarios = "Empresa ficticia para pruebas unitarias SIN reportes"
            };
            empresa_full = new()
            {
                ID = "FullEmp",
                Nombre = "Empresa de Pruebas",
                Moneda = "XXX",
                Comentarios = "Empresa ficticia para pruebas unitarias CON 4 reportes",
                Reportes = new ObservableCollection<Reporte>() { reporte_1, reporte_2, reporte_3, reporte_4 },
                Referencias = new ObservableCollection<Referencia>() { referencia1, referencia2 }
            };
        }

        /// <summary>
        /// Realiza la limpieza depués de la ejecución de TODAS las pruebas unitarias de esta clase
        /// </summary>
        [ClassCleanup()]
        public static void ClassCleanup()
        {
        }
        #endregion

        #region INITIALIZE & CLEANUP TEST
        /// <summary>
        /// Realiza la inicialización antes de la ejecución de CADA prueba unitaria de esta clase
        /// </summary>
        [TestInitialize]
        public void TestInitialize()
        {
            WriteDescription(this.GetType());
            repository = new XMLRepository(XML_FILE_PATH);
        }

        /// <summary>
        /// Realiza la limpieza depués de la ejecución de CADA prueba unitaria de esta clase
        /// </summary>
        [TestCleanup]
        public void TestCleanup()
        {
        }
        #endregion

        #region GET Test
        [TestMethod]
        [Description("Obtener todas las empresas")]
        [TestCategory("NoException")]
        //[Ignore]
        public void GetAllCompanies()
        {
            IEnumerable<Empresa> companies = repository.GetAllCompanies();
            if (companies == null)
            {
                TestContext.WriteLine("El XML no contiene empresas");
            }
            else
            {
                TestContext.WriteLine($"Hay {companies.Count()} empresas en el XML");
            }
        }

        [TestMethod]
        [Description("Obtener todos los reportes de una empresa especificada")]
        [TestCategory("NoException")]
        [DataRow("ConReportes")]
        [DataRow("SinReportes")]
        [DataRow("Vacio")]
        [DataRow("Invalido")]
        //[Ignore]
        public void GetAllReports(string idEmpresa)
        {
            IEnumerable<Reporte> reportes = repository.GetAllReports(idEmpresa);

            int count = 0;
            if (reportes is not null)
            {
                TestContext.WriteLine($"La empresa {idEmpresa} contiene {reportes.Count()} reportes en el XML");
                count = reportes.Count();
            }
            else
            {
                TestContext.WriteLine($"La empresa {idEmpresa} no existe o no contiene reportes");
            }

            if (idEmpresa == "ConReportes") { Assert.AreEqual(2, count); }
            else { Assert.AreEqual(0, count); }
        }

        [TestMethod]
        [Description("Obtener una empresa en base a un ID")]
        [TestCategory("NoException")]
        [DataRow("ConReportes")]
        [DataRow("SinReportes")]
        [DataRow("Vacio")]
        [DataRow("Invalido")]
        //[Ignore]
        public void GetCompanyByID(string id)
        {
            Empresa company = repository.GetCompany(id);
            if (company == null)
            {
                TestContext.WriteLine($"El XML no contiene la empresa {id}");
            }
            else
            {
                Assert.IsInstanceOfType(company, typeof(IEntity));
                TestContext.WriteLine($"ID={company.ID}; Nombre={company.Nombre}; No.Reportes={company.Reportes.Count}");
            }
        }

        [TestMethod]
        [Description("Obtener un Reporte de una empresa en base a sus IDs")]
        [TestCategory("NoException")]
        [DataRow("ConReportes", "2020_1")]
        [DataRow("ConReportes", "Invalido")]
        [DataRow("SinReportes", "2020_1")]
        [DataRow("Vacio", "2020_1")]
        [DataRow("Invalido", "2020_1")]
        //[Ignore]
        public void GetReportByID(string idEmpresa, string idReporte)
        {
            Reporte reporte = repository.GetReport(idEmpresa, idReporte);
            if (reporte == null)
            {
                TestContext.WriteLine($"El XML no contiene el reporte {idReporte} de la empresa {idEmpresa}");
            }
            else
            {
                Assert.IsInstanceOfType(reporte, typeof(IEntity));
                TestContext.WriteLine(
                    $"ID Empresa={idEmpresa}{Environment.NewLine}" +
                    $"ID Reporte={reporte.ID}; Día={reporte.Dia}; Mes={reporte.Mes}; Año={reporte.Ano}");
            }
        }

        [TestMethod]
        [Description("Obtener una Referencia de una empresa en base a sus IDs")]
        [TestCategory("NoException")]
        [DataRow("ConReportes", "Ref1")]
        [DataRow("ConReportes", "Invalido")]
        [DataRow("SinReportes", "Ref1")]
        [DataRow("Vacio", "Ref1")]
        [DataRow("Invalido", "Ref1")]
        //[Ignore]
        public void GetReferenciaByID(string idEmpresa, string idReferencia)
        {
            Referencia referencia = repository.GetReferencia(idEmpresa, idReferencia);
            if (referencia == null)
            {
                TestContext.WriteLine($"El XML no contiene la referencia {idReferencia} de la empresa {idEmpresa}");
            }
            else
            {
                Assert.IsInstanceOfType(referencia, typeof(IEntity));
                TestContext.WriteLine(
                    $"ID Empresa={idEmpresa}{Environment.NewLine}" +
                    $"ID Referencia={referencia.ID}; NumUnidades={referencia.NumUnidades}; Ingresos={referencia.Ingresos}");
            }
        }
        #endregion
        #region ADD Test
        [TestMethod]
        [Description("Agregar una nueva empresa")]
        [TestCategory("NoException")]
        //[Ignore]
        public void AddCompany()
        {
            Assert.IsNull(repository.GetCompany(empresa_full.ID), $"La empresa {empresa_full.ID} ya existe, no es posible ejecutar esta prueba");
            Assert.IsNull(repository.GetCompany(empresa_empty.ID), $"La empresa {empresa_empty.ID} ya existe, no es posible ejecutar esta prueba");

            repository.AddCompany(empresa_full);
            repository.AddCompany(empresa_empty);

            Assert.IsNotNull(repository.GetCompany(empresa_full.ID), $"La empresa {empresa_full.ID} no se agrego");
            Assert.IsNotNull(repository.GetCompany(empresa_empty.ID), $"La empresa {empresa_empty.ID} no se agrego");
        }

        [TestMethod]
        [Description("Lanzar error cuando se agregar una empresa que ya existe")]
        [TestCategory("Exception")]
        [ExpectedException(typeof(ArgumentException), "Se esperaba un error cuando se intenta agregar una empresa que ya existe")]
        //[Ignore]
        public void AddExistingCompany()
        {
            repository.AddCompany(empresa_full);
            repository.AddCompany(empresa_full);
        }

        [TestMethod]
        [Description("Agregar un Reporte a una empresa especifica")]
        [TestCategory("NoException")]
        [DataRow("ConReportes")]
        [DataRow("SinReportes")]
        [DataRow("Vacio")]
        //[Ignore]
        public void AddReport(string idEmpresa)
        {
            Assert.IsNull(repository.GetReport(idEmpresa, reporte_1.ID), $"El reporte {reporte_1.ID} ya existe, no es posible ejecutar esta prueba");
            Assert.IsNull(repository.GetReport(idEmpresa, reporte_2.ID), $"El reporte {reporte_2.ID} ya existe, no es posible ejecutar esta prueba");
            Assert.IsNull(repository.GetReport(idEmpresa, reporte_3.ID), $"El reporte {reporte_3.ID} ya existe, no es posible ejecutar esta prueba");
            Assert.IsNull(repository.GetReport(idEmpresa, reporte_4.ID), $"El reporte {reporte_4.ID} ya existe, no es posible ejecutar esta prueba");

            repository.AddReport(idEmpresa, reporte_4);
            repository.AddReport(idEmpresa, reporte_1);
            repository.AddReport(idEmpresa, reporte_3);
            repository.AddReport(idEmpresa, reporte_2);

            Assert.IsNotNull(repository.GetReport(idEmpresa, reporte_1.ID), $"El reporte {reporte_1.ID} no se agrego");
            Assert.IsNotNull(repository.GetReport(idEmpresa, reporte_2.ID), $"El reporte {reporte_2.ID} no se agrego");
            Assert.IsNotNull(repository.GetReport(idEmpresa, reporte_3.ID), $"El reporte {reporte_3.ID} no se agrego");
            Assert.IsNotNull(repository.GetReport(idEmpresa, reporte_4.ID), $"El reporte {reporte_4.ID} no se agrego");
        }

        [TestMethod]
        [Description("Lanzar error cuando se agregar un Reporte que ya existe")]
        [TestCategory("Exception")]
        [DataRow("Vacio")]
        [ExpectedException(typeof(ArgumentException), "Se esperaba un error cuando se intenta agregar un reporte que ya existe")]
        //[Ignore]
        public void AddExistingReport(string idEmpresa)
        {
            repository.AddReport(idEmpresa, reporte_1);
            repository.AddReport(idEmpresa, reporte_1);
        }

        [TestMethod]
        [Description("Lanzar error cuando se desea agregar un reporte a una empresa que NO existente")]
        [TestCategory("Exception")]
        [DataRow("Invalido")]
        [ExpectedException(typeof(ArgumentException), "Se esperaba un error cuando se intenta agregar un reporte a una empresa que NO existe")]
        //[Ignore]
        public void AddReportToInvalidCompany(string idEmpresa)
        {
            repository.AddReport(idEmpresa, reporte_1);
        }

        [TestMethod]
        [Description("Agregar una Referencia a una empresa especifica")]
        [TestCategory("NoException")]
        [DataRow("ConReportes")]
        [DataRow("SinReportes")]
        [DataRow("Vacio")]
        //[Ignore]
        public void AddReferencia(string idEmpresa)
        {
            Assert.IsNull(repository.GetReferencia(idEmpresa, referencia1.ID), $"La Referencia {referencia1.ID} ya existe, no es posible ejecutar esta prueba");
            Assert.IsNull(repository.GetReferencia(idEmpresa, referencia2.ID), $"La Referencia {referencia2.ID} ya existe, no es posible ejecutar esta prueba");

            repository.AddReferencia(idEmpresa, referencia2);
            repository.AddReferencia(idEmpresa, referencia1);

            Assert.IsNotNull(repository.GetReferencia(idEmpresa, referencia1.ID), $"La Referencia {referencia1.ID} no se agrego");
            Assert.IsNotNull(repository.GetReferencia(idEmpresa, referencia2.ID), $"La Referencia {referencia2.ID} no se agrego");
        }

        [TestMethod]
        [Description("Lanzar error cuando se agregar una Referencia que ya existe")]
        [TestCategory("Exception")]
        [DataRow("Vacio")]
        [ExpectedException(typeof(ArgumentException), "Se esperaba un error cuando se intenta agregar una referencia que ya existe")]
        //[Ignore]
        public void AddExistingReferencia(string idEmpresa)
        {
            repository.AddReferencia(idEmpresa, referencia1);
            repository.AddReferencia(idEmpresa, referencia1);
        }

        [TestMethod]
        [Description("Lanzar error cuando se desea agregar una referencia a una empresa que NO existente")]
        [TestCategory("Exception")]
        [DataRow("Invalido")]
        [ExpectedException(typeof(ArgumentException), "Se esperaba un error cuando se intenta agregar una referencia a una empresa que NO existe")]
        //[Ignore]
        public void AddReferenciaToInvalidCompany(string idEmpresa)
        {
            repository.AddReferencia(idEmpresa, referencia1);
        }
        #endregion
        #region DELETE Test
        [TestMethod]
        [Description("Eliminar una empresa")]
        [TestCategory("NoException")]
        [DataRow("ConReportes")]
        [DataRow("SinReportes")]
        [DataRow("Vacio")]
        [DataRow("Invalido")]
        //[Ignore]
        public void DeleteCompany(string idEmpresa)
        {
            repository.DeleteCompany(idEmpresa);
            Assert.IsNull(repository.GetCompany(idEmpresa), $"La empresa {idEmpresa} no se elimino");
        }

        [TestMethod]
        [Description("Eliminar un reporte de una empresa")]
        [TestCategory("NoException")]
        [DataRow("ConReportes", "2020_1")]
        [DataRow("SinReportes", "2020_1")]
        [DataRow("Vacio", "2020_1")]
        [DataRow("Invalido", "2020_1")]
        //[Ignore]
        public void DeleteReport(string idEmpresa, string idReporte)
        {
            repository.DeleteReport(idEmpresa, idReporte);
            Assert.IsNull(repository.GetReport(idEmpresa, idReporte), $"El reporte {idReporte} no se elimino");
        }

        [TestMethod]
        [Description("Eliminar una referencia de una empresa")]
        [TestCategory("NoException")]
        [DataRow("ConReportes", "Ref1")]
        [DataRow("SinReportes", "Ref1")]
        [DataRow("Vacio", "Ref1")]
        [DataRow("Invalido", "Ref1")]
        //[Ignore]
        public void DeleteReferencia(string idEmpresa, string idReferencia)
        {
            repository.DeleteReferencia(idEmpresa, idReferencia);
            Assert.IsNull(repository.GetReferencia(idEmpresa, idReferencia), $"La referencia {idReferencia} no se elimino");
        }
        #endregion
        #region UPDATE Test
        [TestMethod]
        [Description("Actualizar una empresa existente")]
        [TestCategory("NoException")]
        [DataRow("ConReportes")]
        [DataRow("SinReportes")]
        [DataRow("Vacio")]
        //[Ignore]
        public void UpdateValidCompany(string idEmpresa)
        {
            Empresa emp = repository.GetCompany(idEmpresa);
            int repCount = emp.Reportes.Count;
            int refCount = emp.Referencias.Count;

            empresa_empty.ID = idEmpresa;
            repository.UpdateCompany(empresa_empty);
            emp = repository.GetCompany(idEmpresa);
            Assert.AreEqual(empresa_empty.Nombre, emp.Nombre);
            Assert.AreEqual(empresa_empty.Moneda, emp.Moneda);
            Assert.AreEqual(empresa_empty.Comentarios, emp.Comentarios);
            Assert.AreEqual(repCount, emp.Reportes.Count);
            Assert.AreEqual(refCount, emp.Referencias.Count);
            //Assert.IsTrue(emp.Reportes.Count == 0);

            empresa_full.ID = idEmpresa;
            repository.UpdateCompany(empresa_full);
            emp = repository.GetCompany(idEmpresa);
            Assert.AreEqual(empresa_full.Nombre, emp.Nombre);
            Assert.AreEqual(empresa_full.Moneda, emp.Moneda);
            Assert.AreEqual(empresa_full.Comentarios, emp.Comentarios);
            Assert.AreEqual(repCount, emp.Reportes.Count);
            Assert.AreEqual(refCount, emp.Referencias.Count);
            //Assert.IsTrue(emp.Reportes.Count > 0);
        }

        [TestMethod]
        [Description("Lanzar error cuando se desea Actualizar una empresa inexistente")]
        [TestCategory("Exception")]
        [DataRow("Invalido")]
        [ExpectedException(typeof(ArgumentException), "Se esperaba un error cuando se intenta actualizar una empresa que NO existe")]
        //[Ignore]
        public void UpdateInvalidCompany(string idEmpresa)
        {
            empresa_empty.ID = idEmpresa;
            repository.UpdateCompany(empresa_empty);
        }

        [TestMethod]
        [Description("Actualizar un reporte existente de una empresa")]
        [TestCategory("NoException")]
        [DataRow("ConReportes")]
        //[Ignore]
        public void UpdateValidReport(string idEmpresa)
        {
            Reporte rep;

            reporte_1.ID = "2020_1";
            repository.UpdateReport(idEmpresa, reporte_1);
            rep = repository.GetReport(idEmpresa, reporte_1.ID);
            Assert.AreEqual(reporte_1.Ano, rep.Ano);
            Assert.IsNotNull(rep.NumUnidades);

            Reporte reporteBasico = new()
            {
                ID = "4000_3",          // No es necesario, se sobreescribe al poner fecha
                TrimestreNatural = 4,   // No es necesario, se sobreescribe al poner fecha
                TrimestreReporte = 4,
                Dia = 4,
                Mes = 4,
                Ano = 2020
            };
            repository.UpdateReport(idEmpresa, reporteBasico);
            rep = repository.GetReport(idEmpresa, reporteBasico.ID);
            Assert.AreEqual(reporteBasico.Ano, rep.Ano);
            Assert.AreEqual(0, rep.NumUnidades);
        }

        [TestMethod]
        [Description("Lanzar error cuando se desea Actualizar un reporte Inexistente")]
        [TestCategory("Exception")]
        [DataRow("SinReportes")]
        [DataRow("Vacio")]
        [DataRow("Invalido")]
        [ExpectedException(typeof(ArgumentException), "Se esperaba un error cuando se intenta actualizar un reporte que NO existe")]
        //[Ignore]
        public void UpdateInvalidReport(string idEmpresa)
        {
            reporte_1.ID = "2020_X";
            repository.UpdateReport(idEmpresa, reporte_1);
        }





        [TestMethod]
        [Description("Actualizar una referencia existente de una empresa")]
        [TestCategory("NoException")]
        [DataRow("ConReportes")]
        //[Ignore]
        public void UpdateValidReferencia(string idEmpresa)
        {
            Referencia referencia;

            referencia1.ID = "Ref1";
            repository.UpdateReferencia(idEmpresa, referencia1);
            referencia = repository.GetReferencia(idEmpresa, referencia1.ID);
            Assert.AreEqual(referencia1.NumUnidades, referencia.NumUnidades);
            Assert.AreEqual(referencia1.Efectivo, referencia.Efectivo);
            Assert.AreEqual(referencia1.CostoVentasAuto, referencia.CostoVentasAuto);
            Assert.AreEqual(referencia1.GastosOpAuto, referencia.GastosOpAuto);

            Referencia referenciaBasico = new()
            {
                ID = "Ref2"
            };
            repository.UpdateReferencia(idEmpresa, referenciaBasico);
            referencia = repository.GetReferencia(idEmpresa, referenciaBasico.ID);
            Assert.AreEqual(referenciaBasico.NumUnidades, referencia.NumUnidades);
            Assert.AreEqual(referenciaBasico.Efectivo, referencia.Efectivo);
            Assert.AreEqual(referenciaBasico.CostoVentasAuto, referencia.CostoVentasAuto);
            Assert.AreEqual(referenciaBasico.GastosOpAuto, referencia.GastosOpAuto);
        }

        [TestMethod]
        [Description("Lanzar error cuando se desea Actualizar una referencia Inexistente")]
        [TestCategory("Exception")]
        [DataRow("SinReportes")]
        [DataRow("Vacio")]
        [DataRow("Invalido")]
        [ExpectedException(typeof(ArgumentException), "Se esperaba un error cuando se intenta actualizar una referencia que NO existe")]
        //[Ignore]
        public void UpdateInvalidReferencia(string idEmpresa)
        {
            referencia1.ID = "Ref_X";
            repository.UpdateReferencia(idEmpresa, referencia1);
        }
        #endregion

        [TestMethod]
        [Description("Guardar los cambios realizados")]
        [TestCategory("NoException")]
        //[Ignore]
        public void SubmitChanges()
        {
            Assert.IsNull(repository.GetCompany(empresa_full.ID), $"La empresa {empresa_full.ID} ya existe, no es posible ejecutar esta prueba");
            Assert.IsNull(repository.GetCompany(empresa_empty.ID), $"La empresa {empresa_empty.ID} ya existe, no es posible ejecutar esta prueba");
            repository.AddCompany(empresa_full);
            repository.AddCompany(empresa_empty);

            repository.DeleteCompany("ConReportes");
            repository.DeleteCompany("Vacio");

            empresa_full.ID = "SinReportes";
            repository.UpdateCompany(empresa_full);

            repository.SubmitChanges(XML_FILE_SAVE);

            XMLRepository repository2 = new(XML_FILE_SAVE);
            Assert.IsNotNull(repository2.GetCompany(empresa_full.ID), $"La empresa {empresa_full.ID} no se agrego");
            Assert.IsNotNull(repository2.GetCompany(empresa_empty.ID), $"La empresa {empresa_empty.ID} no se agrego");

            Assert.IsNull(repository2.GetCompany("ConReportes"), $"La empresa 'ConReportes' no se eliminó");
            Assert.IsNull(repository2.GetCompany("Vacio"), $"La empresa 'Vacio' no se eliminó");

            Assert.AreEqual(empresa_full.Comentarios, repository2.GetCompany(empresa_full.ID).Comentarios);
        }
    }
}
