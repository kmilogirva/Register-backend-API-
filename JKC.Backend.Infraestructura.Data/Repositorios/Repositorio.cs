using JKC.Backend.Infraestructura.Data.EntityFramework;
using JKC.Backend.Infraestructura.Framework.RepositoryPattern;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;
using System.Data;
using System.Data.SqlClient;

namespace JKC.Backend.Infraestructura.Data.Repositorios
{
  public class Repository<T> : IRepository<T> where T : class
  {
    private readonly DbContext _context;

    public Repository(CommonDBContext context)
    {
      _context = context;
    }

    public async Task<T> ObtenerPorId(int id)
    {
      try { 
      return await _context.Set<T>().FindAsync(id);
      }
      catch (Exception ex)
      {
        // Manejo de excepciones (puedes registrar el error o lanzar una excepción personalizada)
        // Logger.LogError(ex, "Error al obtener todos los registros");
        throw new Exception("Error al obtener los registros", ex);
      }
    }

    public IQueryable<T> ObtenerTodos()
    {
      try
      {
        return _context.Set<T>().AsQueryable();
      }
      catch (Exception ex)
      {
        // Manejo de excepciones (puedes registrar el error o lanzar una excepción personalizada)
        // Logger.LogError(ex, "Error al obtener todos los registros");
        throw new Exception("Error al obtener los registros", ex);
      }
    }

    public async Task Crear(T entidad)
    {
      try
      {
        await _context.Set<T>().AddAsync(entidad);
        await _context.SaveChangesAsync();
      }
      catch (DbUpdateException dbEx)
      {
        // Error específico de base de datos
        throw new Exception("Error al guardar los cambios en la base de datos.", dbEx);
      }
      catch (Exception ex)
      {
        // Cualquier otro error general
        throw new Exception("Ocurrió un error al crear la entidad.", ex);
      }
    }


    public async Task Actualizar(T entidad)
    {
      _context.Set<T>().Update(entidad);
      await _context.SaveChangesAsync();
    }

    public async Task Eliminar(T entidad)
    {
      _context.Set<T>().Remove(entidad);
      await _context.SaveChangesAsync();
    }

    public async Task EliminarPorId(int id)
    {
      var entidad = await _context.Set<T>().FindAsync(id);
      _context.Set<T>().Remove(entidad);
      await _context.SaveChangesAsync();
    }


    //Para usar procedimientos almacenado y devolver listas.
    //Para ejecutar este tipo de Stored procedure es necesario que la entidad coincida y exista en mi Base de Datos.
    public async Task<IEnumerable<T>> EjecutarProcedimientoAlmacenado<T>(string storedProcedure, params object[] parametros)
    {
      try
      {
        //return await _context.Set<T>().FromSqlInterpolated($"{storedProcedure} {string.Join(", ", parametros)}").ToListAsync();
        var query = $"{storedProcedure} {string.Join(", ", parametros)}";
        return await _context.Database.SqlQueryRaw<T>(query).ToListAsync();
      }
      catch (Exception ex)
      {
        throw new Exception($"Error al ejecutar el procedimiento almacenado: {storedProcedure}", ex);
      }
    }


    //Es más flexible pero no posee el enfoque de EntityFramework
    public DataSet ExecuteStoreProcedure(string sqlQuery, List<DbParameter> parameters)
    {
      DataSet data = new DataSet();

      using (var oaCommand = _context.Database.GetDbConnection().CreateCommand())
      {

        oaCommand.CommandTimeout = 600;

        oaCommand.CommandText = sqlQuery;
        oaCommand.CommandType = CommandType.StoredProcedure;

        foreach (var parameter in parameters)
        {
          oaCommand.Parameters.Add(parameter);
        }

        try
        {
          oaCommand.Connection.Open();

          using (var oda = new SqlDataAdapter(oaCommand as SqlCommand))
          {
            oda.Fill(data);
          }

        }
        catch (Exception)
        {
          data = null;
        }
        finally
        {
          oaCommand.Connection.Close();
        }
      }
      return data;
    }

  }
}

