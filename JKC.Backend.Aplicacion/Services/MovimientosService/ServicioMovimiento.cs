using JKC.Backend.Infraestructura.Framework.RepositoryPattern;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using JKC.Backend.Aplicacion.Services.MovimientoServices;
using JKC.Backend.Aplicacion.Services.MovimientoServices;
using JKC.Backend.Dominio.Entidades.Movimientos;
using JKC.Backend.Infraestructura.Framework.RepositoryPattern;

namespace JKC.Backend.Aplicacion.Services.MovimientoServices
{
  public class ServicioMovimiento : IServicioMovimiento
  {
    private readonly IRepository<Movimiento> _movimientoRepository;

    public ServicioMovimiento(IRepository<Movimiento> movimientoRepository)
    {
      _movimientoRepository = movimientoRepository;
    }

    public async Task<Movimiento> RegistrarMovimiento(Movimiento movimiento)
    {
      await _movimientoRepository.Crear(movimiento);
      return movimiento;
    }

    public async Task<List<Movimiento>> ObtenerListadoMovimientos()
    {
      return await _movimientoRepository.ObtenerTodos();
    }

    public async Task<Movimiento> ObtenerMovimientoPorId(int id)
    {
      return await _movimientoRepository.ObtenerPorId(id);
    }

    public async Task EliminarMovimientoPorId(int id)
    {
      await _movimientoRepository.EliminarPorId(id);
    }

    public async Task ActualizarMovimiento(Movimiento movimiento)
    {
      await _movimientoRepository.Actualizar(movimiento);
    }
  }
}

