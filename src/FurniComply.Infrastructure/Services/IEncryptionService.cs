namespace FurniComply.Infrastructure.Services;

public interface IEncryptionService
{
    string Encrypt(string plainText);
    string Decrypt(string cipherText);
    string EncryptSensitiveData(string data);
    string DecryptSensitiveData(string encryptedData);
    bool IsEncrypted(string value);
    void GenerateNewKey();
    byte[] GetCurrentKey();
    byte[] GetCurrentIV();
}
