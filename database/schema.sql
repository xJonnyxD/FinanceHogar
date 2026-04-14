-- =============================================================================
-- FinanceHogar — Plataforma de Control Financiero para el Hogar
-- El Salvador · .NET 10 Web API + PostgreSQL
-- =============================================================================
-- Convención: snake_case (EF Core UseSnakeCaseNamingConvention)
-- Monetario:  DECIMAL(18,2)  |  UUID como PK
-- Soft-delete: is_deleted + deleted_at en cada tabla
-- =============================================================================

\c financehogar;   -- conectarse a la base de datos (cámbialo si usas otro nombre)

-- ---------------------------------------------------------------------------
-- EXTENSIONES
-- ---------------------------------------------------------------------------
CREATE EXTENSION IF NOT EXISTS "pgcrypto";   -- gen_random_uuid()

-- ---------------------------------------------------------------------------
-- ENUMS PostgreSQL (opcionales — EF los mapea como int por defecto)
-- ---------------------------------------------------------------------------
-- Descomenta si prefieres tipos nativos PG en lugar de enteros.
-- CREATE TYPE tipo_ingreso     AS ENUM ('Salario','Remesa','Negocio','Freelance','IngresoInformal','Tanda','Otro');
-- CREATE TYPE tipo_gasto       AS ENUM ('ServicioBasico','Alimentacion','Educacion','Salud','Transporte','Entretenimiento','Ahorro','CuotaTanda','Otro');
-- CREATE TYPE tipo_moneda      AS ENUM ('USD','BTC');
-- CREATE TYPE estado_alerta    AS ENUM ('Pendiente','Leida','Descartada');
-- CREATE TYPE estado_tanda     AS ENUM ('Activa','Pausada','Completada','Cancelada');
-- CREATE TYPE tipo_frecuencia  AS ENUM ('Diaria','Semanal','Quincenal','Mensual','Bimestral','Trimestral','Semestral','Anual');

-- ===========================================================================
-- TABLA: roles
-- ===========================================================================
CREATE TABLE IF NOT EXISTS roles (
    id          UUID        PRIMARY KEY DEFAULT gen_random_uuid(),
    nombre      VARCHAR(50) NOT NULL UNIQUE,
    descripcion TEXT,
    created_at  TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    updated_at  TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    is_deleted  BOOLEAN     NOT NULL DEFAULT FALSE,
    deleted_at  TIMESTAMPTZ
);

-- Seed: roles base del sistema
INSERT INTO roles (id, nombre, descripcion) VALUES
    ('00000000-0000-0000-0000-000000000001', 'Admin',   'Administrador del sistema'),
    ('00000000-0000-0000-0000-000000000002', 'Miembro', 'Miembro del hogar')
ON CONFLICT DO NOTHING;

-- ===========================================================================
-- TABLA: usuarios
-- ===========================================================================
CREATE TABLE IF NOT EXISTS usuarios (
    id                   UUID         PRIMARY KEY DEFAULT gen_random_uuid(),
    nombre_completo      VARCHAR(200) NOT NULL,
    email                VARCHAR(200) NOT NULL UNIQUE,
    password_hash        TEXT         NOT NULL,
    telefono             VARCHAR(20),
    dui                  VARCHAR(10),                    -- Documento Único de Identidad El Salvador (########-#)
    refresh_token        TEXT,
    refresh_token_expiry TIMESTAMPTZ,
    esta_activo          BOOLEAN      NOT NULL DEFAULT TRUE,
    created_at           TIMESTAMPTZ  NOT NULL DEFAULT NOW(),
    updated_at           TIMESTAMPTZ  NOT NULL DEFAULT NOW(),
    is_deleted           BOOLEAN      NOT NULL DEFAULT FALSE,
    deleted_at           TIMESTAMPTZ
);

CREATE INDEX IF NOT EXISTS idx_usuarios_email      ON usuarios(email)        WHERE NOT is_deleted;
CREATE INDEX IF NOT EXISTS idx_usuarios_dui        ON usuarios(dui)          WHERE dui IS NOT NULL AND NOT is_deleted;
CREATE INDEX IF NOT EXISTS idx_usuarios_refresh    ON usuarios(refresh_token) WHERE refresh_token IS NOT NULL;

-- ===========================================================================
-- TABLA: hogares
-- ===========================================================================
CREATE TABLE IF NOT EXISTS hogares (
    id                      UUID          PRIMARY KEY DEFAULT gen_random_uuid(),
    nombre                  VARCHAR(200)  NOT NULL,
    descripcion             TEXT,
    pais                    VARCHAR(100)  NOT NULL DEFAULT 'El Salvador',
    departamento            VARCHAR(100),
    municipio               VARCHAR(100),
    moneda_principal        SMALLINT      NOT NULL DEFAULT 1,  -- 1=USD, 2=BTC
    presupuesto_mensual_total DECIMAL(18,2) NOT NULL DEFAULT 0,
    created_at              TIMESTAMPTZ   NOT NULL DEFAULT NOW(),
    updated_at              TIMESTAMPTZ   NOT NULL DEFAULT NOW(),
    is_deleted              BOOLEAN       NOT NULL DEFAULT FALSE,
    deleted_at              TIMESTAMPTZ
);

CREATE INDEX IF NOT EXISTS idx_hogares_nombre ON hogares(nombre) WHERE NOT is_deleted;

-- ===========================================================================
-- TABLA: hogar_usuarios  (pivot: miembros de un hogar)
-- ===========================================================================
CREATE TABLE IF NOT EXISTS hogar_usuarios (
    id               UUID        PRIMARY KEY DEFAULT gen_random_uuid(),
    hogar_id         UUID        NOT NULL REFERENCES hogares(id)   ON DELETE CASCADE,
    usuario_id       UUID        NOT NULL REFERENCES usuarios(id)  ON DELETE CASCADE,
    es_administrador BOOLEAN     NOT NULL DEFAULT FALSE,
    fecha_ingreso    TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    created_at       TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    updated_at       TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    is_deleted       BOOLEAN     NOT NULL DEFAULT FALSE,
    deleted_at       TIMESTAMPTZ,
    UNIQUE (hogar_id, usuario_id)
);

CREATE INDEX IF NOT EXISTS idx_hogar_usuarios_hogar   ON hogar_usuarios(hogar_id)   WHERE NOT is_deleted;
CREATE INDEX IF NOT EXISTS idx_hogar_usuarios_usuario ON hogar_usuarios(usuario_id) WHERE NOT is_deleted;

-- ===========================================================================
-- TABLA: categorias
-- ===========================================================================
CREATE TABLE IF NOT EXISTS categorias (
    id           UUID         PRIMARY KEY DEFAULT gen_random_uuid(),
    hogar_id     UUID         REFERENCES hogares(id) ON DELETE CASCADE,  -- NULL = categoría global
    nombre       VARCHAR(100) NOT NULL,
    descripcion  TEXT,
    icono        VARCHAR(50),
    es_global    BOOLEAN      NOT NULL DEFAULT FALSE,
    created_at   TIMESTAMPTZ  NOT NULL DEFAULT NOW(),
    updated_at   TIMESTAMPTZ  NOT NULL DEFAULT NOW(),
    is_deleted   BOOLEAN      NOT NULL DEFAULT FALSE,
    deleted_at   TIMESTAMPTZ
);

-- Seed: 14 categorías globales del sistema
INSERT INTO categorias (id, nombre, es_global, icono) VALUES
    ('10000000-0000-0000-0000-000000000001', 'Salario',           TRUE, 'briefcase'),
    ('10000000-0000-0000-0000-000000000002', 'Remesa',            TRUE, 'send'),
    ('10000000-0000-0000-0000-000000000003', 'Negocio Propio',    TRUE, 'store'),
    ('10000000-0000-0000-0000-000000000004', 'Trabajo Informal',  TRUE, 'handshake'),
    ('10000000-0000-0000-0000-000000000005', 'Tanda',             TRUE, 'people'),
    ('10000000-0000-0000-0000-000000000006', 'Alimentacion',      TRUE, 'restaurant'),
    ('10000000-0000-0000-0000-000000000007', 'Educacion',         TRUE, 'school'),
    ('10000000-0000-0000-0000-000000000008', 'Salud',             TRUE, 'medical'),
    ('10000000-0000-0000-0000-000000000009', 'Transporte',        TRUE, 'directions_bus'),
    ('10000000-0000-0000-0000-000000000010', 'Servicios Basicos', TRUE, 'bolt'),
    ('10000000-0000-0000-0000-000000000011', 'Entretenimiento',   TRUE, 'sports_esports'),
    ('10000000-0000-0000-0000-000000000012', 'Ahorro',            TRUE, 'savings'),
    ('10000000-0000-0000-0000-000000000013', 'Cuota Tanda',       TRUE, 'rotate_right'),
    ('10000000-0000-0000-0000-000000000014', 'Otros',             TRUE, 'category')
ON CONFLICT DO NOTHING;

CREATE INDEX IF NOT EXISTS idx_categorias_hogar    ON categorias(hogar_id) WHERE hogar_id IS NOT NULL AND NOT is_deleted;
CREATE INDEX IF NOT EXISTS idx_categorias_global   ON categorias(es_global) WHERE es_global = TRUE;

-- ===========================================================================
-- TABLA: ingresos
-- ===========================================================================
CREATE TABLE IF NOT EXISTS ingresos (
    id            UUID          PRIMARY KEY DEFAULT gen_random_uuid(),
    usuario_id    UUID          NOT NULL REFERENCES usuarios(id),
    hogar_id      UUID          NOT NULL REFERENCES hogares(id),
    categoria_id  UUID          NOT NULL REFERENCES categorias(id),
    remesa_id     UUID,                                               -- FK a remesas (se agrega al final)
    monto         DECIMAL(18,2) NOT NULL CHECK (monto > 0),
    moneda        SMALLINT      NOT NULL DEFAULT 1,                   -- 1=USD, 2=BTC
    monto_en_usd  DECIMAL(18,2),                                     -- conversión si moneda=BTC
    tipo          SMALLINT      NOT NULL,                             -- TipoIngreso enum
    descripcion   TEXT,
    fecha_ingreso DATE          NOT NULL,
    es_recurrente BOOLEAN       NOT NULL DEFAULT FALSE,
    frecuencia    SMALLINT,                                           -- TipoFrecuencia enum
    created_at    TIMESTAMPTZ   NOT NULL DEFAULT NOW(),
    updated_at    TIMESTAMPTZ   NOT NULL DEFAULT NOW(),
    is_deleted    BOOLEAN       NOT NULL DEFAULT FALSE,
    deleted_at    TIMESTAMPTZ
);

CREATE INDEX IF NOT EXISTS idx_ingresos_hogar        ON ingresos(hogar_id, fecha_ingreso DESC) WHERE NOT is_deleted;
CREATE INDEX IF NOT EXISTS idx_ingresos_usuario      ON ingresos(usuario_id)                   WHERE NOT is_deleted;
CREATE INDEX IF NOT EXISTS idx_ingresos_tipo         ON ingresos(hogar_id, tipo)               WHERE NOT is_deleted;
CREATE INDEX IF NOT EXISTS idx_ingresos_mes          ON ingresos(hogar_id, EXTRACT(YEAR FROM fecha_ingreso), EXTRACT(MONTH FROM fecha_ingreso));

-- ===========================================================================
-- TABLA: gastos
-- ===========================================================================
CREATE TABLE IF NOT EXISTS gastos (
    id                UUID          PRIMARY KEY DEFAULT gen_random_uuid(),
    usuario_id        UUID          NOT NULL REFERENCES usuarios(id),
    hogar_id          UUID          NOT NULL REFERENCES hogares(id),
    categoria_id      UUID          NOT NULL REFERENCES categorias(id),
    servicio_basico_id UUID,                                           -- FK a servicios_basicos (se agrega al final)
    monto             DECIMAL(18,2) NOT NULL CHECK (monto > 0),
    moneda            SMALLINT      NOT NULL DEFAULT 1,
    monto_en_usd      DECIMAL(18,2),
    tipo              SMALLINT      NOT NULL,                          -- TipoGasto enum
    descripcion       TEXT,
    fecha_gasto       DATE          NOT NULL,
    es_recurrente     BOOLEAN       NOT NULL DEFAULT FALSE,
    frecuencia        SMALLINT,
    comprobante       TEXT,                                            -- URL o referencia
    created_at        TIMESTAMPTZ   NOT NULL DEFAULT NOW(),
    updated_at        TIMESTAMPTZ   NOT NULL DEFAULT NOW(),
    is_deleted        BOOLEAN       NOT NULL DEFAULT FALSE,
    deleted_at        TIMESTAMPTZ
);

CREATE INDEX IF NOT EXISTS idx_gastos_hogar     ON gastos(hogar_id, fecha_gasto DESC) WHERE NOT is_deleted;
CREATE INDEX IF NOT EXISTS idx_gastos_categoria ON gastos(hogar_id, categoria_id)     WHERE NOT is_deleted;
CREATE INDEX IF NOT EXISTS idx_gastos_tipo      ON gastos(hogar_id, tipo)             WHERE NOT is_deleted;
CREATE INDEX IF NOT EXISTS idx_gastos_mes       ON gastos(hogar_id, EXTRACT(YEAR FROM fecha_gasto), EXTRACT(MONTH FROM fecha_gasto));

-- ===========================================================================
-- TABLA: servicios_basicos  (ANDA, DELSUR, AES, Claro, Tigo, ANTEL, Gas...)
-- ===========================================================================
CREATE TABLE IF NOT EXISTS servicios_basicos (
    id                            UUID          PRIMARY KEY DEFAULT gen_random_uuid(),
    hogar_id                      UUID          NOT NULL REFERENCES hogares(id),
    usuario_id                    UUID          NOT NULL REFERENCES usuarios(id),
    tipo_servicio                 SMALLINT      NOT NULL,   -- TipoServicio enum
    nombre_proveedor              VARCHAR(100)  NOT NULL,   -- 'ANDA','DELSUR','AES','Claro','Tigo','ANTEL'
    monto_ultimo_pago             DECIMAL(18,2) NOT NULL DEFAULT 0,
    monto_promedio                DECIMAL(18,2),
    fecha_vencimiento             DATE          NOT NULL,
    fecha_ultimo_pago             DATE,
    esta_vencido                  BOOLEAN       NOT NULL DEFAULT FALSE,
    notificacion_activa           BOOLEAN       NOT NULL DEFAULT TRUE,
    dias_anticipacion_notificacion INT          NOT NULL DEFAULT 5,
    numero_cuenta                 VARCHAR(50),
    created_at                    TIMESTAMPTZ   NOT NULL DEFAULT NOW(),
    updated_at                    TIMESTAMPTZ   NOT NULL DEFAULT NOW(),
    is_deleted                    BOOLEAN       NOT NULL DEFAULT FALSE,
    deleted_at                    TIMESTAMPTZ
);

CREATE INDEX IF NOT EXISTS idx_servicios_hogar        ON servicios_basicos(hogar_id)             WHERE NOT is_deleted;
CREATE INDEX IF NOT EXISTS idx_servicios_vencimiento  ON servicios_basicos(hogar_id, fecha_vencimiento) WHERE NOT is_deleted;

-- Ahora se puede agregar la FK de gastos → servicios_basicos
ALTER TABLE gastos
    ADD CONSTRAINT fk_gastos_servicio_basico
    FOREIGN KEY (servicio_basico_id) REFERENCES servicios_basicos(id);

-- ===========================================================================
-- TABLA: presupuestos_mensuales
-- ===========================================================================
CREATE TABLE IF NOT EXISTS presupuestos_mensuales (
    id            UUID          PRIMARY KEY DEFAULT gen_random_uuid(),
    hogar_id      UUID          NOT NULL REFERENCES hogares(id),
    categoria_id  UUID          NOT NULL REFERENCES categorias(id),
    anio          SMALLINT      NOT NULL,
    mes           SMALLINT      NOT NULL CHECK (mes BETWEEN 1 AND 12),
    monto_limite  DECIMAL(18,2) NOT NULL CHECK (monto_limite > 0),
    monto_gastado DECIMAL(18,2) NOT NULL DEFAULT 0,
    created_at    TIMESTAMPTZ   NOT NULL DEFAULT NOW(),
    updated_at    TIMESTAMPTZ   NOT NULL DEFAULT NOW(),
    is_deleted    BOOLEAN       NOT NULL DEFAULT FALSE,
    deleted_at    TIMESTAMPTZ,
    UNIQUE (hogar_id, categoria_id, anio, mes)
);

CREATE INDEX IF NOT EXISTS idx_presupuestos_mes ON presupuestos_mensuales(hogar_id, anio, mes) WHERE NOT is_deleted;

-- ===========================================================================
-- TABLA: alertas
-- ===========================================================================
CREATE TABLE IF NOT EXISTS alertas (
    id             UUID          PRIMARY KEY DEFAULT gen_random_uuid(),
    hogar_id       UUID          NOT NULL REFERENCES hogares(id),
    usuario_id     UUID          REFERENCES usuarios(id),
    categoria_id   UUID          REFERENCES categorias(id),
    tipo           SMALLINT      NOT NULL,   -- TipoAlerta enum
    estado         SMALLINT      NOT NULL DEFAULT 1,  -- 1=Pendiente, 2=Leida, 3=Descartada
    titulo         VARCHAR(200)  NOT NULL,
    mensaje        TEXT          NOT NULL,
    porcentaje_uso DECIMAL(5,2),
    fecha_generada TIMESTAMPTZ   NOT NULL DEFAULT NOW(),
    fecha_leida    TIMESTAMPTZ,
    created_at     TIMESTAMPTZ   NOT NULL DEFAULT NOW(),
    updated_at     TIMESTAMPTZ   NOT NULL DEFAULT NOW(),
    is_deleted     BOOLEAN       NOT NULL DEFAULT FALSE,
    deleted_at     TIMESTAMPTZ
);

CREATE INDEX IF NOT EXISTS idx_alertas_hogar  ON alertas(hogar_id, estado)        WHERE NOT is_deleted;
CREATE INDEX IF NOT EXISTS idx_alertas_fecha  ON alertas(hogar_id, fecha_generada DESC);
CREATE INDEX IF NOT EXISTS idx_alertas_tipo   ON alertas(hogar_id, tipo, EXTRACT(YEAR FROM fecha_generada), EXTRACT(MONTH FROM fecha_generada));

-- ===========================================================================
-- TABLA: tandas  (ahorro rotativo — muy común en El Salvador)
-- ===========================================================================
CREATE TABLE IF NOT EXISTS tandas (
    id                  UUID          PRIMARY KEY DEFAULT gen_random_uuid(),
    hogar_id            UUID          NOT NULL REFERENCES hogares(id),
    organizador_id      UUID          NOT NULL REFERENCES usuarios(id),
    nombre              VARCHAR(200)  NOT NULL,
    cuota_mensual       DECIMAL(18,2) NOT NULL CHECK (cuota_mensual > 0),
    total_participantes INT           NOT NULL CHECK (total_participantes >= 2),
    frecuencia          SMALLINT      NOT NULL DEFAULT 4,   -- 4=Mensual (TipoFrecuencia)
    fecha_inicio        DATE          NOT NULL,
    fecha_fin           DATE,
    estado              SMALLINT      NOT NULL DEFAULT 1,   -- 1=Activa, 2=Pausada, 3=Completada, 4=Cancelada
    turno_actual        INT           NOT NULL DEFAULT 1,
    created_at          TIMESTAMPTZ   NOT NULL DEFAULT NOW(),
    updated_at          TIMESTAMPTZ   NOT NULL DEFAULT NOW(),
    is_deleted          BOOLEAN       NOT NULL DEFAULT FALSE,
    deleted_at          TIMESTAMPTZ
);

CREATE INDEX IF NOT EXISTS idx_tandas_hogar ON tandas(hogar_id) WHERE NOT is_deleted;

-- ===========================================================================
-- TABLA: tanda_participantes
-- ===========================================================================
CREATE TABLE IF NOT EXISTS tanda_participantes (
    id             UUID        PRIMARY KEY DEFAULT gen_random_uuid(),
    tanda_id       UUID        NOT NULL REFERENCES tandas(id) ON DELETE CASCADE,
    usuario_id     UUID        NOT NULL REFERENCES usuarios(id),
    numero_turno   INT         NOT NULL,
    ha_recibido    BOOLEAN     NOT NULL DEFAULT FALSE,
    fecha_recibio  TIMESTAMPTZ,
    cuotas_pagadas INT         NOT NULL DEFAULT 0,
    created_at     TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    updated_at     TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    is_deleted     BOOLEAN     NOT NULL DEFAULT FALSE,
    deleted_at     TIMESTAMPTZ,
    UNIQUE (tanda_id, numero_turno),
    UNIQUE (tanda_id, usuario_id)
);

CREATE INDEX IF NOT EXISTS idx_tanda_participantes_tanda ON tanda_participantes(tanda_id) WHERE NOT is_deleted;

-- ===========================================================================
-- TABLA: remesas
-- ===========================================================================
CREATE TABLE IF NOT EXISTS remesas (
    id                  UUID          PRIMARY KEY DEFAULT gen_random_uuid(),
    hogar_id            UUID          NOT NULL REFERENCES hogares(id),
    receptor_id         UUID          NOT NULL REFERENCES usuarios(id),
    monto               DECIMAL(18,2) NOT NULL CHECK (monto > 0),
    moneda              SMALLINT      NOT NULL DEFAULT 1,   -- 1=USD, 2=BTC
    pais_origen         VARCHAR(100)  DEFAULT 'Estados Unidos',
    nombre_remitente    VARCHAR(200),
    empresa             VARCHAR(100),   -- 'Western Union','MoneyGram','Remitly','Vigo','WorldRemit'
    comision_cobrada    DECIMAL(18,2),
    fecha_recibida      DATE          NOT NULL,
    numero_confirmacion VARCHAR(100),
    proposito           TEXT,
    created_at          TIMESTAMPTZ   NOT NULL DEFAULT NOW(),
    updated_at          TIMESTAMPTZ   NOT NULL DEFAULT NOW(),
    is_deleted          BOOLEAN       NOT NULL DEFAULT FALSE,
    deleted_at          TIMESTAMPTZ
);

CREATE INDEX IF NOT EXISTS idx_remesas_hogar  ON remesas(hogar_id, fecha_recibida DESC) WHERE NOT is_deleted;
CREATE INDEX IF NOT EXISTS idx_remesas_anio   ON remesas(hogar_id, EXTRACT(YEAR FROM fecha_recibida));

-- FK de ingresos → remesas (se agrega al final)
ALTER TABLE ingresos
    ADD CONSTRAINT fk_ingresos_remesa
    FOREIGN KEY (remesa_id) REFERENCES remesas(id);

-- ===========================================================================
-- FUNCIÓN: generar_alertas_temporada
-- Genera alertas de temporada escolar (agosto) y navideña (diciembre).
-- Llamar desde un cron job el día 1 de julio y 1 de noviembre.
-- ===========================================================================
CREATE OR REPLACE FUNCTION generar_alertas_temporada()
RETURNS void AS $$
DECLARE
    v_hogar_id  UUID;
    v_mes_alerta SMALLINT;
    v_tipo_alerta SMALLINT;
    v_titulo    TEXT;
    v_mensaje   TEXT;
BEGIN
    -- Determinar temporada según mes actual
    IF EXTRACT(MONTH FROM NOW()) = 7 THEN
        v_mes_alerta  := 8;   -- alerta para agosto
        v_tipo_alerta := 8;   -- 8 = TemporadaEscolar
        v_titulo      := 'Temporada Escolar — Agosto se acerca';
        v_mensaje     := 'Prepara tu presupuesto para útiles, uniformes y matrícula del año escolar en El Salvador.';
    ELSIF EXTRACT(MONTH FROM NOW()) = 11 THEN
        v_mes_alerta  := 12;  -- alerta para diciembre
        v_tipo_alerta := 9;   -- 9 = TemporadaNavidad
        v_titulo      := 'Temporada Navideña — Diciembre se acerca';
        v_mensaje     := 'Planifica tus gastos navideños y aprovecha el aguinaldo con responsabilidad.';
    ELSE
        RETURN;  -- No es temporada de alerta
    END IF;

    -- Insertar alerta para cada hogar activo que no tenga ya esta alerta en el año/mes destino
    FOR v_hogar_id IN
        SELECT id FROM hogares WHERE NOT is_deleted
    LOOP
        IF NOT EXISTS (
            SELECT 1 FROM alertas
            WHERE hogar_id       = v_hogar_id
              AND tipo           = v_tipo_alerta
              AND EXTRACT(YEAR  FROM fecha_generada) = EXTRACT(YEAR  FROM NOW())
              AND EXTRACT(MONTH FROM fecha_generada) BETWEEN EXTRACT(MONTH FROM NOW()) AND v_mes_alerta
              AND NOT is_deleted
        ) THEN
            INSERT INTO alertas (hogar_id, tipo, estado, titulo, mensaje, fecha_generada)
            VALUES (v_hogar_id, v_tipo_alerta, 1, v_titulo, v_mensaje, NOW());
        END IF;
    END LOOP;
END;
$$ LANGUAGE plpgsql;

-- ===========================================================================
-- FUNCIÓN: actualizar_esta_vencido
-- Marca servicios básicos como vencidos si su fecha_vencimiento < HOY.
-- ===========================================================================
CREATE OR REPLACE FUNCTION marcar_servicios_vencidos()
RETURNS void AS $$
BEGIN
    UPDATE servicios_basicos
    SET    esta_vencido = TRUE,
           updated_at   = NOW()
    WHERE  fecha_vencimiento < CURRENT_DATE
      AND  esta_vencido = FALSE
      AND  NOT is_deleted;
END;
$$ LANGUAGE plpgsql;

-- ===========================================================================
-- FUNCIÓN: calcular_porcentaje_presupuesto (helper)
-- ===========================================================================
CREATE OR REPLACE FUNCTION calcular_porcentaje_presupuesto(
    p_hogar_id    UUID,
    p_categoria_id UUID,
    p_anio        INT,
    p_mes         INT
) RETURNS DECIMAL(5,2) AS $$
DECLARE
    v_limite  DECIMAL(18,2);
    v_gastado DECIMAL(18,2);
BEGIN
    SELECT monto_limite, monto_gastado
    INTO   v_limite, v_gastado
    FROM   presupuestos_mensuales
    WHERE  hogar_id     = p_hogar_id
      AND  categoria_id  = p_categoria_id
      AND  anio          = p_anio
      AND  mes           = p_mes
      AND  NOT is_deleted;

    IF v_limite IS NULL OR v_limite = 0 THEN RETURN 0; END IF;
    RETURN ROUND((v_gastado / v_limite) * 100, 2);
END;
$$ LANGUAGE plpgsql;

-- ===========================================================================
-- VISTA: v_balance_mensual (resumen rápido de ingresos/gastos por hogar/mes)
-- ===========================================================================
CREATE OR REPLACE VIEW v_balance_mensual AS
SELECT
    h.id                                    AS hogar_id,
    h.nombre                                AS hogar_nombre,
    EXTRACT(YEAR  FROM i.fecha_ingreso)::INT AS anio,
    EXTRACT(MONTH FROM i.fecha_ingreso)::INT AS mes,
    COALESCE(SUM(COALESCE(i.monto_en_usd, i.monto)), 0) AS total_ingresos
FROM hogares h
LEFT JOIN ingresos i ON i.hogar_id = h.id AND NOT i.is_deleted
WHERE NOT h.is_deleted
GROUP BY h.id, h.nombre, EXTRACT(YEAR FROM i.fecha_ingreso), EXTRACT(MONTH FROM i.fecha_ingreso);

-- ===========================================================================
-- VISTA: v_alertas_pendientes
-- ===========================================================================
CREATE OR REPLACE VIEW v_alertas_pendientes AS
SELECT
    a.id, a.hogar_id, h.nombre AS hogar_nombre,
    a.tipo, a.titulo, a.mensaje, a.porcentaje_uso, a.fecha_generada
FROM alertas a
JOIN hogares h ON h.id = a.hogar_id
WHERE a.estado = 1   -- Pendiente
  AND NOT a.is_deleted
  AND NOT h.is_deleted
ORDER BY a.fecha_generada DESC;

-- ===========================================================================
-- TRIGGER: updated_at automático
-- ===========================================================================
CREATE OR REPLACE FUNCTION set_updated_at()
RETURNS TRIGGER AS $$
BEGIN
    NEW.updated_at = NOW();
    RETURN NEW;
END;
$$ LANGUAGE plpgsql;

DO $$
DECLARE
    t TEXT;
BEGIN
    FOREACH t IN ARRAY ARRAY[
        'roles','usuarios','hogares','hogar_usuarios','categorias',
        'ingresos','gastos','servicios_basicos','presupuestos_mensuales',
        'alertas','tandas','tanda_participantes','remesas'
    ]
    LOOP
        EXECUTE format(
            'CREATE OR REPLACE TRIGGER trg_%s_updated_at
             BEFORE UPDATE ON %s
             FOR EACH ROW EXECUTE FUNCTION set_updated_at();', t, t);
    END LOOP;
END;
$$;

-- ===========================================================================
-- COMENTARIOS DE TABLAS (documentación)
-- ===========================================================================
COMMENT ON TABLE usuarios            IS 'Usuarios del sistema. Campo DUI = Documento Único de Identidad de El Salvador.';
COMMENT ON TABLE hogares             IS 'Unidad familiar. Múltiples usuarios pueden pertenecer al mismo hogar.';
COMMENT ON TABLE hogar_usuarios      IS 'Relación N:M entre hogares y usuarios con rol administrador/miembro.';
COMMENT ON TABLE ingresos            IS 'Ingresos del hogar: salario, remesas, negocios, tandas, BTC, etc.';
COMMENT ON TABLE gastos              IS 'Gastos del hogar. Si supera umbral de presupuesto genera una alerta.';
COMMENT ON TABLE servicios_basicos   IS 'Servicios como ANDA (agua), DELSUR/AES (luz), Claro/Tigo/ANTEL (internet/teléfono).';
COMMENT ON TABLE presupuestos_mensuales IS 'Límites de gasto por categoría y mes. Base del sistema de alertas presupuestarias.';
COMMENT ON TABLE alertas             IS 'Alertas automáticas: 50/80/100% presupuesto, vencimientos, temporadas (escolar, navidad).';
COMMENT ON TABLE tandas              IS 'Sistema de ahorro rotativo (tanda/cundina). Muy común en El Salvador.';
COMMENT ON TABLE remesas             IS 'El Salvador recibe ~$8B USD anuales en remesas. Registrar aquí y genera ingreso automático.';
COMMENT ON TABLE categorias          IS '14 categorías globales del sistema + categorías personalizadas por hogar.';

-- ===========================================================================
-- FIN DEL SCRIPT
-- ===========================================================================
