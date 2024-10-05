-- Table: Seguridad.Seguridad

-- DROP TABLE IF EXISTS "Seguridad"."Seguridad";
-- SCHEMA: Seguridad

-- DROP SCHEMA IF EXISTS "Seguridad" ;

CREATE SCHEMA IF NOT EXISTS "Seguridad"
    AUTHORIZATION usuario;

COMMENT ON SCHEMA "Seguridad"
    IS 'Esquema de seguridad';

CREATE TABLE IF NOT EXISTS "Seguridad"."Seguridad"
(
    id integer NOT NULL,
    nombres character varying(100) COLLATE pg_catalog."default" NOT NULL,
    apellidos character varying(100) COLLATE pg_catalog."default" NOT NULL,
    fechanacimiento date NOT NULL,
    direccion character varying(255) COLLATE pg_catalog."default" NOT NULL,
    password character varying(255) COLLATE pg_catalog."default" NOT NULL,
    telefono character varying(15) COLLATE pg_catalog."default" NOT NULL,
    email character varying(320) COLLATE pg_catalog."default" NOT NULL,
    fechacreacion date NOT NULL,
    fechamodificacion date,
    CONSTRAINT "Seguridad_pkey" PRIMARY KEY (id)
)

TABLESPACE pg_default;

ALTER TABLE IF EXISTS "Seguridad"."Seguridad"
    OWNER to usuario;

COMMENT ON TABLE "Seguridad"."Seguridad"
    IS 'Tabla de seguridad';

INSERT INTO "Seguridad"."Seguridad" (id, nombres, apellidos, fechanacimiento, direccion, password, telefono, email, fechacreacion, fechamodificacion)
VALUES
    (1, 'Juan', 'Pérez', '1990-01-15', 'Calle 123, Ciudad', '1234', '1234567890', 'juan.perez@example.com', CURRENT_DATE, NULL)
  