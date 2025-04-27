using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace JKC.Backend.Infraestructura.Framework.RepositoryPattern
{
  public interface IRepository<T> where T : class
  {
    Task<T> ObtenerPorId(int id);
    IQueryable<T> ObtenerTodos();
    Task Crear(T entidad);
    Task Actualizar(T entidad);
    Task Eliminar(T entidad);
    Task EliminarPorId(int id);

    //void ExecuteSqlCommand(string procedureName, object parameteres);
    DataSet ExecuteStoreProcedure(string sqlQuery, List<DbParameter> parameters);
    //IEnumerable<TEntityVO> ExecuteStoreProcedure<TEntityVO>(string procedureName, object parameters);
    Task<IEnumerable<T>> EjecutarProcedimientoAlmacenado<T>(string storedProcedure, params object[] parametros);

  }
}
