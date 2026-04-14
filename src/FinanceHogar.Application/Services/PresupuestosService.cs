using FinanceHogar.Application.DTOs.Presupuestos;
using FinanceHogar.Application.Interfaces;
using FinanceHogar.Application.Interfaces.Repositories;
using FinanceHogar.Domain.Entities;

namespace FinanceHogar.Application.Services;

public class PresupuestosService : IPresupuestosService
{
    private readonly IPresupuestosRepository _presupuestos;

    public PresupuestosService(IPresupuestosRepository presupuestos) => _presupuestos = presupuestos;

    public async Task<IReadOnlyList<PresupuestoDto>> ObtenerPorHogarYMesAsync(
        Guid hogarId, int anio, int mes, CancellationToken ct)
    {
        var lista = await _presupuestos.ObtenerPorHogarYMesAsync(hogarId, anio, mes, ct);
        return lista.Select(Mapear).ToList();
    }

    public async Task<PresupuestoDto> ObtenerPorIdAsync(Guid id, CancellationToken ct)
    {
        var p = await _presupuestos.ObtenerPorIdAsync(id, ct)
            ?? throw new KeyNotFoundException($"Presupuesto {id} no encontrado.");
        return Mapear(p);
    }

    public async Task<PresupuestoDto> CrearAsync(CreatePresupuestoRequest req, CancellationToken ct)
    {
        if (await _presupuestos.ExisteAsync(req.HogarId, req.CategoriaId, req.Anio, req.Mes, ct))
            throw new InvalidOperationException(
                $"Ya existe un presupuesto para esa categoría en {req.Mes}/{req.Anio}.");

        var p = new PresupuestoMensual
        {
            HogarId     = req.HogarId,
            CategoriaId = req.CategoriaId,
            Anio        = req.Anio,
            Mes         = req.Mes,
            MontoLimite = req.MontoLimite
        };
        var creado = await _presupuestos.AgregarAsync(p, ct);
        return Mapear(creado);
    }

    public async Task<PresupuestoDto> ActualizarAsync(Guid id, UpdatePresupuestoRequest req, CancellationToken ct)
    {
        var p = await _presupuestos.ObtenerPorIdAsync(id, ct)
            ?? throw new KeyNotFoundException($"Presupuesto {id} no encontrado.");
        p.MontoLimite = req.MontoLimite;
        var actualizado = await _presupuestos.ActualizarAsync(p, ct);
        return Mapear(actualizado);
    }

    public async Task EliminarAsync(Guid id, CancellationToken ct)
    {
        if (await _presupuestos.ObtenerPorIdAsync(id, ct) is null)
            throw new KeyNotFoundException($"Presupuesto {id} no encontrado.");
        await _presupuestos.EliminarAsync(id, ct);
    }

    public async Task<IReadOnlyList<PresupuestoVsRealDto>> ObtenerVsRealAsync(
        Guid hogarId, int anio, int mes, CancellationToken ct)
    {
        var lista = await _presupuestos.ObtenerPorHogarYMesAsync(hogarId, anio, mes, ct);
        return lista.Select(p => new PresupuestoVsRealDto
        {
            CategoriaId     = p.CategoriaId,
            NombreCategoria = p.Categoria?.Nombre ?? string.Empty,
            MontoLimite     = p.MontoLimite,
            MontoGastado    = p.MontoGastado,
            PorcentajeUso   = p.PorcentajeUso,
            Disponible      = p.MontoLimite - p.MontoGastado
        }).ToList();
    }

    public async Task<IReadOnlyList<PresupuestoDto>> CopiarMesAnteriorAsync(
        CopiarPresupuestoRequest req, CancellationToken ct)
    {
        var origen = await _presupuestos.ObtenerPorHogarYMesAsync(
            req.HogarId, req.AnioOrigen, req.MesOrigen, ct);

        var creados = new List<PresupuestoDto>();
        foreach (var p in origen)
        {
            if (await _presupuestos.ExisteAsync(req.HogarId, p.CategoriaId, req.AnioDestino, req.MesDestino, ct))
                continue;

            var nuevo = await _presupuestos.AgregarAsync(new PresupuestoMensual
            {
                HogarId     = req.HogarId,
                CategoriaId = p.CategoriaId,
                Anio        = req.AnioDestino,
                Mes         = req.MesDestino,
                MontoLimite = p.MontoLimite
            }, ct);
            creados.Add(Mapear(nuevo));
        }
        return creados;
    }

    private static PresupuestoDto Mapear(PresupuestoMensual p) => new()
    {
        Id              = p.Id,
        HogarId         = p.HogarId,
        CategoriaId     = p.CategoriaId,
        NombreCategoria = p.Categoria?.Nombre ?? string.Empty,
        Anio            = p.Anio,
        Mes             = p.Mes,
        MontoLimite     = p.MontoLimite,
        MontoGastado    = p.MontoGastado,
        PorcentajeUso   = p.PorcentajeUso,
        CreatedAt       = p.CreatedAt
    };
}
