using JKC.Backend.Aplicacion.Services.ProductoServices;
using JKC.Backend.Dominio.Entidades.Categorias;
using JKC.Backend.Dominio.Entidades.Productos;
using JKC.Backend.Dominio.Entidades.Seguridad.Usuarios;
using JKC.Backend.Infraestructura.Framework.RepositoryPattern;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JKC.Backend.Aplicacion.Services.CategoriasServices
{
  public class ServicioCategoria : IServicioCategoria
  {
    
      private readonly IRepository<Categoria> _CategoriaRepository;

      public ServicioCategoria(IRepository<Categoria> CategoriaRepository)
      {
        _CategoriaRepository = CategoriaRepository;
      }

      public async Task<Categoria> RegistrarCategoria(Categoria registroCategoria)
      {
        await _CategoriaRepository.Crear(registroCategoria);
        return registroCategoria;
      }

      public async Task<List<Categoria>> ObtenerListadoCategorias()
      {
        return await _CategoriaRepository.ObtenerTodos().ToListAsync();
      }

      public async Task<Categoria> ObtenerCategoriaPorId(int id)
      {
        return await _CategoriaRepository.ObtenerPorId(id);
      }

      public async Task EliminarCategoriasPorIds(int id)
      {
        await _CategoriaRepository.EliminarPorId(id);
      }

        //public async Task<List<RolesUsuario>> ObtenerComboCategorias()
        //{
        //  try
        //  {

        //    var obtenerRolesUsuarios = await _CategoriaRepository.EjecutarProcedimientoAlmacenado<ComboResponse>("obtenerRolesUsuario", null);

        //    // Si se obtuvieron resultados, los devolvemos
        //    return obtenerRolesUsuarios.ToList();
        //  }
        //  catch (Exception ex)
        //  {
        //    // Manejo de excepciones: si ocurre algún error, lanzamos una nueva excepción
        //    throw new Exception("Error al obtener roles por usuario", ex);
        //  }
        //}
  }
}
