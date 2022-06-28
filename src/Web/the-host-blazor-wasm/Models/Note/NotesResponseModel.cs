namespace Models
{
    public class NotesResponseModel
    {
        public Note[]? Items { get; set; }
        public long TotalItemCount { get; set; }
        public int Skip { get; set; }
        public int Take { get; set; }
    }
}