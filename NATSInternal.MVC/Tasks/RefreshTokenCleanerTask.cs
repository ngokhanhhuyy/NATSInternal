namespace NATSInternal.Tasks;

public class RefreshTokenCleanerTask : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;

    public RefreshTokenCleanerTask(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        using IServiceScope scope = _scopeFactory.CreateScope();
        IAuthenticationService authenticationService = scope.ServiceProvider
            .GetRequiredService<IAuthenticationService>();
        await authenticationService.CleanExpiredRefreshTokens();
    }
}