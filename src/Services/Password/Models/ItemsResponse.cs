using Microsoft.AspNetCore.Mvc;

public class ItemsResponse
{
    public IEnumerable<object>? Items { get; set; }
    public long TotalItemCount { get; set; }
    public int Skip { get; set; }
    public int Take { get; set; }

}