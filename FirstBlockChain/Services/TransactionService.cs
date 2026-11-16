using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FirstBlockChain.Models;

namespace FirstBlockChain.Services;

public class TransactionService
{
    private readonly WalletService _walletService = new();
    public Transaction CreateTransaction(Wallet sender, string to, decimal amount) 
    {
        var tx = new Transaction(sender.Address, to, amount) 
        {
            SenderPublicKey = sender.PublicKey
        };

        tx.Signature = sender.Sign(tx.GetDataToSign());
        var validationResult = ValidateTransaction(tx);

        if (!validationResult.isValid)
        {
            throw new ArgumentException(validationResult.errorMessage);
        }

        return tx;
    }

    public (bool isValid, string errorMessage) ValidateTransaction(Transaction tx)
    {
        if (string.IsNullOrEmpty(tx.From)) 
        {
            return (false, "Invalid sender address");
        }
        if (string.IsNullOrEmpty(tx.To))
        {
            return (false, "Invalid recipient address");
        }
        if (tx.Amount <= 0) 
        {
            return (false, "Invalid transaction amount");
        }
        if (tx.SenderPublicKey == null || tx.SenderPublicKey.Length == 0)
            return (false, "Missing sender public key");

        if (tx.Signature == null || tx.Signature.Length == 0)
            return (false, "Missing signature");

        if (!_walletService.VerifySignature(tx.SenderPublicKey, tx.GetDataToSign(), tx.Signature))
            return (false, "Invalid signature");

        return (true, string.Empty);
    }
}
