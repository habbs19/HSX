namespace HSX.Infrastructure.Security;

using HSX.Core.Interfaces;
using System;

public class EnvironmentKeyManagementService : IKeyManagementService
{
    public byte[] GetEncryptionKey()
    {
        // Retrieve the encryption key from environment variables or configuration
        string encryptionKey = Environment.GetEnvironmentVariable("ENCRYPTION_KEY") ?? throw new ArgumentNullException("ENCRYPTION_KEY") ;

        if (string.IsNullOrEmpty(encryptionKey) || encryptionKey.Length != 32)
        {
            throw new InvalidOperationException("Encryption key is missing or invalid. It must be 32 characters long.");
        }

        return Convert.FromBase64String(encryptionKey); // Assuming key is stored as Base64
    }

    public byte[] GetInitializationVector()
    {
        // Retrieve the initialization vector (IV) from environment variables or configuration
        string iv = Environment.GetEnvironmentVariable("ENCRYPTION_IV");

        if (string.IsNullOrEmpty(iv) || iv.Length != 16)
        {
            throw new InvalidOperationException("Initialization vector (IV) is missing or invalid. It must be 16 characters long.");
        }

        return Convert.FromBase64String(iv); // Assuming IV is stored as Base64
    }
}
