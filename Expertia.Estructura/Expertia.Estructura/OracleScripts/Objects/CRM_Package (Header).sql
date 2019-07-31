CREAR OR REPLACE PACKAGE DESARROLLO.CRM_MDM_PKG AS
/* *****************************************************************************
   NAME:       CRM TRANSFORMACIONAL
   PURPOSE:

   REVISIONS:
   Ver          Date          Author            Description
   ---------    ----------    ---------------   ---------------------------------
   1.0          24/07/2019    Billy Arredondo   1. Create this package.
******************************************************************************/

-- ▼ CLIENTE (B2B, B2C) --

    -- Crea un Cliente B2B (campos no múltiples) --
    PROCEDURE SP_CREAR_CLIENTE_B2B
    (                                                                                                                                                                                                                                                                                                                                               
        /* Cuenta */
        P_IDSalesforce                SFC_TB_CLIENTE.IDSalesforce%TYPE,
        P_TipoPersona                 SFC_TB_CLIENTE.IDTipoPersona%TYPE,
        P_FechaNacimOrAniv            SFC_TB_CLIENTE.FechaNacimOrAniv%TYPE,
        P_LogoFoto                    SFC_TB_CLIENTE.LogoFoto%TYPE,
        P_PuntoContacto               SFC_TB_CLIENTE.IDPuntoContacto%TYPE,
        P_RecibirInformacion          SFC_TB_CLIENTE.RecibirInformacion%TYPE,
        P_NivelImportancia            SFC_TB_CLIENTE.IDNivelImportancia%TYPE,
        P_FechaIniRelacionComercial   SFC_TB_CLIENTE.FechaIniRelacionComercial%TYPE,
        P_Comentarios                 SFC_TB_CLIENTE.Comentarios%TYPE,
        P_TipoCuenta                  SFC_TB_CLIENTE.IDTipoCuenta%TYPE,
        P_Estado                      SFC_TB_CLIENTE.IDEstado%TYPE,
        P_PaisProcedencia             SFC_TB_CLIENTE.IDPaisProcedencia%TYPE,
        /* Cuenta B2B */
        P_RazonSocial                 SFC_TB_CLIENTE_B2B.RazonSocial%TYPE,
        P_Alias                       SFC_TB_CLIENTE_B2B.Alias%TYPE,
        P_CondicionPago               SFC_TB_CLIENTE_B2B.CondicionPago%TYPE,
        P_TipoMonedaDeLineaCredito    SFC_TB_CLIENTE_B2B.TipoMonedaDeLineaCredito%TYPE,
        P_MontoLineaCredito           SFC_TB_CLIENTE_B2B.MontoLineaCredito%TYPE
    );

    -- Crea un Cliente B2C (campos no múltiples) --
    PROCEDURE SP_CREAR_CLIENTE_B2C
    (
        /* Cuenta */
        P_IDSalesforce                  SFC_TB_CLIENTE.IDSalesforce%TYPE,
        P_TipoPersona                   SFC_TB_CLIENTE.TipoPersona%TYPE,
        P_FechaNacimOrAniv              SFC_TB_CLIENTE.FechaNacimOrAniv%TYPE,
        P_LogoFoto                      SFC_TB_CLIENTE.LogoFoto%TYPE,
        P_PuntoContacto                 SFC_TB_CLIENTE.PuntoContacto%TYPE,
        P_RecibirInformacion            SFC_TB_CLIENTE.RecibirInformacion%TYPE,
        P_NivelImportancia              SFC_TB_CLIENTE.NivelImportancia%TYPE,
        P_FechaIniRelacionComercial     SFC_TB_CLIENTE.FechaIniRelacionComercial%TYPE,
        P_Comentarios                   SFC_TB_CLIENTE.Comentarios%TYPE,
        P_TipoCuenta                    SFC_TB_CLIENTE.TipoCuenta%TYPE,
        P_Estado                        SFC_TB_CLIENTE.Estado%TYPE,
        P_PaisProcedencia               SFC_TB_CLIENTE.PaisProcedencia%TYPE,
        /* Cuenta B2C */
        P_Nombre                        SFC_TB_CLIENTE_B2C.Nombre%TYPE,
        P_ApePaterno                    SFC_TB_CLIENTE_B2C.ApePaterno%TYPE,
        P_ApeMaterno                    SFC_TB_CLIENTE_B2C.ApeMaterno%TYPE,
        P_EstadoCivil                   SFC_TB_CLIENTE_B2C.EstadoCivil%TYPE,
        P_Genero                        SFC_TB_CLIENTE_B2C.Genero%TYPE,
        P_Nacionalidad                  SFC_TB_CLIENTE_B2C.Nacionalidad%TYPE,
        P_GradoEstudios                 SFC_TB_CLIENTE_B2C.GradoEstudios%TYPE,
        P_Profesion                     SFC_TB_CLIENTE_B2C.Profesion%TYPE,
        P_PreferenciasGenerales         SFC_TB_CLIENTE_B2C.PreferenciasGenerales%TYPE,
        P_ConsideracionesSalud          SFC_TB_CLIENTE_B2C.ConsideracionesSalud%TYPE,
        P_TipoViaje                     SFC_TB_CLIENTE_B2C.TipoViaje%TYPE,
        P_TipoAcompanhante              SFC_TB_CLIENTE_B2C.TipoAcompanhante%TYPE
    );
        
    -- Crea un Branch para un Cliente existente --
    PROCEDURE SP_CREAR_CLIENTE_BRANCH
    (
        P_IDSalesforce                  SFC_TB_CLIENTE_BRANCH.IDSalesforce%TYPE,
        P_Item                          SFC_TB_CLIENTE_BRANCH.Item%TYPE,
        P_Tipo                          SFC_TB_CLIENTE_BRANCH.Tipo%TYPE
    );

    -- Crea un Canal de Información para un Cliente existente --
    PROCEDURE SP_CREAR_CLIENTE_CANALINFO
    (
        P_IDSalesforce                  SFC_TB_CLIENTE_CANALINFO.IDSalesforce%TYPE,
        P_Item                          SFC_TB_CLIENTE_CANALINFO.Item%TYPE,
        P_Tipo                          SFC_TB_CLIENTE_CANALINFO.Tipo%TYPE
    );

    -- Crea un Correo para un Cliente existente --
    PROCEDURE SP_CREAR_CLIENTE_CORREO
    (
        P_IDSalesforce                  SFC_TB_CLIENTE_CANALINFO.IDSalesforce%TYPE,
        P_Item                          SFC_TB_CLIENTE_CORREO.Item%TYPE,
        P_Tipo                          SFC_TB_CLIENTE_CORREO.Tipo%TYPE,
        P_Descripcion                   SFC_TB_CLIENTE_CORREO.Descripcion%TYPE
    );

    -- Crea una Dirección para un Cliente existente --
    PROCEDURE SP_CREAR_CLIENTE_DIRECCION
    (
        P_IDSalesforce                  SFC_TB_CLIENTE_DIRECCION.IDSalesforce%TYPE,
        P_Item                          SFC_TB_CLIENTE_DIRECCION.Item%TYPE,
        P_Tipo                          SFC_TB_CLIENTE_DIRECCION.Tipo%TYPE,
        P_Descripcion                   SFC_TB_CLIENTE_DIRECCION.Descripcion%TYPE,
        P_Distrito                      SFC_TB_CLIENTE_DIRECCION.Distrito%TYPE,
        P_Ciudad                        SFC_TB_CLIENTE_DIRECCION.Ciudad%TYPE,
        P_Departamento                  SFC_TB_CLIENTE_DIRECCION.Departamento%TYPE,
        P_Pais                          SFC_TB_CLIENTE_DIRECCION.Pais%TYPE
    );

    -- Crea un Documento para un Cliente existente --
    PROCEDURE SP_CREAR_CLIENTE_DOCUMENTO
    (
        P_IDSalesforce                  SFC_TB_CLIENTE_DOCUMENTO.IDSalesforce%TYPE,
        P_Item                          SFC_TB_CLIENTE_DOCUMENTO.Item%TYPE,
        P_Tipo                          SFC_TB_CLIENTE_DOCUMENTO.Tipo%TYPE,
        P_Numero                        SFC_TB_CLIENTE_DOCUMENTO.Numero%TYPE
    );

    -- Crea un Idioma para un Cliente existente --
    PROCEDURE SP_CREAR_CLIENTE_IDIOMA
    (
        P_IDSalesforce                  SFC_TB_CLIENTE_IDIOMA.IDSalesforce%TYPE,
        P_Item                          SFC_TB_CLIENTE_IDIOMA.Item%TYPE,
        P_Tipo                          SFC_TB_CLIENTE_IDIOMA.Tipo%TYPE
    );

    -- Crea un Participante para un Cliente existente --
    PROCEDURE SP_CREAR_CLIENTE_PARTICIPANTE
    (
        P_IDSalesforce                  SFC_TB_CLIENTE_PARTICIPANTE.IDSalesforce%TYPE,
        P_Item                          SFC_TB_CLIENTE_PARTICIPANTE.Item%TYPE,
        P_EmpleOrEjecResponsable        SFC_TB_CLIENTE_PARTICIPANTE.EmpleOrEjecResponsable%TYPE,
        P_SupervisorKam                 SFC_TB_CLIENTE_PARTICIPANTE.SupervisorKam%TYPE,
        P_Gerente                       SFC_TB_CLIENTE_PARTICIPANTE.Gerente%TYPE,
        P_UnidadNegocio                 SFC_TB_CLIENTE_PARTICIPANTE.UnidadNegocio%TYPE,
        P_GrupoColabEjecRegionBranch    SFC_TB_CLIENTE_PARTICIPANTE.GrupoColabEjecRegionBranch%TYPE,
        P_FlagPrincipal                 SFC_TB_CLIENTE_PARTICIPANTE.FlagPrincipal%TYPE,
        P_Descripcion                   SFC_TB_CLIENTE_PARTICIPANTE.Descripcion%TYPE
    );

    -- Crea un Sitio para un Cliente existente --
    PROCEDURE SP_CREAR_CLIENTE_SITIO
    (
        P_IDSalesforce                  SFC_TB_CLIENTE_SITIO.IDSalesforce%TYPE,
        P_Item                          SFC_TB_CLIENTE_SITIO.Item%TYPE,
        P_Tipo                          SFC_TB_CLIENTE_SITIO.Tipo%TYPE,
        P_Descripcion                   SFC_TB_CLIENTE_SITIO.Descripcion%TYPE
    );

    -- Crea un Telefono para un Cliente existente --
    PROCEDURE SP_CREAR_CLIENTE_TELEFONO
    (
        P_IDSalesforce                  SFC_TB_CLIENTE_TELEFONO.IDSalesforce%TYPE,
        P_Item                          SFC_TB_CLIENTE_TELEFONO.Item%TYPE,
        P_Tipo                          SFC_TB_CLIENTE_TELEFONO.Tipo%TYPE,
        P_Numero                        SFC_TB_CLIENTE_TELEFONO.Numero%TYPE
    );

    /* BUSCADORES DE ID */

    --
    /*
    PROCEDURE SP_BUSCAR_CLIENTE_TIPOPERSONA
    (
        P_
    );
    */

-- ▼ CONTACTO --

    -- Crea un Contacto --
    /*
    PROCEDURE SP_CREAR_CONTACTO
    (

    );
    */

END CRM_MDM_PKG;