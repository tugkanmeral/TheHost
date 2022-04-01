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

    private string? masterKey;
    public string MasterKey
    {
        get
        {
            if (masterKey == null)
                throw new NullReferenceException();

            return masterKey;
        }
        set
        {
            masterKey = value;
        }
    }

    private int[] iv = new int[] { 32, 2, 18, 119, 65, 3, 44, 72, 13, 65, 82, 65 };
    public int[] IV => iv;
}

