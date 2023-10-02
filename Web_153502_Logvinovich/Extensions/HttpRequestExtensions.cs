namespace Web_153502_Logvinovich.Extensions
{
    public static class HttpRequestExtensions
    {
        public static bool IsAjaxRequest(this HttpRequest request)
        {
            return request != null && request.Headers["X-Requested-With"] == "XMLHttpRequest";
        }
    }
}
