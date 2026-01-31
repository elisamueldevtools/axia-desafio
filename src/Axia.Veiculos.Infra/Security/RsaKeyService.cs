using System.Security.Cryptography;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Axia.Veiculos.Infra.Security;

public class RsaKeyService
{
    private readonly RsaSecurityKey _privateKey;
    private readonly RsaSecurityKey _publicKey;

    public RsaKeyService(IConfiguration configuration)
    {
        var privateCert = configuration["Jwt:PrivateCert"]!;
        var publicCert = configuration["Jwt:PublicCert"]!;

        _privateKey = LoadPrivateKey(privateCert);
        _publicKey = LoadPublicKey(publicCert);
    }

    public RsaSecurityKey PrivateKey => _privateKey;
    public RsaSecurityKey PublicKey => _publicKey;

    public SigningCredentials GetSigningCredentials()
    {
        return new SigningCredentials(_privateKey, SecurityAlgorithms.RsaSha256);
    }

    private static RsaSecurityKey LoadPrivateKey(string base64Key)
    {
        var rsa = RSA.Create();
        rsa.ImportRSAPrivateKey(Convert.FromBase64String(base64Key), out _);
        return new RsaSecurityKey(rsa);
    }

    private static RsaSecurityKey LoadPublicKey(string base64Key)
    {
        var rsa = RSA.Create();
        rsa.ImportSubjectPublicKeyInfo(Convert.FromBase64String(base64Key), out _);
        return new RsaSecurityKey(rsa);
    }
}
