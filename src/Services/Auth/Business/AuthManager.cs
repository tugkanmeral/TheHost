// using System.IdentityModel.Tokens;
// using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

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
    public string? GetToken(string username, string password, string? masterKey = null)
    {
        var user = _userRepository.GetByUsername(username);

        if (user == null)
            return null;

        if (!user.Password.Equals(password))
            return null;

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_appSettings.AuthSecretKey);

        var claims = new List<Claim>();
        claims.Add(new Claim(ClaimTypes.Name, user.Username));
        if (masterKey != null)
            claims.Add(new Claim(CustomClaimTypes.MASTER_KEY, masterKey));
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
}