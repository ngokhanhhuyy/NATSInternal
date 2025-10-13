using Avalonia.Collections;

namespace NATSInternal.Desktop.ViewModels;

public abstract class FormViewModel : ViewModelBase
{
    #region Constructors
    protected FormViewModel()
    {
        Errors.CollectionChanged += (_, _) => OnPropertyChanged(nameof(Errors));
    }
    #endregion
    
    #region Properties
    public ErrorCollection Errors { get; } = new();
    #endregion
}