using System.Net;

namespace DNE.CS.Inventory.Library.Interface;

public interface IHttpService
{
    Task<HttpResponse> GetAsync(string uri, string accessToken);
    Task<HttpResponse> GetAsync(string uri,
        string accessToken, CancellationToken cancellationToken);
    Task<HttpResponse> GetAsync(string uri);
    Task<HttpResponse> GetCookieAsync(string uri, string cookie, string cookieName);
    Task<HttpResponse> PostAsync(string uri, string accessToken,
        object? value = null);
    Task<HttpResponse> PostAsync(string uri, object? value = null);
    Task<HttpResponse> PutAsync(string uri, string accessToken,
        object? value = null);
    Task<HttpResponse> PutAsync(string uri, object? value = null);
    Task<HttpResponse> DeleteAsync(string uri, string accessToken);
    Task<HttpResponse> DeleteAsync(string uri);
}

public class HttpResponse
{
    public HttpStatusCode HttpStatusCode { get; set; }
    public bool IsSuccess { get; set; } = false;
    public string? Error { get; set; }
    public string? Data { get; set; }
}