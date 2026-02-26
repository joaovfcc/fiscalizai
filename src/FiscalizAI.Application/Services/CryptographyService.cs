using System.Security.Cryptography;
using System.Text;
using FiscalizAI.Application.Interfaces;
using Microsoft.Extensions.Configuration;

namespace FiscalizAI.Application.Services;

/// <summary>
/// Serviço responsável pela criptografia e descriptografia de dados sensíveis (como senhas de certificados digitais).
/// Utiliza o algoritmo AES-GCM (Advanced Encryption Standard - Galois/Counter Mode) para garantir confidencialidade e integridade.
/// </summary>
public class CryptographyService : ICryptographyService
{
    private readonly byte[] _chaveSecreta;

    /// <summary>
    /// Inicializa uma nova instância do serviço de criptografia.
    /// </summary>
    /// <param name="configuration">Interface de configuração para acessar a chave secreta no appsettings.json.</param>
    /// <exception cref="InvalidOperationException">Lançada quando a chave não está configurada ou não possui o tamanho exato de 32 bytes (256 bits).</exception>
    public CryptographyService(IConfiguration configuration)
    {
        var chaveBase64 = configuration["Cryptography:Key"] 
            ?? throw new InvalidOperationException("Chave de criptografia não configurada.");

        _chaveSecreta = Convert.FromBase64String(chaveBase64);

        if (_chaveSecreta.Length != 32)
            throw new InvalidOperationException("A chave deve ter exatamente 32 bytes (AES-256).");
    }

    /// <summary>
    /// Criptografa um texto puro utilizando AES-GCM.
    /// </summary>
    /// <param name="textoPuro">O texto legível que será protegido (ex: a senha em texto plano).</param>
    /// <returns>Uma string em Base64 contendo o número único (nonce), o lacre de segurança (tag) e os dados criptografados.</returns>
    public string Encrypt(string textoPuro)
    {
        if (string.IsNullOrEmpty(textoPuro)) return textoPuro;

        var bytesTextoPuro = Encoding.UTF8.GetBytes(textoPuro);
        
        var numeroUnico = new byte[AesGcm.NonceByteSizes.MaxSize]; 
        var lacreSeguranca = new byte[AesGcm.TagByteSizes.MaxSize]; 
        var bytesCriptografados = new byte[bytesTextoPuro.Length];

        RandomNumberGenerator.Fill(numeroUnico);

        using var aes = new AesGcm(_chaveSecreta, lacreSeguranca.Length);
        aes.Encrypt(numeroUnico, bytesTextoPuro, bytesCriptografados, lacreSeguranca);

        var pacoteCompleto = new byte[numeroUnico.Length + lacreSeguranca.Length + bytesCriptografados.Length];
        
        Buffer.BlockCopy(numeroUnico, 0, pacoteCompleto, 0, numeroUnico.Length);
        Buffer.BlockCopy(lacreSeguranca, 0, pacoteCompleto, numeroUnico.Length, lacreSeguranca.Length);
        Buffer.BlockCopy(bytesCriptografados, 0, pacoteCompleto, numeroUnico.Length + lacreSeguranca.Length, bytesCriptografados.Length);

        return Convert.ToBase64String(pacoteCompleto);
    }

    /// <summary>
    /// Descriptografa um pacote de dados previamente criptografado com AES-GCM.
    /// </summary>
    /// <param name="textoCriptografado">A string em Base64 gerada pelo método Encrypt.</param>
    /// <returns>O texto original legível.</returns>
    /// <exception cref="CryptographicException">Lançada automaticamente se o texto criptografado foi adulterado no banco de dados (lacre rompido).</exception>
    public string Decrypt(string textoCriptografado)
    {
        if (string.IsNullOrEmpty(textoCriptografado)) return textoCriptografado;

        var pacoteCompleto = Convert.FromBase64String(textoCriptografado);
        
        var numeroUnico = pacoteCompleto[..12];
        var lacreSeguranca = pacoteCompleto[12..28];
        var bytesCriptografados = pacoteCompleto[28..];
        
        var bytesDescriptografados = new byte[bytesCriptografados.Length];

        using var aes = new AesGcm(_chaveSecreta, lacreSeguranca.Length);
        aes.Decrypt(numeroUnico, bytesCriptografados, lacreSeguranca, bytesDescriptografados); 

        return Encoding.UTF8.GetString(bytesDescriptografados);
    }
}