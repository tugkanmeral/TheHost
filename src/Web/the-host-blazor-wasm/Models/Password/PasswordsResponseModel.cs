namespace Models
{
    public class PasswordsResponseModel
    {
        public Password[]? Items { get; set; }
        public long TotalItemCount { get; set; }
        public int Skip { get; set; }
        public int Take { get; set; }
    }
}