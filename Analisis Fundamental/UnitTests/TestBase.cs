using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Reflection;

namespace UnitTests
{
    public class TestBase
    {
        public TestContext TestContext { get; set; }

        /// <summary>
        /// Escribe en el Trace la descripción del método de prueba que se esta ejecutando acutalmente
        /// (solo si este tiene el atributo [Description])
        /// </summary>
        /// <param name="typ"></param>
        protected void WriteDescription(Type typ)
        {
            string testName = TestContext.TestName;

            // Encuentra el método de prueba que se esta ejecutando actualmente
            MemberInfo method = typ.GetMethod(testName);
            if (method != null)
            {
                // Checa si el atributo [Description] existe en esta prueba
                Attribute attr = method.GetCustomAttribute(typeof(DescriptionAttribute));
                if (attr != null)
                {
                    DescriptionAttribute dattr = (DescriptionAttribute)attr;
                    // Despliega la descripcion de la prueba
                    TestContext.WriteLine("Test Description: " + dattr.Description);
                }
            }
        }
    }
}
