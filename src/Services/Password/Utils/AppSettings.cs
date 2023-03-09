public class AppSettings
{
    public string IV { get; set; } = String.Empty;
    public string MongoUri { get; set; } = String.Empty;
    public PasswordSalt Salt { get; set; } = new();
    public string AuthSecretKey { get; set; } = String.Empty;
    public string PasswordPrivateKey { get; set; } = String.Empty;
}

public class PasswordSalt
{
    public string Value { get; set; } = String.Empty;
    public int StartIndex { get; set; }
}