using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests
{
    [TestClass]
    public class TestInitialization
    {
        /// <summary>
        /// Configura los recursos para todas las clases de prueba de este proyecto
        /// Es lo primero que se ejecuta en el proyecto
        /// </summary>
        /// <param name="tc"></param>
        [AssemblyInitialize()]
        public static void AssemblyInitialize(TestContext tc)
        {//Initialize for all tests within an assembly
            tc.WriteLine("In AssemblyInitialize() method");
        }

        /// <summary>
        /// Se ejecuta al finalizar todas las pruebas del proyecto
        /// </summary>
        [AssemblyCleanup()]
        public static void AssemblyCleanup()
        {// Clean up after all tests in an assembly
        }
    }
}
