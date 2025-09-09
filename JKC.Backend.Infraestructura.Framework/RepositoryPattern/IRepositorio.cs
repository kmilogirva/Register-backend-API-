using JKC.Backend.Dominio.Entidades.Seguridad.Usuarios;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace JKC.Backend.Infraestructura.Framework.RepositoryPattern
{
  public interface IRepository<T> where T : class
  {
    Task<T> ObtenerPorId(int? id);
    Task<T> ObtenerPorIdInclude(int id, params Expression<Func<T, object>>[] includes);
    Task<List<T>> ObtenerTodos();
    Task<List<T>> ObtenerAsync(
        Expression<Func<T, bool>> filtro = null,
        Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
        Func<IQueryable<T>, IQueryable<T>> incluir = null,
        bool asNoTracking = true
    );

    Task Crear(T entidad);
    Task Actualizar(T entidad);
    Task Eliminar(T entidad);
    Task EliminarPorId(int id);
    Task<bool> AnyAsync(Expression<Func<T, bool>> predicado);

    //void ExecuteSqlCommand(string procedureName, object parameteres);
    DataSet ExecuteStoreProcedure(string sqlQuery, List<DbParameter> parameters);
    //IEnumerable<TEntityVO> ExecuteStoreProcedure<TEntityVO>(string procedureName, object parameters);
    Task<IEnumerable<T>> EjecutarProcedimientoAlmacenado<T>(string storedProcedure, params object[] parametros);

    Task<List<T>> ObtenerTodosInclude(params Expression<Func<T, object>>[] includes);
  }
}
