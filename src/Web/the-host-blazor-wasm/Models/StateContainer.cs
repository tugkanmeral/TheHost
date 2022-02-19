public class StateContainer
{
    private string? token;

    public string Token
    {
        get => token ?? string.Empty;
        set
        {
            token = value;
            NotifyStateChanged();
        }
    }

    public event Action? OnChange;

    private void NotifyStateChanged() => OnChange?.Invoke();
}

