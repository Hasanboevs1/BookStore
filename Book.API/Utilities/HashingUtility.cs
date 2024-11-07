using System.Security.Cryptography;
using System.Text;

namespace Book.API.Utilities;

public static class HashingUtility
{
    private const int SaltSize = 16; // Salt length in bytes
    private const int HashSize = 32; // Hash length in bytes

    // Generates a hashed password with salt
    public static string HashPassword(string password)
    {
        // Generate a salt
        using var rng = new RNGCryptoServiceProvider();
        byte[] saltBytes = new byte[SaltSize];
        rng.GetBytes(saltBytes);

        // Hash the password with the salt
        using var sha256 = SHA256.Create();
        byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
        byte[] passwordWithSaltBytes = new byte[saltBytes.Length + passwordBytes.Length];
        Buffer.BlockCopy(saltBytes, 0, passwordWithSaltBytes, 0, saltBytes.Length);
        Buffer.BlockCopy(passwordBytes, 0, passwordWithSaltBytes, saltBytes.Length, passwordBytes.Length);

        byte[] hashBytes = sha256.ComputeHash(passwordWithSaltBytes);

        // Combine salt and hash
        byte[] hashWithSaltBytes = new byte[SaltSize + HashSize];
        Buffer.BlockCopy(saltBytes, 0, hashWithSaltBytes, 0, SaltSize);
        Buffer.BlockCopy(hashBytes, 0, hashWithSaltBytes, SaltSize, HashSize);

        // Convert to base64 for storage
        return Convert.ToBase64String(hashWithSaltBytes);
    }

    // Verifies a password against a stored hash
    public static bool VerifyPassword(string enteredPassword, string storedHash)
    {
        // Convert stored hash from base64
        byte[] hashWithSaltBytes = Convert.FromBase64String(storedHash);

        // Extract salt from stored hash
        byte[] saltBytes = new byte[SaltSize];
        Buffer.BlockCopy(hashWithSaltBytes, 0, saltBytes, 0, SaltSize);

        // Hash the entered password with the extracted salt
        using var sha256 = SHA256.Create();
        byte[] passwordBytes = Encoding.UTF8.GetBytes(enteredPassword);
        byte[] passwordWithSaltBytes = new byte[saltBytes.Length + passwordBytes.Length];
        Buffer.BlockCopy(saltBytes, 0, passwordWithSaltBytes, 0, saltBytes.Length);
        Buffer.BlockCopy(passwordBytes, 0, passwordWithSaltBytes, saltBytes.Length, passwordBytes.Length);

        byte[] hashBytes = sha256.ComputeHash(passwordWithSaltBytes);

        // Compare hash in stored hash to calculated hash
        for (int i = 0; i < HashSize; i++)
        {
            if (hashWithSaltBytes[i + SaltSize] != hashBytes[i])
                return false;
        }

        return true;
    }
}
