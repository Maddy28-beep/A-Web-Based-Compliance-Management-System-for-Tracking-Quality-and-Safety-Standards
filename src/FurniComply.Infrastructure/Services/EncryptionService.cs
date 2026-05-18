using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Security.Cryptography;
using System.Text;

namespace FurniComply.Infrastructure.Services;

public class EncryptionService : IEncryptionService
{
    private const string EncryptedPrefix = "ENCv2:";
    private const int KeySizeBytes = 32;
    private const int NonceSizeBytes = 12;
    private const int TagSizeBytes = 16;

    private readonly IConfiguration _config;
    private readonly ILogger<EncryptionService> _logger;
    private readonly string _keyFilePath;
    private readonly bool _isDevelopment;
    private readonly object _lockObject = new();

    private byte[] _key = Array.Empty<byte>();

    public EncryptionService(IConfiguration config, ILogger<EncryptionService> logger, IWebHostEnvironment env)
    {
        _config = config;
        _logger = logger;
        _isDevelopment = string.Equals(env.EnvironmentName, Environments.Development, StringComparison.OrdinalIgnoreCase);

        var keysDirectory = Path.Combine(env.ContentRootPath, "Keys");
        Directory.CreateDirectory(keysDirectory);
        _keyFilePath = Path.Combine(keysDirectory, "encryption.key.bin");

        InitializeEncryption();
    }

    public string Encrypt(string plainText)
    {
        if (string.IsNullOrEmpty(plainText))
        {
            return plainText;
        }

        if (IsEncrypted(plainText))
        {
            return plainText;
        }

        try
        {
            var plaintextBytes = Encoding.UTF8.GetBytes(plainText);
            var nonce = RandomNumberGenerator.GetBytes(NonceSizeBytes);
            var cipherBytes = new byte[plaintextBytes.Length];
            var tag = new byte[TagSizeBytes];

            using var aesGcm = new AesGcm(_key, TagSizeBytes);
            aesGcm.Encrypt(nonce, plaintextBytes, cipherBytes, tag);

            var payload = new byte[nonce.Length + tag.Length + cipherBytes.Length];
            Buffer.BlockCopy(nonce, 0, payload, 0, nonce.Length);
            Buffer.BlockCopy(tag, 0, payload, nonce.Length, tag.Length);
            Buffer.BlockCopy(cipherBytes, 0, payload, nonce.Length + tag.Length, cipherBytes.Length);

            return EncryptedPrefix + Convert.ToBase64String(payload);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to encrypt data");
            throw new InvalidOperationException("Encryption failed", ex);
        }
    }

    public string Decrypt(string cipherText)
    {
        if (string.IsNullOrEmpty(cipherText))
        {
            return cipherText;
        }

        if (!IsEncrypted(cipherText))
        {
            return cipherText;
        }

        try
        {
            var payload = Convert.FromBase64String(cipherText[EncryptedPrefix.Length..]);
            if (payload.Length < NonceSizeBytes + TagSizeBytes)
            {
                throw new InvalidOperationException("Encrypted payload is invalid.");
            }

            var nonce = payload[..NonceSizeBytes];
            var tag = payload[NonceSizeBytes..(NonceSizeBytes + TagSizeBytes)];
            var cipherBytes = payload[(NonceSizeBytes + TagSizeBytes)..];
            var plaintextBytes = new byte[cipherBytes.Length];

            using var aesGcm = new AesGcm(_key, TagSizeBytes);
            aesGcm.Decrypt(nonce, cipherBytes, tag, plaintextBytes);

            return Encoding.UTF8.GetString(plaintextBytes);
        }
        catch (Exception ex)
        {
            if (_isDevelopment)
            {
                _logger.LogWarning(ex,
                    "Failed to decrypt data in Development. Returning the stored value so localhost can keep running.");
                return cipherText;
            }

            _logger.LogError(ex, "Failed to decrypt data");
            throw new InvalidOperationException("Decryption failed", ex);
        }
    }

    public string EncryptSensitiveData(string data) => Encrypt(data);

    public string DecryptSensitiveData(string encryptedData) => Decrypt(encryptedData);

    public bool IsEncrypted(string value) =>
        !string.IsNullOrWhiteSpace(value) && value.StartsWith(EncryptedPrefix, StringComparison.Ordinal);

    public void GenerateNewKey()
    {
        lock (_lockObject)
        {
            _key = RandomNumberGenerator.GetBytes(KeySizeBytes);
            PersistKey(_key);
            _logger.LogInformation("A new application encryption key was generated.");
        }
    }

    public byte[] GetCurrentKey() => _key.ToArray();

    public byte[] GetCurrentIV() => Array.Empty<byte>();

    private void InitializeEncryption()
    {
        lock (_lockObject)
        {
            try
            {
                var configuredKey = _config["Encryption:MasterKey"] ?? _config["APP_ENCRYPTION_KEY"];
                if (!string.IsNullOrWhiteSpace(configuredKey))
                {
                    _key = ParseConfiguredKey(configuredKey);
                    _logger.LogInformation("Using encryption key from configuration.");
                    return;
                }

                if (File.Exists(_keyFilePath))
                {
                    _key = LoadPersistedKey();
                    if (_key.Length == KeySizeBytes)
                    {
                        return;
                    }

                    _logger.LogWarning("Stored encryption key had an invalid length. Regenerating.");
                }

                GenerateNewKey();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to initialize encryption service. Regenerating key material.");
                GenerateNewKey();
            }
        }
    }

    private byte[] ParseConfiguredKey(string configuredKey)
    {
        try
        {
            var keyBytes = Convert.FromBase64String(configuredKey.Trim());
            if (keyBytes.Length != KeySizeBytes)
            {
                throw new InvalidOperationException($"Configured encryption key must be {KeySizeBytes} bytes after Base64 decoding.");
            }

            return keyBytes;
        }
        catch (FormatException ex)
        {
            throw new InvalidOperationException("Configured encryption key must be valid Base64.", ex);
        }
    }

    private void PersistKey(byte[] key)
    {
        var dataToPersist = ProtectForCurrentMachine(key);
        File.WriteAllBytes(_keyFilePath, dataToPersist);

        try
        {
            var fileInfo = new FileInfo(_keyFilePath);
            fileInfo.Attributes |= FileAttributes.Hidden | FileAttributes.System;
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Could not set secure file attributes on the encryption key file.");
        }
    }

    private byte[] LoadPersistedKey()
    {
        var persistedBytes = File.ReadAllBytes(_keyFilePath);
        return UnprotectForCurrentMachine(persistedBytes);
    }

    private byte[] ProtectForCurrentMachine(byte[] key)
    {
        if (OperatingSystem.IsWindows())
        {
            return ProtectedData.Protect(key, null, DataProtectionScope.LocalMachine);
        }

        _logger.LogWarning("DPAPI is unavailable on this platform. Persisting the encryption key without OS-level protection.");
        return key;
    }

    private byte[] UnprotectForCurrentMachine(byte[] persistedBytes)
    {
        if (OperatingSystem.IsWindows())
        {
            return ProtectedData.Unprotect(persistedBytes, null, DataProtectionScope.LocalMachine);
        }

        return persistedBytes;
    }
}
