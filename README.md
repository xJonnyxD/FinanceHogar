# 🏠 FinanceHogar

> Plataforma de control financiero para el hogar, diseñada para familias salvadoreñas.

**FinanceHogar** es una API REST que permite a familias llevar el control completo de sus finanzas: ingresos, gastos, presupuestos mensuales por categoría, servicios básicos, tandas de ahorro y remesas del exterior — todo en un solo lugar, con alertas automáticas cuando se acercan a sus límites de gasto.

---

## Stack tecnológico

| Capa | Tecnología |
|---|---|
| Backend | .NET 10 Web API |
| Base de datos | PostgreSQL 18 |
| ORM | Entity Framework Core 10 |
| Autenticación | JWT Bearer + Refresh Tokens |
| Contraseñas | BCrypt |
| Validaciones | FluentValidation |
| Documentación | Swagger / OpenAPI |
| Logging | Serilog |
| Arquitectura | Clean Architecture |

---

## Arquitectura

El proyecto sigue **Clean Architecture** con 4 capas independientes:

```
FinanceHogar.Domain          ← Entidades, Enums, Reglas de negocio
FinanceHogar.Application     ← Servicios, DTOs, Interfaces, Validadores
FinanceHogar.Infrastructure  ← EF Core, Repositorios, PostgreSQL, AuthService
FinanceHogar.API             ← Controllers, Middleware, Program.cs, Swagger
```

Cada capa solo depende de las capas internas. El Domain no conoce nada de bases de datos ni HTTP.

---

## Módulos del sistema

| Módulo | Descripción |
|---|---|
| **Auth** | Registro, login, refresh token, cambio de contraseña |
| **Hogares** | Gestión de hogares con múltiples miembros y roles |
| **Ingresos** | Registro de salarios, remesas, negocios, trabajo informal |
| **Gastos** | Registro de gastos con actualización automática del presupuesto |
| **Presupuestos** | Límites mensuales por categoría con comparativo vs real |
| **Alertas** | Notificaciones automáticas al cruzar el 50%, 80% y 100% del presupuesto |
| **Servicios Básicos** | Control de agua, luz, internet con alertas de vencimiento |
| **Tandas** | Clubes de ahorro rotativos (exclusivo El Salvador) |
| **Remesas** | Registro de remesas del exterior con creación automática de ingreso |
| **Reportes** | Balance mensual, tendencias, puntaje financiero familiar 0-100 |

---

## Features exclusivos El Salvador 🇸🇻

Este proyecto está pensado para la realidad financiera salvadoreña:

- **Tandas** — sistema de ahorro rotativo colectivo, sin equivalente en apps financieras locales
- **Remesas** — El Salvador recibe ~$8,000 millones anuales; el 30% de familias dependen de ellas
- **Ingreso Informal** — cubre la economía informal (ventas, trabajos eventuales)
- **Proveedores locales** — ANDA, DELSUR, AES, Claro, Tigo, ANTEL
- **Alertas de temporada** — agosto (útiles escolares) y diciembre (navidad/aguinaldo)
- **Bitcoin** — El Salvador es el único país con BTC como moneda legal
- **DUI** — validación del Documento Único de Identidad (`########-#`)
- **Puntaje Financiero Familiar** — score 0-100 gamificado por hogar

---

## Correr el proyecto

### Requisitos

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- [PostgreSQL 18](https://www.postgresql.org/download/)

### 1. Clonar el repositorio

```bash
git clone https://github.com/xJonnyxD/FinanceHogar.git
cd FinanceHogar
```

### 2. Configurar la base de datos

Crea el archivo `src/FinanceHogar.API/appsettings.Development.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=FinanceHogar_Dev;Username=postgres;Password=TU_PASSWORD"
  },
  "Serilog": {
    "MinimumLevel": {
      "Default": "Debug",
      "Override": {
        "Microsoft.EntityFrameworkCore.Database.Command": "Information"
      }
    }
  }
}
```

### 3. Ejecutar

```bash
dotnet run --project src/FinanceHogar.API --launch-profile http
```

La primera vez, la API crea automáticamente la base de datos y aplica las migraciones.

### 4. Abrir Swagger

```
http://localhost:5153
```

---

## Endpoints principales

```
POST   /api/v1/auth/register          Registrar usuario y crear hogar
POST   /api/v1/auth/login             Iniciar sesión → obtener JWT
POST   /api/v1/auth/refresh           Renovar JWT

GET    /api/v1/ingresos               Listar ingresos con filtros
POST   /api/v1/ingresos               Registrar ingreso

GET    /api/v1/gastos                 Listar gastos con filtros
POST   /api/v1/gastos                 Registrar gasto → actualiza presupuesto → genera alerta

GET    /api/v1/presupuestos/vs-real   Presupuesto vs gasto real por categoría
GET    /api/v1/alertas                Ver alertas del hogar

POST   /api/v1/servicios-basicos/{id}/pagar   Pagar servicio → crea Gasto automáticamente
POST   /api/v1/remesas                Registrar remesa → crea Ingreso automáticamente

GET    /api/v1/reportes/balance-mensual       Balance del mes
GET    /api/v1/reportes/puntaje-financiero    Puntaje financiero 0-100
GET    /health                        Estado del servicio
```

---

## Flujo de alerta automática

Al registrar un gasto, el sistema evalúa el presupuesto mensual en tiempo real:

```
POST /api/v1/gastos  →  actualiza MontoGastado del presupuesto
                     →  calcula porcentaje de uso
                     →  si cruza 50%, 80% o 100%: crea Alerta automáticamente
                     →  devuelve el gasto + la alerta generada en la respuesta
```

---

## Puntaje Financiero

El sistema calcula un score 0-100 para el hogar basado en 5 criterios:

| Criterio | Peso |
|---|---|
| Cumplimiento del presupuesto mensual | 30 pts |
| Tasa de ahorro | 25 pts |
| Puntualidad en pago de servicios básicos | 20 pts |
| Diversidad en distribución de gastos | 15 pts |
| Estabilidad de ingresos recurrentes | 10 pts |

---

## Estructura del proyecto

```
src/
├── FinanceHogar.Domain/
│   ├── Entities/          13 entidades del negocio
│   ├── Enums/             8 enumeraciones
│   ├── Common/            BaseEntity (soft delete)
│   └── BusinessRules/     AlertaRules, PuntajeFinancieroCalculator
│
├── FinanceHogar.Application/
│   ├── DTOs/              Objetos de transferencia de datos
│   ├── Interfaces/        Contratos de servicios y repositorios
│   ├── Services/          10 servicios de lógica de negocio
│   └── Validators/        15 validadores FluentValidation
│
├── FinanceHogar.Infrastructure/
│   ├── Data/              AppDbContext, Configuraciones EF Core, Migraciones
│   ├── Repositories/      9 repositorios
│   └── Services/          AuthService (JWT + BCrypt)
│
└── FinanceHogar.API/
    ├── Controllers/       11 controladores REST
    ├── Middleware/        Manejo global de errores
    └── Program.cs         Configuración y arranque
```

---

## Seguridad

- Contraseñas hasheadas con **BCrypt**
- Autenticación mediante **JWT** (60 min) + **Refresh Token** (7 días)
- **Rate limiting**: 100 peticiones por minuto por IP
- **Soft delete**: los registros eliminados nunca se borran físicamente
- `appsettings.Development.json` excluido del repositorio (credenciales locales)

---

## Base de datos

El script SQL completo del esquema está disponible en [`database/schema.sql`](database/schema.sql).

Incluye:
- 13 tablas con UUIDs, soft delete y timestamps automáticos
- Seed inicial: 2 roles y 14 categorías globales
- Índices optimizados para consultas frecuentes
- Funciones para alertas de temporada

---

## Categorías globales predefinidas

| Ingresos | Gastos |
|---|---|
| Salario | Alimentacion |
| Remesa | Educacion |
| Negocio Propio | Salud |
| Trabajo Informal | Transporte |
| Tanda | Servicios Basicos |
| | Entretenimiento |
| | Ahorro |
| | Cuota Tanda |
| | Otros |
