namespace HSX.Core.Interfaces;
public interface IKeyManagementService
{
    byte[] GetEncryptionKey();       // Method to retrieve the encryption key
    byte[] GetInitializationVector(); // Method to retrieve the IV (Initialization Vector)
}
