CREATE OR REPLACE PROCEDURE condor.SP_CREAR_CLIENTE
(
    P_TIPO_DOCUMENTO VARCHAR2,
    P_DOCUMENTO VARCHAR2,
    P_NOMBRE VARCHAR2,
    P_AP_PATERNO VARCHAR2,
    P_AP_MATERNO VARCHAR2,
    P_GENERO VARCHAR2,
    P_ESTADO_CIVIL VARCHAR2,
    P_DIRECCION VARCHAR2
)
IS
    V_DETALLE_DIRECCION INTEGER;
    V_NUM_REGS INTEGER;
BEGIN

    SELECT  NVL(MAX(DETALLE_DIRECCION) + 1, 1)
    INTO    V_DETALLE_DIRECCION
    FROM    CONDOR.DETALLE_DIRECCION;

    INSERT INTO CONDOR.DETALLE_DIRECCION
    (
        DETALLE_DIRECCION,
        DIRECCION,
        CAMPO_1
    ) 
    VALUES
    (
        V_DETALLE_DIRECCION,
        'ESTANDAR',
        P_DIRECCION
    );

    SELECT      COUNT(*)
    INTO        V_NUM_REGS
    FROM        CLIENTE
    WHERE       CLIENTE = P_DOCUMENTO;

    -- SI EL CLIENTE NO EXISTE LO CREA --
    IF (V_NUM_REGS = 0) THEN
    
        INSERT INTO CONDOR.CLIENTE
        (
            CLIENTE,
            NOMBRE,
            DETALLE_DIRECCION,
            ALIAS,
            CONTACTO,
            CARGO,
            DIRECCION,
            DIR_EMB_DEFAULT,
            TELEFONO1,
            TELEFONO2,
            FAX,
            CONTRIBUYENTE,
            FECHA_INGRESO,
            MULTIMONEDA,
            MONEDA,
            SALDO,
            SALDO_LOCAL,
            SALDO_DOLAR,
            SALDO_CREDITO,
            SALDO_NOCARGOS,
            LIMITE_CREDITO,
            EXCEDER_LIMITE,
            TASA_INTERES, 
            TASA_INTERES_MORA,
            FECHA_ULT_MORA,
            FECHA_ULT_MOV,
            CONDICION_PAGO, 
            NIVEL_PRECIO,
            DESCUENTO,
            MONEDA_NIVEL,
            ACEPTA_BACKORDER,
            PAIS,
            ZONA,
            RUTA,
            VENDEDOR,
            COBRADOR,
            ACEPTA_FRACCIONES,
            ACTIVO,
            EXENTO_IMPUESTOS,
            EXENCION_IMP1,
            EXENCION_IMP2,
            COBRO_JUDICIAL,
            CATEGORIA_CLIENTE, 
            DIAS_ABASTECIMIEN,
            USA_TARJETA,
            E_MAIL,
            REQUIERE_OC,
            TIENE_CONVENIO,
            REGISTRARDOCSACORP,
            USAR_DIREMB_CORP,
            APLICAC_ABIERTAS,
            VERIF_LIMCRED_CORP,
            USAR_DESC_CORP,
            DOC_A_GENERAR,
            DIAS_PROMED_ATRASO,
            ASOCOBLIGCONTFACT,
            ES_CORPORACION,
            USAR_PRECIOS_CORP,
            USAR_EXENCIMP_CORP,
            FECHA_AUD_CONDOR, 
            USUARIO_AUD_CONDOR,
            AJUSTE_FECHA_COBRO,
            NOTEEXISTSFLAG
        )
        VALUES
        (
            -- { CLIENTE } --
                P_DOCUMENTO,
            -- { NOMBRE } --
                P_NOMBRE || ' ' || P_AP_PATERNO || ' ' || P_AP_MATERNO,
            -- { DETALLE_DIRECCION } --
                V_DETALLE_DIRECCION,
            -- { ALIAS } --
                P_NOMBRE || ' ' || P_AP_PATERNO || ' ' || P_AP_MATERNO,
            -- { CONTACTO } --
                ' ',
            -- { CARGO } --
                ' ',
            -- { DIRECCION } --
                P_DIRECCION,
            -- { DIR_EMB_DEFAULT } --
                'ND',
            -- { TELEFONO1 } --
                ' ',
            -- { TELEFONO2 } --
                ' ',
            -- { FAX } --
                ' ',
            -- { CONTRIBUYENTE } --
                'ND',
            -- { FECHA_INGRESO } --
                SYSDATE,
            -- { MULTIMONEDA } --
                'S',
            -- { MONEDA } --
                'US$',
            -- { SALDO } --
                0,
            -- { SALDO_LOCAL } --
                0,
            -- { SALDO_DOLAR } --
                0,
            -- { SALDO_CREDITO } --
                0,
            -- { SALDO_NOCARGOS } --
                0,
            -- { LIMITE_CREDITO } --
                0,
            -- { EXCEDER_LIMITE } --
                'N',
            -- { TASA_INTERES } --
                0,
            -- { TASA_INTERES_MORA } --
                0,
            -- { FECHA_ULT_MORA } --
                SYSDATE,
            -- { FECHA_ULT_MOV } --
                SYSDATE,
            -- { CONDICION_PAGO } --
                '0',
            -- { NIVEL_PRECIO } --
                'ND-LOCAL',
            -- { DESCUENTO } --
                0,
            -- { MONEDA_NIVEL } --
                'L',
            -- { ACEPTA_BACKORDER } --
                'S',
            -- { PAIS } --
                'PE',
            -- { ZONA } --
                'ND',
            -- { RUTA } --
                'ND',
            -- { VENDEDOR } --
                'ND',
            -- { COBRADOR } --
                'ND',
            -- { ACEPTA_FRACCIONES } --
                'S',
            -- { ACTIVO } --
                'S',
            -- { EXENTO_IMPUESTOS } --
                'N',
            -- { EXENCION_IMP1 } --
                0,
            -- { EXENCION_IMP2 } --
                0,
            -- { COBRO_JUDICIAL } --
                'N',
            -- { CATEGORIA_CLIENTE } --
                'PAIS',
            -- { DIAS_ABASTECIMIEN } --
                0,
            -- { USA_TARJETA } --
                'N',
            -- { E_MAIL } --
                ' ',
            -- { REQUIERE_OC } --
                'S',
            -- { TIENE_CONVENIO } --
                'N',
            -- { REGISTRARDOCSACORP } --
                'N',
            -- { USAR_DIREMB_CORP } --
                'N',
            -- { APLICAC_ABIERTAS } --
                'N',
            -- { VERIF_LIMCRED_CORP } --
                'N',
            -- { USAR_DESC_CORP } --
                'N',
            -- { DOC_A_GENERAR } --
                'F',
            -- { DIAS_PROMED_ATRASO } --
                0,
            -- { ASOCOBLIGCONTFACT } --
                'S',
            -- { ES_CORPORACION } --
                'S',
            -- { USAR_PRECIOS_CORP } --
                'N',
            -- { USAR_EXENCIMP_CORP } --
                'N',
            -- { FECHA_AUD_CONDOR } --
                SYSDATE,
            -- { USUARIO_AUD_CONDOR } --
                'CONDOR',
            -- { AJUSTE_FECHA_COBRO } --
                'A',
            -- { NOTEEXISTSFLAG } --
                0
        );

        INSERT INTO CONDOR.CLIENTE_VENDEDOR
        (
            CLIENTE,
            VENDEDOR
        ) 
        VALUES
        (
            P_DOCUMENTO,
            'ND'
        );

        INSERT INTO CONDOR.DIRECC_EMBARQUE
        (
            CLIENTE,
            DIRECCION,
            DESCRIPCION
        )
        VALUES
        (
            P_DOCUMENTO,
            'ND',
            P_DIRECCION
        );

        INSERT INTO CONDOR.RB_CLIENTE
        (
            CLIENTE,
            TIPO_CLIENTE,
            TIPO_PERSONA,
            NOMBRE,
            APELLIDO_PATERNO,
            APELLIDO_MATERNO,
            ESTADO_CIVIL,
            SEXO,
            ES_VIP,
            ES_POTENCIAL,
            TIPO_DOCUMENTO,
            DOCUMENTO,
            PORC_VEND,
            PORC_DSCTO,
            CIUDAD,
            IDIOMA,
            ES_PROVEEDOR,
            EXCEDER_CREDITO,
            EMP_PROV,
            FECHA_REGISTRO,
            USUARIO_REGISTRO,
            FECHA_AUD_CONDOR,
            USUARIO_AUD_CONDOR,
            APLICA_FEE
        )
        VALUES
        (
            -- { CLIENTE } --
                P_DOCUMENTO,
            -- { TIPO_CLIENTE } --
                'VARI',
            -- { TIPO_PERSONA } --
                'N',
            -- { NOMBRE } --
                P_NOMBRE,
            -- { APELLIDO_PATERNO } --
                P_AP_PATERNO,
            -- { APELLIDO_MATERNO } --
                P_AP_MATERNO,
            -- { ESTADO_CIVIL } --
                P_ESTADO_CIVIL,
            -- { SEXO } --
                P_GENERO,
            -- { ES_VIP } --
                'N',
            -- { ES_POTENCIAL } --
                'N',
            -- { TIPO_DOCUMENTO } --
                P_TIPO_DOCUMENTO,
            -- { DOCUMENTO } --
                P_DOCUMENTO,
            -- { PORC_VEND } --
                0,
            -- { PORC_DSCTO } --
                0,
            -- { CIUDAD } --
                'LIM',
            -- { IDIOMA } --
                'SPAN',
            -- { ES_PROVEEDOR } --
                'N',
            -- { EXCEDER_CREDITO } --
                'N',
            -- { EMP_PROV } --
                'P',
            -- { FECHA_REGISTRO } --
                SYSDATE,
            -- { USUARIO_REGISTRO } --
                'CONDOR',
            -- { FECHA_AUD_CONDOR } --
                SYSDATE,
            -- { USUARIO_AUD_CONDOR } --
                'CONDOR',
            -- { APLICA_FEE } --
                'N'
        );

        INSERT INTO CONDOR.RB_CLIENTE_UNIDAD
        (
            CLIENTE,
            UNIDAD_NEGOCIO
        )
        VALUES
        (
            P_DOCUMENTO,
            'E'
        );

        INSERT INTO CONDOR.RB_CLIENTE_MERCADO
        (
            CLIENTE,
            MERCADO,
            DESCUENTO_HOTEL,
            DESCUENTO_SERVICIO
        ) 
        VALUES
        (
            P_DOCUMENTO,
            'EGRE',
            0,
            0
        );

    END IF;


    EXCEPTION
        WHEN NO_DATA_FOUND THEN
            --ROLLBACK;
            RAISE;
        WHEN OTHERS THEN
            --ROLLBACK;
            RAISE;   
END SP_CREAR_CLIENTE;
