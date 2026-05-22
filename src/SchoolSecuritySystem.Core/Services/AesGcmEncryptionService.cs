using SchoolSecuritySystem.Core.Interfaces.Services;
using System.Security.Cryptography;
using System.Text;

namespace SchoolSecuritySystem.Core.Services
{
    public class AesGcmEncryptionService : IEncryptionService
    {
        private readonly byte[] _key; // 這個就是您的 KEK
        private const int NonceSize = 12;
        private const int TagSize = 16;

        public AesGcmEncryptionService(string base64Key)
        {
            _key = Convert.FromBase64String(base64Key);
            if (_key.Length != 32)
                throw new ArgumentException("預設 KEK 必須精準為 32 Bytes。");
        }

        public string Encrypt(string plainText, byte[]? customKey = null)
        {
            if (string.IsNullOrEmpty(plainText))
                throw new ArgumentNullException(nameof(plainText));

            byte[] activeKey = customKey ?? _key;
            if (activeKey.Length != 32)
                throw new ArgumentException("傳入的金鑰長度必須為 32 Bytes。");

            byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);
            byte[] nonce = new byte[NonceSize];
            RandomNumberGenerator.Fill(nonce);

            byte[] cipherBytes = new byte[plainBytes.Length];
            byte[] tag = new byte[TagSize];

            using (var aesGcm = new AesGcm(activeKey, TagSize))
            {
                aesGcm.Encrypt(nonce, plainBytes, cipherBytes, tag);
            }

            byte[] encryptedPayload = new byte[NonceSize + TagSize + cipherBytes.Length];
            Buffer.BlockCopy(nonce, 0, encryptedPayload, 0, NonceSize);
            Buffer.BlockCopy(tag, 0, encryptedPayload, NonceSize, TagSize);
            Buffer.BlockCopy(cipherBytes, 0, encryptedPayload, NonceSize + TagSize, cipherBytes.Length);

            return Convert.ToBase64String(encryptedPayload);
        }

        public string Decrypt(string encryptedText, byte[]? customKey = null)
        {
            if (string.IsNullOrEmpty(encryptedText))
                throw new ArgumentNullException(nameof(encryptedText));

            byte[] activeKey = customKey ?? _key;
            if (activeKey.Length != 32)
                throw new ArgumentException("傳入的金鑰長度必須為 32 Bytes。");

            byte[] encryptedPayload = Convert.FromBase64String(encryptedText);

            if (encryptedPayload.Length < NonceSize + TagSize)
                throw new CryptographicException("無效的加密字串。");

            byte[] nonce = new byte[NonceSize];
            byte[] tag = new byte[TagSize];
            byte[] cipherBytes = new byte[encryptedPayload.Length - NonceSize - TagSize];

            Buffer.BlockCopy(encryptedPayload, 0, nonce, 0, NonceSize);
            Buffer.BlockCopy(encryptedPayload, NonceSize, tag, 0, TagSize);
            Buffer.BlockCopy(encryptedPayload, NonceSize + TagSize, cipherBytes, 0, cipherBytes.Length);

            byte[] plainBytes = new byte[cipherBytes.Length];

            using (var aesGcm = new AesGcm(activeKey, TagSize))
            {
                aesGcm.Decrypt(nonce, cipherBytes, tag, plainBytes);
            }

            return Encoding.UTF8.GetString(plainBytes);
        }
    }
}