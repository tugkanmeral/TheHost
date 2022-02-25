using System.Security.Claims;
using System.Text;
public static class Utils
{
    public static string GenerateRandomPassword(int len = 8)
    {
        StringBuilder strBuilder = new();
        Random random = new();

        char ch;
        int ascii;

        for (int i = 0; i < len; i++)
        {
            ascii = random.Next(21, 127);
            ch = (char)ascii;
            strBuilder.Append(ch);
        }

        return strBuilder.ToString();
    }

    public static string GetUserMasterKey(this ClaimsPrincipal user)
    {
        var masterKey = string.Empty;
        masterKey = user.Claims.FirstOrDefault(c => c.Type == CustomClaimTypes.MASTER_KEY)?.Value;
        return masterKey ?? string.Empty;
    }

    public static string GetUserId(this ClaimsPrincipal user)
    {
        return user.Claims.Where(x => x.Type == ClaimTypes.NameIdentifier).Single().Value ?? String.Empty;
    }
}