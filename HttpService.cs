using DNE.CS.Inventory.Library.Interface;
using Microsoft.AspNetCore.Components;
using Microsoft.IdentityModel.Tokens;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace DNE.CS.Inventory.Library;

public class HttpService : IHttpService
{
    private readonly HttpClient _httpClient;
    private readonly NavigationManager _navigationManager;

    public HttpService(HttpClient httpClient, NavigationManager navigationManager)
    {
        _httpClient = httpClient;
        _navigationManager = navigationManager;
    }

    public async Task<HttpResponse> DeleteAsync(string uri,
        string accessToken)
    {
        var request = new HttpRequestMessage(HttpMethod.Delete, uri);

        return await SendRequestAsync(request, accessToken);
    }

    public async Task<HttpResponse> DeleteAsync(string uri)
    {
        var request = new HttpRequestMessage(HttpMethod.Delete, uri);

        return await SendRequestAsync(request);
    }

    public async Task<HttpResponse> GetAsync(string uri,
        string accessToken)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, uri);

        return await SendRequestAsync(request, accessToken);
    }

    public async Task<HttpResponse> GetAsync(string uri,
        string accessToken, CancellationToken cancellationToken)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, uri);

        return await SendRequestAsync(request, accessToken, cancellationToken);
    }

    public async Task<HttpResponse> GetAsync(string uri)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, uri);

        return await SendRequestAsync(request);
    }

    public async Task<HttpResponse> GetCookieAsync(string uri, string cookie, string cookieName)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, uri);

        return await SendRequestCookieAsync(request, cookie, cookieName);
    }

    public async Task<HttpResponse> PostAsync(string uri,
        string accessToken, object? value = null)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, uri);

        if (value != null)
        {
            request.Content = new StringContent(JsonSerializer.Serialize(value), Encoding.UTF8, "application/json");
        }

        return await SendRequestAsync(request, accessToken);
    }

    public async Task<FileHttpResponse> PostForFileAsync(string uri,
        string accessToken, object? value = null)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, uri);

        if (value != null)
        {
            request.Content = new StringContent(JsonSerializer.Serialize(value), Encoding.UTF8, "application/json");
        }

        return await SendFileRequestAsync(request, accessToken);
    }

    public async Task<HttpResponse> PostAsync(string uri,
        string accessToken, MultipartFormDataContent content)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, uri);
        request.Content = content;

        return await SendRequestAsync(request, accessToken);
    }

    public async Task<HttpResponse> PostAsync(string uri,
        object? value = null)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, uri);

        if (value != null)
        {
            request.Content = new StringContent(JsonSerializer.Serialize(value), Encoding.UTF8, "application/json");
        }

        return await SendRequestAsync(request);
    }

    public async Task<HttpResponse> PutAsync(string uri,
        string accessToken, object? value = null)
    {
        var request = new HttpRequestMessage(HttpMethod.Put, uri);

        if (value != null)
        {
            request.Content = new StringContent(JsonSerializer.Serialize(value), Encoding.UTF8, "application/json");
        }

        return await SendRequestAsync(request, accessToken);
    }

    public async Task<HttpResponse> PutAsync(string uri, object? value = null)
    {
        var request = new HttpRequestMessage(HttpMethod.Put, uri);

        if (value != null)
        {
            request.Content = new StringContent(JsonSerializer.Serialize(value), Encoding.UTF8, "application/json");
        }

        return await SendRequestAsync(request);
    }

    private async Task<HttpResponse> SendRequestAsync(HttpRequestMessage request,
        string accessToken, CancellationToken cancellationToken)
    {
        HttpResponse httpResponse = new HttpResponse();

        try
        {
            var isApiUrl = request.RequestUri?.IsAbsoluteUri ?? false;

            if (!string.IsNullOrEmpty(accessToken) && isApiUrl)
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            using HttpResponseMessage response = await _httpClient.SendAsync(request, cancellationToken);

            httpResponse.IsSuccess = response.IsSuccessStatusCode;
            httpResponse.HttpStatusCode = response.StatusCode;

            if (!response.IsSuccessStatusCode)
            {
                httpResponse.Error = response.Content.ReadAsStringAsync().Result;
                return httpResponse;
            }

            httpResponse.Data = response.Content.ReadAsStringAsync().Result;
            return httpResponse;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    private async Task<HttpResponse> SendRequestAsync(HttpRequestMessage request,
        string accessToken)
    {
        HttpResponse httpResponse = new HttpResponse();

        try
        {
            var isApiUrl = request.RequestUri?.IsAbsoluteUri ?? false;

            if (!string.IsNullOrEmpty(accessToken) && isApiUrl)
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            using HttpResponseMessage response = await _httpClient.SendAsync(request);

            httpResponse.IsSuccess = response.IsSuccessStatusCode;
            httpResponse.HttpStatusCode = response.StatusCode;

            if (!response.IsSuccessStatusCode)
            {
                httpResponse.Error = response.Content.ReadAsStringAsync().Result;
                return httpResponse;
            }

            httpResponse.Data = response.Content.ReadAsStringAsync().Result;
            return httpResponse;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    private async Task<FileHttpResponse> SendFileRequestAsync(HttpRequestMessage request,
        string accessToken)
    {
        FileHttpResponse httpResponse = new FileHttpResponse();

        try
        {
            var isApiUrl = request.RequestUri?.IsAbsoluteUri ?? false;

            if (!string.IsNullOrEmpty(accessToken) && isApiUrl)
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            using HttpResponseMessage response = await _httpClient.SendAsync(request);

            httpResponse.IsSuccess = response.IsSuccessStatusCode;
            httpResponse.HttpStatusCode = response.StatusCode;

            if (!response.IsSuccessStatusCode)
            {
                httpResponse.Error = response.Content.ReadAsStringAsync().Result;
                return httpResponse;
            }

            httpResponse.FileBytes = await response.Content.ReadAsByteArrayAsync();
            return httpResponse;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    private async Task<HttpResponse> SendRequestCookieAsync(HttpRequestMessage request,
        string cookie, string cookieName)
    {
        HttpResponse httpResponse = new HttpResponse();

        try
        {
            var isApiUrl = request.RequestUri?.IsAbsoluteUri ?? false;

            if (!string.IsNullOrEmpty(cookie) && isApiUrl && !cookieName.IsNullOrEmpty())
            {
                cookie = $"{cookieName}={cookie};";
                request.Headers.Add("Cookie", cookie);
            }

            using HttpResponseMessage response = await _httpClient.SendAsync(request);

            httpResponse.IsSuccess = response.IsSuccessStatusCode;
            httpResponse.HttpStatusCode = response.StatusCode;

            if (!response.IsSuccessStatusCode)
            {
                httpResponse.Error = response.Content.ReadAsStringAsync().Result;
                return httpResponse;
            }

            httpResponse.Data = response.Content.ReadAsStringAsync().Result;
            return httpResponse;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    private async Task<HttpResponse> SendRequestAsync(HttpRequestMessage request)
    {
        HttpResponse httpResponse = new HttpResponse();
        try
        {
            using HttpResponseMessage response = await _httpClient.SendAsync(request);
            httpResponse.IsSuccess = response.IsSuccessStatusCode;
            httpResponse.HttpStatusCode = response.StatusCode;

            if (!response.IsSuccessStatusCode)
            {
                httpResponse.Error = response.Content.ReadAsStringAsync().Result;
                return httpResponse;
            }

            httpResponse.Data = response.Content.ReadAsStringAsync().Result;
            return httpResponse;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
}
