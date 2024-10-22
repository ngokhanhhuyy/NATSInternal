namespace NATSInternal.Blazor.Components;

public class ModelState : ComponentBase
{
    private ValidationMessageStore _messageStore;

    [CascadingParameter]
    private EditContext CurrentEditContext { get; set; }

    public bool IsValid { get; private set; }

    protected override void OnInitialized()
    {
        _messageStore = new ValidationMessageStore(CurrentEditContext);
        CurrentEditContext.OnValidationRequested += (sender, e) => _messageStore.Clear();
        CurrentEditContext.OnFieldChanged += (sender, e) => _messageStore.Clear();
    }

    public void DisplayErrors(List<ValidationFailure> failures)
    {
        Dictionary<string, string> errors;
        errors = failures.ToDictionary(f => f.PropertyName, f => f.ErrorMessage);
        AddErrors(errors);
    }

    public void DisplayErrors(OperationException exception)
    {
        Dictionary<string, string> errors = new Dictionary<string, string>();
        errors[exception.PropertyName] = exception.Message;
        AddErrors(errors);
    }

    public void ClearErrors()
    {
        _messageStore?.Clear();
        CurrentEditContext?.NotifyValidationStateChanged();
        IsValid = false;
    }

    public string GetInputClass(string propertyName)
    {
        return IsPropertyValid(propertyName) ? "is-valid" : "is-invalid";
    }

    private void AddErrors(IDictionary<string, string> errors)
    {
        _messageStore?.Clear();
        foreach (KeyValuePair<string, string> error in errors)
        {
            FieldIdentifier field = CurrentEditContext.Field(error.Key);
            _messageStore.Add(field, error.Value);
        }

        CurrentEditContext.NotifyValidationStateChanged();
    }

    private bool IsPropertyValid(string propertyName)
    {
        IEnumerable<string> messages = _messageStore[CurrentEditContext.Field(propertyName)];
        return messages != null && !messages.Any();
    }
}
