public class AuthRequest
{
    public string Username { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string? MasterKey { get; set; }
}