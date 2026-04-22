using Avalonia.Controls;

namespace NATSInternal.Desktop.Views;

public abstract class AbstractPage : UserControl, IDisposable
{
    #region Fields
    private readonly IServiceScope _serviceScope;
    #endregion
    
    #region Constructors
    protected AbstractPage()
    {
        IServiceScopeFactory scopeFactory = App.RootServiceProvider.GetRequiredService<IServiceScopeFactory>();
        _serviceScope = scopeFactory.CreateScope();
    }
    #endregion
    
    #region Properties
    protected IServiceProvider Services => _serviceScope.ServiceProvider;
    #endregion
    
    #region AbstractMethods
    public virtual void Dispose()
    {
        _serviceScope.Dispose();
        GC.SuppressFinalize(this);
    }
    #endregion
}