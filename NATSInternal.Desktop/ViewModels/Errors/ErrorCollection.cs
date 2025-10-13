using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

namespace NATSInternal.Desktop.ViewModels;

public sealed class ErrorCollection : ObservableCollection<ErrorDetail>
{
    #region Fields
    private bool _hasErrors;
    #endregion
    
    #region Constructors
    public ErrorCollection()
    {
        PropertyChanged += UpdateHasErrorWhenCountChanged;
        CollectionChanged += RaiseIndexerPropertyChanged;
    }
    #endregion
    
    #region Properties
    public bool HasErrors
    {
        get => _hasErrors;
        set
        {
            if (_hasErrors != value)
            {
                _hasErrors = value;
                OnPropertyChanged(new(nameof(HasErrors)));
            }
        }
    }
    #endregion
    
    public string? this[string propertyPath] => this.FirstOrDefault(e => e.PropertyPath == propertyPath)?.Message;
    
    #region PrivateMethods
    private void UpdateHasErrorWhenCountChanged(object? _, PropertyChangedEventArgs args)
    {
        if ((args.PropertyName ?? string.Empty).StartsWith("Item"))
        {
            Console.WriteLine(args.PropertyName);
        }
        
        if (args.PropertyName == nameof(Count))
        {
            HasErrors = Count > 0;
        }
    }
    
    private void RaiseIndexerPropertyChanged(object? _, NotifyCollectionChangedEventArgs args)
    {
        HashSet<string> changedPropertyPaths = new();
        if (args.OldItems is not null)
        {
            foreach (object item in args.OldItems)
            {
                AddToPropertyPaths(item);
            }
        }
    
        if (args.NewItems is not null)
        {
            foreach (object item in args.NewItems)
            {
                AddToPropertyPaths(item);
            }
        }
    
        foreach (string propertyPath in changedPropertyPaths)
        {
            base.OnPropertyChanged(new($"Item[{propertyPath}]"));
        }
    
        return;
    
        void AddToPropertyPaths(object item) => changedPropertyPaths.Add(((ErrorDetail)item).PropertyPath);
    }
    #endregion
}