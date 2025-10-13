using Avalonia.Controls;

namespace NATSInternal.Desktop.Views;

public partial class MainWindow : Window
{
    #region Constructors
    public MainWindow()
    {
        InitializeComponent();
        Content = new SignInPage();
    }
    #endregion
    
    #region PrivateMethods
    public void NavigateTo<TPage>() where TPage : Control, IDisposable, new()
    {
        if (Content is not null and IDisposable page)
        {
            page.Dispose();
        }

        Content = new TPage();
    }
    #endregion
}