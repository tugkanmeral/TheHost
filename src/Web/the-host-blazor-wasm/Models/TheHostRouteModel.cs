public class TheHostRouteModel
{
    public string Service { get; set; } = null!;
    public string Url { get; set; } = null!;
    public HashSet<EndPoint> EndPoints { get; set; } = new();
}

public class EndPoint
{
    public string Name { get; set; } = null!;
    public string Url { get; set; } = null!;
}