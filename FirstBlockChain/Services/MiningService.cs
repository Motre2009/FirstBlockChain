using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using FirstBlockChain.Models;

namespace FirstBlockChain.Services;

public class MiningService
{
    private readonly HashingService _hashingService;

    public MiningService()
    {
        _hashingService = new HashingService();
    }

    public void MineBlock(Block block, int difficulty)
    {
        string target = new string('0', difficulty);
        var stopwatch = Stopwatch.StartNew();
        while (true)
        {
            block.Hash = _hashingService.ComputeHashBlock(block);
            if (block.Hash.Substring(0, difficulty) == target)
            {
                stopwatch.Stop();
                block.MiningDuration = stopwatch.Elapsed.TotalSeconds;
                Console.WriteLine($"Hash found for block {block.Index}: {block.Hash}: {block.MiningDuration} seconds");
                break;
            }
            if (block.Nonce % 100000 == 0) 
            { 
                Console.WriteLine($"Mining block {block.Index}: Nonce={block.Nonce}, Hash={block.Hash}"); 
            }
            block.Nonce++;
        }
    }
}
