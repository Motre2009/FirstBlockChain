using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using FirstBlockChain.Models;


namespace FirstBlockChain.Services;

public class WalletService
{
    public Wallet CreateWallet(string name)
    {
        using var ecdsa = ECDsa.Create(ECCurve.NamedCurves.nistP256);
        var privateKey = ecdsa.ExportECPrivateKey();
        var publicKey = ecdsa.ExportSubjectPublicKeyInfo();
        var address = Convert.ToBase64String(publicKey)[^10..];
        return new Wallet(name, address, publicKey, privateKey);
    }

    public bool VerifySignature(byte[] publicKey, byte[] data, byte[] signature)
    {
        using var ecdsa = ECDsa.Create();
        ecdsa.ImportSubjectPublicKeyInfo(publicKey, out _);
        return ecdsa.VerifyData(data, signature, HashAlgorithmName.SHA256);
    }
}
