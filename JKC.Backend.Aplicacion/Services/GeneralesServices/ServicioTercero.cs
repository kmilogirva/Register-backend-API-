using JKC.Backend.Aplicacion.Services.DTOS;
using JKC.Backend.Dominio.Entidades.Generales;
using JKC.Backend.Dominio.Entidades.Usuario;
using JKC.Backend.Infraestructura.Framework.RepositoryPattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JKC.Backend.Aplicacion.Services.GeneralesServices
{
  public class ServicioTercero : IServicioTercero
  {

    private readonly IRepository<Tercero> _terceroRepository;

    public ServicioTercero(IRepository<Tercero> terceroRepository)
    {
      _terceroRepository = terceroRepository;
    }

    public async Task<Tercero> ObtenerTerceroPorId(int id)
    {
      return await _terceroRepository.ObtenerPorIdInclude(
       id,
         t => t.TipoIdentificacion,
        t => t.TipoPersona,
        t => t.TipoTercero
   );
    }

    //public async Task<List<Tercero>> ObtenerListadoTerceros()
    //{
    //  return await _terceroRepository.ObtenerTodos();
    //}

    public async Task<List<Tercero>> ObtenerListadoTerceros()
    {
      return await _terceroRepository.ObtenerTodosInclude(
           t => t.TipoIdentificacion,
        t => t.TipoPersona,
        t => t.TipoTercero
      );
    }

    public async Task<ResponseMessages> RegistrarTercero(Tercero nuevoTercero)
    {
      var terceros = await _terceroRepository.ObtenerTodos();

      var tercerosExistente = terceros.Any(u => u.codDocumento == nuevoTercero.codDocumento);

      if (tercerosExistente)
      {
        return new ResponseMessages
        {
          Exitoso = false,
          Mensaje = "Usuario ya existe en la Base de Datos."
        };
      }

      await _terceroRepository.Crear(nuevoTercero);

      return new ResponseMessages
      {
        Exitoso = true,
        Mensaje = "Usuario registrado exitosamente."
      };
    }

    public async Task<bool> ActualizarTercero(Tercero terceroActualizado)
    {
      var terceroExistente = await _terceroRepository.ObtenerPorId(terceroActualizado.IdTercero);

      if (terceroExistente == null)
        return false;

      // Actualiza los campos del usuario existente con los del actualizado
      terceroExistente.Nombre1 = terceroActualizado.Nombre1;
      terceroExistente.Nombre2 = terceroActualizado.Nombre2;
      terceroExistente.Apellido1 = terceroActualizado.Apellido1;
      terceroExistente.Apellido2 = terceroActualizado.Apellido2;
      terceroExistente.Email = terceroActualizado.Email;
      terceroExistente.Telefono = terceroActualizado.Telefono;
      terceroExistente.IdEstado = terceroActualizado.IdEstado;



      await _terceroRepository.Actualizar(terceroExistente);
      return true;
    }

    public async Task<bool> EliminarTerceroPorId(int id)
    {
      var tercero = await _terceroRepository.ObtenerPorId(id);

      if (tercero == null)
        return false;

      await _terceroRepository.EliminarPorId(id);
      return true;
    }

    

   

    
  }
}
