﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Este código fue generado por una herramienta.
//     Versión de runtime:4.0.30319.42000
//
//     Los cambios en este archivo podrían causar un comportamiento incorrecto y se perderán si
//     se vuelve a generar el código.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// Microsoft.VSDesigner generó automáticamente este código fuente, versión=4.0.30319.42000.
// 
#pragma warning disable 1591

namespace Expertia.Estructura.ws_pax_aux.ws_pax_paxdoc {
    using System;
    using System.Web.Services;
    using System.Diagnostics;
    using System.Web.Services.Protocols;
    using System.Xml.Serialization;
    using System.ComponentModel;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.3761.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name="ws_pax_paxdocSoap", Namespace="http://tempuri.org/")]
    public partial class ws_pax_paxdoc : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        private System.Threading.SendOrPostCallback prc_web_insert_paxOperationCompleted;
        
        private bool useDefaultCredentialsSetExplicitly;
        
        /// <remarks/>
        public ws_pax_paxdoc() {
            this.Url = global::Expertia.Estructura.Properties.Settings.Default.Expertia_Estructura_ws_pax_aux_ws_pax_paxdoc_ws_pax_paxdoc;
            if ((this.IsLocalFileSystemWebService(this.Url) == true)) {
                this.UseDefaultCredentials = true;
                this.useDefaultCredentialsSetExplicitly = false;
            }
            else {
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        public new string Url {
            get {
                return base.Url;
            }
            set {
                if ((((this.IsLocalFileSystemWebService(base.Url) == true) 
                            && (this.useDefaultCredentialsSetExplicitly == false)) 
                            && (this.IsLocalFileSystemWebService(value) == false))) {
                    base.UseDefaultCredentials = false;
                }
                base.Url = value;
            }
        }
        
        public new bool UseDefaultCredentials {
            get {
                return base.UseDefaultCredentials;
            }
            set {
                base.UseDefaultCredentials = value;
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        /// <remarks/>
        public event prc_web_insert_paxCompletedEventHandler prc_web_insert_paxCompleted;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/prc_web_insert_pax", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public r_message[] prc_web_insert_pax(string p_tipo_doc_identidad, string p_numero_doc_identidad, string p_apellido_paterno, string p_apellido_materno, string p_nombres, string p_nombres2, string p_direccion, string p_email, string p_fecha_cumple, string p_telefono_casa, string p_telefono_celular, string p_telefono_oficina, string p_usuario) {
            object[] results = this.Invoke("prc_web_insert_pax", new object[] {
                        p_tipo_doc_identidad,
                        p_numero_doc_identidad,
                        p_apellido_paterno,
                        p_apellido_materno,
                        p_nombres,
                        p_nombres2,
                        p_direccion,
                        p_email,
                        p_fecha_cumple,
                        p_telefono_casa,
                        p_telefono_celular,
                        p_telefono_oficina,
                        p_usuario});
            return ((r_message[])(results[0]));
        }
        
        /// <remarks/>
        public void prc_web_insert_paxAsync(string p_tipo_doc_identidad, string p_numero_doc_identidad, string p_apellido_paterno, string p_apellido_materno, string p_nombres, string p_nombres2, string p_direccion, string p_email, string p_fecha_cumple, string p_telefono_casa, string p_telefono_celular, string p_telefono_oficina, string p_usuario) {
            this.prc_web_insert_paxAsync(p_tipo_doc_identidad, p_numero_doc_identidad, p_apellido_paterno, p_apellido_materno, p_nombres, p_nombres2, p_direccion, p_email, p_fecha_cumple, p_telefono_casa, p_telefono_celular, p_telefono_oficina, p_usuario, null);
        }
        
        /// <remarks/>
        public void prc_web_insert_paxAsync(string p_tipo_doc_identidad, string p_numero_doc_identidad, string p_apellido_paterno, string p_apellido_materno, string p_nombres, string p_nombres2, string p_direccion, string p_email, string p_fecha_cumple, string p_telefono_casa, string p_telefono_celular, string p_telefono_oficina, string p_usuario, object userState) {
            if ((this.prc_web_insert_paxOperationCompleted == null)) {
                this.prc_web_insert_paxOperationCompleted = new System.Threading.SendOrPostCallback(this.Onprc_web_insert_paxOperationCompleted);
            }
            this.InvokeAsync("prc_web_insert_pax", new object[] {
                        p_tipo_doc_identidad,
                        p_numero_doc_identidad,
                        p_apellido_paterno,
                        p_apellido_materno,
                        p_nombres,
                        p_nombres2,
                        p_direccion,
                        p_email,
                        p_fecha_cumple,
                        p_telefono_casa,
                        p_telefono_celular,
                        p_telefono_oficina,
                        p_usuario}, this.prc_web_insert_paxOperationCompleted, userState);
        }
        
        private void Onprc_web_insert_paxOperationCompleted(object arg) {
            if ((this.prc_web_insert_paxCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.prc_web_insert_paxCompleted(this, new prc_web_insert_paxCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        public new void CancelAsync(object userState) {
            base.CancelAsync(userState);
        }
        
        private bool IsLocalFileSystemWebService(string url) {
            if (((url == null) 
                        || (url == string.Empty))) {
                return false;
            }
            System.Uri wsUri = new System.Uri(url);
            if (((wsUri.Port >= 1024) 
                        && (string.Compare(wsUri.Host, "localHost", System.StringComparison.OrdinalIgnoreCase) == 0))) {
                return true;
            }
            return false;
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.8.3761.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://tempuri.org/")]
    public partial class r_message {
        
        private string idField;
        
        private string msgField;
        
        /// <remarks/>
        public string Id {
            get {
                return this.idField;
            }
            set {
                this.idField = value;
            }
        }
        
        /// <remarks/>
        public string msg {
            get {
                return this.msgField;
            }
            set {
                this.msgField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.3761.0")]
    public delegate void prc_web_insert_paxCompletedEventHandler(object sender, prc_web_insert_paxCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.3761.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class prc_web_insert_paxCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal prc_web_insert_paxCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public r_message[] Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((r_message[])(this.results[0]));
            }
        }
    }
}

#pragma warning restore 1591