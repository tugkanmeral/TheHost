public class HttpResponse
{
    public bool isError { get; }
    public string message { get; } = null!;
    public object data { get; } = null!;
}

