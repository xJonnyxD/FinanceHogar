# 🎤 SCRIPT DE PRESENTACIÓN - FinanceHogar

**Duración total: 8-10 minutos**  
**Estructura: Problema → Solución → Diferenciadores → Demo → Conclusión**

---

## ⏱️ 0:00-1:00 | INTRODUCCIÓN & PROBLEMA (1 minuto)

### Qué decir:

> **Buenos días/tardes. Mi nombre es [TU NOMBRE] y presento FinanceHogar, una plataforma de control financiero diseñada específicamente para familias salvadoreñas.**
> 
> El problema que resolvemos es simple pero crítico: **70% de familias salvadoreñas desconoce su situación financiera real.** 
> 
> ¿Por qué? Porque las herramientas que existen (Spei, WAVE, Rivalo) están hechas para contextos gringos. No contemplan las realidades de nuestro país:
> 
> - **30% de familias dependen de remesas** del exterior (~$8 mil millones anuales)
> - **60% tiene ingresos informales** (vendedoras, trabajos eventuales)
> - **Las tandas** son un fenómeno único de Centroamérica (sin equivalente en apps mundiales)
> 
> Resultado: Una familia salvadoreña gana por múltiples fuentes, **NO sabe cuánto gasta realmente**, **NO puede ahorrar estructuradamente**, y **NO tiene alertas** cuando sus gastos salen de control.

### Tips de entrega:
- 🎯 Mira a los evaluadores
- 🗣️ Habla lentamente (1 pausa de 2 segundos después de "problema crítico")
- 😊 Sonríe cuando mencionas El Salvador (muestra orgullo)

---

## ⏱️ 1:00-3:00 | SOLUCIÓN: QUÉ ES FINANCHOGAR (2 minutos)

### Qué decir:

> **FinanceHogar es una plataforma única que integra TODO lo que una familia salvadoreña necesita en un solo lugar.**
> 
> Tiene 10 funcionalidades clave:
> 
> 1. **Registra tus ingresos** — salarios, remesas, trabajos informales, todo en un lugar
> 2. **Registra tus gastos** — y automáticamente actualiza tu presupuesto
> 3. **Recibe alertas inteligentes** — cuando llegas al 50%, 80% o 100% de tu presupuesto
> 4. **Gestiona servicios básicos** — agua, luz, internet con alertas de vencimiento (ANDA, AES, Claro, Tigo)
> 5. **Participa en tandas** — clubes de ahorro rotativos
> 6. **Ve reportes en tiempo real** — balance mensual, tendencias
> 7. **Obtén un puntaje financiero familiar** — de 0 a 100, para gamificar mejores decisiones
> 8. **Acceso multi-miembro** — toda la familia puede ver el estado (con roles: Admin, Miembro)
> 9. **Seguridad de datos** — cifrado, autenticación JWT, soft delete
> 10. **API abierta** — para futuras integraciones con bancos salvadoreños
> 
> **Lo importante: TODO integrado. Una sola app. Una sola contraseña.**

### Tips de entrega:
- 🎯 Usa tus dedos para contar (1, 2, 3...)
- 🗣️ Pausa de 3 segundos después de "una sola app"
- 💪 Énfasis en "TODO integrado"

---

## ⏱️ 3:00-5:30 | DIFERENCIADORES ÚNICOS (2.5 minutos)

### Qué decir:

> **Ahora, ¿qué hace a FinanceHogar diferente de Spei, Rivalo y WAVE?**
> 
> **Diferenciador #1: Tandas**
> 
> Una tanda es un club de ahorro rotativo. 10 personas aportan $10/mes, **una recibe $100 cada mes (rotando)**. Es cultura salvadoreña pura, única en Centroamérica. Ninguna app global tiene esto. FinanceHogar sí.
> 
> **Diferenciador #2: Remesas automáticas**
> 
> Cuando registras una remesa, el sistema **crea automáticamente un ingreso**. Perfecto para familias que reciben dinero del exterior mensualmente. Otras apps NO lo hacen automático.
> 
> **Diferenciador #3: Servicios básicos locales**
> 
> ANDA, AES, Claro, Tigo, ANTEL. Las categorías están precargadas. Apps gringas te dan "Utilities" genérico. Nosotros entendemos tu realidad.
> 
> **Diferenciador #4: Puntaje Financiero Familiar**
> 
> Un algoritmo que calcula un score de 0 a 100 basado en:
> - 30% en cumplimiento de presupuesto
> - 25% en tasa de ahorro
> - 20% en puntualidad de servicios básicos
> - 15% en diversidad de gastos
> - 10% en estabilidad de ingresos
> 
> **Gamificación**: Las familias pueden competir entre sí. "Mi puntaje es 78, voy a subirlo". Eso motiva mejores decisiones.
> 
> **Diferenciador #5: DUI Validation**
> 
> Solo en El Salvador: Validamos el Documento Único de Identidad. No es cosa de magia, es validación de formato (########-#) pero es detalles como este que muestran que FinanceHogar fue hecha POR salvadoreños PARA salvadoreños.

### Tips de entrega:
- 🎯 Levanta la mano derecha cuando dices "Diferenciador #1", cíclicamente
- 🗣️ Haz pausas dramáticas entre diferenciadores (2-3 segundos)
- 😊 Sonríe cuando explicas Tandas (es lo más local)
- 💪 Énfasis en "PARA salvadoreños"

---

## ⏱️ 5:30-7:00 | ARQUITECTURA & TECNOLOGÍA (1.5 minutos)

### Qué decir:

> **¿Cómo está construido FinanceHogar?**
> 
> Usamos **Clean Architecture** — separación en 4 capas:
> 
> - **Domain**: Entidades del negocio (Usuarios, Hogares, Gastos, Presupuestos)
> - **Application**: Servicios de lógica (AlertaService, PuntajeService)
> - **Infrastructure**: Base de datos (PostgreSQL), autenticación (JWT)
> - **API**: Controllers REST (JSON in/out)
> 
> **Stack tecnológico:**
> - Backend: .NET 10 (elegimos esto porque es seguro, rápido, y perfectamente tipado)
> - BD: PostgreSQL 18
> - ORM: Entity Framework Core 10 (migraciones automáticas)
> - Autenticación: JWT Bearer + Refresh Tokens
> - Documentación: Swagger/OpenAPI (API completamente documentada)
> - Validación: FluentValidation (15 validadores personalizados)
> - Logging: Serilog (trazas de lo que pasa en producción)
> 
> **¿Por qué .NET?** Porque para finanzas, **seguridad > velocidad**. C# es fuertemente tipado. Errores de tipos se detectan en compilación, no en runtime. Un monto de dinero no puede ser un string accidentalmente.

### Tips de entrega:
- 🎯 Cuenta las 4 capas con dedos
- 🗣️ Pausa después de "Clean Architecture"
- 💪 Énfasis en "seguridad > velocidad" (critico para finanzas)

---

## ⏱️ 7:00-8:00 | DEMO EN VIVO (1 minuto)

### Qué hacer:

**Abre el proyecto corriendo en localhost:5153**

> **Vamos a ver FinanceHogar en acción. Les muestro un flujo real:**
> 
> 1. **Registro** — Creo un usuario, se crea automáticamente un hogar
> 2. **Ingreso** — Registro un salario de $500
> 3. **Presupuesto** — Asigno $100 a Alimentación, $50 a Transporte
> 4. **Gasto** — Registro un gasto de $80 en Alimentación (presupuesto ahora 80%)
> 5. **Alerta** — El sistema genera una alerta: "Ya gastaste 80% de tu presupuesto de Alimentación"
> 6. **Reportes** — Veo balance mensual y mi puntaje familiar actual (ej: 72/100)

### Tips de entrega:
- 🎯 Haz clic lentamente (deja tiempo para ver)
- 🗣️ Narrador mientras haces click (no silencio)
- 🛑 Si algo falla, di con calma: "A veces hay demoras de red, pero el flujo es este"

---

## ⏱️ 8:00-9:00 | IMPACTO & CONCLUSIÓN (1 minuto)

### Qué decir:

> **¿Cuál es el impacto de FinanceHogar?**
> 
> Mercado potencial: **~1 millón de familias salvadoreñas**.
> 
> Si 10% adopta FinanceHogar (100K familias), y pagan $3/mes (modelo freemium):
> - Ingresos: $3.6M anuales
> - Pero más importante: **100K familias con mejor control de sus finanzas**
> 
> Eso significa:
> - 👨‍👩‍👧‍👦 Mejor toma de decisiones
> - 💰 Más ahorro nacional
> - 🎓 Mejor educación financiera
> - 🚀 Desarrollo económico
> 
> **En conclusión:**
> 
> FinanceHogar no es solo una app. Es una herramienta de **inclusión financiera** pensada para la realidad salvadoreña. 
> 
> Tenemos arquitectura sólida, features únicos, y roadmap claro. 
> 
> Creemos que en 12 meses, FinanceHogar puede ser la herramienta financiera #1 para familias salvadoreñas.
> 
> **Gracias.**

### Tips de entrega:
- 🎯 Mira a cada evaluador al menos una vez
- 🗣️ Pausa de 3 segundos antes de "Gracias"
- 😊 Sonríe (transmite confianza)
- 🙌 Puedes hacer un gesto de cierre (manos unidas o levantadas)

---

## ⏱️ TIMELINE TOTAL

| Parte | Tiempo | Cumulative |
|-------|--------|-----------|
| Intro & Problema | 1:00 | 1:00 |
| Solución | 2:00 | 3:00 |
| Diferenciadores | 2:30 | 5:30 |
| Arquitectura | 1:30 | 7:00 |
| Demo | 1:00 | 8:00 |
| Impacto & Cierre | 1:00 | 9:00 |
| **TOTAL** | **9:00** | |

> ✅ Objetivo: 8-10 minutos → **9 minutos exactos**

---

## 🎯 CHECKLIST ANTES DE PRESENTAR

Practica esto en voz alta 5+ veces:

- [ ] Pronunciación clara (sin "eh", "um", "osea")
- [ ] Pausas naturales (respira entre secciones)
- [ ] Contacto visual (mira a los 3 evaluadores rotativamente)
- [ ] Timing (crono: ¿cuánto tardó realmente?)
- [ ] Sonrisa al final (confianza)
- [ ] Demo funciona (probalo 2 horas antes)

---

## 💡 VARIACIONES SEGÚN PREGUNTAS

Si preguntan algo durante la presentación:

**"¿Cómo manejan la seguridad?"**
→ "Excelente pregunta. JWT con expiración automática cada 60 minutos, datos encriptados en tránsito y en repo. Además soft delete — nunca borramos datos reales, solo los marcamos como eliminados."

**"¿Escalabilidad?"**
→ "Pensamos en esto desde el inicio. La arquitectura es stateless — corre en múltiples servidores. PostgreSQL soporta miles de usuarios. Futuro: Redis para cache + ElasticSearch para búsquedas."

**"¿Tandas qué tan complejo es?"**
→ "En MVP es simplificado — gestión básica del grupo. V1.0 incluye rotación automática, splits de contribuciones, todo inteligente."

---

## 🎬 ÚLTIMOS TIPS

1. **Ensaya frente a un espejo** (ves tus gestos)
2. **Graba tu voz** (escucha tus "eh" y "um")
3. **Practica con amigos** (feedback externo)
4. **Duerme bien la noche anterior** (claridad mental)
5. **Llega 15 min antes** (respira, relaja)
6. **Tu primer párrafo debe ser perfecto** (sienta la audiencia)
7. **Si te equivocas, continúa sin pausar** (los evaluadores ni se dieron cuenta)

---

**¡Mucho éxito en tu defensa! 🚀**
