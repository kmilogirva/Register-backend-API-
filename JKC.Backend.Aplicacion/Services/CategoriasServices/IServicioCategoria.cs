using JKC.Backend.Dominio.Entidades.Categorias;
using JKC.Backend.Dominio.Entidades.Productos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JKC.Backend.Aplicacion.Services.CategoriasServices
{
  public interface IServicioCategoria
  {
    Task<Categoria> RegistrarCategoria(Categoria registroCategoria);
    Task<List<Categoria>> ObtenerListadoCategorias();
    Task<Categoria> ObtenerCategoriaPorId(int id);
    Task EliminarCategoriasPorIds(int id);
  }
}
