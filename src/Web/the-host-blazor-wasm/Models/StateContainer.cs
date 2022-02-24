public class StateContainer
{
    private string? token;
    public string Token
    {
        get => token ?? string.Empty;
        set
        {
            token = value;
            NotifyTokenStateChanged();
        }
    }
    public event Action? OnTokenChange;
    private void NotifyTokenStateChanged() => OnTokenChange?.Invoke();

    private HashSet<TheHostRouteModel>? routes;
    public HashSet<TheHostRouteModel>? Routes
    {
        get => routes ?? new();
        set
        {
            routes = value;
        }
    }
}

