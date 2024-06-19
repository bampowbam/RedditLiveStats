using System.Net.Http;
using System.Threading.Tasks;
using RestSharp;

public class RestClient : IRestClient
{
    private readonly HttpClient _httpClient;

    public RestClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request)
    {
        return await _httpClient.SendAsync(request);
    }
}
