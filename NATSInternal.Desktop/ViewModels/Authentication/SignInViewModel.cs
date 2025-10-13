using System.Threading.Tasks;
using Avalonia;
using CommunityToolkit.Mvvm.Input;
using MediatR;
using FluentValidation;
using FluentValidation.Results;
using NATSInternal.Application.Exceptions;
using NATSInternal.Application.UseCases.Authentication;

namespace NATSInternal.Desktop.ViewModels;

public class SignInViewModel : FormViewModel
{
    #region Fields
    private readonly IMediator _mediator;
    private string _userName = string.Empty;
    private string _password = string.Empty;
    private bool _isSigningIn;
    #endregion
    
    #region Constructors
    public SignInViewModel(IMediator mediator)
    {
        _mediator = mediator;
        SignInCommand = new AsyncRelayCommand(SignInAsync);
    }
    #endregion
    
    #region Properties
    public string UserName
    {
        get => _userName;
        set => SetProperty(ref _userName, value);
    }

    public string Password
    {
        get => _password;
        set => SetProperty(ref _password, value);
    }

    public bool IsSigninIn
    {
        get => _isSigningIn;
        private set => SetProperty(ref _isSigningIn, value);
    }
    
    public Thickness ErrorMessagesContainerMargin { get; private set; } = new(0, 0, 0, 0);
    
    public IAsyncRelayCommand SignInCommand { get; }
    #endregion
    
    #region PrivateMethods
    private async Task SignInAsync()
    {
        IsSigninIn = true;
        Errors.Clear();
        try
        {
            VerifyUserNameAndPasswordRequestDto requestDto = new()
            {
                UserName = _userName,
                Password = _password
            };

            await Task.Delay(2000);
            await _mediator.Send(requestDto);
        }
        catch (Exception exception)
        {
            Errors.Clear();
            switch (exception)
            {
                case ValidationException validationException:
                    // Errors.AddFromValidationException(validationException);
                    foreach (ValidationFailure failure in validationException.Errors)
                    {
                        Errors.Add(new(failure.PropertyName, failure.ErrorMessage));
                    }
                    return;
                case OperationException operationException:
                    foreach (KeyValuePair<object[], string> pair in operationException.Errors)
                    {
                        Errors.Add(new(pair.Key, pair.Value));
                    }
                    return;
            }

            throw;
        }
        finally
        {
            IsSigninIn = false;
            OnPropertyChanged(nameof(Errors));
        }
    }

    // private void OnErrorsHasErrorsChanged(object? _, PropertyChangedEventArgs args)
    // {
    //     Thickness oldMargin = ErrorMessagesContainerMargin;
    //     if (args.PropertyName == nameof(Errors.HasErrors))
    //     {
    //         int bottomMargin = Errors.HasErrors ? 5 : 0;
    //         ErrorMessagesContainerMargin = new(0, 0, 0, bottomMargin);
    //
    //         if (!Equals(oldMargin, ErrorMessagesContainerMargin))
    //         {
    //             OnPropertyChanged(nameof(ErrorMessagesContainerMargin));
    //         }
    //     }
    // }
    #endregion
}