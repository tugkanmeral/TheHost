using System.Security.Cryptography;

public class UserManager : IUserService
{
    IUserRepository _userRepository;
    AppSettings _appSettings;
    public UserManager(IUserRepository userRepository, AppSettings appSettings)
    {
        _userRepository = userRepository;
        _appSettings = appSettings;
    }

    public void Delete(string userId)
    {
        _userRepository.Delete(userId);
    }

    // public User GetUser(string userId)
    // {
    //     return _userRepository.GetById(userId);
    // }

    // public IEnumerable<User> GetUsers()
    // {
    //     return _userRepository.Get();
    // }

    public void InsertUser(User user)
    {
        var existUser = _userRepository.GetByUsername(user.Username);
        if (existUser != null){
            throw new System.Exception("Username is reserved!");
        }

        StringBuilder rawPassStrBuilder = new();
        rawPassStrBuilder.Append(user.Username);
        rawPassStrBuilder.Append(user.Password);
        var rawPass = rawPassStrBuilder.ToString();

        using SHA256 sha256Hash = SHA256.Create();
        var passwordHash = getHash(sha256Hash, rawPass);

        user.Password = passwordHash;
        _userRepository.Insert(user);
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

    // Verify a hash against a string.
    private static bool verifyHash(HashAlgorithm hashAlgorithm, string input, string hash)
    {
        // Hash the input.
        var hashOfInput = getHash(hashAlgorithm, input);

        // Create a StringComparer an compare the hashes.
        StringComparer comparer = StringComparer.OrdinalIgnoreCase;

        return comparer.Compare(hashOfInput, hash) == 0;
    }
}