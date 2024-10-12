namespace HSX.Core.Interfaces;

public interface IHashingService
{
    string Hash(string input);
    bool Verify(string input, string hashedValue);
}
