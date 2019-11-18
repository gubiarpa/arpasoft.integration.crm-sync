namespace Expertia.Estructura.Models.Behavior
{
    public interface ICrmApiResponse
    {
        string CodigoError { get; set; }
        string MensajeError { get; set; }
    }

    public class CrmApiResponse : ICrmApiResponse
    {
        public string CodigoError { get; set; }
        public string MensajeError { get; set; }

        public CrmApiResponse(string codigoError, string mensajeError)
        {
            CodigoError = codigoError;
            MensajeError = mensajeError;
        }
    }
}
