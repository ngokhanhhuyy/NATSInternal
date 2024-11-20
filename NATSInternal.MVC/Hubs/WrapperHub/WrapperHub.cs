using System.Net;

namespace NATSInternal.Hubs;

[Authorize]
public class WrapperHub : Hub
{
    private static readonly Dictionary<string, HttpClient> _httpClients;
    private readonly IHttpClientFactory _httpClientFactory;

    static WrapperHub()
    {
        _httpClients = new Dictionary<string, HttpClient>();
    }

    public WrapperHub(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
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
        HttpClient httpClient = GetHttpClient();
        HttpResponseMessage httpResponseMessage;
        Console.WriteLine("http://localhost:5000" + pathName);
        httpResponseMessage = await httpClient.GetAsync("http://localhost:5000" + pathName);
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
        HttpClient httpClient = GetHttpClient();

        Console.WriteLine(JsonSerializer.Serialize(formData));
        FormUrlEncodedContent content = new FormUrlEncodedContent(formData);
        
        HttpResponseMessage httpResponseMessage;
        Console.WriteLine("http://localhost:5000" + pathName);
        httpResponseMessage = await httpClient.PostAsync(
            "http://localhost:5000" +
            pathName, content);
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

    private HttpClient GetHttpClient()
    {
        HttpContext httpContext = Context.GetHttpContext()
            ?? throw new InvalidOperationException("HttpContext doesn't exist (null)");

        HttpClient httpClient = _httpClientFactory.CreateClient();
        string cookieHeader = httpContext.Request.Cookies.ToString();
        if (!string.IsNullOrEmpty(cookieHeader))
        {
            httpClient.DefaultRequestHeaders.Add("Cookie", cookieHeader);
        }

        return httpClient;
    }

    private bool IsRedirectResponseMessage(HttpResponseMessage message, out string redirectedPath)
    {
        HttpStatusCode[] redirectStatusCodes = new[]
        {
            HttpStatusCode.MovedPermanently,
            HttpStatusCode.Found,
            HttpStatusCode.TemporaryRedirect
        };
        redirectedPath = message.Headers.Location?.ToString().Replace("http://localhost:5000", "");
        bool isRedirected = redirectStatusCodes.Contains(message.StatusCode);
        Console.WriteLine($"IsRedirected: {isRedirected}, RedirectedPath: {redirectedPath}");
        return isRedirected;
    }
}
