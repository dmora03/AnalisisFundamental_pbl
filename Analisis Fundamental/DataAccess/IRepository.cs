/*********************************************************************/
/***** LLENAR ESTA INTERFAZ CONFORME A LA NECESIDAD DEL PROYECTO *****/
/*        Hay que agregar metodos o renombrar los existentes         */
/*********************************************************************/
using Models;
using System.Collections.Generic;
using System;

namespace DataAccess
{
    ///// <summary>
    ///// Define metodos genericos para leer y escribir datos
    ///// </summary>
    //public interface IRepository
    //{
    //    #region Read
    //    /// <summary>
    //    /// Obtiene todas las empresas registadas XXX
    //    /// </summary>
    //    /// <returns>Una colleccion de todas las Empresa</returns>
    //    IEnumerable<Empresa> GetAllCompanies();
    //    /// <summary>
    //    /// Obtiene todos los reportes de una empresa especificada
    //    /// </summary>
    //    /// <param name="idEmpresa">El ID de la empresa de la que se quiere obtener sus reportes</param>
    //    /// <returns>Una colección de todos los reportes de la empresa especificada</returns>
    //    IEnumerable<Reporte> GetAllReports(string idEmpresa);

    //    /// <summary>
    //    /// Obtiene la empresa que tenga el ID especificado
    //    /// </summary>
    //    /// <param name="idEmpresa">El ID de la empresa a encontrar</param>
    //    /// <returns>La <see cref="Empresa"/> con el ID deseado</returns>
    //    Empresa GetCompany(string idEmpresa);
    //    /// <summary>
    //    /// Obtiene el Reporte especificado por el ID del reporte y el ID de la empresa deseada
    //    /// si no existe regresa NULL
    //    /// </summary>
    //    /// <param name="idEmpresa">ID del reporte deseado</param>
    //    /// <param name="idReporte">ID de la empresa donde esta el reporte</param>
    //    /// <returns>El <see cref="Reporte"/> deseado o <see cref="Nullable"/> si no existe</returns>
    //    Reporte GetReport(string idEmpresa, string idReporte);
    //    #endregion

    //    #region Write
    //    /// <summary>
    //    /// Agrega la empresa especificada
    //    /// </summary>
    //    /// <param name="empresa">La empresa que se agregará</param>
    //    /// <exception cref="ArgumentException">Lanzado cuando la empresa a agregar ya existe</exception>
    //    void AddCompany(Empresa empresa);
    //    /// <summary>
    //    /// Agrega un reporte a una empresa especificada
    //    /// </summary>
    //    /// <param name="idEmpresa">El ID de la empresa que se le agregarea el reporte</param>
    //    /// <param name="reporte">El reporte que se agregará</param>
    //    /// <exception cref="ArgumentException">Lanzado cuando la empresa no existe o el reporte a agregar ya existe</exception>
    //    void AddReport(string idEmpresa, Reporte reporte);

    //    /// <summary>
    //    /// Actualiza una empresa
    //    /// </summary>
    //    /// <param name="empresa">La empresa que se actualizará en la base de datos</param>
    //    /// <exception cref="ArgumentException">Lanzado cuando la empresa a actualizar NO existe</exception>
    //    void UpdateCompany(Empresa empresa);
    //    /// <summary>
    //    /// Actualiza un reporte dado en la empresa especificada
    //    /// </summary>
    //    /// <param name="idEmpresa">El ID de la empresa</param>
    //    /// <param name="reporte">El reporte que se actualizará en la base de datos</param>
    //    /// <exception cref="ArgumentException">Lanzado cuando el reporte a actualizar NO existe o la empresa NO existe</exception>
    //    void UpdateReport(string idEmpresa, Reporte reporte);

    //    /// <summary>
    //    /// Eliminar la empresa con el ID especificado
    //    /// </summary>
    //    /// <param name="idEmpresa">ID de la empresa a eliminar</param>
    //    void DeleteCompany(string idEmpresa);
    //    /// <summary>
    //    /// Elimina un reporte de una empresa especificada
    //    /// </summary>
    //    /// <param name="idEmpresa"></param>
    //    /// <param name="idReporte"></param>
    //    void DeleteReport(string idEmpresa, string idReporte);
    //    /// <summary>
    //    /// Guarda todos los cambios hechos
    //    /// </summary>
    //    void SubmitChanges();
    //    #endregion
    //}

    /// <summary>
    /// Define metodos genericos para leer y escribir datos
    /// </summary>
    /// <typeparam name="T">El Tipo de objeto de Dato</typeparam>
    /// <typeparam name="TKey">El Tipo del ID</typeparam>
    public interface IRepository<T, TKey> : IReadRepository<T, TKey>, IWriteRepository<T, TKey> 
    {
        void SubmitChanges();
    }

    /// <summary>
    /// Define metodos genericos para leer datos
    /// Covariante: Inidca que se puede usar un Tipo MENOS específico [Ej. IRead<OBJECT> repo = new Data<Empleado>()]
    /// </summary>
    /// <typeparam name="T">El Tipo de objeto de Dato (Covariante)</typeparam>
    /// <typeparam name="TKey">El Tipo del ID</typeparam>
    public interface IReadRepository<out T, TKey>
    {// Curso: Pluralsight => C# Generics -> 4. Working with Generic Interfaces: Understand Covariance

        /// <summary>
        /// Obtiene todas las empresas registadas
        /// </summary>
        /// <returns>Una colleccion de todas las Empresa</returns>
        IEnumerable<T> GetAllCompanies();
        /// <summary>
        /// Obtiene todos los reportes de una empresa especificada
        /// </summary>
        /// <param name="idEmpresa">El ID de la empresa de la que se quiere obtener sus reportes</param>
        /// <returns>Una colección de todos los reportes de la empresa especificada</returns>
        IEnumerable<Reporte> GetAllReports(TKey idEmpresa);

        /// <summary>
        /// Obtiene la empresa que tenga el ID especificado
        /// </summary>
        /// <param name="id">El ID de la empresa a encontrar</param>
        /// <returns>La <see cref="T"/> con el ID deseado</returns>
        T? GetCompany(TKey id);
        /// <summary>
        /// Obtiene el Reporte especificado por el ID del reporte y el ID de la empresa deseada
        /// si no existe regresa NULL
        /// </summary>
        /// <param name="idEmpresa">ID de la empresa donde esta el reporte</param>
        /// <param name="idReporte">ID del reporte deseado</param>
        /// <returns>El <see cref="Reporte"/> deseado o <see cref="Nullable"/> si no existe</returns>
        Reporte? GetReport(TKey idEmpresa, TKey idReporte);

        /// <summary>
        /// Obtiene la Referencia especificada por el ID de la referencia y el ID de la empresa deseada
        /// si no existe regresa NULL
        /// </summary>
        /// <param name="idEmpresa">ID de la empresa donde esta el reporte</param>
        /// <param name="idReferencia">ID de la referencia deseada</param>
        /// <returns>La <see cref="Referencia"/> deseada o <see cref="Nullable"/> si no existe</returns>
        Referencia? GetReferencia(TKey idEmpresa, TKey idReferencia);
    }

    /// <summary>
    /// Define metodos genericos para escribir datos
    /// Contravariante: Inidca que se puede usar un Tipo MAS específico [Ej. IWrite<MANAGER> repo = new Data<Empleado>()]
    /// </summary>
    /// <typeparam name="T">El Tipo de objeto de Dato (Contravariante)</typeparam>
    /// <typeparam name="TKey">El Tipo del ID</typeparam>
    public interface IWriteRepository<in T, TKey>
    {// Curso: Pluralsight => C# Generics -> 4. Working with Generic Interfaces: Understand Contravariance

        /// <summary>
        /// Agrega la empresa especificada
        /// </summary>
        /// <param name="empresa">La empresa que se agregará</param>
        /// <exception cref="ArgumentException">Lanzado cuando la empresa a agregar ya existe</exception>
        void AddCompany(T empresa);
        /// <summary>
        /// Agrega un reporte a una empresa especificada
        /// </summary>
        /// <param name="idEmpresa">El ID de la empresa que se le agregarea el reporte</param>
        /// <param name="reporte">El reporte que se agregará</param>
        /// <exception cref="ArgumentException">Lanzado cuando la empresa no existe o el reporte a agregar ya existe</exception>
        void AddReport(TKey idEmpresa, Reporte reporte);

        /// <summary>
        /// Agrega una referencia a una empresa especificada
        /// </summary>
        /// <param name="idEmpresa">El ID de la empresa que se le agregara la referencia</param>
        /// <param name="referencia">La referencia que se agregará</param>
        /// <exception cref="ArgumentException">Lanzado cuando la empresa no existe o la referencia a agregar ya existe</exception>
        void AddReferencia(TKey idEmpresa, Referencia referencia);

        /// <summary>
        /// Actualiza SOLO el Nombre, la Moneda y los Comentarios de una empresa
        /// </summary>
        /// <param name="empresa">La empresa que se actualizará en la base de datos</param>
        /// <exception cref="ArgumentException">Lanzado cuando la empresa a actualizar NO existe</exception>
        void UpdateCompany(T empresa);
        /// <summary>
        /// Actualiza un reporte dado en la empresa especificada
        /// </summary>
        /// <param name="idEmpresa">El ID de la empresa</param>
        /// <param name="reporte">El reporte que se actualizará en la base de datos</param>
        /// <exception cref="ArgumentException">Lanzado cuando el reporte a actualizar NO existe o la empresa NO existe</exception>
        void UpdateReport(TKey idEmpresa, Reporte reporte);

        /// <summary>
        /// Actualiza una referencia dada en la empresa especificada
        /// </summary>
        /// <param name="idEmpresa">El ID de la empresa</param>
        /// <param name="referencia">La referencia que se actualizará en la base de datos</param>
        /// <exception cref="ArgumentException">Lanzado cuando la referencia a actualizar NO existe o la empresa NO existe</exception>
        void UpdateReferencia(string idEmpresa, Referencia referencia);

        /// <summary>
        /// Eliminar la empresa con el ID especificado
        /// </summary>
        /// <param name="id">ID de la empresa a eliminar</param>
        void DeleteCompany(TKey id);
        /// <summary>
        /// Elimina un reporte de una empresa especificada
        /// </summary>
        /// <param name="idEmpresa">ID de la empresa</param>
        /// <param name="idReporte">ID del reporte a eliminar</param>
        void DeleteReport(TKey idEmpresa, TKey idReporte);

        /// <summary>
        /// Elimina una referencia de una empresa especificada
        /// </summary>
        /// <param name="idEmpresa">ID de la empresa</param>
        /// <param name="idReferencia">ID de la referencia a eliminar</param>
        void DeleteReferencia(TKey idEmpresa, TKey idReferencia);
    }
}