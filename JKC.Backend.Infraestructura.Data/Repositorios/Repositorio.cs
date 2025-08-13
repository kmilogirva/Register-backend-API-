using JKC.Backend.Dominio.Entidades.Seguridad;
using JKC.Backend.Dominio.Entidades.Seguridad.Usuarios;
using JKC.Backend.Infraestructura.Data.EntityFramework;
using JKC.Backend.Infraestructura.Framework.RepositoryPattern;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq.Expressions;

namespace JKC.Backend.Infraestructura.Data.Repositorios
{
  public class Repository<T> : IRepository<T> where T : class
  {
    private readonly DbContext _context;
    public Repository(CommonDBContext context)
    {
      _context = context;
    }

    private DbSet<T> Entities => _context.Set<T>();

    public async Task<T> ObtenerPorId(int? id)
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
    public async Task<List<T>> ObtenerTodos()
    {
      try
      {
        // Verifica si el contexto está siendo correctamente configurado
        if (_context == null)
        {
          throw new InvalidOperationException("El contexto de base de datos no está configurado correctamente.");
        }

          return await _context.Set<T>().ToListAsync();
      }
      catch (InvalidOperationException invOpEx)
      {
        // Excepción si el contexto está mal configurado
        throw new Exception("Error en la configuración del contexto de la base de datos", invOpEx);
      }
      catch (Exception ex)
      {
        // Captura cualquier otro tipo de excepción
        throw new Exception("Error al obtener los registros desde la base de datos", ex);
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
      if (id == null)
        throw new ArgumentNullException(nameof(id), "El ID no puede ser nulo.");

      var entidad = await _context.Set<T>().FindAsync(id);

      if (entidad == null)
        throw new KeyNotFoundException($"No se encontró la entidad con ID {id}.");

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

    public async Task<bool> AnyAsync(Expression<Func<T, bool>> predicado)
    {
      if (predicado is null)
        throw new ArgumentNullException(nameof(predicado));

      return await Entities.AnyAsync(predicado).ConfigureAwait(false);
    }

    public async Task<List<T>> ObtenerTodosInclude(params Expression<Func<T, object>>[] includes)
    {
      IQueryable<T> query = Entities;
      if (includes != null)
      {
        foreach (var include in includes)
        {
          query = query.Include(include);
        }
      }
      return await query.ToListAsync().ConfigureAwait(false);
    }

    //public async Task<List<RolPermiso>> ObtenerPermisosRol(int idRol)
    //{
   
    //}
  }
}

