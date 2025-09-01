using JKC.Backend.Dominio.Entidades.Categorias;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JKC.Backend.Aplicacion.Services.CategoriasServices
{
  public interface IServicioCategoria
  {
    Task<Categoria> RegistrarCategoria(Categoria registroCategoria);
    Task<List<Categoria>> ObtenerListadoCategorias();
    Task<Categoria> ObtenerCategoriaPorId(int id);
    Task<Categoria> ActualizarCategoria(Categoria categoria);
    Task EliminarCategoriasPorIds(int id);
  }
}
