using JKC.Backend.Aplicacion.Services.ProductoServices;
using JKC.Backend.Dominio.Entidades.Categorias;
using JKC.Backend.Dominio.Entidades.Productos;
using JKC.Backend.Infraestructura.Framework.RepositoryPattern;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        return await _CategoriaRepository.ObtenerTodos();
      }

      public async Task<Categoria> ObtenerCategoriaPorId(int id)
      {
        return await _CategoriaRepository.ObtenerPorId(id);
      }

      public async Task EliminarCategoriasPorIds(int id)
      {
        await _CategoriaRepository.EliminarPorId(id);
      }
    }
}
