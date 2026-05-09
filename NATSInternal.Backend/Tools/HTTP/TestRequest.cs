#:project ../../NATSInternal.Api
#:project ../../NATSInternal.Core
#:property PublishAot=false

using NATSInternal.Core.Features.Authentication;
using NATSInternal.Core.Features.Users;
using System.Collections.Specialized;
using System.Net;
using System.Net.Http.Json;
using System.Reflection;
using System.Text.Json;
using System.Web;

TestClient testClient = new();
await testClient.RunAsync();

partial class TestClient
{
    private readonly HttpClient _httpClient;
    private readonly CookieContainer _cookieContainer;
    private static readonly JsonSerializerOptions serializerOptions;
    private const string Host = "http://localhost:5000/api";

    public TestClient()
    {
        _cookieContainer = new();
        HttpClientHandler handler = new()
        {
            CookieContainer = _cookieContainer,
            UseCookies = true
        };

        _httpClient = new(handler);
    }

    static TestClient()
    {
        serializerOptions = new()
        {
            WriteIndented = true
        };
    }

    public async Task RunAsync()
    {
        await LoginAsync();
        await GetUserListAsync();
    }

    private async Task GetUserListAsync()
    {
        Uri uri = new($"{Host}/users");
        Dictionary<string, object>? userListResponseDto = await _httpClient.GetFromJsonAsync<Dictionary<string, object>>(uri);

        Console.WriteLine(JsonSerializer.Serialize(userListResponseDto, serializerOptions));
    }

    private async Task LoginAsync()
    {
        VerifyUserNameAndPasswordRequestDto requestDto = new()
        {
            UserName = "ngokhanhhuyy",
            Password = "Huyy47b1"
        };

        Uri uri = new($"{Host}/authentication/get-access-cookie");
        HttpResponseMessage response = await _httpClient.PostAsJsonAsync(uri, requestDto);
        response.EnsureSuccessStatusCode();
    }
}