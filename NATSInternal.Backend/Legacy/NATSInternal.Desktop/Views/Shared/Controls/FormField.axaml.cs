using System.Collections.Specialized;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using NATSInternal.Desktop.ViewModels;

namespace NATSInternal.Desktop.Views;

public partial class FormField : ContentControl
{
    #region Fields
    private string? _errorMessageText;
    #endregion
    
    #region StaticFields
    public static readonly StyledProperty<string?> LabelTextProperty =
        AvaloniaProperty.Register<FormField, string?>(
            nameof(LabelText),
            defaultBindingMode: BindingMode.OneWay);
    
    public static readonly StyledProperty<ErrorCollection?> ErrorCollectionProperty =
        AvaloniaProperty.Register<FormField, ErrorCollection?>(
            nameof(ErrorCollection),
            defaultBindingMode: BindingMode.OneWay);
    
    public static readonly StyledProperty<string?> ErrorPropertyPathProperty =
        AvaloniaProperty.Register<FormField, string?>(
            nameof(ErrorPropertyPath),
            defaultBindingMode: BindingMode.OneWay);

    private static readonly DirectProperty<FormField, string?> ErrorMessageTextProperty =
        AvaloniaProperty.RegisterDirect<FormField, string?>(
            nameof(ErrorMessageText),
            formField => formField.ErrorMessageText);
    #endregion
    
    #region Constructors
    public FormField()
    {
        InitializeComponent();
        this.GetObservable(ErrorCollectionProperty).Subscribe(_ => UpdateErrorBinding());
        this.GetObservable(ErrorPropertyPathProperty).Subscribe(_ => UpdateErrorBinding());
    }
    #endregion
    
    #region Properties
    public string? LabelText
    {
        get => GetValue(LabelTextProperty);
        set => SetValue(LabelTextProperty, value);
    }

    public ErrorCollection? ErrorCollection
    {
        get => GetValue(ErrorCollectionProperty);
        set => SetValue(ErrorCollectionProperty, value);
    }

    public string? ErrorPropertyPath
    {
        get => GetValue(ErrorPropertyPathProperty);
        set => SetValue(ErrorPropertyPathProperty, value);
    }

    public string? ErrorMessageText
    {
        get => _errorMessageText;
        private set => SetAndRaise(ErrorMessageTextProperty, ref _errorMessageText, value);
    }
    #endregion

    #region PrivateMethods
    private void UpdateErrorBinding()
    {
        if (ErrorCollection is null || string.IsNullOrEmpty(ErrorPropertyPath))
        {
            ErrorMessageText = string.Empty;
            return;
        }

        ErrorMessageText = ErrorCollection[ErrorPropertyPath];

        ErrorCollection.CollectionChanged += (_, args) =>
        {
            if (args.Action is not NotifyCollectionChangedAction.Add and not NotifyCollectionChangedAction.Reset)
            {
                return;
            }

            bool shouldAssign = false;
            if (args.OldItems is not null)
            {
                if (args.OldItems.Cast<object?>().Any(item => ((ErrorDetail)item!).PropertyPath == ErrorPropertyPath))
                {
                    shouldAssign = true;
                }
            }
            
            if (args.NewItems is not null)
            {
                if (args.NewItems.Cast<object?>().Any(item => ((ErrorDetail)item!).PropertyPath == ErrorPropertyPath))
                {
                    shouldAssign = true;
                }
            }

            if (shouldAssign)
            {
                ErrorMessageText = ErrorCollection[ErrorPropertyPath];
            }
        };
    }
    #endregion
}