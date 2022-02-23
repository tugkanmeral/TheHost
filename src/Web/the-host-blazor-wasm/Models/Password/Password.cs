namespace Models
{
    public class Password
    {
        public string Id { get; set; } = String.Empty;
        public string Title { get; set; } = String.Empty;
        public string Detail { get; set; } = String.Empty;
        public string Username { get; set; } = String.Empty;
        public string Pass { get; set; } = String.Empty;
        public string CreationDate { get; set; } = String.Empty;
        public string? LastUpdateDate { get; set; }
    }
}