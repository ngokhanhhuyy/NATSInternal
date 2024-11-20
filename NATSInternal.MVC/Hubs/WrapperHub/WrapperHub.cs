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
        bool isRedirected = IsRedirectResponseMessage(
            httpResponseMessage,
            out string redirectedPath);
        await Clients.Caller.SendAsync("ResponsePost", new
        {
            PathName = pathName,
            httpResponseMessage.StatusCode,
            Template = template,
            RedirectTo = isRedirected ? redirectedPath : null,
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
        bool isRedirected = IsRedirectResponseMessage(
            httpResponseMessage,
            out string redirectedPath);
        await Clients.Caller.SendAsync("ResponsePost", new
        {
            PathName = pathName,
            httpResponseMessage.StatusCode,
            Template = template,
            RedirectTo = isRedirected ? redirectedPath : null,
        });
    }

    private bool IsRedirectResponseMessage(HttpResponseMessage message, out string redirectedPath)
    {
        HttpStatusCode[] redirectStatusCodes = new[]
        {
            HttpStatusCode.MovedPermanently,
            HttpStatusCode.Found,
            HttpStatusCode.TemporaryRedirect
        };
        redirectedPath = message.Headers.Location?.AbsolutePath;
        bool isRedirected = redirectStatusCodes.Contains(message.StatusCode);
        Console.WriteLine($"IsRedirected: {isRedirected}, RedirectedPath: {redirectedPath}");
        return isRedirected;
    }
}
