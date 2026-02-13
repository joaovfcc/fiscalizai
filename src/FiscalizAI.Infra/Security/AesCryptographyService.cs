using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace FiscalizAI.Infra.Security;

public class AesCryptographyService{
    
    private readonly byte[] _key;

    public AesCryptographyService(IConfiguration configuration)
    {
        // A chave deve vir do appsettings.json ou Variável de Ambiente
        // Deve ter 32 caracteres para AES-256
        var keyString = configuration["Security:MasterKey"];
        
        if (string.IsNullOrEmpty(keyString) || keyString.Length < 32)
            throw new ArgumentException("A chave de criptografia deve ter pelo menos 32 caracteres.");

        // Ajusta para exatamente 32 bytes
        _key = Encoding.UTF8.GetBytes(keyString.Substring(0, 32));
    }

    public string Encrypt(string plainText)
    {
        if (string.IsNullOrEmpty(plainText)) return plainText;

        using var aes = Aes.Create();
        aes.Key = _key;
        aes.GenerateIV(); // Gera IV único e aleatório

        using var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
        using var ms = new MemoryStream();
        
        // Escreve o IV no início do stream (precisaremos dele para descriptografar)
        ms.Write(aes.IV, 0, aes.IV.Length);

        using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
        using (var sw = new StreamWriter(cs))
        {
            sw.Write(plainText);
        }

        return Convert.ToBase64String(ms.ToArray());
    }

    public string Decrypt(string cipherText)
    {
        if (string.IsNullOrEmpty(cipherText)) return cipherText;

        var fullCipher = Convert.FromBase64String(cipherText);

        using var aes = Aes.Create();
        aes.Key = _key;

        // Extrai o IV (primeiros 16 bytes)
        var iv = new byte[16];
        Array.Copy(fullCipher, 0, iv, 0, iv.Length);
        aes.IV = iv;

        // O resto é o texto cifrado
        using var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
        using var ms = new MemoryStream(fullCipher, 16, fullCipher.Length - 16);
        using var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);
        using var sr = new StreamReader(cs);

        return sr.ReadToEnd();
    }
}