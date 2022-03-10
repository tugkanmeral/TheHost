// using System.IdentityModel.Tokens;
// using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Security.Cryptography;

public class AuthManager : IAuthService
{
    IUserRepository _userRepository;
    AppSettings _appSettings;

    public AuthManager(IUserRepository userRepository, AppSettings appSettings)
    {
        _userRepository = userRepository;
        _appSettings = appSettings;
    }

    //TODO: password will be stored as combined hash of username and password
    public string? GetToken(string username, string password)
    {
        var user = _userRepository.GetByUsername(username);

        if (user == null)
            return null;

        StringBuilder rawPassStrBuilder = new();
        rawPassStrBuilder.Append(username);
        rawPassStrBuilder.Append(password);
        var rawPass = rawPassStrBuilder.ToString();

        using SHA256 sha256Hash = SHA256.Create();
        var passwordHash = getHash(sha256Hash, rawPass);

        if (!user.Password.Equals(passwordHash))
            return null;

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_appSettings.AuthSecretKey);

        var claims = new List<Claim>();
        claims.Add(new Claim(ClaimTypes.Name, user.Username));
        if (user.Id != null)
            claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        var tokenStr = tokenHandler.WriteToken(token);

        return tokenStr;
    }

    private static string getHash(HashAlgorithm hashAlgorithm, string input)
    {
        // Convert the input string to a byte array and compute the hash.
        byte[] data = hashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(input));

        // Create a new Stringbuilder to collect the bytes
        // and create a string.
        var sBuilder = new StringBuilder();

        // Loop through each byte of the hashed data
        // and format each one as a hexadecimal string.
        for (int i = 0; i < data.Length; i++)
        {
            sBuilder.Append(data[i].ToString("x2"));
        }

        // Return the hexadecimal string.
        return sBuilder.ToString();
    }
}