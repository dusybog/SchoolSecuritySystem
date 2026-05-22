namespace SchoolSecuritySystem.Core.Interfaces.Services
{
    public interface IEncryptionService
    {
        string Encrypt(string plainText, byte[]? customKey = null);
        string Decrypt(string encryptedText, byte[]? customKey = null);
    }
}
