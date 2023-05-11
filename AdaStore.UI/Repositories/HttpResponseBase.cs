using System.Net;

namespace AdaStore.UI.Repositories
{
    public class HttpResponseBase<T>
    {
        public bool IsSuccess { get; set; }
        public T Data { get; set; }
        public HttpResponseMessage Response { get; set; }

        public async Task<string> GetErrorMessage()
        {
            if (IsSuccess) return string.Empty;

            if (Response == null) return "Ha ocurrido un error inesperado";

            var status = Response.StatusCode;

            if (status == HttpStatusCode.NotFound)
            {
                return "El recurso no fue encontrado";
            }
            else if (status == HttpStatusCode.BadRequest)
            {
                return await Response.Content.ReadAsStringAsync();
            }
            else if (status == HttpStatusCode.Unauthorized)
            {
                return "Tienes que loguearte para hacer esto";
            }
            else if (status == HttpStatusCode.Forbidden)
            {
                return "No tienes permisos para hacer esto";
            }
            else
            {
                return "Ha ocurrido un error inesperado";
            }
        }
    }
}
