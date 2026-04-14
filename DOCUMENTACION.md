# FinanceHogar API — Documentación Completa

Plataforma de control financiero para el hogar orientada a familias salvadoreñas.  
Backend: **.NET 10 Web API** · Base de datos: **PostgreSQL 18** · Arquitectura: **Clean Architecture**

---

## Tabla de Contenidos

1. [¿Qué es este proyecto?](#1-qué-es-este-proyecto)
2. [Arquitectura del sistema](#2-arquitectura-del-sistema)
3. [Cómo correr el proyecto](#3-cómo-correr-el-proyecto)
4. [Capas del proyecto explicadas](#4-capas-del-proyecto-explicadas)
5. [Módulos y endpoints](#5-módulos-y-endpoints)
6. [Flujos de negocio importantes](#6-flujos-de-negocio-importantes)
7. [Validaciones](#7-validaciones)
8. [Base de datos](#8-base-de-datos)
9. [Seguridad y autenticación](#9-seguridad-y-autenticación)
10. [Features exclusivos El Salvador](#10-features-exclusivos-el-salvador)
11. [Glosario de enums](#11-glosario-de-enums)

---

## 1. ¿Qué es este proyecto?

**FinanceHogar** es una API REST que permite a familias salvadoreñas llevar el control de sus finanzas del hogar. Un hogar puede tener varios miembros (con roles Admin o Miembro), registrar ingresos, gastos, presupuestos mensuales por categoría, y recibir alertas automáticas cuando se acercan a los límites de gasto.

### ¿Qué puede hacer una familia con esta API?

| Necesidad | Solución en la API |
|---|---|
| Registrar salarios y otros ingresos | Módulo de **Ingresos** |
| Controlar en qué gastan el dinero | Módulo de **Gastos** |
| No pasarse del presupuesto mensual | **Presupuestos** + **Alertas automáticas** |
| Llevar el control de agua, luz, internet | **Servicios Básicos** |
| Participar en tandas de ahorro | Módulo de **Tandas** |
| Registrar remesas del exterior | Módulo de **Remesas** |
| Ver reportes financieros y puntaje | **Reportes** |

---

## 2. Arquitectura del sistema

El proyecto usa **Clean Architecture** dividida en 4 capas. Cada capa tiene una responsabilidad única y solo puede depender de las capas internas.

```
┌─────────────────────────────────────────────┐
│              FinanceHogar.API               │  ← Capa más externa: controllers, middleware
│  (HTTP, Swagger, JWT middleware, Program.cs) │
└──────────────────┬──────────────────────────┘
                   │ depende de
┌──────────────────▼──────────────────────────┐
│          FinanceHogar.Infrastructure        │  ← Acceso a datos: EF Core, PostgreSQL
│     (AppDbContext, Repositories, AuthService)│
└──────────────────┬──────────────────────────┘
                   │ depende de
┌──────────────────▼──────────────────────────┐
│           FinanceHogar.Application          │  ← Lógica de negocio: Services, DTOs, Validators
│          (Services, DTOs, Interfaces)        │
└──────────────────┬──────────────────────────┘
                   │ depende de
┌──────────────────▼──────────────────────────┐
│             FinanceHogar.Domain             │  ← Núcleo: Entidades, Enums, Reglas
│         (Entities, Enums, BusinessRules)     │
└─────────────────────────────────────────────┘
```

**Regla de dependencia:** Las capas externas conocen a las internas, nunca al revés. El Domain no conoce nada de EF Core ni de HTTP.

### Estructura de archivos

```
D:\Proyectos\Finace_Hogar\
├── FinanceHogar.sln                      ← Solución de Visual Studio
├── DOCUMENTACION.md                       ← Este archivo
├── database/
│   └── schema.sql                         ← Script SQL completo del esquema
└── src/
    ├── FinanceHogar.Domain/               ← Capa 1: Núcleo
    │   ├── Common/BaseEntity.cs           ← Clase base con Id, CreatedAt, IsDeleted
    │   ├── Entities/                      ← 13 entidades del negocio
    │   └── Enums/                         ← 8 enumeraciones
    │
    ├── FinanceHogar.Application/          ← Capa 2: Lógica de negocio
    │   ├── DTOs/                          ← Objetos de transferencia de datos
    │   ├── Interfaces/                    ← Contratos de servicios y repositorios
    │   ├── Services/                      ← Implementaciones de lógica de negocio
    │   └── Validators/                    ← Validaciones de entrada (FluentValidation)
    │
    ├── FinanceHogar.Infrastructure/       ← Capa 3: Datos
    │   ├── Data/
    │   │   ├── AppDbContext.cs            ← Contexto de EF Core
    │   │   ├── Configurations/            ← Mapeo entidad ↔ tabla
    │   │   └── Migrations/                ← Historial de cambios a la BD
    │   ├── Repositories/                  ← Acceso a datos por entidad
    │   └── Services/AuthService.cs        ← Autenticación (JWT + BCrypt)
    │
    └── FinanceHogar.API/                  ← Capa 4: HTTP
        ├── Controllers/                   ← 11 controladores REST
        ├── Middleware/                    ← Manejo global de errores
        ├── Properties/launchSettings.json ← Configuración de arranque
        ├── appsettings.json               ← Configuración producción
        ├── appsettings.Development.json   ← Configuración desarrollo
        └── Program.cs                     ← Punto de entrada, inyección de dependencias
```

---

## 3. Cómo correr el proyecto

### Requisitos previos

| Herramienta | Versión mínima | Verificar con |
|---|---|---|
| .NET SDK | 10.0 | `dotnet --version` |
| PostgreSQL | 18.x | `psql --version` |

### Paso 1 — Configurar la base de datos

Asegúrate de que PostgreSQL esté corriendo. La cadena de conexión está en:

```
src/FinanceHogar.API/appsettings.Development.json
```

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=FinanceHogar_Dev;Username=postgres;Password=TU_PASSWORD"
  }
}
```

Cambia `TU_PASSWORD` por la contraseña de tu usuario `postgres`.

### Paso 2 — Correr la API

Abre una terminal en la carpeta raíz del proyecto (`D:\Proyectos\Finace_Hogar`) y ejecuta:

```bash
dotnet run --project src/FinanceHogar.API --launch-profile http
```

La primera vez que corres en modo Development, la API **crea automáticamente la base de datos y aplica las migraciones** (gracias al `MigrateAsync()` en Program.cs).

### Paso 3 — Abrir en el navegador

Una vez que veas en la consola que el servidor está escuchando, abre:

```
http://localhost:5153
```

Verás el **Swagger UI** — la interfaz interactiva para probar todos los endpoints.

### Paso 4 — Probar desde Swagger

1. Haz clic en **`POST /api/v1/auth/register`** → **Try it out**
2. Pega este JSON y ejecuta:
```json
{
  "nombreCompleto": "María García",
  "email": "maria@ejemplo.com",
  "password": "MiClave123",
  "nombreHogar": "Hogar García"
}
```
3. Copia el campo `token` de la respuesta
4. Haz clic en el botón **Authorize** (arriba a la derecha, ícono de candado)
5. Escribe `Bearer <pega el token aquí>` y confirma
6. Ya puedes usar todos los endpoints protegidos

### Detener la API

Presiona `Ctrl + C` en la terminal donde está corriendo.

### Otros comandos útiles

```bash
# Compilar sin correr
dotnet build

# Verificar salud de la BD
curl http://localhost:5153/health

# Ver documentación de la API (JSON)
curl http://localhost:5153/swagger/v1/swagger.json

# Crear nueva migración (cuando cambias entidades)
dotnet ef migrations add NombreDeLaMigracion \
  --project src/FinanceHogar.Infrastructure \
  --startup-project src/FinanceHogar.API \
  --output-dir "Data/Migrations"

# Aplicar migraciones manualmente
dotnet ef database update \
  --project src/FinanceHogar.Infrastructure \
  --startup-project src/FinanceHogar.API
```

---

## 4. Capas del proyecto explicadas

### 4.1 Domain — El corazón del sistema

Contiene las reglas del negocio puras. No conoce nada de bases de datos ni de HTTP.

#### BaseEntity.cs
Todos los objetos del sistema heredan de esta clase:

```csharp
public abstract class BaseEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();       // ID único automático
    public DateTime CreatedAt { get; set; }               // Fecha de creación
    public DateTime? UpdatedAt { get; set; }              // Fecha de última modificación
    public bool IsDeleted { get; set; } = false;          // Borrado lógico
    public DateTime? DeletedAt { get; set; }              // Cuándo se "borró"
}
```

**Borrado lógico (soft delete):** Cuando "eliminas" un registro, en realidad no se borra de la BD — se marca con `IsDeleted = true`. Así puedes recuperar datos históricos y evitar errores de integridad referencial.

#### Entidades principales

| Entidad | Qué representa |
|---|---|
| `Usuario` | Persona registrada. Tiene email, contraseña hasheada, DUI (ID salvadoreño) |
| `Hogar` | Familia o grupo. Es el contenedor de todos los registros financieros |
| `HogarUsuario` | Relación entre usuario y hogar (puede ser Admin o Miembro) |
| `Rol` | "Admin" o "Miembro" — permisos dentro del hogar |
| `Categoria` | Clasificación de ingresos/gastos (Salario, Alimentación, etc.) |
| `Ingreso` | Un ingreso registrado (salario, remesa, negocio, etc.) |
| `Gasto` | Un gasto registrado (mercado, transporte, servicios, etc.) |
| `PresupuestoMensual` | Límite de gasto por categoría en un mes específico |
| `Alerta` | Notificación generada automáticamente (presupuesto al 80%, etc.) |
| `ServicioBasico` | Servicio del hogar (agua, luz, internet) con fecha de vencimiento |
| `Tanda` | Club de ahorro rotativo (exclusivo El Salvador) |
| `TandaParticipante` | Miembro de una tanda con número de turno |
| `Remesa` | Dinero recibido del exterior (exclusivo El Salvador) |

#### Reglas de negocio — AlertaRules.cs

```csharp
// Determina qué tipo de alerta generar según los umbrales cruzados
public static TipoAlerta? EvaluarPresupuesto(decimal porcentajeNuevo, decimal porcentajeAnterior)
{
    return (porcentajeAnterior, porcentajeNuevo) switch
    {
        var (prev, curr) when prev < 100 && curr >= 100 => TipoAlerta.PresupuestoSuperado,
        var (prev, curr) when prev < 80  && curr >= 80  => TipoAlerta.PresupuestoAlOchentaPorciento,
        var (prev, curr) when prev < 50  && curr >= 50  => TipoAlerta.PresupuestoAlCincuentaPorciento,
        _ => null  // No cruzó ningún umbral, no generar alerta
    };
}
```

**Ejemplo:** Si el presupuesto de Alimentación va de 45% a 55% de uso, se genera una alerta de tipo `PresupuestoAlCincuentaPorciento`. Si ya estaba en 52% y sube a 83%, se genera `PresupuestoAlOchentaPorciento`.

---

### 4.2 Application — La lógica de negocio

Contiene los servicios que implementan los casos de uso del sistema. Recibe DTOs de la API y coordina las operaciones con los repositorios.

#### DTOs (Data Transfer Objects)

Los DTOs son objetos simples que viajan entre la API y los servicios. Hay tres tipos:

- **Request DTO**: Lo que el cliente envía (`CreateGastoRequest`, `LoginRequest`)
- **Response DTO**: Lo que la API devuelve (`GastoDto`, `LoginResponse`)
- **Query DTO**: Parámetros para filtrar (`ResumenMensualIngresosDto`)

**Ejemplo — CreateGastoRequest:**
```json
{
  "hogarId": "uuid-del-hogar",
  "categoriaId": "c0000000-0000-0000-0000-000000000006",
  "monto": 45.50,
  "moneda": "USD",
  "tipo": 2,
  "descripcion": "Compras del mercado La Tiendona",
  "fechaGasto": "2026-04-13",
  "esRecurrente": false
}
```

**Ejemplo — GastoDto (respuesta):**
```json
{
  "id": "uuid-generado",
  "nombreUsuario": "María García",
  "nombreCategoria": "Alimentacion",
  "monto": 45.50,
  "alertaGenerada": {
    "tipo": "PresupuestoAlOchentaPorciento",
    "mensaje": "Advertencia: 80% del presupuesto consumido. Quedan $9.50.",
    "porcentajeUso": 82.5
  }
}
```

#### Validadores (FluentValidation)

Cada request DTO tiene un validador que verifica los datos antes de que lleguen al servicio. Si la validación falla, la API devuelve automáticamente un error 400 con descripción detallada, sin necesidad de código adicional.

**Ejemplo — validación del DUI salvadoreño:**
```csharp
RuleFor(r => r.DUI)
    .Matches(@"^\d{8}-\d$")
    .WithMessage("El DUI debe tener el formato 00000000-0 (8 dígitos, guion, 1 dígito).")
    .When(r => !string.IsNullOrEmpty(r.DUI));
```

Si envías `"dui": "1234"`, recibirás:
```json
{
  "errors": {
    "DUI": ["El DUI debe tener el formato 00000000-0 (8 dígitos, guion, 1 dígito)."]
  }
}
```

---

### 4.3 Infrastructure — El acceso a datos

Implementa las interfaces definidas en Application. Aquí vive todo lo que toca la base de datos.

#### AppDbContext.cs

Es el puente entre las entidades C# y las tablas PostgreSQL.

```csharp
// Todas las entidades registradas como tablas
public DbSet<Usuario>           Usuarios           { get; set; }
public DbSet<Hogar>             Hogares            { get; set; }
public DbSet<Ingreso>           Ingresos           { get; set; }
public DbSet<Gasto>             Gastos             { get; set; }
// ... etc
```

**Soft delete automático:** El contexto intercepta cada `SaveChanges` y convierte los borrados reales en borrados lógicos:

```csharp
// Antes de guardar, busca entidades marcadas como Deleted
var deletedEntries = ChangeTracker.Entries<BaseEntity>()
    .Where(e => e.State == EntityState.Deleted);

foreach (var entry in deletedEntries)
{
    entry.State = EntityState.Modified;  // Cambia a Modified
    entry.Entity.IsDeleted  = true;      // Marca como borrado
    entry.Entity.DeletedAt  = DateTime.UtcNow;
}
```

**Filtros globales:** Los registros con `IsDeleted = true` nunca aparecen en las consultas:
```csharp
modelBuilder.Entity<Ingreso>().HasQueryFilter(e => !e.IsDeleted);
```

#### Repositorios

Cada entidad tiene su repositorio con operaciones específicas. Ejemplo: `IngresosRepository` tiene `ObtenerTotalMensualAsync` que suma todos los ingresos de un hogar en un mes.

#### AuthService.cs

Maneja todo lo relacionado con autenticación:

- **BCrypt:** Las contraseñas nunca se guardan en texto plano. Se usa BCrypt para hashearlas: `BCrypt.HashPassword("MiClave123")` → `"$2a$11$..."`
- **JWT:** Al hacer login, se genera un token firmado con 60 minutos de vigencia
- **Refresh Token:** Token de 7 días para renovar el JWT sin volver a hacer login

---

### 4.4 API — La interfaz HTTP

#### Program.cs — El arranque

Es el archivo que configura y enciende todo el sistema:

```
1. Serilog (logs)
2. Base de datos (EF Core + PostgreSQL)
3. Servicios de Application e Infrastructure
4. JWT Authentication
5. Política de autorización AdminHogar
6. CORS
7. Rate Limiting (100 peticiones/minuto)
8. Controllers + FluentValidation
9. Swagger
10. Health Checks
```

#### BaseController.cs

Todos los controladores heredan de este. Provee acceso fácil a los datos del usuario autenticado extraídos del JWT:

```csharp
// Obtiene el ID del usuario desde el token JWT
protected Guid UsuarioActualId =>
    Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

// Obtiene el ID del hogar desde el token JWT
protected Guid HogarActualId =>
    Guid.TryParse(User.FindFirstValue("HogarId"), out var id) ? id : Guid.Empty;

// Verifica si el usuario es administrador del hogar
protected bool EsAdministrador =>
    User.FindFirstValue("EsAdministrador") == "true";
```

#### ExceptionHandlingMiddleware.cs

Captura todas las excepciones no manejadas y devuelve respuestas HTTP apropiadas:

| Excepción | Código HTTP | Cuándo ocurre |
|---|---|---|
| `KeyNotFoundException` | 404 Not Found | Registro no encontrado |
| `UnauthorizedAccessException` | 403 Forbidden | Sin permisos |
| `ArgumentException` | 400 Bad Request | Datos inválidos (email ya existe) |
| `InvalidOperationException` | 409 Conflict | Estado inválido del negocio |
| Cualquier otra | 500 Internal Server Error | Error inesperado del sistema |

---

## 5. Módulos y endpoints

Base de todos los endpoints: `http://localhost:5153/api/v1`

### 5.1 Autenticación — `/auth`

| Método | Endpoint | Descripción | Auth |
|---|---|---|---|
| POST | `/auth/register` | Registrar nuevo usuario y crear su hogar | No |
| POST | `/auth/login` | Iniciar sesión, obtener JWT | No |
| POST | `/auth/refresh` | Renovar JWT con refresh token | No |
| POST | `/auth/logout` | Cerrar sesión (invalida refresh token) | Sí |
| POST | `/auth/change-password` | Cambiar contraseña | Sí |

**Ejemplo — Registrar usuario:**
```bash
curl -X POST http://localhost:5153/api/v1/auth/register \
  -H "Content-Type: application/json" \
  -d '{
    "nombreCompleto": "Juan Pérez",
    "email": "juan@ejemplo.com",
    "password": "MiClave123",
    "dui": "12345678-9",
    "nombreHogar": "Hogar Pérez"
  }'
```

**Respuesta:**
```json
{
  "token": "eyJhbGci...",
  "refreshToken": "mrMq7Z...",
  "expiration": "2026-04-14T06:00:00Z",
  "usuarioId": "187f0326-...",
  "nombreCompleto": "Juan Pérez",
  "email": "juan@ejemplo.com"
}
```

---

### 5.2 Hogares — `/hogares`

Un hogar es el contenedor de todo el historial financiero. Una persona puede pertenecer a varios hogares.

| Método | Endpoint | Descripción | Auth |
|---|---|---|---|
| GET | `/hogares` | Listar hogares del usuario actual | Sí |
| POST | `/hogares` | Crear nuevo hogar | Sí |
| GET | `/hogares/{id}` | Detalle de un hogar | Sí |
| PUT | `/hogares/{id}` | Actualizar hogar | Sí (Admin) |
| DELETE | `/hogares/{id}` | Eliminar hogar | Sí (Admin) |
| POST | `/hogares/{id}/invitar` | Invitar miembro por email | Sí (Admin) |
| GET | `/hogares/{id}/miembros` | Ver todos los miembros | Sí |
| DELETE | `/hogares/{id}/miembros/{usuarioId}` | Remover miembro | Sí (Admin) |

**Ejemplo — Crear hogar:**
```json
{
  "nombre": "Familia Martínez",
  "departamento": "San Salvador",
  "municipio": "Soyapango",
  "monedaPrincipal": 1,
  "presupuestoMensualTotal": 1500.00
}
```

---

### 5.3 Ingresos — `/ingresos`

| Método | Endpoint | Descripción |
|---|---|---|
| GET | `/ingresos?hogarId=&fechaInicio=&fechaFin=&categoriaId=` | Listar con filtros |
| POST | `/ingresos` | Registrar ingreso |
| GET | `/ingresos/{id}` | Detalle |
| PUT | `/ingresos/{id}` | Actualizar |
| DELETE | `/ingresos/{id}` | Eliminar (soft delete) |
| GET | `/ingresos/resumen-mensual?hogarId=&anio=&mes=` | Total del mes + variación vs mes anterior |
| GET | `/ingresos/por-tipo?hogarId=&anio=&mes=` | Totales agrupados por tipo |
| GET | `/ingresos/recurrentes?hogarId=` | Solo ingresos recurrentes |

**Ejemplo — Registrar salario quincenal:**
```json
{
  "hogarId": "uuid-del-hogar",
  "categoriaId": "c0000000-0000-0000-0000-000000000001",
  "monto": 750.00,
  "moneda": "USD",
  "tipo": 1,
  "descripcion": "Salario primera quincena abril",
  "fechaIngreso": "2026-04-15",
  "esRecurrente": true,
  "frecuencia": 2
}
```

Los tipos de ingreso (campo `tipo`): `1`=Salario, `2`=Remesa, `3`=Negocio, `4`=Freelance, `5`=IngresoInformal, `6`=Tanda, `7`=Otro

---

### 5.4 Gastos — `/gastos`

Este es el módulo más importante. Al registrar un gasto, el sistema automáticamente:
1. Actualiza el `MontoGastado` del presupuesto mensual correspondiente
2. Evalúa si se cruzó un umbral (50%, 80%, 100%)
3. Si se cruzó, genera una `Alerta` en la BD
4. Devuelve la alerta generada en la respuesta del gasto

| Método | Endpoint | Descripción |
|---|---|---|
| GET | `/gastos?hogarId=&fechaInicio=&fechaFin=&categoriaId=` | Listar con filtros |
| POST | `/gastos` | Registrar gasto (activa lógica de alertas) |
| GET | `/gastos/{id}` | Detalle |
| PUT | `/gastos/{id}` | Actualizar |
| DELETE | `/gastos/{id}` | Eliminar (soft delete) |
| GET | `/gastos/por-categoria?hogarId=&anio=&mes=` | Total por categoría en el mes |
| GET | `/gastos/resumen-mensual?hogarId=&anio=&mes=` | Resumen del mes |
| GET | `/gastos/recurrentes?hogarId=` | Solo gastos recurrentes |

**Ejemplo — Registrar pago de mercado:**
```json
{
  "hogarId": "uuid-del-hogar",
  "categoriaId": "c0000000-0000-0000-0000-000000000006",
  "monto": 85.00,
  "moneda": "USD",
  "tipo": 2,
  "descripcion": "Mercado La Tiendona - compras semanales",
  "fechaGasto": "2026-04-13"
}
```

**Respuesta con alerta automática:**
```json
{
  "id": "5405637e-...",
  "monto": 85.00,
  "nombreCategoria": "Alimentacion",
  "alertaGenerada": {
    "tipo": "PresupuestoAlOchentaPorciento",
    "mensaje": "Advertencia: 85% del presupuesto de Alimentacion consumido. Quedan $15.00.",
    "porcentajeUso": 85.0
  }
}
```

Los tipos de gasto (campo `tipo`): `1`=ServicioBasico, `2`=Alimentacion, `3`=Educacion, `4`=Salud, `5`=Transporte, `6`=Entretenimiento, `7`=Ahorro, `8`=TandaCuota, `9`=Otro

---

### 5.5 Presupuestos — `/presupuestos`

Define cuánto puede gastar el hogar en cada categoría por mes.

| Método | Endpoint | Descripción |
|---|---|---|
| GET | `/presupuestos?hogarId=&anio=&mes=` | Presupuestos de un mes |
| POST | `/presupuestos` | Crear presupuesto |
| GET | `/presupuestos/{id}` | Detalle |
| PUT | `/presupuestos/{id}` | Cambiar el límite |
| DELETE | `/presupuestos/{id}` | Eliminar |
| GET | `/presupuestos/vs-real?hogarId=&anio=&mes=` | Límite vs gasto real por categoría |
| POST | `/presupuestos/copiar` | Copiar todos los presupuestos de un mes a otro |

**Ejemplo — Crear presupuesto de alimentación:**
```json
{
  "hogarId": "uuid-del-hogar",
  "categoriaId": "c0000000-0000-0000-0000-000000000006",
  "anio": 2026,
  "mes": 4,
  "montoLimite": 300.00
}
```

**Ejemplo — Ver presupuesto vs gasto real (GET `/presupuestos/vs-real`):**
```json
[
  {
    "categoriaId": "c0000000-0000-0000-0000-000000000006",
    "nombreCategoria": "Alimentacion",
    "montoLimite": 300.00,
    "montoGastado": 255.00,
    "porcentajeUso": 85.0,
    "disponible": 45.00
  },
  {
    "nombreCategoria": "Transporte",
    "montoLimite": 80.00,
    "montoGastado": 32.00,
    "porcentajeUso": 40.0,
    "disponible": 48.00
  }
]
```

---

### 5.6 Alertas — `/alertas`

Las alertas se generan automáticamente. Este módulo permite consultarlas y gestionarlas.

| Método | Endpoint | Descripción |
|---|---|---|
| GET | `/alertas?hogarId=&estado=` | Listar alertas (filtrar por estado) |
| GET | `/alertas/{id}` | Detalle de una alerta |
| PUT | `/alertas/{id}/leer` | Marcar como leída |
| PUT | `/alertas/{id}/descartar` | Descartar alerta |
| DELETE | `/alertas/{id}` | Eliminar alerta |
| POST | `/alertas/generar?hogarId=` | Generar alertas de temporada manualmente (Admin) |
| GET | `/alertas/no-leidas/count?hogarId=` | Contar alertas sin leer |

**Tipos de alerta posibles:**

| Tipo | Cuándo se genera |
|---|---|
| `PresupuestoAlCincuentaPorciento` | Gastos alcanzan el 50% del presupuesto mensual |
| `PresupuestoAlOchentaPorciento` | Gastos alcanzan el 80% del presupuesto mensual |
| `PresupuestoSuperado` | Gastos superan el 100% del presupuesto mensual |
| `VencimientoServicioBasico` | Un servicio básico vence en los próximos N días |
| `TemporadaEscolar` | Agosto — preparación para útiles escolares |
| `TemporadaNavidad` | Diciembre — preparación para gastos navideños |
| `TandaPendientePago` | Cuota de tanda pendiente de pago |
| `MetaAhorroAlcanzada` | El hogar alcanzó su meta de ahorro mensual |

---

### 5.7 Servicios Básicos — `/servicios-basicos`

Controla agua, luz, internet, etc. Cuando el vencimiento se acerca, genera una alerta automática.

| Método | Endpoint | Descripción |
|---|---|---|
| GET | `/servicios-basicos?hogarId=` | Listar servicios del hogar |
| POST | `/servicios-basicos` | Registrar servicio |
| GET | `/servicios-basicos/{id}` | Detalle |
| PUT | `/servicios-basicos/{id}` | Actualizar |
| DELETE | `/servicios-basicos/{id}` | Eliminar |
| POST | `/servicios-basicos/{id}/pagar` | Registrar pago (crea un Gasto automáticamente) |
| GET | `/servicios-basicos/vencimientos?hogarId=&dias=` | Servicios que vencen en N días |

**Ejemplo — Registrar recibo de agua:**
```json
{
  "hogarId": "uuid-del-hogar",
  "tipoServicio": 1,
  "nombreProveedor": "ANDA",
  "montoPromedio": 12.50,
  "fechaVencimiento": "2026-04-30",
  "diasAnticipacionNotificacion": 5
}
```

Los tipos de servicio (campo `tipoServicio`): `1`=Agua(ANDA), `2`=ElectricidadDelsur, `3`=ElectricidadAES, `4`=InternetClaro, `5`=InternetTigo, `6`=InternetANTEL, `7`=TelefonoClaro, `8`=TelefonoTigo, `9`=Gas, `10`=Otro

Al hacer **POST `/servicios-basicos/{id}/pagar`**, se crea automáticamente un `Gasto` con el monto del servicio y se actualiza la fecha de vencimiento para el próximo mes.

---

### 5.8 Tandas — `/tandas`

Las tandas son clubes de ahorro rotativos. Un grupo de personas aporta una cuota mensual y cada mes una persona diferente recibe el total acumulado, según el turno asignado.

**Ejemplo con 5 personas, $100/mes:** El grupo genera $500 cada mes. En enero el turno 1 recibe $500, en febrero el turno 2, y así sucesivamente.

| Método | Endpoint | Descripción |
|---|---|---|
| GET | `/tandas?hogarId=` | Listar tandas |
| POST | `/tandas` | Crear tanda |
| GET | `/tandas/{id}` | Detalle con participantes |
| PUT | `/tandas/{id}` | Actualizar |
| DELETE | `/tandas/{id}` | Eliminar |
| POST | `/tandas/{id}/participantes` | Agregar participante con número de turno |
| DELETE | `/tandas/{id}/participantes/{usuarioId}` | Remover participante |
| POST | `/tandas/{id}/registrar-pago/{participanteId}` | Registrar que alguien pagó su cuota |
| POST | `/tandas/{id}/avanzar-turno` | Avanzar al siguiente turno (Admin) |

**Ejemplo — Crear tanda:**
```json
{
  "hogarId": "uuid-del-hogar",
  "nombre": "Tanda del Trabajo — Abril 2026",
  "cuotaMensual": 100.00,
  "totalParticipantes": 5,
  "fechaInicio": "2026-05-01"
}
```

---

### 5.9 Remesas — `/remesas`

El Salvador recibe ~$8 mil millones anuales en remesas. Este módulo las registra y al guardar una remesa, **automáticamente crea un Ingreso** de tipo `Remesa`.

| Método | Endpoint | Descripción |
|---|---|---|
| GET | `/remesas?hogarId=&anio=` | Listar remesas |
| POST | `/remesas` | Registrar remesa (crea Ingreso automáticamente) |
| GET | `/remesas/{id}` | Detalle |
| PUT | `/remesas/{id}` | Actualizar |
| DELETE | `/remesas/{id}` | Eliminar |
| GET | `/remesas/estadisticas?hogarId=&anio=` | Total anual, promedio, empresa más usada |

**Ejemplo — Registrar remesa de Western Union:**
```json
{
  "hogarId": "uuid-del-hogar",
  "categoriaId": "c0000000-0000-0000-0000-000000000002",
  "monto": 300.00,
  "moneda": "USD",
  "paisOrigen": "Estados Unidos",
  "empresa": "Western Union",
  "numeroConfirmacion": "WU-2026-123456",
  "fechaRecepcion": "2026-04-13"
}
```

**Estadísticas (GET `/remesas/estadisticas`):**
```json
{
  "totalRemesas": 12,
  "montoTotalAnual": 3600.00,
  "promedioMensual": 300.00,
  "empresaMasFrecuente": "Western Union",
  "paisOrigenMasFrecuente": "Estados Unidos"
}
```

---

### 5.10 Reportes — `/reportes`

| Método | Endpoint | Descripción |
|---|---|---|
| GET | `/reportes/balance-mensual?hogarId=&anio=&mes=` | Ingresos vs Gastos del mes |
| GET | `/reportes/tendencias?hogarId=&meses=6` | Tendencias de los últimos N meses |
| GET | `/reportes/gastos-por-categoria?hogarId=&anio=&mes=` | Distribución de gastos |
| GET | `/reportes/flujo-mensual?hogarId=&anio=` | Flujo mes a mes durante el año |
| GET | `/reportes/puntaje-financiero?hogarId=` | Puntaje financiero familiar 0-100 |
| GET | `/reportes/remesas?hogarId=&anio=` | Reporte anual de remesas |

**Ejemplo — Balance mensual:**
```json
{
  "anio": 2026,
  "mes": 4,
  "totalIngresos": 1500.00,
  "totalGastos": 980.00,
  "balance": 520.00,
  "variacionIngresosVsMesAnteriorPct": 5.2,
  "variacionGastosVsMesAnteriorPct": -3.1,
  "gastosPorCategoria": [
    { "nombreCategoria": "Alimentacion", "total": 300.00, "porcentajeDelTotal": 30.6 },
    { "nombreCategoria": "Servicios Basicos", "total": 180.00, "porcentajeDelTotal": 18.4 }
  ]
}
```

**Ejemplo — Puntaje financiero:**
```json
{
  "puntaje": 72,
  "nivel": "Bueno",
  "tasaAhorro": 34.7,
  "serviciosPagadosPuntual": true,
  "recomendaciones": [
    "Tu tasa de ahorro del 34% es excelente.",
    "Considera reducir gastos en Entretenimiento (15% del total).",
    "Tienes 2 servicios básicos próximos a vencer esta semana."
  ]
}
```

El puntaje se calcula con 5 criterios:
- Cumplimiento de presupuesto (30 pts)
- Tasa de ahorro (25 pts)
- Puntualidad en servicios básicos (20 pts)
- Diversidad de gastos (15 pts)
- Estabilidad de ingresos recurrentes (10 pts)

---

### 5.11 Usuarios — `/usuarios`

| Método | Endpoint | Descripción |
|---|---|---|
| GET | `/usuarios/perfil` | Ver perfil del usuario autenticado |
| PUT | `/usuarios/perfil` | Actualizar nombre, teléfono, DUI |
| DELETE | `/usuarios/perfil` | Eliminar cuenta (soft delete) |

---

## 6. Flujos de negocio importantes

### Flujo completo: Registrar un gasto con alerta

```
Cliente                        API                         Base de Datos
  │                             │                               │
  │──POST /gastos──────────────►│                               │
  │                             │─ FluentValidation ─────────── │ (verifica campos)
  │                             │                               │
  │                             │─ INSERT Gastos ──────────────►│
  │                             │                               │
  │                             │─ SELECT PresupuestoMensual ──►│
  │                             │◄── (montoGastado, montoLimite)│
  │                             │                               │
  │                             │  porcentajeAnterior = 45%     │
  │                             │  montoGastado += 85.00        │
  │                             │  porcentajeNuevo = 83%        │
  │                             │                               │
  │                             │─ UPDATE PresupuestoMensual ──►│
  │                             │                               │
  │                             │  AlertaRules.Evaluar(83, 45)  │
  │                             │  → PresupuestoAlOchenta...    │
  │                             │                               │
  │                             │─ INSERT Alertas ─────────────►│
  │                             │                               │
  │◄── GastoDto + alertaGenerada│                               │
```

### Flujo: Pagar un servicio básico

```
POST /servicios-basicos/{id}/pagar
  ↓
  Crea un Gasto automáticamente con el monto del servicio
  ↓
  Marca el servicio como pagado (EstaVencido = false)
  ↓
  Actualiza FechaVencimiento al próximo mes
  ↓
  Retorna el Gasto creado (que puede traer alerta si cruzó umbral)
```

### Flujo: Registrar una remesa

```
POST /remesas
  ↓
  Crea el registro de Remesa en la BD
  ↓
  Crea automáticamente un Ingreso con:
    - Tipo = TipoIngreso.Remesa (2)
    - Monto = el de la remesa
    - Descripcion = "Remesa desde {paisOrigen} vía {empresa}"
  ↓
  Retorna la RemesaDto
```

---

## 7. Validaciones

Todas las validaciones devuelven HTTP 400 con errores descriptivos en español.

### Formato DUI (Documento Único de Identidad)
```
Formato válido: 12345678-9
Regla: 8 dígitos, guion, 1 dígito
```

### Contraseñas
```
Mínimo 8 caracteres
Al menos 1 letra mayúscula
Al menos 1 número
```

### Montos en Bitcoin
Si `moneda = "BTC"` (2), es obligatorio enviar `montoEnUSD` para tener la equivalencia en dólares.

### Fechas
- Gastos e ingresos: no pueden ser fecha futura
- Servicios básicos: la fecha de vencimiento no puede ser en el pasado
- Presupuestos: año entre 2020 y 2100, mes entre 1 y 12

### Tandas
- Mínimo 2 participantes, máximo 50
- La cuota mensual debe ser mayor a 0

---

## 8. Base de datos

### Tablas principales

```sql
Roles               -- "Admin", "Miembro" (2 registros seed)
Hogares             -- Familias o grupos
Usuarios            -- Personas registradas
HogarUsuarios       -- Relación muchos-a-muchos entre Hogar y Usuario
Categorias          -- 14 categorías globales (seed) + personalizadas por hogar
Ingresos            -- Registro de ingresos
Gastos              -- Registro de gastos
PresupuestosMensuales -- Límites por categoría/mes
Alertas             -- Notificaciones automáticas
ServiciosBasicos    -- Agua, luz, internet, etc.
Tandas              -- Clubes de ahorro rotativos
TandaParticipantes  -- Miembros de cada tanda
Remesas             -- Dinero del exterior
```

### Categorías globales predefinidas

| ID (últimos 2 dígitos) | Nombre | Tipo |
|---|---|---|
| `...001` | Salario | Ingreso |
| `...002` | Remesa | Ingreso |
| `...003` | Negocio Propio | Ingreso |
| `...004` | Trabajo Informal | Ingreso |
| `...005` | Tanda | Ingreso |
| `...006` | Alimentacion | Gasto |
| `...007` | Educacion | Gasto |
| `...008` | Salud | Gasto |
| `...009` | Transporte | Gasto |
| `...010` | Servicios Basicos | Gasto |
| `...011` | Entretenimiento | Gasto |
| `...012` | Ahorro | Gasto |
| `...013` | Cuota Tanda | Gasto |
| `...014` | Otros | Gasto |

### Patrón de soft delete

Ningún registro se borra físicamente. Al eliminar:
```sql
UPDATE "Gastos"
SET "IsDeleted" = TRUE, "DeletedAt" = NOW()
WHERE "Id" = 'uuid';
```

Las consultas automáticamente excluyen registros con `IsDeleted = TRUE` gracias a los filtros globales de EF Core.

### Ver datos directamente en PostgreSQL

```bash
# Conectar a la base de datos de desarrollo
psql -U postgres -d FinanceHogar_Dev

# Ver todos los hogares
SELECT * FROM "Hogares" WHERE "IsDeleted" = FALSE;

# Ver gastos del mes actual
SELECT g."Descripcion", g."Monto", c."Nombre" as categoria
FROM "Gastos" g
JOIN "Categorias" c ON g."CategoriaId" = c."Id"
WHERE g."IsDeleted" = FALSE
  AND EXTRACT(MONTH FROM g."FechaGasto") = EXTRACT(MONTH FROM NOW());
```

---

## 9. Seguridad y autenticación

### JWT (JSON Web Token)

Al hacer login, el sistema genera un token con esta información codificada:

```json
{
  "sub": "uuid-del-usuario",
  "email": "juan@ejemplo.com",
  "NombreCompleto": "Juan Pérez",
  "HogarId": "uuid-del-hogar",
  "EsAdministrador": "true",
  "exp": 1776146060
}
```

Este token se envía en cada petición:
```
Authorization: Bearer eyJhbGci...
```

Configuración en `appsettings.json`:
```json
"JwtSettings": {
  "SecretKey": "clave-secreta-minimo-32-caracteres",
  "Issuer": "FinanceHogar.API",
  "Audience": "FinanceHogar.Clients",
  "ExpirationMinutes": 60,
  "RefreshTokenExpirationDays": 7
}
```

### Rate Limiting

La API limita las peticiones para prevenir abuso:
- **100 peticiones por minuto** por IP
- Si se supera: HTTP **429 Too Many Requests**

### Política AdminHogar

Algunos endpoints requieren que el usuario sea administrador del hogar. Se verifican con la claim `EsAdministrador = "true"` del token JWT.

---

## 10. Features exclusivos El Salvador

### Por qué son únicos

| Feature | Por qué importa |
|---|---|
| **Tandas** | Práctica cultural salvadoreña de ahorro colectivo. Ninguna app bancaria local la modela formalmente |
| **Remesas** | El Salvador recibe ~$8,000 millones anuales. El 30% de familias dependen de ellas como ingreso principal |
| **TipoIngreso.IngresoInformal** | Cubre la economía informal (ventas en mercado, trabajos eventuales) — invisible en apps financieras convencionales |
| **Servicios con proveedores locales** | ANDA, DELSUR, AES, Claro, Tigo, ANTEL — no nombres genéricos |
| **Alertas de temporada** | Agosto (útiles escolares) y Diciembre (navidad/aguinaldo) — calendario escolar salvadoreño |
| **Bitcoin (TipoMoneda.BTC)** | El Salvador es el único país con BTC como moneda legal. Campo `MontoEnUSD` para conversión |
| **DUI** | Documento Único de Identidad — validación del formato salvadoreño (`########-#`) |
| **Puntaje Financiero Familiar** | Score 0-100 gamificado por hogar — incentiva mejora colectiva |
| **Departamento/Municipio** | 14 departamentos de El Salvador para análisis regional futuro |

---

## 11. Glosario de enums

Cuando la API devuelve un número en campos de tipo enum, usa esta tabla para interpretarlo:

### TipoIngreso
| Valor | Nombre |
|---|---|
| 1 | Salario |
| 2 | Remesa |
| 3 | Negocio |
| 4 | Freelance |
| 5 | IngresoInformal |
| 6 | Tanda |
| 7 | Otro |

### TipoGasto
| Valor | Nombre |
|---|---|
| 1 | ServicioBasico |
| 2 | Alimentacion |
| 3 | Educacion |
| 4 | Salud |
| 5 | Transporte |
| 6 | Entretenimiento |
| 7 | Ahorro |
| 8 | TandaCuota |
| 9 | Otro |

### TipoServicio
| Valor | Nombre |
|---|---|
| 1 | Agua (ANDA) |
| 2 | ElectricidadDelsur |
| 3 | ElectricidadAES |
| 4 | InternetClaro |
| 5 | InternetTigo |
| 6 | InternetANTEL |
| 7 | TelefonoClaro |
| 8 | TelefonoTigo |
| 9 | Gas |
| 10 | Otro |

### TipoMoneda
| Valor | Nombre |
|---|---|
| 1 | USD |
| 2 | BTC |

### TipoAlerta
| Valor | Nombre |
|---|---|
| 1 | PresupuestoAlCincuentaPorciento |
| 2 | PresupuestoAlOchentaPorciento |
| 3 | PresupuestoSuperado |
| 4 | VencimientoServicioBasico |
| 5 | TemporadaEscolar |
| 6 | TemporadaNavidad |
| 7 | TandaPendientePago |
| 8 | MetaAhorroAlcanzada |

### EstadoAlerta
| Valor | Nombre |
|---|---|
| 1 | Pendiente |
| 2 | Leida |
| 3 | Descartada |

### EstadoTanda
| Valor | Nombre |
|---|---|
| 1 | Activa |
| 2 | Completada |
| 3 | Cancelada |

### TipoFrecuencia (para ingresos/gastos recurrentes)
| Valor | Nombre |
|---|---|
| 1 | Semanal |
| 2 | Quincenal |
| 3 | Mensual |

---

*FinanceHogar — Plataforma de Control Financiero para el Hogar · El Salvador*
