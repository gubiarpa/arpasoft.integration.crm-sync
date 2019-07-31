
CREATE TABLE SFC_TB_CLIENTE (
       IDSalesforce         CHAR(18) NOT NULL,
       LogoFoto             VARCHAR2(200) NULL,
       FechaNacimOrAniv     DATE NULL,
       RecibirInformacion   VARCHAR2(20) NULL,
       IDTipoPersona        CHAR(32) NULL,
       IDPuntoContacto      CHAR(32) NULL,
       IDNivelImportancia   CHAR(32) NULL,
       IDTipoCuenta         CHAR(32) NULL,
       IDEstado             CHAR(32) NULL,
       IDPaisProcedencia    CHAR(32) NULL,
       FechaIniRelacionComercial DATE NULL,
       Comentarios          VARCHAR2(500) NULL
);

CREATE INDEX XIF26SFC_TB_CLIENTE ON SFC_TB_CLIENTE
(
       IDTipoPersona                  ASC
);

CREATE INDEX XIF27SFC_TB_CLIENTE ON SFC_TB_CLIENTE
(
       IDPuntoContacto                ASC
);

CREATE INDEX XIF28SFC_TB_CLIENTE ON SFC_TB_CLIENTE
(
       IDNivelImportancia             ASC
);

CREATE INDEX XIF29SFC_TB_CLIENTE ON SFC_TB_CLIENTE
(
       IDTipoCuenta                   ASC
);

CREATE INDEX XIF30SFC_TB_CLIENTE ON SFC_TB_CLIENTE
(
       IDEstado                       ASC
);

CREATE INDEX XIF31SFC_TB_CLIENTE ON SFC_TB_CLIENTE
(
       IDPaisProcedencia              ASC
);


ALTER TABLE SFC_TB_CLIENTE
       ADD  ( PRIMARY KEY (IDSalesforce) ) ;


CREATE TABLE SFC_TB_CLIENTE_B2B (
       IDSalesforce         CHAR(18) NOT NULL,
       RazonSocial          VARCHAR2(200) NULL,
       Alias                VARCHAR2(200) NULL,
       CondicionPago        VARCHAR2(20) NULL,
       TipoMonedaDeLineaCredito VARCHAR2(20) NULL,
       MontoLineaCredito    DECIMAL(20,6) NULL
);


ALTER TABLE SFC_TB_CLIENTE_B2B
       ADD  ( PRIMARY KEY (IDSalesforce) ) ;


CREATE TABLE SFC_TB_CLIENTE_B2C (
       IDSalesforce         CHAR(18) NOT NULL,
       Nombre               VARCHAR2(200) NULL,
       ApePaterno           VARCHAR2(200) NULL,
       ApeMaterno           VARCHAR2(200) NULL,
       EstadoCivil          VARCHAR2(20) NULL,
       Genero               VARCHAR2(20) NULL,
       Nacionalidad         VARCHAR2(200) NULL,
       GradoEstudios        VARCHAR2(200) NULL,
       Profesion            VARCHAR2(200) NULL,
       PreferenciasGenerales VARCHAR2(200) NULL,
       ConsideracionesSalud VARCHAR2(500) NULL,
       TipoViaje            VARCHAR2(200) NULL,
       TipoAcompanhante     VARCHAR2(200) NULL
);


ALTER TABLE SFC_TB_CLIENTE_B2C
       ADD  ( PRIMARY KEY (IDSalesforce) ) ;


CREATE TABLE SFC_TB_CLIENTE_BRANCH (
       IDSalesforce         CHAR(18) NOT NULL,
       Item                 NUMBER NOT NULL,
       Tipo                 VARCHAR2(200) NULL
);

CREATE INDEX XIF15SFC_TB_CLIENTE_BRANCH ON SFC_TB_CLIENTE_BRANCH
(
       IDSalesforce                   ASC
);


ALTER TABLE SFC_TB_CLIENTE_BRANCH
       ADD  ( PRIMARY KEY (IDSalesforce, Item) ) ;


CREATE TABLE SFC_TB_CLIENTE_CANALINFO (
       IDSalesforce         CHAR(18) NOT NULL,
       Item                 NUMBER NOT NULL,
       Tipo                 VARCHAR2(200) NULL
);

CREATE INDEX XIF14SFC_TB_CLIENTE_CANALINFO ON SFC_TB_CLIENTE_CANALINFO
(
       IDSalesforce                   ASC
);


ALTER TABLE SFC_TB_CLIENTE_CANALINFO
       ADD  ( PRIMARY KEY (IDSalesforce, Item) ) ;


CREATE TABLE SFC_TB_CLIENTE_CORREO (
       IDSalesforce         CHAR(18) NOT NULL,
       Item                 NUMBER NOT NULL,
       Tipo                 VARCHAR2(20) NULL,
       Descripcion          VARCHAR2(500) NULL
);

CREATE INDEX XIF11SFC_TB_CLIENTE_CORREO ON SFC_TB_CLIENTE_CORREO
(
       IDSalesforce                   ASC
);


ALTER TABLE SFC_TB_CLIENTE_CORREO
       ADD  ( PRIMARY KEY (IDSalesforce, Item) ) ;


CREATE TABLE SFC_TB_CLIENTE_DIRECCION (
       IDSalesforce         CHAR(18) NOT NULL,
       Item                 NUMBER NOT NULL,
       Tipo                 VARCHAR2(20) NULL,
       Descripcion          VARCHAR2(200) NULL,
       Distrito             VARCHAR2(200) NULL,
       Ciudad               VARCHAR2(200) NULL,
       Departamento         VARCHAR2(200) NULL,
       Pais                 VARCHAR2(200) NULL
);

CREATE INDEX XIF8SFC_TB_CLIENTE_DIRECCION ON SFC_TB_CLIENTE_DIRECCION
(
       IDSalesforce                   ASC
);


ALTER TABLE SFC_TB_CLIENTE_DIRECCION
       ADD  ( PRIMARY KEY (IDSalesforce, Item) ) ;


CREATE TABLE SFC_TB_CLIENTE_DOCUMENTO (
       IDSalesforce         CHAR(18) NOT NULL,
       Item                 NUMBER NOT NULL,
       Tipo                 VARCHAR2(20) NULL,
       Numero               VARCHAR2(20) NULL
);

CREATE INDEX XIF7SFC_TB_CLIENTE_DOCUMENTO ON SFC_TB_CLIENTE_DOCUMENTO
(
       IDSalesforce                   ASC
);


ALTER TABLE SFC_TB_CLIENTE_DOCUMENTO
       ADD  ( PRIMARY KEY (IDSalesforce, Item) ) ;


CREATE TABLE SFC_TB_CLIENTE_IDIOMA (
       IDSalesforce         CHAR(18) NOT NULL,
       Item                 NUMBER NOT NULL,
       Tipo                 VARCHAR2(200) NULL
);

CREATE INDEX XIF16SFC_TB_CLIENTE_IDIOMA ON SFC_TB_CLIENTE_IDIOMA
(
       IDSalesforce                   ASC
);


ALTER TABLE SFC_TB_CLIENTE_IDIOMA
       ADD  ( PRIMARY KEY (IDSalesforce, Item) ) ;


CREATE TABLE SFC_TB_CLIENTE_PARTICIPANTE (
       IDSalesforce         CHAR(18) NOT NULL,
       Item                 NUMBER NOT NULL,
       EmpleOrEjecResponsable VARCHAR2(20) NULL,
       SupervisorKam        VARCHAR2(200) NULL,
       Gerente              VARCHAR2(200) NULL,
       UnidadNegocio        VARCHAR2(200) NULL,
       GrupoColabEjecRegionBranch VARCHAR2(200) NULL,
       FlagPrincipal        CHAR(18) NULL,
       Descripcion          VARCHAR2(500) NULL
);

CREATE INDEX XIF17SFC_TB_CLIENTE_PARTICIPAN ON SFC_TB_CLIENTE_PARTICIPANTE
(
       IDSalesforce                   ASC
);


ALTER TABLE SFC_TB_CLIENTE_PARTICIPANTE
       ADD  ( PRIMARY KEY (IDSalesforce, Item) ) ;


CREATE TABLE SFC_TB_CLIENTE_SITIO (
       IDSalesforce         CHAR(18) NOT NULL,
       Item                 NUMBER NOT NULL,
       Tipo                 VARCHAR2(20) NULL,
       Descripcion          VARCHAR2(500) NULL
);

CREATE INDEX XIF10SFC_TB_CLIENTE_SITIO ON SFC_TB_CLIENTE_SITIO
(
       IDSalesforce                   ASC
);


ALTER TABLE SFC_TB_CLIENTE_SITIO
       ADD  ( PRIMARY KEY (IDSalesforce, Item) ) ;


CREATE TABLE SFC_TB_CLIENTE_TELEFONO (
       IDSalesforce         CHAR(18) NOT NULL,
       Item                 NUMBER NOT NULL,
       Tipo                 VARCHAR2(20) NULL,
       Numero               VARCHAR2(50) NULL
);

CREATE INDEX XIF9SFC_TB_CLIENTE_TELEFONO ON SFC_TB_CLIENTE_TELEFONO
(
       IDSalesforce                   ASC
);


ALTER TABLE SFC_TB_CLIENTE_TELEFONO
       ADD  ( PRIMARY KEY (IDSalesforce, Item) ) ;


CREATE TABLE SFC_TB_CONTACTO (
       IDSalesforce         CHAR(18) NOT NULL,
       IDClienteSalesforce  CHAR(18) NULL,
       Nombre               VARCHAR2(200) NULL,
       ApePaterno           VARCHAR2(200) NULL,
       ApeMaterno           VARCHAR2(200) NULL,
       FechaNacimiento      DATE NULL,
       EstadoCivil          VARCHAR2(20) NULL,
       Genero               VARCHAR2(20) NULL,
       Nacionalidad         VARCHAR2(20) NULL,
       LogoFoto             VARCHAR2(500) NULL,
       Hijos                NUMBER NULL,
       Profesion            VARCHAR2(200) NULL,
       CargoEmpresa         VARCHAR2(200) NULL,
       TiempoEmpresa        NUMBER NULL,
       NivelRiesgo          VARCHAR2(200) NULL,
       RegionMercadoBranch  VARCHAR2(200) NULL,
       Estado               VARCHAR2(200) NULL,
       Comentarios          VARCHAR2(500) NULL,
       OrigenContacto       VARCHAR2(200) NULL
);


ALTER TABLE SFC_TB_CONTACTO
       ADD  ( PRIMARY KEY (IDSalesforce) ) ;


CREATE TABLE SFC_TB_CONTACTO_CORREO (
       IDSalesforce         CHAR(18) NOT NULL,
       Item                 NUMBER NOT NULL,
       Tipo                 VARCHAR2(20) NULL,
       Descripcion          VARCHAR2(500) NULL
);

CREATE INDEX XIF9SFC_TB_CONTACTO_CORREO ON SFC_TB_CONTACTO_CORREO
(
       IDSalesforce                   ASC
);


ALTER TABLE SFC_TB_CONTACTO_CORREO
       ADD  ( PRIMARY KEY (IDSalesforce, Item) ) ;


CREATE TABLE SFC_TB_CONTACTO_DIRECCION (
       IDSalesforce         CHAR(18) NOT NULL,
       Item                 NUMBER NOT NULL,
       Tipo                 VARCHAR2(20) NULL,
       Descripcion          VARCHAR2(200) NULL,
       Distrito             VARCHAR2(200) NULL,
       Ciudad               VARCHAR2(200) NULL,
       Departamento         VARCHAR2(200) NULL,
       Pais                 VARCHAR2(200) NULL
);

CREATE INDEX XIF6SFC_TB_CONTACTO_DIRECCION ON SFC_TB_CONTACTO_DIRECCION
(
       IDSalesforce                   ASC
);


ALTER TABLE SFC_TB_CONTACTO_DIRECCION
       ADD  ( PRIMARY KEY (IDSalesforce, Item) ) ;


CREATE TABLE SFC_TB_CONTACTO_DOCUMENTO (
       IDSalesforce         CHAR(18) NOT NULL,
       Item                 NUMBER NOT NULL,
       Tipo                 VARCHAR2(20) NULL,
       Numero               VARCHAR2(20) NULL
);

CREATE INDEX XIF5SFC_TB_CONTACTO_DOCUMENTO ON SFC_TB_CONTACTO_DOCUMENTO
(
       IDSalesforce                   ASC
);


ALTER TABLE SFC_TB_CONTACTO_DOCUMENTO
       ADD  ( PRIMARY KEY (IDSalesforce, Item) ) ;


CREATE TABLE SFC_TB_CONTACTO_IDIOMA (
       IDSalesforce         CHAR(18) NOT NULL,
       Item                 NUMBER NOT NULL,
       Tipo                 VARCHAR2(200) NULL
);

CREATE INDEX XIF10SFC_TB_CONTACTO_IDIOMA ON SFC_TB_CONTACTO_IDIOMA
(
       IDSalesforce                   ASC
);


ALTER TABLE SFC_TB_CONTACTO_IDIOMA
       ADD  ( PRIMARY KEY (IDSalesforce, Item) ) ;


CREATE TABLE SFC_TB_CONTACTO_INTERPRODACT (
       IDSalesforce         CHAR(18) NOT NULL,
       Item                 NUMBER NOT NULL,
       Tipo                 VARCHAR2(200) NULL
);

CREATE INDEX XIF13SFC_TB_CONTACTO_INTERPROD ON SFC_TB_CONTACTO_INTERPRODACT
(
       IDSalesforce                   ASC
);


ALTER TABLE SFC_TB_CONTACTO_INTERPRODACT
       ADD  ( PRIMARY KEY (IDSalesforce, Item) ) ;


CREATE TABLE SFC_TB_CONTACTO_SITIO (
       IDSalesforce         CHAR(18) NOT NULL,
       Item                 NUMBER NOT NULL,
       Tipo                 VARCHAR2(20) NULL,
       Descripcion          VARCHAR2(500) NULL
);

CREATE INDEX XIF8SFC_TB_CONTACTO_SITIO ON SFC_TB_CONTACTO_SITIO
(
       IDSalesforce                   ASC
);


ALTER TABLE SFC_TB_CONTACTO_SITIO
       ADD  ( PRIMARY KEY (IDSalesforce, Item) ) ;


CREATE TABLE SFC_TB_CONTACTO_TELEFONO (
       IDSalesforce         CHAR(18) NOT NULL,
       Item                 NUMBER NOT NULL,
       Tipo                 VARCHAR2(20) NULL,
       Numero               VARCHAR2(200) NULL
);

CREATE INDEX XIF7SFC_TB_CONTACTO_TELEFONO ON SFC_TB_CONTACTO_TELEFONO
(
       IDSalesforce                   ASC
);


ALTER TABLE SFC_TB_CONTACTO_TELEFONO
       ADD  ( PRIMARY KEY (IDSalesforce, Item) ) ;


CREATE TABLE SFC_TB_ESTADO (
       IDEstado             CHAR(32) NOT NULL,
       Descripcion          VARCHAR2(200) NULL
);


ALTER TABLE SFC_TB_ESTADO
       ADD  ( PRIMARY KEY (IDEstado) ) ;


CREATE TABLE SFC_TB_NIVELIMPORTANCIA (
       IDNivelImportancia   CHAR(32) NOT NULL,
       Descripcion          VARCHAR2(200) NULL
);


ALTER TABLE SFC_TB_NIVELIMPORTANCIA
       ADD  ( PRIMARY KEY (IDNivelImportancia) ) ;


CREATE TABLE SFC_TB_PAISPROCEDENCIA (
       IDPaisProcedencia    CHAR(32) NOT NULL,
       Descripcion          VARCHAR2(200) NULL
);


ALTER TABLE SFC_TB_PAISPROCEDENCIA
       ADD  ( PRIMARY KEY (IDPaisProcedencia) ) ;


CREATE TABLE SFC_TB_PUNTOCONTACTO (
       IDPuntoContacto      CHAR(32) NOT NULL,
       Descripcion          VARCHAR2(200) NULL
);


ALTER TABLE SFC_TB_PUNTOCONTACTO
       ADD  ( PRIMARY KEY (IDPuntoContacto) ) ;


CREATE TABLE SFC_TB_TIPOCUENTA (
       IDTipoCuenta         CHAR(32) NOT NULL,
       Descripcion          VARCHAR2(200) NULL
);


ALTER TABLE SFC_TB_TIPOCUENTA
       ADD  ( PRIMARY KEY (IDTipoCuenta) ) ;


CREATE TABLE SFC_TB_TIPOPERSONA (
       IDTipoPersona        CHAR(32) NOT NULL,
       Descripcion          VARCHAR2(200) NULL
);


ALTER TABLE SFC_TB_TIPOPERSONA
       ADD  ( PRIMARY KEY (IDTipoPersona) ) ;


ALTER TABLE SFC_TB_CLIENTE
       ADD  ( FOREIGN KEY (IDPaisProcedencia)
                             REFERENCES SFC_TB_PAISPROCEDENCIA ) ;


ALTER TABLE SFC_TB_CLIENTE
       ADD  ( FOREIGN KEY (IDEstado)
                             REFERENCES SFC_TB_ESTADO ) ;


ALTER TABLE SFC_TB_CLIENTE
       ADD  ( FOREIGN KEY (IDTipoCuenta)
                             REFERENCES SFC_TB_TIPOCUENTA ) ;


ALTER TABLE SFC_TB_CLIENTE
       ADD  ( FOREIGN KEY (IDNivelImportancia)
                             REFERENCES SFC_TB_NIVELIMPORTANCIA ) ;


ALTER TABLE SFC_TB_CLIENTE
       ADD  ( FOREIGN KEY (IDPuntoContacto)
                             REFERENCES SFC_TB_PUNTOCONTACTO ) ;


ALTER TABLE SFC_TB_CLIENTE
       ADD  ( FOREIGN KEY (IDTipoPersona)
                             REFERENCES SFC_TB_TIPOPERSONA ) ;


ALTER TABLE SFC_TB_CLIENTE_B2B
       ADD  ( FOREIGN KEY (IDSalesforce)
                             REFERENCES SFC_TB_CLIENTE ) ;


ALTER TABLE SFC_TB_CLIENTE_B2C
       ADD  ( FOREIGN KEY (IDSalesforce)
                             REFERENCES SFC_TB_CLIENTE ) ;


ALTER TABLE SFC_TB_CLIENTE_BRANCH
       ADD  ( FOREIGN KEY (IDSalesforce)
                             REFERENCES SFC_TB_CLIENTE ) ;


ALTER TABLE SFC_TB_CLIENTE_CANALINFO
       ADD  ( FOREIGN KEY (IDSalesforce)
                             REFERENCES SFC_TB_CLIENTE ) ;


ALTER TABLE SFC_TB_CLIENTE_CORREO
       ADD  ( FOREIGN KEY (IDSalesforce)
                             REFERENCES SFC_TB_CLIENTE ) ;


ALTER TABLE SFC_TB_CLIENTE_DIRECCION
       ADD  ( FOREIGN KEY (IDSalesforce)
                             REFERENCES SFC_TB_CLIENTE ) ;


ALTER TABLE SFC_TB_CLIENTE_DOCUMENTO
       ADD  ( FOREIGN KEY (IDSalesforce)
                             REFERENCES SFC_TB_CLIENTE ) ;


ALTER TABLE SFC_TB_CLIENTE_IDIOMA
       ADD  ( FOREIGN KEY (IDSalesforce)
                             REFERENCES SFC_TB_CLIENTE ) ;


ALTER TABLE SFC_TB_CLIENTE_PARTICIPANTE
       ADD  ( FOREIGN KEY (IDSalesforce)
                             REFERENCES SFC_TB_CLIENTE ) ;


ALTER TABLE SFC_TB_CLIENTE_SITIO
       ADD  ( FOREIGN KEY (IDSalesforce)
                             REFERENCES SFC_TB_CLIENTE ) ;


ALTER TABLE SFC_TB_CLIENTE_TELEFONO
       ADD  ( FOREIGN KEY (IDSalesforce)
                             REFERENCES SFC_TB_CLIENTE ) ;


ALTER TABLE SFC_TB_CONTACTO_CORREO
       ADD  ( FOREIGN KEY (IDSalesforce)
                             REFERENCES SFC_TB_CONTACTO ) ;


ALTER TABLE SFC_TB_CONTACTO_DIRECCION
       ADD  ( FOREIGN KEY (IDSalesforce)
                             REFERENCES SFC_TB_CONTACTO ) ;


ALTER TABLE SFC_TB_CONTACTO_DOCUMENTO
       ADD  ( FOREIGN KEY (IDSalesforce)
                             REFERENCES SFC_TB_CONTACTO ) ;


ALTER TABLE SFC_TB_CONTACTO_IDIOMA
       ADD  ( FOREIGN KEY (IDSalesforce)
                             REFERENCES SFC_TB_CONTACTO ) ;


ALTER TABLE SFC_TB_CONTACTO_INTERPRODACT
       ADD  ( FOREIGN KEY (IDSalesforce)
                             REFERENCES SFC_TB_CLIENTE ) ;


ALTER TABLE SFC_TB_CONTACTO_SITIO
       ADD  ( FOREIGN KEY (IDSalesforce)
                             REFERENCES SFC_TB_CONTACTO ) ;


ALTER TABLE SFC_TB_CONTACTO_TELEFONO
       ADD  ( FOREIGN KEY (IDSalesforce)
                             REFERENCES SFC_TB_CONTACTO ) ;



