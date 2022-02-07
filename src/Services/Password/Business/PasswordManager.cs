using System.Security.Cryptography;
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

    public void DeletePassword(string id)
    {
        _passwordRepository.Delete(id);
    }

    public En.Password GetPassword(string id, string masterKey)
    {
        if (String.IsNullOrWhiteSpace(masterKey))
        {
            throw new Exception("Enter master key before all");
        }

        var password = _passwordRepository.Get(id);

        try
        {
            password.Pass = Crypto.Decrypt(password.Pass, masterKey, _appSettings.IV);

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

    public List<En.Password> GetPasswords()
    {
        List<En.Password> passwords = new();
        try
        {
            passwords = _passwordRepository.Get();
        }
        catch (Exception)
        {
            throw new Exception("Error while passwords getting");
        }

        return passwords;
    }

    public void InsertPassword(En.Password password, string masterKey)
    {
        if (String.IsNullOrWhiteSpace(masterKey))
        {
            throw new Exception("Enter master key before all");
        }

        if (password.Pass.Length > _appSettings.Salt.StartIndex)
        {
            password.Pass = SaltPassword(password.Pass);
        }

        password.Pass = Crypto.Encrypt(password.Pass, masterKey, _appSettings.IV);
        password.CreationDate = DateTime.Now.ToString();

        try
        {
            _passwordRepository.Insert(password);
        }
        catch (Exception)
        {
            throw new Exception("Error while password adding");
        }
    }

    public void UpdatePassword(En.Password password, string masterKey)
    {
        if (String.IsNullOrWhiteSpace(masterKey))
        {
            throw new Exception("Enter master key before all");
        }

        var existPassword = _passwordRepository.Get(password.Id);
        if (existPassword == null)
            throw new Exception("Password could not find!");

        var currentPass = existPassword.Pass;
        var saltedPass = Crypto.Decrypt(currentPass, masterKey, _appSettings.IV);
        var pass = DesaltPassword(saltedPass);

        if (!String.IsNullOrWhiteSpace(password.Pass) & password.Pass != pass) // password changed
        {
            var saltedNewPass = SaltPassword(password.Pass);
            existPassword.Pass = Crypto.Encrypt(saltedNewPass, masterKey, _appSettings.IV);
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
}