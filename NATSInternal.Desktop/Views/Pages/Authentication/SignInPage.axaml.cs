using Avalonia.Data.Converters;
using NATSInternal.Desktop.ViewModels;

namespace NATSInternal.Desktop.Views;

public partial class SignInPage : AbstractPage
{
    #region Constructors
    public SignInPage()
    {
        DataContext = App.RootServiceProvider.GetRequiredService<SignInViewModel>();
        InitializeComponent();
    }

    static SignInPage()
    {
        IsSignInToTextConverter = new(isSignIn => isSignIn ?? false ? "Đang đăng nhập" : "Đăng nhập");
    }
    #endregion
    
    
    #region StaticProperties
    public static FuncValueConverter<bool?, string> IsSignInToTextConverter { get; set; }
    #endregion
}