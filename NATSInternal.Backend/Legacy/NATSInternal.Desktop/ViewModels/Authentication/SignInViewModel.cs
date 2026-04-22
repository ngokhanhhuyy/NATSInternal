using System.Threading.Tasks;
using Avalonia.Collections;
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
    private bool _hasSignedInSuccessfully;
    #endregion
    
    #region Constructors
    public SignInViewModel(IMediator mediator)
    {
        _mediator = mediator;
        SignInCommand = new AsyncRelayCommand(SignInAsync);
        PropertyChanged += (_, args) =>
        {
            if (args.PropertyName == nameof(Errors))
            {
                OnPropertyChanged(nameof(TestingCollection));
            }
        };
    }
    #endregion
    
    #region Properties
    public AvaloniaDictionary<string, string> TestingCollection { get; set; } = new()
    {
        { "UserName", "Testing" }
    };
    
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

    public bool HasSignedInSuccessfully
    {
        get => _hasSignedInSuccessfully;
        private set => SetProperty(ref _hasSignedInSuccessfully, value);
    }

    public IAsyncRelayCommand SignInCommand { get; }
    #endregion
    
    #region PrivateMethods
    private async Task SignInAsync()
    {
        IsSigninIn = true;
        Errors.Clear();
        TestingCollection.Clear();
        try
        {
            VerifyUserNameAndPasswordRequestDto requestDto = new()
            {
                UserName = _userName,
                Password = _password
            };

            await _mediator.Send(requestDto);
            HasSignedInSuccessfully = true;
        }
        catch (Exception exception)
        {
            Errors.Clear();
            switch (exception)
            {
                case ValidationException validationException:
                    foreach (ValidationFailure failure in validationException.Errors)
                    {
                        Errors.Add(new(failure.PropertyName, failure.ErrorMessage));
                        TestingCollection.Add(failure.PropertyName, failure.ErrorMessage);
                        TestingCollection.TryGetValue("UserName", out string? message);
                        Console.WriteLine(message ?? "null");
                    }
                    return;
                case OperationException operationException:
                    foreach (KeyValuePair<object[], string> pair in operationException.Errors)
                    {
                        Errors.Add(new(pair.Key, pair.Value));
                        TestingCollection.Add(string.Join(".", pair.Key), pair.Value);
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
    #endregion
}