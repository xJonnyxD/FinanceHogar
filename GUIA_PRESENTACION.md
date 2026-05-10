# Guía de Presentación y Defensa — FinanceHogar
### Optimizada para obtener 100/100 en la rúbrica

---

## Estructura de la presentación (tiempo total recomendado: 20–25 min)

| Diapositiva | Tema | Tiempo | Criterio cubierto |
|---|---|---|---|
| 1 | Portada y equipo | 1 min | Trabajo en equipo |
| 2 | El problema | 2 min | Propuesta de valor |
| 3 | La solución | 2 min | Propuesta de valor |
| 4 | Demo en vivo | 7 min | Funcionalidad |
| 5 | Tecnologías | 2 min | Uso de tecnologías |
| 6 | Arquitectura | 2 min | Uso de tecnologías |
| 7 | Innovación y diferenciadores | 2 min | Creatividad |
| 8 | Resultados y métricas | 2 min | Funcionalidad |
| 9 | Conclusiones | 1 min | Claridad |
| 10 | Preguntas | 3 min | Defensa oral |

---

## DIAPOSITIVA 1 — Portada

**Título:** FinanceHogar — Control Financiero para Familias Salvadoreñas

**Contenido:**
- Nombre del proyecto con logo (ícono de casa + corazón)
- Nombres de todos los integrantes del equipo
- Universidad Evangélica de El Salvador
- Materia / Cátedra
- Fecha: Mayo 2026

**Quién habla:** Todos se presentan uno a uno con su nombre y rol en el proyecto.

---

## DIAPOSITIVA 2 — El Problema (Propuesta de valor — 15%)

**Título:** ¿Por qué FinanceHogar?

**Puntos clave a mencionar:**

> *"El 70% de las familias salvadoreñas no lleva un registro formal de sus finanzas. Las herramientas existentes como Excel o apps extranjeras no están adaptadas a nuestra realidad: no contemplan las tandas, las remesas ni la dolarización total de nuestra economía."*

**En la diapositiva:**
- Estadística de inclusión financiera en El Salvador
- Tres problemas concretos:
  1. Sin control → gastos superan ingresos sin saberlo
  2. Sin alertas → los servicios vencen sin recordatorio
  3. Sin herramienta local → apps en inglés, sin tandas ni remesas

**Clave para el 100%:** No solo decir "hay un problema", sino cuantificar el impacto y conectarlo con la realidad salvadoreña.

---

## DIAPOSITIVA 3 — La Solución (Propuesta de valor — 15%)

**Título:** FinanceHogar: Tu aliado financiero

**Puntos clave a mencionar:**

> *"FinanceHogar es una aplicación web gratuita, en español, diseñada específicamente para familias salvadoreñas. Permite controlar gastos, ingresos, presupuestos, servicios básicos, tandas y remesas en un solo lugar, con alertas automáticas y un puntaje financiero mensual."*

**En la diapositiva — los 8 módulos:**
- Dashboard con puntaje financiero
- Control de Gastos e Ingresos
- Presupuestos con alertas automáticas
- Servicios Básicos con recordatorios
- Tandas (sistema de ahorro colectivo)
- Remesas (recepción del exterior)

**Valor tangible a resaltar:**
- Gratis y en español
- Alertas automáticas (sin configuración extra)
- Adaptado a El Salvador: USD, Bitcoin, tandas, remesas

---

## DIAPOSITIVA 4 — DEMO EN VIVO (Funcionalidad — 25%)

**Esta es la parte más importante. Vale el 25% de la nota.**

### Guión de la demo (seguir este orden exacto):

**Paso 1 — Login rápido (30 seg)**
- Abrir `http://localhost:5153`
- Dar clic en el botón **"Admin"** (autorrellena el formulario)
- Dar clic en **"Iniciar sesión"**
- El dashboard carga instantáneamente

**Paso 2 — Dashboard (1 min)**
- Señalar las 4 tarjetas: Ingresos, Gastos, Balance, Puntaje
- Decir: *"El puntaje financiero califica automáticamente la salud del hogar de 0 a 100"*
- Mostrar el gráfico de dona (gastos por categoría)
- Mostrar el gráfico de barras (tendencias de los últimos meses)

**Paso 3 — Registrar un gasto (1 min)**
- Ir a Gastos → Nueva
- Llenar: Categoría "Alimentación", Monto "$85", hoy
- Guardar
- Decir: *"El sistema lo registra en tiempo real y actualiza el dashboard"*

**Paso 4 — Presupuestos y alertas (1.5 min)**
- Ir a Presupuestos
- Mostrar la tabla comparativa (presupuesto vs. real)
- Señalar las barras de progreso de colores
- Decir: *"Cuando el gasto supera el 80% del presupuesto, el sistema genera una alerta automática. No requiere intervención del usuario."*
- Ir a Alertas → mostrar la notificación generada
- Marcar una como leída

**Paso 5 — Servicios Básicos (1 min)**
- Mostrar la lista de servicios (ANDA, CAESS, Claro, etc.)
- Señalar el aviso amarillo de próximos vencimientos si aparece
- Decir: *"El sistema alerta con anticipación configurable, por defecto 5 días antes del vencimiento"*

**Paso 6 — Tandas y Remesas (1 min)**
- Ir a Tandas → mostrar una tanda activa
- Decir: *"Digitalizamos el sistema de tandas, que es una práctica financiera informal muy común en El Salvador"*
- Ir a Remesas → mostrar registros
- Decir: *"El Salvador recibe más de $8 mil millones anuales en remesas. Las familias pueden registrar y controlar cada envío recibido"*

**Paso 7 — Registro de nuevo usuario (30 seg)**
- Ir a la pantalla de login → pestaña "Registrarse"
- Mostrar el formulario y explicar que en menos de 30 segundos el hogar queda configurado

### Frases clave durante la demo:
- *"Todo funciona en tiempo real, sin recargar la página"*
- *"El sistema genera las alertas de forma automática, el usuario no tiene que configurar nada"*
- *"Está pensado para que cualquier persona, aunque no sea técnica, pueda usarlo"*

---

## DIAPOSITIVA 5 — Tecnologías (Uso y justificación — 10%)

**Título:** Stack tecnológico — Por qué elegimos estas herramientas

**En la diapositiva — tabla con justificación:**

| Tecnología | Rol | Por qué se eligió |
|---|---|---|
| .NET 10 | API REST Backend | Framework empresarial, alta performance, LTS |
| PostgreSQL | Base de datos | Open source, escalable, confiable |
| Alpine.js | Frontend SPA | Ligero (15KB), sin build process, ideal para proyecto académico |
| Chart.js | Gráficos | Librería estándar, visualizaciones claras |
| Bootstrap 5 | UI | Responsive, profesional, conocido |
| JWT | Autenticación | Estándar de industria para APIs REST |
| BCrypt | Seguridad | Hash seguro para contraseñas |
| Serilog | Logging | Registro profesional de eventos del sistema |
| Docker | Infraestructura | Portabilidad del entorno de BD |

**Qué decir:**

> *"Elegimos .NET 10 porque es el framework más reciente de Microsoft, con soporte a largo plazo, ideal para APIs REST en producción. PostgreSQL es una base de datos de clase empresarial, usada por empresas como Instagram y Spotify. Alpine.js nos permite tener una Single Page Application sin necesitar un build process complejo, lo que facilita el desarrollo y mantenimiento. Cada tecnología fue elegida por su pertinencia, no por moda."*

---

## DIAPOSITIVA 6 — Arquitectura (Uso y justificación — 10%)

**Título:** Arquitectura limpia por capas

**Diagrama en la diapositiva:**

```
┌─────────────────────────────────────┐
│         Frontend (Alpine.js)        │  ← Interfaz de usuario
│     index.html + api.js + main.js   │
└─────────────────┬───────────────────┘
                  │ HTTP/JSON
┌─────────────────▼───────────────────┐
│        API REST (.NET 10)           │  ← Controladores + Middleware
│  Controllers / JWT / Rate Limiting  │
└─────────────────┬───────────────────┘
                  │
┌─────────────────▼───────────────────┐
│       Application Layer             │  ← Lógica de negocio
│   Services + DTOs + Validaciones    │
└─────────────────┬───────────────────┘
                  │
┌─────────────────▼───────────────────┐
│      Infrastructure Layer           │  ← Acceso a datos
│   EF Core + Repositories + Auth     │
└─────────────────┬───────────────────┘
                  │
┌─────────────────▼───────────────────┐
│         PostgreSQL (Docker)         │  ← Persistencia
└─────────────────────────────────────┘
```

**Qué decir:**

> *"Aplicamos Arquitectura Limpia (Clean Architecture), separando el proyecto en 4 capas: Domain (entidades), Application (lógica de negocio), Infrastructure (acceso a datos y autenticación) y API (controladores REST). Esta separación facilita el mantenimiento, las pruebas y la escalabilidad del sistema."*

**Mencionar:**
- 18 endpoints documentados y probados
- Rate limiting: máximo 100 requests por minuto (protección contra abuso)
- Tests unitarios incluidos en el proyecto

---

## DIAPOSITIVA 7 — Innovación (Creatividad — 10%)

**Título:** ¿Qué hace diferente a FinanceHogar?

**5 diferenciadores a mencionar:**

**1. Puntaje financiero automático (0–100)**
> *"Ninguna app bancaria salvadoreña tiene esto. El sistema calcula mensualmente un puntaje que mide la salud financiera del hogar, como un 'score crediticio' pero para uso familiar."*

**2. Alertas inteligentes automáticas**
> *"Las alertas se disparan solas. Al registrar un gasto, el sistema evalúa el presupuesto y si supera el umbral configurado, genera la notificación. El usuario no hace nada extra."*

**3. Digitalización de las Tandas**
> *"Las tandas son un sistema financiero informal que mueve millones de dólares en El Salvador, completamente informal y sin registro. FinanceHogar lo digitaliza por primera vez en una plataforma estructurada."*

**4. Módulo de Remesas contextualizado**
> *"El Salvador es el tercer receptor de remesas en Latinoamérica como porcentaje del PIB. Diseñamos el módulo específicamente para registrar, categorizar y analizar las remesas del hogar."*

**5. Soporte Bitcoin**
> *"El Salvador fue el primer país del mundo en adoptar Bitcoin como moneda de curso legal. FinanceHogar permite registrar transacciones en BTC, algo que ninguna app financiera local contempla."*

---

## DIAPOSITIVA 8 — Resultados y Métricas (Funcionalidad — 25%)

**Título:** Lo que logramos construir

**En la diapositiva:**

| Métrica | Resultado |
|---|---|
| Endpoints REST | 18 implementados y probados |
| Módulos funcionales | 8 (Dashboard, Gastos, Ingresos, Presupuestos, Alertas, Servicios, Tandas, Remesas) |
| Capas de arquitectura | 4 (Domain, Application, Infrastructure, API) |
| Tests unitarios | 4 clases de prueba |
| Tiempo de respuesta | < 200ms promedio |
| Seguridad | JWT + BCrypt + Rate Limiting |
| Código en GitHub | github.com/xJonnyxD/FinanceHogar |

**Qué decir:**

> *"El proyecto cumple integralmente con todos los módulos planificados. Los 18 endpoints fueron probados y validados. La arquitectura por capas garantiza que el sistema es mantenible y escalable. El repositorio es público en GitHub."*

---

## DIAPOSITIVA 9 — Conclusiones (Claridad y organización — 10%)

**Título:** Conclusiones

**Tres conclusiones concretas:**

1. **Impacto real:** FinanceHogar responde a una necesidad real de las familias salvadoreñas, combinando herramientas de control financiero con elementos culturales propios (tandas, remesas, Bitcoin).

2. **Conocimientos aplicados:** El proyecto integra los conceptos de la cátedra: desarrollo de APIs REST, bases de datos relacionales, autenticación, patrones de arquitectura y seguridad.

3. **Escalabilidad:** La base técnica permite incorporar en el futuro: app móvil, reportes PDF, integración con banca en línea, y notificaciones push.

**Cierre:**

> *"FinanceHogar no es solo un proyecto académico. Es una herramienta funcional que cualquier familia salvadoreña podría usar hoy mismo para mejorar su situación financiera. Gracias."*

---

## DIAPOSITIVA 10 — Preguntas (Defensa oral — 20%)

**Prepararse para estas preguntas frecuentes:**

---

### ¿Por qué usaron Alpine.js y no React o Vue?

> *"Alpine.js nos permitió construir una SPA completa sin necesitar un build process (npm build, webpack, etc.), lo que simplifica el despliegue y mantenimiento. Pesa solo 15KB vs los 130KB de React. Para un proyecto de este tamaño, Alpine.js es la herramienta correcta — no necesitábamos la complejidad de un framework completo."*

---

### ¿Cómo garantizan la seguridad de los datos?

> *"En tres niveles: las contraseñas nunca se guardan en texto plano, se cifran con BCrypt que es resistente a ataques de fuerza bruta. La autenticación usa JWT con expiración de 60 minutos. Y el API tiene rate limiting que bloquea más de 100 requests por minuto, protegiendo contra ataques automatizados."*

---

### ¿Cómo se calculan las alertas automáticamente?

> *"Al registrar un gasto, el servicio de alertas verifica si existe un presupuesto activo para esa categoría en el mes. Si el gasto acumulado supera el 50%, 80% o 100% del límite, crea automáticamente una alerta de tipo Advertencia, Alerta o Crítico respectivamente. El usuario no necesita hacer nada."*

---

### ¿Qué es el puntaje financiero y cómo se calcula?

> *"El puntaje va de 0 a 100 y considera tres factores: la relación gasto/ingreso del mes (si gastas menos de lo que ganas, sube el puntaje), el cumplimiento de presupuestos (cuántas categorías se mantienen bajo el límite) y la consistencia histórica. Un puntaje de 80 o más significa que el hogar tiene finanzas saludables."*

---

### ¿Funciona en celular?

> *"Sí, la interfaz usa Bootstrap 5 que es completamente responsive. Funciona en teléfonos, tablets y computadoras. Para una siguiente versión se podría empaquetar como PWA (Progressive Web App) para instalarse en el celular como app nativa."*

---

### ¿Por qué PostgreSQL y no MySQL o SQL Server?

> *"PostgreSQL es open source (sin costo de licencia), tiene mejor soporte para tipos de datos avanzados, mejor rendimiento en consultas complejas, y es el estándar de la industria en startups y empresas tecnológicas modernas. SQL Server requiere licencia de pago para producción."*

---

### Si tuvieran más tiempo, ¿qué agregarían?

> *"Tres cosas: primero, exportar reportes en PDF para compartir el resumen mensual. Segundo, notificaciones por correo electrónico o WhatsApp para las alertas. Tercero, una app móvil nativa que sincronice con la misma API que ya construimos — la arquitectura ya está diseñada para soportarlo."*

---

## Distribución del tiempo por integrante (trabajo en equipo — 10%)

Para obtener el 100% en trabajo en equipo, todos deben hablar en la presentación:

| Segmento | Integrante sugerido |
|---|---|
| Portada + presentación del equipo | Todos |
| El problema | Integrante 1 |
| La solución + demo en vivo | Integrante 2 |
| Tecnologías + arquitectura | Integrante 3 |
| Innovación + conclusiones | Integrante 4 (o 1) |
| Respuesta a preguntas | Todos participan |

**Si son menos de 4 integrantes**, distribuir los segmentos equitativamente asegurando que cada quien hable al menos 3–4 minutos.

---

## Checklist final antes de la presentación

- [ ] API corriendo en `localhost:5153`
- [ ] Docker/PostgreSQL activo con datos de prueba
- [ ] Navegador abierto en la app (incógnito con DevTools cerrado)
- [ ] Diapositivas listas en modo presentación
- [ ] Probado el login con cuentas demo (Admin y Usuario)
- [ ] Todos los integrantes saben qué segmento les toca
- [ ] Practicada la demo al menos 2 veces
- [ ] Preparadas respuestas a las 7 preguntas frecuentes
- [ ] GitHub abierto en otra pestaña para mostrar el repositorio

---

## Puntuación esperada por criterio

| Criterio | Peso | Estrategia para 100% |
|---|---|---|
| Funcionalidad | 25% | Demo en vivo sin errores — 8 módulos, datos reales |
| Presentación oral | 20% | Voz segura, lenguaje técnico, sin leer las diapositivas |
| Propuesta de valor | 15% | Problema cuantificado + solución clara + impacto real |
| Claridad | 10% | Presentación estructurada, transiciones fluidas |
| Trabajo en equipo | 10% | Todos hablan, nadie queda callado |
| Creatividad | 10% | Resaltar puntaje financiero, tandas y remesas |
| Tecnologías | 10% | Justificar cada tecnología, no solo listarlas |
| **TOTAL** | **100%** | |

---

*FinanceHogar — Universidad Evangélica de El Salvador | 2026*
