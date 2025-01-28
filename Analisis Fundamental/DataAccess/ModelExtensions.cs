using Models;

namespace DataAccess
{
    /// <summary>
    /// Clase estática con funciones de extension para el Modelo
    /// </summary>
    public static class ModelExtensions
    {
        /// <summary>
        /// Construye el ID de un número dado de trimestres antes o despues a este
        /// </summary>
        /// <param name="reporte">Reporte del que se parte (Actual)</param>
        /// <param name="numTrim">Número de trimestras a avanzar (Positivo) o retroceder (Negativo)</param>
        /// <returns>ID del trimestre buscado, no es garantia que exista en la base de datos, validar ID antes de usarlo</returns>
        public static string TrimestreIDOffset(this Reporte reporte, int numTrim)
        {
            int anoOffset = reporte.Ano + (numTrim / 4);
            int trimOffset = reporte.TrimestreNatural + (numTrim % 4);

            if (trimOffset > 4)
            {
                trimOffset -= 4;
                anoOffset++;
            }
            else if (trimOffset < 1)
            {
                trimOffset += 4;
                anoOffset--;
            }
            return $"{anoOffset}_{trimOffset}";
        }
    }
}
