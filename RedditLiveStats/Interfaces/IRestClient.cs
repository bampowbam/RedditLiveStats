public interface IRestClient
{
    Task<HttpResponseMessage> SendAsync(HttpRequestMessage request);
}
