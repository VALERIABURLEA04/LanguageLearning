using System.Security.Cryptography;

namespace sa.Services;

public static class PasswordHasher
{
    private const int SaltSize = 16;
    private const int KeySize  = 32;
    private const int Iter     = 100_000;

    public static string Hash(string password)
    {
        var salt = RandomNumberGenerator.GetBytes(SaltSize);
        var key  = Rfc2898DeriveBytes.Pbkdf2(password, salt, Iter, HashAlgorithmName.SHA256, KeySize);
        return $"{Convert.ToBase64String(salt)}.{Convert.ToBase64String(key)}";
    }

    public static bool Verify(string password, string stored)
    {
        var parts = stored.Split('.');
        if (parts.Length != 2) return false;
        var salt = Convert.FromBase64String(parts[0]);
        var key  = Convert.FromBase64String(parts[1]);
        var test = Rfc2898DeriveBytes.Pbkdf2(password, salt, Iter, HashAlgorithmName.SHA256, KeySize);
        return CryptographicOperations.FixedTimeEquals(test, key);
    }
}
