using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstBlockChain.Services;

public class DisplayService
{
    public void DisplayBlockChain(BlockChain blockChain)
    {
        foreach (var block in blockChain.Chain)
        {
            Console.WriteLine($"Index: {block.Index}");
            Console.WriteLine($"Timestamp: {block.Timestamp}");
            Console.WriteLine($"Hash: {block.Hash}");
            Console.WriteLine($"Previous Hash: {block.PreviousHash}");
            Console.WriteLine($"Nonce: {block.Nonce}");
            foreach (var transaction in block.Transactions)
            {
                Console.WriteLine($"{transaction.ToRawString()}");
            }
            Console.WriteLine(new string('-', 50));
        }
    }
}
