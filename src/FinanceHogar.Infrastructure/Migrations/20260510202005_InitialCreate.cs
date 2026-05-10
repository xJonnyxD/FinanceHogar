using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FinanceHogar.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Hogares",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Nombre = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    Descripcion = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Pais = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false, defaultValue: "El Salvador"),
                    Departamento = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Municipio = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    MonedaPrincipal = table.Column<int>(type: "integer", nullable: false),
                    PresupuestoMensualTotal = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hogares", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Nombre = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Descripcion = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    NombreCompleto = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: false),
                    Telefono = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    DUI = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    RefreshToken = table.Column<string>(type: "text", nullable: true),
                    RefreshTokenExpiry = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    EstaActivo = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Categorias",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Nombre = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Descripcion = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    Icono = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Color = table.Column<string>(type: "character varying(7)", maxLength: 7, nullable: true),
                    EsIngreso = table.Column<bool>(type: "boolean", nullable: false),
                    EsGlobal = table.Column<bool>(type: "boolean", nullable: false),
                    HogarId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categorias", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Categorias_Hogares_HogarId",
                        column: x => x.HogarId,
                        principalTable: "Hogares",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "HogarUsuarios",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    HogarId = table.Column<Guid>(type: "uuid", nullable: false),
                    UsuarioId = table.Column<Guid>(type: "uuid", nullable: false),
                    RolId = table.Column<Guid>(type: "uuid", nullable: false),
                    EsAdministrador = table.Column<bool>(type: "boolean", nullable: false),
                    FechaIngreso = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HogarUsuarios", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HogarUsuarios_Hogares_HogarId",
                        column: x => x.HogarId,
                        principalTable: "Hogares",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HogarUsuarios_Roles_RolId",
                        column: x => x.RolId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HogarUsuarios_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Remesas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    HogarId = table.Column<Guid>(type: "uuid", nullable: false),
                    ReceptorId = table.Column<Guid>(type: "uuid", nullable: false),
                    Monto = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    Moneda = table.Column<int>(type: "integer", nullable: false),
                    PaisOrigen = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    NombreRemitente = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    Empresa = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    ComisionCobrada = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    FechaRecibida = table.Column<DateOnly>(type: "date", nullable: false),
                    NumeroConfirmacion = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Proposito = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Remesas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Remesas_Hogares_HogarId",
                        column: x => x.HogarId,
                        principalTable: "Hogares",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Remesas_Usuarios_ReceptorId",
                        column: x => x.ReceptorId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ServiciosBasicos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    HogarId = table.Column<Guid>(type: "uuid", nullable: false),
                    UsuarioId = table.Column<Guid>(type: "uuid", nullable: false),
                    TipoServicio = table.Column<int>(type: "integer", nullable: false),
                    NombreProveedor = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    MontoUltimoPago = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    MontoPromedio = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    FechaVencimiento = table.Column<DateOnly>(type: "date", nullable: false),
                    FechaUltimoPago = table.Column<DateOnly>(type: "date", nullable: true),
                    EstaVencido = table.Column<bool>(type: "boolean", nullable: false),
                    NotificacionActiva = table.Column<bool>(type: "boolean", nullable: false),
                    DiasAnticipacionNotificacion = table.Column<int>(type: "integer", nullable: false),
                    NumeroCuenta = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiciosBasicos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ServiciosBasicos_Hogares_HogarId",
                        column: x => x.HogarId,
                        principalTable: "Hogares",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ServiciosBasicos_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Tandas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    HogarId = table.Column<Guid>(type: "uuid", nullable: false),
                    OrganizadorId = table.Column<Guid>(type: "uuid", nullable: false),
                    Nombre = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    CuotaMensual = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    TotalParticipantes = table.Column<int>(type: "integer", nullable: false),
                    Frecuencia = table.Column<int>(type: "integer", nullable: false),
                    FechaInicio = table.Column<DateOnly>(type: "date", nullable: false),
                    FechaFin = table.Column<DateOnly>(type: "date", nullable: true),
                    Estado = table.Column<int>(type: "integer", nullable: false),
                    TurnoActual = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tandas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tandas_Hogares_HogarId",
                        column: x => x.HogarId,
                        principalTable: "Hogares",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Tandas_Usuarios_OrganizadorId",
                        column: x => x.OrganizadorId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Alertas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    HogarId = table.Column<Guid>(type: "uuid", nullable: false),
                    UsuarioId = table.Column<Guid>(type: "uuid", nullable: true),
                    CategoriaId = table.Column<Guid>(type: "uuid", nullable: true),
                    Tipo = table.Column<int>(type: "integer", nullable: false),
                    Estado = table.Column<int>(type: "integer", nullable: false),
                    Titulo = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Mensaje = table.Column<string>(type: "text", nullable: false),
                    PorcentajeUso = table.Column<decimal>(type: "numeric(5,2)", nullable: true),
                    FechaGenerada = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FechaLeida = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Alertas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Alertas_Categorias_CategoriaId",
                        column: x => x.CategoriaId,
                        principalTable: "Categorias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Alertas_Hogares_HogarId",
                        column: x => x.HogarId,
                        principalTable: "Hogares",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Alertas_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "PresupuestosMensuales",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    HogarId = table.Column<Guid>(type: "uuid", nullable: false),
                    CategoriaId = table.Column<Guid>(type: "uuid", nullable: false),
                    Anio = table.Column<int>(type: "integer", nullable: false),
                    Mes = table.Column<int>(type: "integer", nullable: false),
                    MontoLimite = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    MontoGastado = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PresupuestosMensuales", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PresupuestosMensuales_Categorias_CategoriaId",
                        column: x => x.CategoriaId,
                        principalTable: "Categorias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PresupuestosMensuales_Hogares_HogarId",
                        column: x => x.HogarId,
                        principalTable: "Hogares",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Ingresos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UsuarioId = table.Column<Guid>(type: "uuid", nullable: false),
                    HogarId = table.Column<Guid>(type: "uuid", nullable: false),
                    CategoriaId = table.Column<Guid>(type: "uuid", nullable: false),
                    RemesaId = table.Column<Guid>(type: "uuid", nullable: true),
                    Monto = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    Moneda = table.Column<int>(type: "integer", nullable: false),
                    MontoEnUSD = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    Tipo = table.Column<int>(type: "integer", nullable: false),
                    Descripcion = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    FechaIngreso = table.Column<DateOnly>(type: "date", nullable: false),
                    EsRecurrente = table.Column<bool>(type: "boolean", nullable: false),
                    Frecuencia = table.Column<int>(type: "integer", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ingresos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Ingresos_Categorias_CategoriaId",
                        column: x => x.CategoriaId,
                        principalTable: "Categorias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Ingresos_Hogares_HogarId",
                        column: x => x.HogarId,
                        principalTable: "Hogares",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Ingresos_Remesas_RemesaId",
                        column: x => x.RemesaId,
                        principalTable: "Remesas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Ingresos_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Gastos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UsuarioId = table.Column<Guid>(type: "uuid", nullable: false),
                    HogarId = table.Column<Guid>(type: "uuid", nullable: false),
                    CategoriaId = table.Column<Guid>(type: "uuid", nullable: false),
                    ServicioBasicoId = table.Column<Guid>(type: "uuid", nullable: true),
                    Monto = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    Moneda = table.Column<int>(type: "integer", nullable: false),
                    MontoEnUSD = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    Tipo = table.Column<int>(type: "integer", nullable: false),
                    Descripcion = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    FechaGasto = table.Column<DateOnly>(type: "date", nullable: false),
                    EsRecurrente = table.Column<bool>(type: "boolean", nullable: false),
                    Frecuencia = table.Column<int>(type: "integer", nullable: true),
                    Comprobante = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Gastos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Gastos_Categorias_CategoriaId",
                        column: x => x.CategoriaId,
                        principalTable: "Categorias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Gastos_Hogares_HogarId",
                        column: x => x.HogarId,
                        principalTable: "Hogares",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Gastos_ServiciosBasicos_ServicioBasicoId",
                        column: x => x.ServicioBasicoId,
                        principalTable: "ServiciosBasicos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Gastos_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TandaParticipantes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TandaId = table.Column<Guid>(type: "uuid", nullable: false),
                    UsuarioId = table.Column<Guid>(type: "uuid", nullable: false),
                    NumeroTurno = table.Column<int>(type: "integer", nullable: false),
                    HaRecibido = table.Column<bool>(type: "boolean", nullable: false),
                    FechaRecibio = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CuotasPagadas = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TandaParticipantes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TandaParticipantes_Tandas_TandaId",
                        column: x => x.TandaId,
                        principalTable: "Tandas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TandaParticipantes_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Categorias",
                columns: new[] { "Id", "Color", "CreatedAt", "DeletedAt", "Descripcion", "EsGlobal", "EsIngreso", "HogarId", "Icono", "IsDeleted", "Nombre", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("c0000000-0000-0000-0000-000000000001"), "#2ECC71", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, true, true, null, "money-bill", false, "Salario", null },
                    { new Guid("c0000000-0000-0000-0000-000000000002"), "#3498DB", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, true, true, null, "plane", false, "Remesa", null },
                    { new Guid("c0000000-0000-0000-0000-000000000003"), "#9B59B6", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, true, true, null, "store", false, "Negocio Propio", null },
                    { new Guid("c0000000-0000-0000-0000-000000000004"), "#E67E22", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, true, true, null, "handshake", false, "Trabajo Informal", null },
                    { new Guid("c0000000-0000-0000-0000-000000000005"), "#1ABC9C", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, true, true, null, "users", false, "Tanda", null },
                    { new Guid("c0000000-0000-0000-0000-000000000006"), "#E74C3C", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, true, false, null, "utensils", false, "Alimentacion", null },
                    { new Guid("c0000000-0000-0000-0000-000000000007"), "#3498DB", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, true, false, null, "graduation-cap", false, "Educacion", null },
                    { new Guid("c0000000-0000-0000-0000-000000000008"), "#E91E63", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, true, false, null, "heartbeat", false, "Salud", null },
                    { new Guid("c0000000-0000-0000-0000-000000000009"), "#FF9800", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, true, false, null, "bus", false, "Transporte", null },
                    { new Guid("c0000000-0000-0000-0000-000000000010"), "#FFC107", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, true, false, null, "bolt", false, "Servicios Basicos", null },
                    { new Guid("c0000000-0000-0000-0000-000000000011"), "#9C27B0", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, true, false, null, "gamepad", false, "Entretenimiento", null },
                    { new Guid("c0000000-0000-0000-0000-000000000012"), "#4CAF50", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, true, false, null, "piggy-bank", false, "Ahorro", null },
                    { new Guid("c0000000-0000-0000-0000-000000000013"), "#00BCD4", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, true, false, null, "users", false, "Cuota Tanda", null },
                    { new Guid("c0000000-0000-0000-0000-000000000014"), "#95A5A6", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, true, false, null, "ellipsis-h", false, "Otros", null }
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "CreatedAt", "DeletedAt", "Descripcion", "IsDeleted", "Nombre", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("a1b2c3d4-0001-0001-0001-000000000001"), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Administrador del hogar con acceso total", false, "Admin", null },
                    { new Guid("a1b2c3d4-0001-0001-0001-000000000002"), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Miembro del hogar con acceso limitado", false, "Miembro", null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Alertas_CategoriaId",
                table: "Alertas",
                column: "CategoriaId");

            migrationBuilder.CreateIndex(
                name: "IX_Alertas_Estado",
                table: "Alertas",
                column: "Estado");

            migrationBuilder.CreateIndex(
                name: "IX_Alertas_HogarId",
                table: "Alertas",
                column: "HogarId");

            migrationBuilder.CreateIndex(
                name: "IX_Alertas_Tipo",
                table: "Alertas",
                column: "Tipo");

            migrationBuilder.CreateIndex(
                name: "IX_Alertas_UsuarioId",
                table: "Alertas",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Categorias_EsGlobal",
                table: "Categorias",
                column: "EsGlobal");

            migrationBuilder.CreateIndex(
                name: "IX_Categorias_EsIngreso",
                table: "Categorias",
                column: "EsIngreso");

            migrationBuilder.CreateIndex(
                name: "IX_Categorias_HogarId",
                table: "Categorias",
                column: "HogarId");

            migrationBuilder.CreateIndex(
                name: "IX_Gastos_CategoriaId",
                table: "Gastos",
                column: "CategoriaId");

            migrationBuilder.CreateIndex(
                name: "IX_Gastos_FechaGasto",
                table: "Gastos",
                column: "FechaGasto");

            migrationBuilder.CreateIndex(
                name: "IX_Gastos_HogarId",
                table: "Gastos",
                column: "HogarId");

            migrationBuilder.CreateIndex(
                name: "IX_Gastos_HogarId_FechaGasto",
                table: "Gastos",
                columns: new[] { "HogarId", "FechaGasto" });

            migrationBuilder.CreateIndex(
                name: "IX_Gastos_ServicioBasicoId",
                table: "Gastos",
                column: "ServicioBasicoId");

            migrationBuilder.CreateIndex(
                name: "IX_Gastos_UsuarioId",
                table: "Gastos",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Hogares_Departamento",
                table: "Hogares",
                column: "Departamento");

            migrationBuilder.CreateIndex(
                name: "IX_HogarUsuarios_HogarId",
                table: "HogarUsuarios",
                column: "HogarId");

            migrationBuilder.CreateIndex(
                name: "IX_HogarUsuarios_HogarId_UsuarioId",
                table: "HogarUsuarios",
                columns: new[] { "HogarId", "UsuarioId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_HogarUsuarios_RolId",
                table: "HogarUsuarios",
                column: "RolId");

            migrationBuilder.CreateIndex(
                name: "IX_HogarUsuarios_UsuarioId",
                table: "HogarUsuarios",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Ingresos_CategoriaId",
                table: "Ingresos",
                column: "CategoriaId");

            migrationBuilder.CreateIndex(
                name: "IX_Ingresos_FechaIngreso",
                table: "Ingresos",
                column: "FechaIngreso");

            migrationBuilder.CreateIndex(
                name: "IX_Ingresos_HogarId",
                table: "Ingresos",
                column: "HogarId");

            migrationBuilder.CreateIndex(
                name: "IX_Ingresos_HogarId_FechaIngreso",
                table: "Ingresos",
                columns: new[] { "HogarId", "FechaIngreso" });

            migrationBuilder.CreateIndex(
                name: "IX_Ingresos_RemesaId",
                table: "Ingresos",
                column: "RemesaId");

            migrationBuilder.CreateIndex(
                name: "IX_Ingresos_UsuarioId",
                table: "Ingresos",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_PresupuestosMensuales_Anio_Mes",
                table: "PresupuestosMensuales",
                columns: new[] { "Anio", "Mes" });

            migrationBuilder.CreateIndex(
                name: "IX_PresupuestosMensuales_CategoriaId",
                table: "PresupuestosMensuales",
                column: "CategoriaId");

            migrationBuilder.CreateIndex(
                name: "IX_PresupuestosMensuales_HogarId_CategoriaId_Anio_Mes",
                table: "PresupuestosMensuales",
                columns: new[] { "HogarId", "CategoriaId", "Anio", "Mes" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Remesas_FechaRecibida",
                table: "Remesas",
                column: "FechaRecibida");

            migrationBuilder.CreateIndex(
                name: "IX_Remesas_HogarId",
                table: "Remesas",
                column: "HogarId");

            migrationBuilder.CreateIndex(
                name: "IX_Remesas_ReceptorId",
                table: "Remesas",
                column: "ReceptorId");

            migrationBuilder.CreateIndex(
                name: "IX_Roles_Nombre",
                table: "Roles",
                column: "Nombre",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ServiciosBasicos_FechaVencimiento",
                table: "ServiciosBasicos",
                column: "FechaVencimiento");

            migrationBuilder.CreateIndex(
                name: "IX_ServiciosBasicos_HogarId",
                table: "ServiciosBasicos",
                column: "HogarId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiciosBasicos_TipoServicio",
                table: "ServiciosBasicos",
                column: "TipoServicio");

            migrationBuilder.CreateIndex(
                name: "IX_ServiciosBasicos_UsuarioId",
                table: "ServiciosBasicos",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_TandaParticipantes_TandaId",
                table: "TandaParticipantes",
                column: "TandaId");

            migrationBuilder.CreateIndex(
                name: "IX_TandaParticipantes_TandaId_NumeroTurno",
                table: "TandaParticipantes",
                columns: new[] { "TandaId", "NumeroTurno" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TandaParticipantes_TandaId_UsuarioId",
                table: "TandaParticipantes",
                columns: new[] { "TandaId", "UsuarioId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TandaParticipantes_UsuarioId",
                table: "TandaParticipantes",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Tandas_Estado",
                table: "Tandas",
                column: "Estado");

            migrationBuilder.CreateIndex(
                name: "IX_Tandas_HogarId",
                table: "Tandas",
                column: "HogarId");

            migrationBuilder.CreateIndex(
                name: "IX_Tandas_OrganizadorId",
                table: "Tandas",
                column: "OrganizadorId");

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_DUI",
                table: "Usuarios",
                column: "DUI",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_Email",
                table: "Usuarios",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_EstaActivo",
                table: "Usuarios",
                column: "EstaActivo");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Alertas");

            migrationBuilder.DropTable(
                name: "Gastos");

            migrationBuilder.DropTable(
                name: "HogarUsuarios");

            migrationBuilder.DropTable(
                name: "Ingresos");

            migrationBuilder.DropTable(
                name: "PresupuestosMensuales");

            migrationBuilder.DropTable(
                name: "TandaParticipantes");

            migrationBuilder.DropTable(
                name: "ServiciosBasicos");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Remesas");

            migrationBuilder.DropTable(
                name: "Categorias");

            migrationBuilder.DropTable(
                name: "Tandas");

            migrationBuilder.DropTable(
                name: "Hogares");

            migrationBuilder.DropTable(
                name: "Usuarios");
        }
    }
}
