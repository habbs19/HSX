namespace aznV.Infrastructure.Security;

using HSX.Core.Interfaces;
using System;
using System.Security.Cryptography;

public class HashingService : RandomNumberGenerator, IHashingService
{
    private const int SaltSize = 16;  // 128 bit
    private const int KeySize = 32;   // 256 bit
    private const int Iterations = 10000;  // Number of PBKDF2 iterations
    private readonly HashAlgorithmName _hashAlgorithm = HashAlgorithmName.SHA256;

    public override void GetBytes(byte[] data)
    {
        var random = Create();
        random.GetBytes(data);
    }

    public string Hash(string password)
    {
        // Generate a salt
        byte[] salt = new byte[SaltSize];
        GetBytes(salt);

        // Hash the password with the salt
        byte[] hash = Rfc2898DeriveBytes.Pbkdf2(password, salt, Iterations, _hashAlgorithm, KeySize);

        // Combine salt and hash for storage
        byte[] hashBytes = new byte[SaltSize + KeySize];
        Array.Copy(salt, 0, hashBytes, 0, SaltSize);
        Array.Copy(hash, 0, hashBytes, SaltSize, KeySize);

        // Return the combined salt+hash as a Base64 string
        return Convert.ToBase64String(hashBytes);
    }

    public bool Verify(string password, string hashedPassword)
    {
        // Convert the stored Base64 string back to byte array
        byte[] hashBytes = Convert.FromBase64String(hashedPassword);

        // Extract the salt from the hashBytes (first 16 bytes)
        byte[] salt = new byte[SaltSize];
        Array.Copy(hashBytes, 0, salt, 0, SaltSize);

        // Hash the input password with the extracted salt
        byte[] hash = Rfc2898DeriveBytes.Pbkdf2(password, salt, Iterations, _hashAlgorithm, KeySize);

        // Compare the result with the original hash
        for (int i = 0; i < KeySize; i++)
        {
            if (hashBytes[i + SaltSize] != hash[i])
            {
                return false;
            }
        }

        return true;
    }
}
