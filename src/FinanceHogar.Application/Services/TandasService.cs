using FinanceHogar.Application.DTOs.Tandas;
using FinanceHogar.Application.Interfaces;
using FinanceHogar.Application.Interfaces.Repositories;
using FinanceHogar.Domain.Entities;
using FinanceHogar.Domain.Enums;

namespace FinanceHogar.Application.Services;

public class TandasService : ITandasService
{
    private readonly ITandasRepository _tandas;

    public TandasService(ITandasRepository tandas) => _tandas = tandas;

    public async Task<IReadOnlyList<TandaDto>> ObtenerPorHogarAsync(Guid hogarId, CancellationToken ct)
    {
        var lista = await _tandas.ObtenerPorHogarAsync(hogarId, ct);
        return lista.Select(Mapear).ToList();
    }

    public async Task<TandaDto> ObtenerPorIdAsync(Guid id, CancellationToken ct)
    {
        var t = await _tandas.ObtenerPorIdAsync(id, ct)
            ?? throw new KeyNotFoundException($"Tanda {id} no encontrada.");
        return Mapear(t);
    }

    public async Task<TandaDto> CrearAsync(CreateTandaRequest req, Guid organizadorId, CancellationToken ct)
    {
        var tanda = new Tanda
        {
            HogarId           = req.HogarId,
            OrganizadorId     = organizadorId,
            Nombre            = req.Nombre,
            CuotaMensual      = req.CuotaMensual,
            TotalParticipantes = req.TotalParticipantes,
            FechaInicio       = req.FechaInicio,
            Estado            = EstadoTanda.Activa,
            TurnoActual       = 1
        };
        var creada = await _tandas.AgregarAsync(tanda, ct);
        return Mapear(creada);
    }

    public async Task<TandaDto> ActualizarAsync(Guid id, UpdateTandaRequest req, CancellationToken ct)
    {
        var t = await _tandas.ObtenerPorIdAsync(id, ct)
            ?? throw new KeyNotFoundException($"Tanda {id} no encontrada.");
        t.Nombre       = req.Nombre;
        t.CuotaMensual = req.CuotaMensual;
        var actualizada = await _tandas.ActualizarAsync(t, ct);
        return Mapear(actualizada);
    }

    public async Task EliminarAsync(Guid id, CancellationToken ct)
    {
        if (!await _tandas.ExisteAsync(id, ct))
            throw new KeyNotFoundException($"Tanda {id} no encontrada.");
        await _tandas.EliminarAsync(id, ct);
    }

    public async Task<TandaDto> AgregarParticipanteAsync(
        Guid tandaId, AgregarParticipanteRequest req, CancellationToken ct)
    {
        var t = await _tandas.ObtenerPorIdAsync(tandaId, ct)
            ?? throw new KeyNotFoundException($"Tanda {tandaId} no encontrada.");

        if (t.Participantes.Any(p => p.NumeroTurno == req.NumeroTurno))
            throw new InvalidOperationException($"El turno {req.NumeroTurno} ya está ocupado.");

        if (t.Participantes.Any(p => p.UsuarioId == req.UsuarioId))
            throw new InvalidOperationException("El usuario ya participa en esta tanda.");

        t.Participantes.Add(new TandaParticipante
        {
            TandaId     = tandaId,
            UsuarioId   = req.UsuarioId,
            NumeroTurno = req.NumeroTurno
        });
        var actualizada = await _tandas.ActualizarAsync(t, ct);
        return Mapear(actualizada);
    }

    public async Task RemoverParticipanteAsync(Guid tandaId, Guid usuarioId, CancellationToken ct)
    {
        var t = await _tandas.ObtenerPorIdAsync(tandaId, ct)
            ?? throw new KeyNotFoundException($"Tanda {tandaId} no encontrada.");

        var participante = t.Participantes.FirstOrDefault(p => p.UsuarioId == usuarioId)
            ?? throw new KeyNotFoundException("El usuario no participa en esta tanda.");

        t.Participantes.Remove(participante);
        await _tandas.ActualizarAsync(t, ct);
    }

    public async Task<TandaDto> RegistrarPagoAsync(Guid tandaId, Guid participanteId, CancellationToken ct)
    {
        var t = await _tandas.ObtenerPorIdAsync(tandaId, ct)
            ?? throw new KeyNotFoundException($"Tanda {tandaId} no encontrada.");

        var participante = t.Participantes.FirstOrDefault(p => p.UsuarioId == participanteId)
            ?? throw new KeyNotFoundException("El usuario no participa en esta tanda.");

        participante.CuotasPagadas++;
        var actualizada = await _tandas.ActualizarAsync(t, ct);
        return Mapear(actualizada);
    }

    public async Task<TandaDto> AvanzarTurnoAsync(Guid tandaId, CancellationToken ct)
    {
        var t = await _tandas.ObtenerPorIdAsync(tandaId, ct)
            ?? throw new KeyNotFoundException($"Tanda {tandaId} no encontrada.");

        if (t.Estado != EstadoTanda.Activa)
            throw new InvalidOperationException("La tanda no está activa.");

        // Marcar al participante del turno actual como receptor
        var ganador = t.Participantes.FirstOrDefault(p => p.NumeroTurno == t.TurnoActual);
        if (ganador is not null)
        {
            ganador.HaRecibido   = true;
            ganador.FechaRecibio = DateTime.UtcNow;
        }

        t.TurnoActual++;
        if (t.TurnoActual > t.TotalParticipantes)
        {
            t.Estado    = EstadoTanda.Completada;
            t.FechaFin  = DateOnly.FromDateTime(DateTime.UtcNow);
        }

        var actualizada = await _tandas.ActualizarAsync(t, ct);
        return Mapear(actualizada);
    }

    private static TandaDto Mapear(Tanda t) => new()
    {
        Id                 = t.Id,
        HogarId            = t.HogarId,
        Nombre             = t.Nombre,
        CuotaMensual       = t.CuotaMensual,
        TotalParticipantes = t.TotalParticipantes,
        TurnoActual        = t.TurnoActual,
        Estado             = t.Estado.ToString(),
        FechaInicio        = t.FechaInicio,
        CreatedAt          = t.CreatedAt,
        Participantes      = t.Participantes.Select(p => new ParticipanteDto
        {
            UsuarioId      = p.UsuarioId,
            NombreUsuario  = p.Usuario?.NombreCompleto ?? string.Empty,
            NumeroTurno    = p.NumeroTurno,
            HaRecibido     = p.HaRecibido,
            CuotaPagadaMes = p.CuotasPagadas > 0
        }).OrderBy(p => p.NumeroTurno).ToList()
    };
}
