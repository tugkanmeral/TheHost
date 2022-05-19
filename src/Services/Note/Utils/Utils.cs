using System.Security.Claims;
using System.Text;
public static class Utils
{
    public static string GetUserId(this ClaimsPrincipal user)
    {
        return user.Claims.Where(x => x.Type == ClaimTypes.NameIdentifier).Single().Value ?? String.Empty;
    }
}