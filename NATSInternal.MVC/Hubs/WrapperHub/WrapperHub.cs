using System.Net;

namespace NATSInternal.Hubs;

public class WrapperHub : Hub
{
    private static readonly Dictionary<string, HttpClient> _httpClients;

    static WrapperHub()
    {
        _httpClients = new Dictionary<string, HttpClient>();
    }

    public override async Task OnConnectedAsync()
    {
        _httpClients.Add(Context.ConnectionId, null);
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception exception)
    {
        _httpClients.Remove(Context.ConnectionId);
        await base.OnDisconnectedAsync(exception);
    }

    public async Task RequestGet(string pathName)
    {
        HttpClient httpClient = _httpClients[Context.ConnectionId];
        if (httpClient == null)
        {
            CookieContainer cookieContainer = new CookieContainer();
            HttpClientHandler httpClientHandler = new HttpClientHandler
            {
                CookieContainer = cookieContainer,
                AllowAutoRedirect = false
            };
            httpClient = new HttpClient(httpClientHandler);
        }

        
        HttpResponseMessage httpResponseMessage;
        httpResponseMessage = await httpClient.GetAsync(pathName);
        string template = await httpResponseMessage.Content.ReadAsStringAsync();
        bool isRedirected = new List<HttpStatusCode>
        { 301, 302, 307 }
        
        .Contains(httpResponseMessage.StatusCode)
            .TryGetValues("Location", out IEnumerable<string> redirectedUrl);
        Console.WriteLine(isRedirected);
        await Clients.Caller.SendAsync("ResponsePost", new
        {
            PathName = isRedirected ? redirectedUrl.First() : pathName,
            httpResponseMessage.StatusCode,
            Template = template,
            IsRedirected = redirectedUrl.First()
        });
    }

    public async Task RequestPost(string pathName, Dictionary<string, string> formData)
    {
        HttpClient httpClient = _httpClients[Context.ConnectionId];
        if (httpClient == null)
        {
            CookieContainer cookieContainer = new CookieContainer();
            HttpClientHandler httpClientHandler = new HttpClientHandler
            {
                CookieContainer = cookieContainer,
                AllowAutoRedirect = false
            };
            httpClient = new HttpClient(httpClientHandler);
        }

        Console.WriteLine(JsonSerializer.Serialize(formData));
        FormUrlEncodedContent content = new FormUrlEncodedContent(formData);
        
        HttpResponseMessage httpResponseMessage;
        httpResponseMessage = await httpClient.PostAsync(pathName, content);
        string template = await httpResponseMessage.Content.ReadAsStringAsync();
        bool isRedirected = httpResponseMessage.Headers
            .TryGetValues("Location", out IEnumerable<string> redirectedUrl);
        Console.WriteLine(isRedirected);
        await Clients.Caller.SendAsync("ResponsePost", new
        {
            PathName = isRedirected ? redirectedUrl.First() : pathName,
            httpResponseMessage.StatusCode,
            Template = template,
            IsRedirected = redirectedUrl.First()
        });
    }

    private bool
}
