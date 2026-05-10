# ❓ PREGUNTAS FRECUENTES - Defensa FinanceHogar

## 🎯 Objetivo
Estar preparado para responder con **confianza, claridad y datos** cualquier pregunta que el tribunal haga.

---

## CATEGORÍA 1: PROBLEMA & SOLUCIÓN

### P1: ¿Cuál es exactamente el problema que resuelve FinanceHogar?

**Respuesta Esperada:**
> Familias salvadoreñas no tienen una herramienta integrada para gestionar finanzas en su contexto local. Reciben remesas del exterior, participan en tandas, tienen ingresos informales, y usan apps gringas que no contemplan estas realidades. Resultado: 70% de familias desconocen su situación financiera real.

**Datos para citar:**
- 30% de familias dependen de remesas ($8B anuales)
- 60% con ingresos informales
- Tandas = fenómeno único Centroamérica (sin equivalente en apps internacionales)

---

### P2: ¿Por qué no usar una app como Spei, Rivalo o WAVE?

**Respuesta Esperada:**
> Esas apps son buenas para presupuesto básico, pero **no contemplan la realidad salvadoreña**:

| Aspecto | FinanceHogar | Competencia |
|--------|---|---|
| Tandas | ✅ Sistema rotativo completo | ❌ No existe |
| Remesas | ✅ Crea ingreso automático | Algunas tienen, pero genérico |
| Servicios locales (ANDA, AES) | ✅ Específicos | ❌ Genéricos |
| Puntaje Familiar | ✅ 0-100 gamificado | ❌ No existe |
| DUI validation | ✅ Salvadoreño | ❌ No existe |

> Además, FinanceHogar es **open source y API REST** → flexible para integraciones con bancos locales.

---

### P3: ¿Cuántas personas usarían esto realmente?

**Respuesta Esperada:**
> Mercado potencial: ~1 millón de familias salvadoreñas.

**Segmentos:**
1. **Receptores de remesas**: ~300K familias
2. **Emprendedores informales**: ~500K familias
3. **Trabajadores formales con disciplina**: ~200K familias

**Monetización (futuro)**:
- Freemium: versión básica gratis, premium ($2-5/mes)
- B2B2C: Cooperativas / Cajas de crédito pagan por FinanceHogar
- 1% de penetración = 10K usuarios × $3/mes = $30K/mes = $360K/año

---

## CATEGORÍA 2: FUNCIONALIDAD & FEATURES

### P4: ¿El proyecto está 100% funcional?

**Respuesta Esperada:**
> Sí. MVP completo con:
- ✅ Autenticación (JWT + Refresh Token)
- ✅ 10 módulos (Auth, Hogares, Ingresos, Gastos, Presupuestos, Alertas, Servicios Básicos, Remesas, Reportes, Puntaje)
- ✅ Swagger completo
- ✅ Base de datos (PostgreSQL con migraciones)

> **Nota**: Removimos el módulo de Tandas del MVP inicial porque requería lógica rotativa compleja. Se implementará en V1.0.

---

### P5: ¿Cómo funcionan las alertas automáticas?

**Respuesta Esperada:**
> En tiempo real, cuando registras un gasto:

1. Se descuenta del presupuesto de la categoría
2. Se calcula el % de uso (gasto / presupuesto)
3. Si cruza 50%, 80% o 100% → **se genera una alerta automáticamente**
4. La alerta se devuelve en la respuesta del API

**Ejemplo:**
```
POST /api/v1/gastos
{
  "monto": 50,
  "categoriaId": "Alimentacion",
  "hogarId": "abc123"
}

Respuesta:
{
  "gasto": { ... },
  "presupuesto": {
    "montoAsignado": 100,
    "montoGastado": 80,
    "porcentajeUso": 80
  },
  "alertaGenerada": {
    "tipo": "PRESUPUESTO_80",
    "mensaje": "Ya gastaste el 80% de tu presupuesto de Alimentación"
  }
}
```

---

### P6: ¿Cómo se calcula el Puntaje Financiero?

**Respuesta Esperada:**
> Algoritmo con 5 criterios ponderados:

```
Puntaje = (
  (PresupuestoCumplimiento / 100) * 0.30 +
  (TasaAhorro / 100) * 0.25 +
  (PuntualidadServicios / 100) * 0.20 +
  (DiversidadGastos / 100) * 0.15 +
  (EstabilidadIngresos / 100) * 0.10
) * 100
```

**Ejemplo familia**:
- Cumplió presupuesto al 90% → 27 pts
- Ahorró 15% de ingresos → 3.75 pts
- Pagó servicios a tiempo → 20 pts
- Gasta en 6+ categorías → 15 pts
- Ingresos estables (salario mensual) → 10 pts
- **Total: 75.75/100** → Buena situación financiera

---

### P7: ¿Qué pasa con los datos sensibles (ingresos, gastos)?

**Respuesta Esperada:**
> Seguridad en múltiples niveles:

1. **En tránsito**: HTTPS (SSL/TLS)
2. **Autenticación**: JWT con expiración (60 min)
3. **Base de datos**: PostgreSQL con encriptación de columnas sensibles
4. **Aislamiento**: Cada hogar ve solo sus datos (filtro en queries)
5. **Soft delete**: Nunca se borran registros, solo se marcan como eliminados

> Además, `appsettings.Development.json` (con credenciales) está `.gitignored` → **no se versionan secretos**.

---

## CATEGORÍA 3: ARQUITECTURA & TECNOLOGÍA

### P8: ¿Por qué elegiste .NET 10 en lugar de Node.js, Python o Java?

**Respuesta Esperada:**
> 3 razones:

1. **Seguridad**: C# es fuertemente tipado (Type Safety) → errores en tiempo de compilación vs runtime
   - Ej: Un parámetro de dinero no puede ser string accidentalmente

2. **Rendimiento**: .NET 10 es 2-3x más rápido que Node.js en CPU-bound tasks (cálculo de puntaje, alertas)

3. **Clean Architecture**: EF Core + Dependency Injection nativa → separación clara de responsabilidades

**Comparativa:**
| Aspecto | .NET | Node.js | Python |
|--------|-----|---------|--------|
| Type safety | ✅ Strong | ⚠️ TS optional | ⚠️ Optional |
| ORM quality | ✅ EF Core | ⚠️ Sequelize/TypeORM | ✅ SQLAlchemy |
| Performance | ✅ Alto | ⚠️ Medio | ❌ Bajo |
| Learning curve | ⚠️ Medio-alto | ✅ Bajo | ✅ Bajo |

> Para finanzas (donde errores son caros), **elegí seguridad sobre velocidad de desarrollo**.

---

### P9: ¿Escalabilidad? ¿Qué pasa si tienes 100K usuarios?

**Respuesta Esperada:**
> Arquitectura escala verticalmente (hardware mejor) y horizontalmente (múltiples servidores):

**DB:**
- PostgreSQL soporta 10K+ conexiones concurrentes
- Índices en tablas críticas (presupuestos, alertas)
- Connection pooling en aplicación

**API:**
- Stateless: cada request es independiente
- Corre en múltiples servidores detrás de load balancer
- Caching de datos de referencia (categorías globales)

**Rate limiting:**
- 100 peticiones/minuto por IP → evita abuso

**Futuro:**
- Redis para cache de puntajes (cálculo pesado)
- ElasticSearch para históricos (búsquedas rápidas)

---

### P10: ¿Testing? ¿Hay pruebas unitarias?

**Respuesta Esperada:**
> [AQUÍ DEPENDE DE SI TIENEN TESTS REALES]

**Si SÍ tienen**:
> Cobertura en servicios críticos:
- ✅ Validadores (FluentValidation) → 15 tests
- ✅ AlertaService → 8 tests (50%, 80%, 100% y combos)
- ✅ PuntajeFinancieroCalculator → 6 tests
- ✅ RepositoriesTest → 5 tests

> Framework: xUnit + Moq

**Si NO tienen (RECOMENDADO AGREGAR)**:
> "En el proyecto MVP nos enfocamos en funcionalidad. El testing lo agregamos en V1.0 post-defensa con:
- Unit tests para servicios críticos (alertas, puntaje)
- Integration tests para API endpoints
- Load tests con K6"

---

## CATEGORÍA 4: TRABAJO EN EQUIPO

### P11: ¿Quién hizo qué en el equipo?

**Respuesta Esperada:**
> [PERSONALIZAR CON NOMBRES REALES]

**Ejemplo:**
- **Juan (Backend)**: Controladores, servicios, base de datos, migraciones
- **María (Frontend)**: [Si aplica] Swagger UI enhancements, documentación
- **Pedro (QA/DevOps)**: Testing, documentación API, deployment

> Cada uno escribió código específico (verificable en `git log`).

---

### P12: ¿Cómo se distribuyó el trabajo?

**Respuesta Esperada:**
> Usamos:
- Git branches por feature
- Pull requests con code review
- Commits descriptivos

> Aquí está el breakdown por commits:
```
git shortlog -sn
   45 Juan Rodriguez (Backend)
   28 Maria Garcia (Frontend/Docs)
   12 Pedro Martinez (Testing)
```

---

## CATEGORÍA 5: INNOVACIÓN & CREATIVIDAD

### P13: ¿Qué hace a FinanceHogar diferente/innovador?

**Respuesta Esperada:**
> 5 diferenciadores únicos (no existen en Spei, Rivalo, WAVE, etc.):

1. **Tandas** → Sistema rotativo de ahorro colectivo (único Centroamérica)
2. **Remesas automáticas** → Al registrar, crea ingreso automáticamente
3. **Puntaje Familiar 0-100** → Gamificación para motivar mejores decisiones
4. **Servicios Básicos locales** → ANDA, AES, Claro, Tigo (no apps gringas)
5. **DUI validation** → Validación de Documento Único Identidad salvadoreño

> Especialmente #1 y #3 son novedosos en el mercado Latinoamericano.

---

### P14: ¿Qué es lo más difícil que resolviste técnicamente?

**Respuesta Esperada:**
> [PERSONALIZAR]

**Ejemplos posibles:**
1. **Alertas en cascada**: Cuando gastas, actualiza presupuesto + evalúa 3 umbrales (50%, 80%, 100%) + genera alerta en tiempo real
   - Solución: Servicio transaccional, una operación atómica

2. **Puntaje Financiero**: Cálculo complejo con 5 variables + validaciones
   - Solución: Servicio separado que se recalcula mensualmente

3. **Soft Delete global**: Eliminar es lógico, no físico
   - Solución: Base entity + interceptor EF Core que filtra automáticamente

---

## CATEGORÍA 6: CRÍTICAS & LIMITACIONES

### P15: ¿Cuáles son las limitaciones de FinanceHogar?

**Respuesta Esperada:**
> Sé honesto:

1. **No hay app móvil** (MVP web/API only)
   - Roadmap: Flutter V1.0 (3 meses post-defensa)

2. **No integra con bancos aún** (datos se ingresan manualmente)
   - Roadmap: Open Banking El Salvador (6 meses)

3. **Tandas está simplificada** (lógica rotativa básica)
   - Roadmap: Versión completa con splits automáticos

4. **Sin reportes PDF exportables** (datos en JSON only)
   - Roadmap: V1.0

> **Pero**: Para un MVP de proyecto universidad, estos son trade-offs aceptables. Prioridad #1 fue funcionalidad core + seguridad.

---

### P16: ¿Qué hiciste diferente de otros proyectos de clase?

**Respuesta Esperada:**
> [PERSONALIZAR]

**Ejemplos:**
- Elegimos .NET (menos común en clase) porque es mejor para finanzas
- Implementamos Clean Architecture (real, no copy-paste)
- Incluimos algoritmo de Puntaje (novedoso, necesitó research)
- Adaptamos a contexto local salvadoreño (tandas, remesas, DUI)
- Documentación Swagger + README + Propuesta de Valor + Roadmap

---

## CATEGORÍA 7: FUTURO & VISIÓN

### P17: ¿Qué sigue después de la defensa?

**Respuesta Esperada:**
> Roadmap claro:

**V1.0 (3 meses)**:
- [ ] App móvil (Flutter)
- [ ] Exportar reportes (PDF/Excel)
- [ ] Integración Open Banking
- [ ] Chat soporte

**V2.0 (6+ meses)**:
- [ ] Bitcoin (El Salvador es único)
- [ ] Marketplace de seguros/créditos
- [ ] Predicción con ML
- [ ] Certificaciones financieras

**Monetización:**
- Freemium ($2-5/mes premium)
- B2B2C con cooperativas
- APIs de datos anonimizados

---

### P18: ¿Cómo impacta esto a familias salvadoreñas?

**Respuesta Esperada:**
> **Visión:**

Queremos que 1 millón de familias salvadoreñas:
1. **Visualicen** sus finanzas reales
2. **Controlen** sus gastos con alertas
3. **Ahorren estructuradamente** (tandas)
4. **Mejoren** su puntaje financiero (gamificación)
5. **Accedan** a herramientas que bancos no les ofrecen (inclusión financiera)

> Resultado: Mejor toma de decisiones → más ahorro → desarrollo económico local.

---

## 📋 CHECKLIST DE RESPUESTAS

Antes de la defensa, practica respondiendo estas 18 preguntas en voz alta:

- [ ] P1-3: Problema & Solución (impecable)
- [ ] P4-7: Funcionalidad & Features (demo mental)
- [ ] P8-10: Arquitectura & Tech (confianza)
- [ ] P11-12: Trabajo en Equipo (nombres + roles)
- [ ] P13-16: Innovación & Críticas (honestos)
- [ ] P17-18: Futuro (visión clara)

**Timing**: ~15-20 minutos hablando + 5-10 min demo + 5-10 min preguntas = **25-40 minutos totales**.

---

## 🎤 TIPS FINALES

1. **NO digas "eh", "um", "osea"** → practica hasta que sea natural
2. **Mantén contacto visual** con los evaluadores
3. **Sonríe** cuando hables del impacto
4. **Si no sabes, di la verdad**: "Excelente pregunta, no lo tenemos documentado pero..."
5. **Cierra cada pregunta con certeza**, no con duda
