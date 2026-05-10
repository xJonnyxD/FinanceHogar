# ✅ CHECKLIST VALIDACIÓN TÉCNICA - FinanceHogar

**Antes de la defensa, valida que TODO funciona 100%. Nada arruina una presentación como un error live.**

---

## 🔧 SECCIÓN 1: COMPILACIÓN & EJECUCIÓN

### Paso 1: Limpiar y compilar
```bash
cd src/FinanceHogar.API
dotnet clean
dotnet build
```
- [ ] ✅ Sin errores de compilación
- [ ] ✅ Sin warnings (especialmente null-reference)

### Paso 2: Ejecutar la API
```bash
dotnet run --launch-profile http
```
- [ ] ✅ Se inicia sin errores
- [ ] ✅ Puerto es localhost:5153 (verificar en output)
- [ ] ✅ Logs aparecen correctamente (Serilog)

### Paso 3: Acceder a Swagger
Abre: **http://localhost:5153/swagger/index.html**
- [ ] ✅ Swagger carga sin errores
- [ ] ✅ Todos los endpoints están listados
- [ ] ✅ Descripciones de endpoints son claras

---

## 🗄️ SECCIÓN 2: BASE DE DATOS

### Paso 4: Verificar conexión DB
En Swagger, ejecuta: **GET /health**
- [ ] ✅ Responde con status OK
- [ ] ✅ BD está conectada

### Paso 5: Verificar schema
Conéctate a PostgreSQL:
```bash
psql -h localhost -U postgres -d FinanceHogar_Dev
```

Verifica tablas:
```sql
\dt
```
- [ ] ✅ Existen 13 tablas (Users, Hogares, Ingresos, Gastos, etc.)
- [ ] ✅ Soft delete está implementado (columna IsDeleted en cada tabla)
- [ ] ✅ Timestamps están presentes (CreatedAt, UpdatedAt)

---

## 🔐 SECCIÓN 3: AUTENTICACIÓN

### Paso 6: Registrar usuario
**POST** `/api/v1/auth/register`
```json
{
  "email": "test@example.com",
  "password": "Test123!@",
  "nombreHogar": "Mi Familia"
}
```
- [ ] ✅ Responde con JWT token
- [ ] ✅ Usuario se crea en BD
- [ ] ✅ Hogar se crea automáticamente

### Paso 7: Login
**POST** `/api/v1/auth/login`
```json
{
  "email": "test@example.com",
  "password": "Test123!@"
}
```
- [ ] ✅ Retorna accessToken + refreshToken
- [ ] ✅ Tokens son válidos JWTs

### Paso 8: Usar token
Copia el token. En Swagger:
1. Click en "Authorize"
2. Pega: `Bearer <tu_token>`
3. Click "Authorize"
- [ ] ✅ Autorización funciona
- [ ] ✅ Requests posteriores incluyen auth header

---

## 💰 SECCIÓN 4: FUNCIONALIDADES CRÍTICAS

### Paso 9: Registrar Ingreso
**POST** `/api/v1/ingresos`
```json
{
  "monto": 500,
  "descripcion": "Salario mensual",
  "categoriaId": "<uuid_de_Salario>",
  "hogarId": "<uuid_hogar_creado>"
}
```
- [ ] ✅ Se crea ingreso
- [ ] ✅ Aparece en **GET** `/api/v1/ingresos` (con filtros)

### Paso 10: Registrar Presupuesto
**POST** `/api/v1/presupuestos`
```json
{
  "categoriaId": "<uuid_Alimentacion>",
  "montoAsignado": 100,
  "hogarId": "<uuid_hogar>"
}
```
- [ ] ✅ Se crea presupuesto
- [ ] ✅ Monto inicial es 100

### Paso 11: Registrar PRIMER gasto (sin alerta)
**POST** `/api/v1/gastos`
```json
{
  "monto": 30,
  "descripcion": "Compras mercado",
  "categoriaId": "<uuid_Alimentacion>",
  "hogarId": "<uuid_hogar>"
}
```
- [ ] ✅ Se crea gasto
- [ ] ✅ Presupuesto se actualiza (montoGastado = 30)
- [ ] ✅ Porcentaje: 30/100 = 30% (sin alerta)

### Paso 12: Registrar SEGUNDO gasto (genera alerta 80%)
**POST** `/api/v1/gastos`
```json
{
  "monto": 50,
  "descripcion": "Más compras",
  "categoriaId": "<uuid_Alimentacion>",
  "hogarId": "<uuid_hogar>"
}
```
- [ ] ✅ Se crea gasto
- [ ] ✅ Presupuesto actualiza (montoGastado = 80)
- [ ] ✅ **Alerta generada**: Tipo = "PRESUPUESTO_80"
- [ ] ✅ Respuesta incluye objeto alertaGenerada

### Paso 13: Registrar TERCER gasto (genera alerta 100%)
**POST** `/api/v1/gastos`
```json
{
  "monto": 20,
  "descripcion": "Último gasto",
  "categoriaId": "<uuid_Alimentacion>",
  "hogarId": "<uuid_hogar>"
}
```
- [ ] ✅ Se crea gasto
- [ ] ✅ Presupuesto actualiza (montoGastado = 100)
- [ ] ✅ **Alerta generada**: Tipo = "PRESUPUESTO_100"

### Paso 14: Ver Presupuesto vs Real
**GET** `/api/v1/presupuestos/vs-real?hogarId=<uuid>&mes=5&anio=2026`
- [ ] ✅ Muestra Alimentación con:
  - montoAsignado: 100
  - montoGastado: 100
  - porcentajeUso: 100%
- [ ] ✅ Muestra otras categorías sin gastos (0% uso)

### Paso 15: Ver Alertas
**GET** `/api/v1/alertas?hogarId=<uuid>`
- [ ] ✅ Aparecen las 2 alertas (80% y 100%)
- [ ] ✅ Tienen timestamps

---

## 📊 SECCIÓN 5: REPORTES

### Paso 16: Balance Mensual
**GET** `/api/v1/reportes/balance-mensual?hogarId=<uuid>&mes=5&anio=2026`
- [ ] ✅ Muestra:
  - totalIngresos: 500
  - totalGastos: 100
  - balance: 400
  - categorias con desglose

### Paso 17: Puntaje Financiero
**GET** `/api/v1/reportes/puntaje-financiero?hogarId=<uuid>`
- [ ] ✅ Responde con objeto:
  ```json
  {
    "puntaje": 75,
    "criterios": {
      "presupuestoCumplimiento": 70,
      "tasaAhorro": 80,
      "puntualidadServicios": 100,
      "diversidadGastos": 60,
      "estabilidadIngresos": 50
    }
  }
  ```
- [ ] ✅ Puntaje está entre 0-100
- [ ] ✅ Cálculo es consistente (2 llamadas = mismo resultado)

---

## 🔄 SECCIÓN 6: FLUJOS AUTOMÁTICOS

### Paso 18: Remesa → Ingreso Automático
**POST** `/api/v1/remesas`
```json
{
  "monto": 200,
  "procedencia": "USA",
  "hogarId": "<uuid>"
}
```
- [ ] ✅ Remesa se crea
- [ ] ✅ **Automáticamente** se crea un Ingreso con categoría "Remesa"
- [ ] ✅ Ingreso tiene monto = 200

### Paso 19: Servicio Básico → Gasto Automático
**POST** `/api/v1/servicios-basicos/<id>/pagar`
```json
{
  "monto": 25,
  "hogarId": "<uuid>"
}
```
- [ ] ✅ Pago se registra
- [ ] ✅ **Automáticamente** se crea un Gasto
- [ ] ✅ Presupuesto se actualiza
- [ ] ✅ Si corresponde, alerta se genera

---

## 🛡️ SECCIÓN 7: SEGURIDAD

### Paso 20: Validación de datos
**POST** `/api/v1/gastos` (con monto negativo - INVÁLIDO)
```json
{
  "monto": -50,
  "descripcion": "Bad data",
  "categoriaId": "<uuid>",
  "hogarId": "<uuid>"
}
```
- [ ] ✅ Responde con 400 Bad Request
- [ ] ✅ Mensaje explica: "Monto debe ser positivo"

### Paso 21: DUI Validation
**POST** `/api/v1/auth/register` (con DUI inválido)
```json
{
  "email": "test2@example.com",
  "password": "Test123!@",
  "nombreHogar": "Familia 2",
  "dui": "123456"  // Formato incorrecto
}
```
- [ ] ✅ Responde con 400
- [ ] ✅ Mensaje: "DUI debe tener formato ########-#"

### Paso 22: Soft Delete
Crea un gasto, luego intenta eliminarlo:
**DELETE** `/api/v1/gastos/<id>`
- [ ] ✅ Responde con 200 OK
- [ ] ✅ Gasto NO aparece en **GET** `/api/v1/gastos`
- [ ] ✅ En BD, registro tiene IsDeleted = true (no se borró)

### Paso 23: Aislamiento por Hogar
Crea 2 usuarios en 2 hogares diferentes:
- Usuario A: Hogar "Casa 1"
- Usuario B: Hogar "Casa 2"

Usuario A intenta: **GET** `/api/v1/gastos?hogarId=<uuid_hogar_B>`
- [ ] ✅ Responde con 403 Forbidden
- [ ] ✅ NO ve gastos del Hogar B

---

## 📝 SECCIÓN 8: DOCUMENTACIÓN

### Paso 24: Swagger completo
En **http://localhost:5153/swagger/index.html**:
- [ ] ✅ Cada endpoint tiene descripción
- [ ] ✅ Modelos tienen propiedades documentadas
- [ ] ✅ Códigos de respuesta están documentados (200, 400, 403, 500)
- [ ] ✅ Ejemplos de request/response están presentes

### Paso 25: README actualizado
En la raíz del proyecto, archivo **README.md**:
- [ ] ✅ Explica qué es FinanceHogar
- [ ] ✅ Sección "Cómo correr el proyecto"
- [ ] ✅ Requisitos (.NET 10, PostgreSQL 18)
- [ ] ✅ Pasos 1-4 están claros
- [ ] ✅ Link a Swagger incluido

---

## 🎯 SECCIÓN 9: DEMO SCRIPT (LO QUE MOSTRARÁS)

### Paso 26: Practica la demo en vivo
Haz esto **exactamente como la harás en la defensa:**

1. **Abre Swagger** (http://localhost:5153/swagger)
2. **Registro** → POST auth/register
3. **Presupuesto** → POST presupuestos (Alimentación, $100)
4. **Gasto 1** → POST gastos ($30)
5. **Gasto 2** → POST gastos ($60) → **Verifica alerta 80%**
6. **Presupuesto vs Real** → GET presupuestos/vs-real
7. **Puntaje** → GET reportes/puntaje-financiero

**Timing**: Todo debe tomar <3 minutos
- [ ] ✅ Cada paso funciona
- [ ] ✅ Respuestas son claras (no errores)
- [ ] ✅ Las alertas se generan en tiempo real
- [ ] ✅ Puntaje se calcula correctamente

---

## 🧪 SECCIÓN 10: TESTING (OPCIONAL PERO RECOMENDADO)

Si tienen tests:
```bash
dotnet test
```
- [ ] ✅ Todos los tests pasan
- [ ] ✅ Cobertura de alertas (50%, 80%, 100%)
- [ ] ✅ Cobertura de puntaje
- [ ] ✅ No hay warnings

Si NO tienen tests (está bien para MVP):
- [ ] ✅ Manual testing está completo (pasos 1-26)
- [ ] ✅ Documentan que testing está en roadmap V1.0

---

## 🚀 SECCIÓN 11: DEPLOYMENT (OPCIONAL)

Si van a hacer demo en servidor remoto:

- [ ] ✅ Servidor está up (heroku, azure, tu servidor)
- [ ] ✅ URL pública funciona
- [ ] ✅ BD remota está alimentada con datos de prueba
- [ ] ✅ HTTPS está activo
- [ ] ✅ Tiempos de respuesta < 2 segundos

**Alternativa**: Demo en localhost (preferible, menos variables)
- [ ] ✅ API corre en localhost:5153
- [ ] ✅ Laptop tiene conexión estable
- [ ] ✅ Traes un cable HDMI (por si la sala no tiene WiFi)

---

## 📋 SECCIÓN 12: DÍA DE LA DEFENSA

### 2 horas antes:
- [ ] ✅ Arranca la API (`dotnet run`)
- [ ] ✅ Verifica Swagger carga
- [ ] ✅ Acceso a BD desde PostgreSQL
- [ ] ✅ Crea un usuario de prueba
- [ ] ✅ Corre la demo completa (pasos 1-26)

### 30 minutos antes:
- [ ] ✅ API sigue corriendo sin errores
- [ ] ✅ Swagger sigue responsivo
- [ ] ✅ Demo está lista (no modifiques nada después)
- [ ] ✅ Laptop está conectada a proyector

### EN VIVO:
- [ ] ✅ Abre Swagger en pantalla grande
- [ ] ✅ Haz la demo lentamente (deja que vean cada click)
- [ ] ✅ Si algo falla, di: "A veces hay latencia, pero miren la respuesta JSON aquí"

---

## 🎯 RESULTADO FINAL

Si pasaste TODO este checklist (todos los ✅):

**TU PROYECTO ESTÁ 100% LISTO PARA LA DEFENSA**

✨ Funcionalidad: **25%** → ✅ Excelente  
✨ Propuesta de Valor: **15%** → ✅ Excelente (documentos creados)  
✨ Tecnologías: **10%** → ✅ Excelente (bien justificadas)  
✨ Presentación Oral: **20%** → ✅ Excelente (script + FAQ)  
✨ Claridad/Organización: **10%** → ✅ Excelente (documentación)  
✨ Creatividad/Innovación: **10%** → ✅ Bueno (diferenciadores claros)  

---

**¿Algo no pasó? Documéntalo y arréglalo AHORA, antes de la defensa.** 🛠️
