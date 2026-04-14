using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FinanceHogar.Infrastructure.Data.Migrations
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
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    nombre = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    descripcion = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    pais = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false, defaultValue: "El Salvador"),
                    departamento = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    municipio = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    moneda_principal = table.Column<int>(type: "integer", nullable: false),
                    presupuesto_mensual_total = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_hogares", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    nombre = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    descripcion = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_roles", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    nombre_completo = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    password_hash = table.Column<string>(type: "text", nullable: false),
                    telefono = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    dui = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    refresh_token = table.Column<string>(type: "text", nullable: true),
                    refresh_token_expiry = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    esta_activo = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_usuarios", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Categorias",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    nombre = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    descripcion = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    icono = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    color = table.Column<string>(type: "character varying(7)", maxLength: 7, nullable: true),
                    es_ingreso = table.Column<bool>(type: "boolean", nullable: false),
                    es_global = table.Column<bool>(type: "boolean", nullable: false),
                    hogar_id = table.Column<Guid>(type: "uuid", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_categorias", x => x.id);
                    table.ForeignKey(
                        name: "fk_categorias_hogares_hogar_id",
                        column: x => x.hogar_id,
                        principalTable: "Hogares",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "HogarUsuarios",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    hogar_id = table.Column<Guid>(type: "uuid", nullable: false),
                    usuario_id = table.Column<Guid>(type: "uuid", nullable: false),
                    rol_id = table.Column<Guid>(type: "uuid", nullable: false),
                    es_administrador = table.Column<bool>(type: "boolean", nullable: false),
                    fecha_ingreso = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_hogar_usuarios", x => x.id);
                    table.ForeignKey(
                        name: "fk_hogar_usuarios_hogares_hogar_id",
                        column: x => x.hogar_id,
                        principalTable: "Hogares",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_hogar_usuarios_roles_rol_id",
                        column: x => x.rol_id,
                        principalTable: "Roles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_hogar_usuarios_usuarios_usuario_id",
                        column: x => x.usuario_id,
                        principalTable: "Usuarios",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Remesas",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    hogar_id = table.Column<Guid>(type: "uuid", nullable: false),
                    receptor_id = table.Column<Guid>(type: "uuid", nullable: false),
                    monto = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    moneda = table.Column<int>(type: "integer", nullable: false),
                    pais_origen = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    nombre_remitente = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    empresa = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    comision_cobrada = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    fecha_recibida = table.Column<DateOnly>(type: "date", nullable: false),
                    numero_confirmacion = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    proposito = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_remesas", x => x.id);
                    table.ForeignKey(
                        name: "fk_remesas_hogares_hogar_id",
                        column: x => x.hogar_id,
                        principalTable: "Hogares",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_remesas_usuarios_receptor_id",
                        column: x => x.receptor_id,
                        principalTable: "Usuarios",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ServiciosBasicos",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    hogar_id = table.Column<Guid>(type: "uuid", nullable: false),
                    usuario_id = table.Column<Guid>(type: "uuid", nullable: false),
                    tipo_servicio = table.Column<int>(type: "integer", nullable: false),
                    nombre_proveedor = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    monto_ultimo_pago = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    monto_promedio = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    fecha_vencimiento = table.Column<DateOnly>(type: "date", nullable: false),
                    fecha_ultimo_pago = table.Column<DateOnly>(type: "date", nullable: true),
                    esta_vencido = table.Column<bool>(type: "boolean", nullable: false),
                    notificacion_activa = table.Column<bool>(type: "boolean", nullable: false),
                    dias_anticipacion_notificacion = table.Column<int>(type: "integer", nullable: false),
                    numero_cuenta = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_servicios_basicos", x => x.id);
                    table.ForeignKey(
                        name: "fk_servicios_basicos_hogares_hogar_id",
                        column: x => x.hogar_id,
                        principalTable: "Hogares",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_servicios_basicos_usuarios_usuario_id",
                        column: x => x.usuario_id,
                        principalTable: "Usuarios",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Tandas",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    hogar_id = table.Column<Guid>(type: "uuid", nullable: false),
                    organizador_id = table.Column<Guid>(type: "uuid", nullable: false),
                    nombre = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    cuota_mensual = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    total_participantes = table.Column<int>(type: "integer", nullable: false),
                    frecuencia = table.Column<int>(type: "integer", nullable: false),
                    fecha_inicio = table.Column<DateOnly>(type: "date", nullable: false),
                    fecha_fin = table.Column<DateOnly>(type: "date", nullable: true),
                    estado = table.Column<int>(type: "integer", nullable: false),
                    turno_actual = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_tandas", x => x.id);
                    table.ForeignKey(
                        name: "fk_tandas_hogares_hogar_id",
                        column: x => x.hogar_id,
                        principalTable: "Hogares",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_tandas_usuarios_organizador_id",
                        column: x => x.organizador_id,
                        principalTable: "Usuarios",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Alertas",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    hogar_id = table.Column<Guid>(type: "uuid", nullable: false),
                    usuario_id = table.Column<Guid>(type: "uuid", nullable: true),
                    categoria_id = table.Column<Guid>(type: "uuid", nullable: true),
                    tipo = table.Column<int>(type: "integer", nullable: false),
                    estado = table.Column<int>(type: "integer", nullable: false),
                    titulo = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    mensaje = table.Column<string>(type: "text", nullable: false),
                    porcentaje_uso = table.Column<decimal>(type: "numeric(5,2)", nullable: true),
                    fecha_generada = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    fecha_leida = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_alertas", x => x.id);
                    table.ForeignKey(
                        name: "fk_alertas_categorias_categoria_id",
                        column: x => x.categoria_id,
                        principalTable: "Categorias",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "fk_alertas_hogares_hogar_id",
                        column: x => x.hogar_id,
                        principalTable: "Hogares",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_alertas_usuarios_usuario_id",
                        column: x => x.usuario_id,
                        principalTable: "Usuarios",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "PresupuestosMensuales",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    hogar_id = table.Column<Guid>(type: "uuid", nullable: false),
                    categoria_id = table.Column<Guid>(type: "uuid", nullable: false),
                    anio = table.Column<int>(type: "integer", nullable: false),
                    mes = table.Column<int>(type: "integer", nullable: false),
                    monto_limite = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    monto_gastado = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_presupuestos_mensuales", x => x.id);
                    table.ForeignKey(
                        name: "fk_presupuestos_mensuales_categorias_categoria_id",
                        column: x => x.categoria_id,
                        principalTable: "Categorias",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_presupuestos_mensuales_hogares_hogar_id",
                        column: x => x.hogar_id,
                        principalTable: "Hogares",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Ingresos",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    usuario_id = table.Column<Guid>(type: "uuid", nullable: false),
                    hogar_id = table.Column<Guid>(type: "uuid", nullable: false),
                    categoria_id = table.Column<Guid>(type: "uuid", nullable: false),
                    remesa_id = table.Column<Guid>(type: "uuid", nullable: true),
                    monto = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    moneda = table.Column<int>(type: "integer", nullable: false),
                    monto_en_usd = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    tipo = table.Column<int>(type: "integer", nullable: false),
                    descripcion = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    fecha_ingreso = table.Column<DateOnly>(type: "date", nullable: false),
                    es_recurrente = table.Column<bool>(type: "boolean", nullable: false),
                    frecuencia = table.Column<int>(type: "integer", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_ingresos", x => x.id);
                    table.ForeignKey(
                        name: "fk_ingresos_categorias_categoria_id",
                        column: x => x.categoria_id,
                        principalTable: "Categorias",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_ingresos_hogares_hogar_id",
                        column: x => x.hogar_id,
                        principalTable: "Hogares",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_ingresos_remesas_remesa_id",
                        column: x => x.remesa_id,
                        principalTable: "Remesas",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "fk_ingresos_usuarios_usuario_id",
                        column: x => x.usuario_id,
                        principalTable: "Usuarios",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Gastos",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    usuario_id = table.Column<Guid>(type: "uuid", nullable: false),
                    hogar_id = table.Column<Guid>(type: "uuid", nullable: false),
                    categoria_id = table.Column<Guid>(type: "uuid", nullable: false),
                    servicio_basico_id = table.Column<Guid>(type: "uuid", nullable: true),
                    monto = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    moneda = table.Column<int>(type: "integer", nullable: false),
                    monto_en_usd = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    tipo = table.Column<int>(type: "integer", nullable: false),
                    descripcion = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    fecha_gasto = table.Column<DateOnly>(type: "date", nullable: false),
                    es_recurrente = table.Column<bool>(type: "boolean", nullable: false),
                    frecuencia = table.Column<int>(type: "integer", nullable: true),
                    comprobante = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_gastos", x => x.id);
                    table.ForeignKey(
                        name: "fk_gastos_categorias_categoria_id",
                        column: x => x.categoria_id,
                        principalTable: "Categorias",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_gastos_hogares_hogar_id",
                        column: x => x.hogar_id,
                        principalTable: "Hogares",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_gastos_servicios_basicos_servicio_basico_id",
                        column: x => x.servicio_basico_id,
                        principalTable: "ServiciosBasicos",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "fk_gastos_usuarios_usuario_id",
                        column: x => x.usuario_id,
                        principalTable: "Usuarios",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TandaParticipantes",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    tanda_id = table.Column<Guid>(type: "uuid", nullable: false),
                    usuario_id = table.Column<Guid>(type: "uuid", nullable: false),
                    numero_turno = table.Column<int>(type: "integer", nullable: false),
                    ha_recibido = table.Column<bool>(type: "boolean", nullable: false),
                    fecha_recibio = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    cuotas_pagadas = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_tanda_participantes", x => x.id);
                    table.ForeignKey(
                        name: "fk_tanda_participantes_tandas_tanda_id",
                        column: x => x.tanda_id,
                        principalTable: "Tandas",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_tanda_participantes_usuarios_usuario_id",
                        column: x => x.usuario_id,
                        principalTable: "Usuarios",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Categorias",
                columns: new[] { "id", "color", "created_at", "deleted_at", "descripcion", "es_global", "es_ingreso", "hogar_id", "icono", "is_deleted", "nombre", "updated_at" },
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
                columns: new[] { "id", "created_at", "deleted_at", "descripcion", "is_deleted", "nombre", "updated_at" },
                values: new object[,]
                {
                    { new Guid("a1b2c3d4-0001-0001-0001-000000000001"), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Administrador del hogar con acceso total", false, "Admin", null },
                    { new Guid("a1b2c3d4-0001-0001-0001-000000000002"), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Miembro del hogar con acceso limitado", false, "Miembro", null }
                });

            migrationBuilder.CreateIndex(
                name: "ix_alertas_categoria_id",
                table: "Alertas",
                column: "categoria_id");

            migrationBuilder.CreateIndex(
                name: "ix_alertas_estado",
                table: "Alertas",
                column: "estado");

            migrationBuilder.CreateIndex(
                name: "ix_alertas_hogar_id",
                table: "Alertas",
                column: "hogar_id");

            migrationBuilder.CreateIndex(
                name: "ix_alertas_tipo",
                table: "Alertas",
                column: "tipo");

            migrationBuilder.CreateIndex(
                name: "ix_alertas_usuario_id",
                table: "Alertas",
                column: "usuario_id");

            migrationBuilder.CreateIndex(
                name: "ix_categorias_es_global",
                table: "Categorias",
                column: "es_global");

            migrationBuilder.CreateIndex(
                name: "ix_categorias_es_ingreso",
                table: "Categorias",
                column: "es_ingreso");

            migrationBuilder.CreateIndex(
                name: "ix_categorias_hogar_id",
                table: "Categorias",
                column: "hogar_id");

            migrationBuilder.CreateIndex(
                name: "ix_gastos_categoria_id",
                table: "Gastos",
                column: "categoria_id");

            migrationBuilder.CreateIndex(
                name: "ix_gastos_fecha_gasto",
                table: "Gastos",
                column: "fecha_gasto");

            migrationBuilder.CreateIndex(
                name: "ix_gastos_hogar_id",
                table: "Gastos",
                column: "hogar_id");

            migrationBuilder.CreateIndex(
                name: "ix_gastos_hogar_id_fecha_gasto",
                table: "Gastos",
                columns: new[] { "hogar_id", "fecha_gasto" });

            migrationBuilder.CreateIndex(
                name: "ix_gastos_servicio_basico_id",
                table: "Gastos",
                column: "servicio_basico_id");

            migrationBuilder.CreateIndex(
                name: "ix_gastos_usuario_id",
                table: "Gastos",
                column: "usuario_id");

            migrationBuilder.CreateIndex(
                name: "ix_hogares_departamento",
                table: "Hogares",
                column: "departamento");

            migrationBuilder.CreateIndex(
                name: "ix_hogar_usuarios_hogar_id",
                table: "HogarUsuarios",
                column: "hogar_id");

            migrationBuilder.CreateIndex(
                name: "ix_hogar_usuarios_hogar_id_usuario_id",
                table: "HogarUsuarios",
                columns: new[] { "hogar_id", "usuario_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_hogar_usuarios_rol_id",
                table: "HogarUsuarios",
                column: "rol_id");

            migrationBuilder.CreateIndex(
                name: "ix_hogar_usuarios_usuario_id",
                table: "HogarUsuarios",
                column: "usuario_id");

            migrationBuilder.CreateIndex(
                name: "ix_ingresos_categoria_id",
                table: "Ingresos",
                column: "categoria_id");

            migrationBuilder.CreateIndex(
                name: "ix_ingresos_fecha_ingreso",
                table: "Ingresos",
                column: "fecha_ingreso");

            migrationBuilder.CreateIndex(
                name: "ix_ingresos_hogar_id",
                table: "Ingresos",
                column: "hogar_id");

            migrationBuilder.CreateIndex(
                name: "ix_ingresos_hogar_id_fecha_ingreso",
                table: "Ingresos",
                columns: new[] { "hogar_id", "fecha_ingreso" });

            migrationBuilder.CreateIndex(
                name: "ix_ingresos_remesa_id",
                table: "Ingresos",
                column: "remesa_id");

            migrationBuilder.CreateIndex(
                name: "ix_ingresos_usuario_id",
                table: "Ingresos",
                column: "usuario_id");

            migrationBuilder.CreateIndex(
                name: "ix_presupuestos_mensuales_anio_mes",
                table: "PresupuestosMensuales",
                columns: new[] { "anio", "mes" });

            migrationBuilder.CreateIndex(
                name: "ix_presupuestos_mensuales_categoria_id",
                table: "PresupuestosMensuales",
                column: "categoria_id");

            migrationBuilder.CreateIndex(
                name: "ix_presupuestos_mensuales_hogar_id_categoria_id_anio_mes",
                table: "PresupuestosMensuales",
                columns: new[] { "hogar_id", "categoria_id", "anio", "mes" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_remesas_fecha_recibida",
                table: "Remesas",
                column: "fecha_recibida");

            migrationBuilder.CreateIndex(
                name: "ix_remesas_hogar_id",
                table: "Remesas",
                column: "hogar_id");

            migrationBuilder.CreateIndex(
                name: "ix_remesas_receptor_id",
                table: "Remesas",
                column: "receptor_id");

            migrationBuilder.CreateIndex(
                name: "ix_roles_nombre",
                table: "Roles",
                column: "nombre",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_servicios_basicos_fecha_vencimiento",
                table: "ServiciosBasicos",
                column: "fecha_vencimiento");

            migrationBuilder.CreateIndex(
                name: "ix_servicios_basicos_hogar_id",
                table: "ServiciosBasicos",
                column: "hogar_id");

            migrationBuilder.CreateIndex(
                name: "ix_servicios_basicos_tipo_servicio",
                table: "ServiciosBasicos",
                column: "tipo_servicio");

            migrationBuilder.CreateIndex(
                name: "ix_servicios_basicos_usuario_id",
                table: "ServiciosBasicos",
                column: "usuario_id");

            migrationBuilder.CreateIndex(
                name: "ix_tanda_participantes_tanda_id",
                table: "TandaParticipantes",
                column: "tanda_id");

            migrationBuilder.CreateIndex(
                name: "ix_tanda_participantes_tanda_id_numero_turno",
                table: "TandaParticipantes",
                columns: new[] { "tanda_id", "numero_turno" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_tanda_participantes_tanda_id_usuario_id",
                table: "TandaParticipantes",
                columns: new[] { "tanda_id", "usuario_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_tanda_participantes_usuario_id",
                table: "TandaParticipantes",
                column: "usuario_id");

            migrationBuilder.CreateIndex(
                name: "ix_tandas_estado",
                table: "Tandas",
                column: "estado");

            migrationBuilder.CreateIndex(
                name: "ix_tandas_hogar_id",
                table: "Tandas",
                column: "hogar_id");

            migrationBuilder.CreateIndex(
                name: "ix_tandas_organizador_id",
                table: "Tandas",
                column: "organizador_id");

            migrationBuilder.CreateIndex(
                name: "ix_usuarios_dui",
                table: "Usuarios",
                column: "dui",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_usuarios_email",
                table: "Usuarios",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_usuarios_esta_activo",
                table: "Usuarios",
                column: "esta_activo");
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
