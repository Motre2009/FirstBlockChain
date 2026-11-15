using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using FirstBlockChain.Models;

namespace FirstBlockChain.Services;

public class HashingService
{

    public string ComputeHash(string input)
    {
        using (var sha256 = SHA256.Create())
        {
            byte[] inputBytes = Encoding.UTF8.GetBytes(input);
            byte[] hashBytes = sha256.ComputeHash(inputBytes);
            return BitConverter.ToString(hashBytes).Replace("-", "").ToLowerInvariant();
        }
    }

    public string ComputeHashBlock(Block block) 
    {
        var transactionData = string.Concat(block.Transactions.Select(t => t.ToRawString()));
        string blockData = $"{block.Index}{block.Timestamp}{transactionData}{block.PreviousHash}{block.Nonce}";
        return ComputeHash(blockData);
    }
}
