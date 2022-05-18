using System.Security.Cryptography;
using System.Security.Claims;
using En = Password.Entities;
using System.Text;

public class PasswordManager : IPasswordService
{
    IPasswordRepository _passwordRepository;
    AppSettings _appSettings;
    public PasswordManager(IPasswordRepository passwordRepository, AppSettings appSettings)
    {
        _passwordRepository = passwordRepository;
        _appSettings = appSettings;
    }

    public void DeletePassword(string id, string userId)
    {
        _passwordRepository.Delete(userId, id);
    }

    public En.Password GetPassword(string id, string userId, string passwordPrivateKey)
    {
        if (String.IsNullOrWhiteSpace(passwordPrivateKey))
        {
            throw new Exception("Enter master key before all");
        }

        var password = _passwordRepository.Get(userId, id);

        try
        {
            password.Pass = Crypto.Decrypt(password.Pass, passwordPrivateKey, _appSettings.IV);

            if (password.Pass.Length > _appSettings.Salt.StartIndex + _appSettings.Salt.Value.Length)
            {
                password.Pass = DesaltPassword(password.Pass);
            }
        }
        catch (CryptographicException)
        {
            Random random = new();
            var fakePassword = Utils.GenerateRandomPassword(random.Next(8, 10));
            password.Pass = fakePassword;
        }
        catch (Exception)
        {
            throw new Exception("Error while password getting");
        }

        return password;
    }

    public List<En.Password> GetPasswords(string userId, int skip, int take, string? searchText)
    {
        List<En.Password> passwords = new();
        try
        {
            passwords = _passwordRepository.Get(userId, skip, take, searchText);
        }
        catch (Exception ex)
        {
            throw new Exception("Error while passwords getting", ex);
        }

        return passwords;
    }

    public void InsertPassword(En.Password password, string userId, string passwordPrivateKey)
    {
        if (password.Pass.Length > _appSettings.Salt.StartIndex)
        {
            password.Pass = SaltPassword(password.Pass);
        }

        password.Pass = Crypto.Encrypt(password.Pass, passwordPrivateKey, _appSettings.IV);
        password.CreationDate = DateTime.Now.ToString();
        password.OwnerId = userId;

        try
        {
            _passwordRepository.Insert(password);
        }
        catch (Exception)
        {
            throw new Exception("Error while password adding");
        }
    }

    public void UpdatePassword(En.Password password, string userId, string passwordPrivateKey)
    {
        var existPassword = _passwordRepository.Get(userId, password.Id);
        if (existPassword == null)
            throw new Exception("Password could not find!");

        var currentPass = existPassword.Pass;
        var saltedPass = Crypto.Decrypt(currentPass, passwordPrivateKey, _appSettings.IV);
        var pass = DesaltPassword(saltedPass);

        if (!String.IsNullOrWhiteSpace(password.Pass) & password.Pass != pass) // password changed
        {
            var saltedNewPass = SaltPassword(password.Pass);
            existPassword.Pass = Crypto.Encrypt(saltedNewPass, passwordPrivateKey, _appSettings.IV);
        }

        if (password.Detail != existPassword.Detail) // detail changed
        {
            existPassword.Detail = password.Detail;
        }

        existPassword.LastUpdateDate = DateTime.Now.ToString();

        try
        {
            _passwordRepository.Update(existPassword);
        }
        catch (Exception)
        {
            throw new Exception("Error while password updating");
        }
    }

    public string SaltPassword(string rawPassword)
    {
        StringBuilder strBuilder = new();
        var first = rawPassword.Substring(0, _appSettings.Salt.StartIndex);
        var last = rawPassword.Substring(_appSettings.Salt.StartIndex, rawPassword.Length - _appSettings.Salt.StartIndex);
        strBuilder.Append(first);
        strBuilder.Append(_appSettings.Salt.Value);
        strBuilder.Append(last);
        return strBuilder.ToString();
    }

    public string DesaltPassword(string saltedPassword)
    {
        StringBuilder strBuilder = new();
        var first = saltedPassword.Substring(0, _appSettings.Salt.StartIndex);
        var salt = saltedPassword.Substring(_appSettings.Salt.StartIndex, _appSettings.Salt.Value.Length);
        var last = saltedPassword.Substring(_appSettings.Salt.StartIndex + _appSettings.Salt.Value.Length, saltedPassword.Length - (_appSettings.Salt.StartIndex + _appSettings.Salt.Value.Length));
        strBuilder.Append(first);
        strBuilder.Append(last);
        return strBuilder.ToString();
    }

    public async Task<long> GetTotalPasswordsCount(string userId)
    {
        return await _passwordRepository.GetUserPasswordsCount(userId);
    }
}