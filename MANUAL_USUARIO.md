# Manual de Usuario — FinanceHogar
### Control Financiero Familiar para El Salvador

---

## Índice
1. [Descripción general](#descripción-general)
2. [Requisitos para ejecutar](#requisitos)
3. [Cómo iniciar el sistema](#iniciar-el-sistema)
4. [Registro e inicio de sesión](#registro-e-inicio-de-sesión)
5. [Dashboard](#dashboard)
6. [Gastos](#gastos)
7. [Ingresos](#ingresos)
8. [Presupuestos](#presupuestos)
9. [Alertas](#alertas)
10. [Servicios Básicos](#servicios-básicos)
11. [Tandas](#tandas)
12. [Remesas](#remesas)
13. [Cuentas de demostración](#cuentas-de-demostración)
14. [Preguntas frecuentes](#preguntas-frecuentes)

---

## 1. Descripción General

**FinanceHogar** es una plataforma web de control financiero diseñada para familias salvadoreñas. Permite registrar ingresos, gastos, presupuestos y recibir alertas automáticas cuando el gasto se acerca al límite establecido. Incluye módulos especiales para **tandas** y **remesas**, elementos propios de la economía familiar salvadoreña.

### Características principales
- Registro y control de gastos e ingresos en USD
- Presupuestos mensuales por categoría con alertas automáticas
- Puntaje financiero mensual (0–100 puntos)
- Control de servicios básicos con recordatorios de vencimiento
- Gestión de tandas (sistema de ahorro colectivo)
- Registro de remesas recibidas del exterior
- Dashboard con gráficos de tendencias

---

## 2. Requisitos

| Componente | Versión mínima |
|---|---|
| .NET SDK | 10.0 |
| PostgreSQL | 15 o superior |
| Navegador | Chrome, Edge o Firefox (reciente) |
| Docker (opcional) | Para levantar PostgreSQL en contenedor |

---

## 3. Cómo Iniciar el Sistema

### Opción A — Con Docker (recomendado)

```bash
# 1. Levantar PostgreSQL en Docker
docker run -d --name financehogar-pg \
  -e POSTGRES_USER=postgres \
  -e POSTGRES_PASSWORD=postgres \
  -e POSTGRES_DB=FinanceHogar \
  -p 5432:5432 postgres:latest

# 2. Ir a la carpeta de la API
cd src/FinanceHogar.API

# 3. Aplicar migraciones y datos iniciales
dotnet ef database update

# 4. Iniciar el servidor
dotnet run
```

### Opción B — PostgreSQL local instalado

Verificar que PostgreSQL esté corriendo en `localhost:5432` con usuario `postgres` y contraseña `postgres`, y que exista la base de datos `FinanceHogar`.

```bash
cd src/FinanceHogar.API
dotnet ef database update
dotnet run
```

### Acceso a la aplicación

Abrir el navegador en: **http://localhost:5153**

---

## 4. Registro e Inicio de Sesión

### Crear cuenta nueva

1. Abrir la aplicación en el navegador
2. Hacer clic en la pestaña **"Registrarse"**
3. Completar el formulario:
   - **Nombre completo** (requerido)
   - **Correo electrónico** (requerido)
   - **Contraseña** — mínimo 6 caracteres, debe incluir mayúscula, número y símbolo (ej: `MiClave1!`)
   - **Teléfono** (opcional)
   - **DUI** (opcional)
   - **Nombre del hogar** (requerido) — ej: "Familia García"
4. Clic en **"Crear cuenta"**

El sistema crea automáticamente el hogar y redirige al dashboard.

### Iniciar sesión

1. Ingresar correo y contraseña
2. Clic en **"Iniciar sesión"**

### Acceso rápido (demo)

En la pantalla de login aparecen dos botones de acceso rápido:

- **Admin** — precarga `admin@financehogar.com` / `Admin1234!`
- **Usuario** — precarga `usuario@financehogar.com` / `User1234!`

Solo dar clic al botón y luego presionar **"Iniciar sesión"**.

---

## 5. Dashboard

El dashboard es la pantalla principal. Muestra un resumen financiero del mes actual.

### Tarjetas de resumen

| Tarjeta | Qué muestra |
|---|---|
| Ingresos del mes | Total de ingresos registrados en el mes |
| Gastos del mes | Total de gastos registrados en el mes |
| Balance | Diferencia (Ingresos − Gastos) |
| Puntaje financiero | Calificación 0–100 con nivel (Excelente/Bueno/Regular/Crítico) |

### Gráfico de gastos por categoría (dona)
Muestra la distribución porcentual de gastos entre las categorías del mes.

### Gráfico de tendencias (barras)
Compara ingresos vs. gastos de los últimos meses para identificar patrones de comportamiento.

---

## 6. Gastos

Registro y control de todos los gastos del hogar.

### Registrar un gasto

1. Ir a **Gastos** en el menú lateral
2. Clic en **"+ Nuevo Gasto"**
3. Completar el formulario:
   - **Categoría** — seleccionar del listado (Alimentación, Transporte, Salud, etc.)
   - **Monto** — en USD
   - **Fecha** — fecha en que ocurrió el gasto
   - **Descripción** — detalle del gasto
   - **¿Es recurrente?** — marcar si se repite cada mes
4. Clic en **"Guardar"**

### Filtrar gastos
Usar los campos **"Desde"** y **"Hasta"** para filtrar gastos por rango de fechas, luego clic en **"Filtrar"**.

### Editar o eliminar un gasto
- Clic en el ícono **lápiz** para editar
- Clic en el ícono **basura** para eliminar (pide confirmación)

---

## 7. Ingresos

Registro de todos los ingresos del hogar (salarios, trabajos independientes, bonos, etc.).

### Registrar un ingreso

1. Ir a **Ingresos** en el menú lateral
2. Clic en **"+ Nuevo Ingreso"**
3. Completar:
   - **Categoría** — Salario, Negocio Propio, Freelance, etc.
   - **Monto** en USD
   - **Fecha de ingreso**
   - **Descripción**
4. Clic en **"Guardar"**

El panel superior muestra el **resumen del mes**: total de ingresos, cantidad de registros y promedio.

---

## 8. Presupuestos

Define límites de gasto por categoría para el mes. El sistema genera alertas automáticas cuando el gasto se acerca o supera el límite.

### Crear un presupuesto

1. Ir a **Presupuestos** en el menú lateral
2. Clic en **"+ Nuevo Presupuesto"**
3. Seleccionar:
   - **Categoría** (ej: Alimentación)
   - **Mes y Año**
   - **Monto límite** (ej: $300.00)
4. Clic en **"Guardar"**

### Comparativo presupuesto vs. gasto real

La tabla inferior muestra para cada categoría:
- **Presupuesto**: lo que planificaste gastar
- **Gastado**: lo que realmente se gastó
- **Diferencia**: cuánto queda disponible
- **Barra de progreso**: visual del porcentaje consumido

### Umbrales de alerta automática

| Color | Umbral | Significado |
|---|---|---|
| Verde | < 50% | Gasto controlado |
| Amarillo | 50%–79% | Advertencia, cuidado |
| Naranja | 80%–99% | Alerta, casi al límite |
| Rojo | ≥ 100% | Crítico, presupuesto superado |

---

## 9. Alertas

Centro de notificaciones del sistema. Las alertas se generan **automáticamente** cuando:

- Un gasto supera el 50%, 80% o 100% del presupuesto de una categoría
- Un servicio básico está próximo a vencer

### Gestionar alertas

- Clic en **"Marcar como leída"** para archivar la alerta
- Clic en el ícono **basura** para eliminarla
- El número en rojo en el menú lateral indica alertas pendientes

---

## 10. Servicios Básicos

Control de pagos recurrentes: agua, luz, internet, teléfono, gas, cable TV.

### Registrar un servicio

1. Ir a **Servicios Básicos** en el menú lateral
2. Clic en **"Agregar Servicio"**
3. Completar:
   - **Tipo** — Agua, Electricidad, Internet, etc.
   - **Proveedor** — nombre de la empresa (ej: ANDA, CAESS, Claro)
   - **Monto promedio** — costo mensual aproximado en USD
   - **Fecha de vencimiento** — día en que vence el pago
   - **Días de anticipación** — con cuántos días antes deseas recibir la alerta (defecto: 5)
4. Clic en **"Guardar"**

### Panel de próximos vencimientos

Al ingresar a Servicios Básicos, si algún servicio vence en los próximos días, aparece un aviso amarillo en la parte superior indicando cuáles están próximos a vencer y su fecha.

---

## 11. Tandas

Las tandas son un sistema de ahorro colectivo muy común en El Salvador. Un grupo de personas aporta una cuota mensual y cada participante recibe el total en su turno.

### Registrar una tanda

1. Ir a **Tandas** en el menú lateral
2. Clic en **"Nueva Tanda"**
3. Completar:
   - **Nombre** — ej: "Tanda del trabajo"
   - **Cuota mensual** — monto que cada participante aporta
   - **Total de participantes** — número de personas
   - **Fecha de inicio**
4. Clic en **"Guardar"**

La tarjeta de la tanda muestra el estado (Activa / Completada / Cancelada), el total del pozo (`cuota × participantes`) y las fechas de inicio y finalización estimada.

---

## 12. Remesas

Registro de remesas recibidas del exterior. El Salvador es uno de los países con mayor recepción de remesas en Centroamérica.

### Registrar una remesa

1. Ir a **Remesas** en el menú lateral
2. Clic en **"Nueva Remesa"**
3. Completar:
   - **Monto** recibido en USD
   - **País de origen** — ej: Estados Unidos
   - **Empresa** — Western Union, Remitly, MoneyGram, etc.
   - **Número de confirmación**
   - **Fecha de recepción**
4. Clic en **"Guardar"**

---

## 13. Cuentas de Demostración

El sistema incluye dos cuentas preconfiguradas con datos de ejemplo:

| Rol | Correo | Contraseña |
|---|---|---|
| Administrador | admin@financehogar.com | Admin1234! |
| Usuario estándar | usuario@financehogar.com | User1234! |

Estas cuentas tienen datos precargados para mostrar el funcionamiento del dashboard, gráficos y alertas.

---

## 14. Preguntas Frecuentes

**¿En qué moneda trabaja el sistema?**
En USD (dólares americanos), moneda oficial de El Salvador. El sistema también soporta registro en Bitcoin (BTC), moneda de curso legal desde 2021.

**¿Las alertas son automáticas?**
Sí. El sistema las genera al registrar un gasto que supera los umbrales configurados (50%, 80%, 100% del presupuesto). No requiere acción manual.

**¿Qué es el puntaje financiero?**
Una calificación de 0 a 100 que mide la salud financiera del hogar según la relación entre ingresos, gastos y presupuestos. Se calcula automáticamente cada vez que se carga el dashboard.

| Puntaje | Nivel |
|---|---|
| 80–100 | Excelente |
| 60–79 | Bueno |
| 40–59 | Regular |
| 0–39 | Crítico |

**¿Puedo tener varios hogares?**
Actualmente cada usuario pertenece a un hogar. La arquitectura permite extensión futura para hogares múltiples.

**¿Los datos son seguros?**
Sí. Las contraseñas se cifran con BCrypt. La sesión usa tokens JWT con expiración de 60 minutos y refresh token de 7 días. La comunicación va sobre HTTPS en producción.

---

*FinanceHogar — Universidad Evangélica de El Salvador | 2026*
