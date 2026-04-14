# FinanceHogar — Instrucciones de ejecución

## Requisitos
- .NET 10 SDK
- PostgreSQL 15+
- (Opcional) pgAdmin 4

---

## 1. Base de datos

```bash
# Crear la base de datos
psql -U postgres -c "CREATE DATABASE financehogar;"

# Ejecutar el script de esquema (tablas, índices, seed data, funciones)
psql -U postgres -d financehogar -f database/schema.sql
```

---

## 2. Configuración

Editar `src/FinanceHogar.API/appsettings.Development.json` y actualizar:

```json
"DefaultConnection": "Host=localhost;Database=financehogar;Username=postgres;Password=TU_PASSWORD"
```

Cambiar también el `JwtSettings:SecretKey` por una clave segura de al menos 32 caracteres.

---

## 3. Migraciones EF Core

```bash
cd src/FinanceHogar.API

# Primera vez: crear migración inicial
dotnet ef migrations add InitialCreate \
    --project ../FinanceHogar.Infrastructure \
    --startup-project .

# Aplicar migraciones (también se aplican automáticamente en Development al iniciar)
dotnet ef database update \
    --project ../FinanceHogar.Infrastructure \
    --startup-project .
```

---

## 4. Ejecutar la API

```bash
cd src/FinanceHogar.API
dotnet run
```

Swagger UI disponible en: **http://localhost:5000**

---

## 5. Endpoints principales

| Módulo            | Ruta base                        |
|-------------------|----------------------------------|
| Autenticación     | `POST /api/v1/auth/register`     |
|                   | `POST /api/v1/auth/login`        |
|                   | `POST /api/v1/auth/refresh`      |
| Hogares           | `GET/POST /api/v1/hogares`       |
| Ingresos          | `GET/POST /api/v1/ingresos`      |
| Gastos            | `GET/POST /api/v1/gastos`        |
| Presupuestos      | `GET/POST /api/v1/presupuestos`  |
| Alertas           | `GET /api/v1/alertas`            |
| Servicios Básicos | `GET/POST /api/v1/serviciosbasicos` |
| Tandas            | `GET/POST /api/v1/tandas`        |
| Remesas           | `GET/POST /api/v1/remesas`       |
| Reportes          | `GET /api/v1/reportes/balance-mensual` |
|                   | `GET /api/v1/reportes/puntaje-financiero` |
|                   | `GET /api/v1/reportes/tendencias` |
| Usuarios          | `GET /api/v1/usuarios/perfil`    |

---

## 6. Flujo rápido de prueba (Swagger)

1. `POST /api/v1/auth/register` → obtienes JWT
2. Copiar el token → **Authorize** en Swagger UI → `Bearer <token>`
3. `POST /api/v1/presupuestos` → crear límite para una categoría
4. `POST /api/v1/gastos` → registrar gasto
   - Si supera el 50/80/100% del presupuesto, la respuesta incluirá `alertaGenerada`
5. `GET /api/v1/reportes/balance-mensual?hogarId=...&anio=2026&mes=4`
6. `GET /api/v1/reportes/puntaje-financiero?hogarId=...` → score 0-100

---

## 7. Funciones PostgreSQL de mantenimiento

```sql
-- Generar alertas de temporada (julio=escolar, noviembre=navidad)
SELECT generar_alertas_temporada();

-- Marcar servicios vencidos
SELECT marcar_servicios_vencidos();

-- Ver balance mensual
SELECT * FROM v_balance_mensual WHERE hogar_id = '...' AND anio = 2026 AND mes = 4;

-- Ver alertas pendientes
SELECT * FROM v_alertas_pendientes WHERE hogar_id = '...';
```
