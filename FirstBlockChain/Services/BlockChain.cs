using FirstBlockChain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstBlockChain.Services;

public class BlockChain
{
    public List<Block> Chain { get; set; }
    public readonly HashingService _hashingService;
    private readonly MiningService _miningService;
    private readonly TransactionService _transactionService;
    public int Difficulty { get; set; }
    public readonly double _targetBlockTime = 5;
    public readonly int _adjustmentInterval = 5;

    public BlockChain(int difficulty)
    {
        _miningService = new MiningService();
        _hashingService = new HashingService();
        Chain = new List<Block>();
        Difficulty = difficulty;
        _transactionService = new TransactionService();
        CreateGenesisBlock();
    }

    private void CreateGenesisBlock() 
    { 
        var genesisBlock = new Block
        {
            Index = 0,
            Transactions = new List<Transaction>(),
            PreviousHash = "0",
            Timestamp = DateTime.UtcNow
        };

        _miningService.MineBlock(genesisBlock, Difficulty);
        Chain.Add(genesisBlock);
    }

    public void AddBlock(List<Transaction> transactions) 
    { 
        var previousBlock = Chain[Chain.Count - 1];
        var newBlock = new Block
        {
            Index = previousBlock.Index + 1,
            Transactions = new List<Transaction>(transactions),
            PreviousHash = previousBlock.Hash,
            Timestamp = DateTime.UtcNow
        };
        _miningService.MineBlock(newBlock, Difficulty);
        Chain.Add(newBlock);

        if (newBlock.Index % _adjustmentInterval == 0) 
        {
            AdjustDifficulty();
        }
    }

    private void AdjustDifficulty() 
    {
        var recent = Chain.Where(b => b.Index > 0).TakeLast(_adjustmentInterval).ToList();
        if (recent.Count == 0) return;

        var avgMiningTime = recent.Average(b => b.MiningDuration);

        if (avgMiningTime < _targetBlockTime) 
        {
            Difficulty++;
            Console.WriteLine($"Difficulty increased to {Difficulty}");
        } 
        else if (avgMiningTime > _targetBlockTime) 
        {
            Difficulty = Math.Max(1, Difficulty - 1);
            Console.WriteLine($"Difficulty decreased to {Difficulty}");
        }
    }

    public bool IsChainValid()
    {
        for (int i = 1; i < Chain.Count; i++)
        {
            var currentBlock = Chain[i];
            var previousBlock = Chain[i - 1];

            if (currentBlock.Index == 0)
            {
                continue;
            }
            if (currentBlock.Hash != _hashingService.ComputeHashBlock(currentBlock))
            {
                return false;
            }
            if (currentBlock.PreviousHash != previousBlock.Hash)
            {
                return false;
            }
            if (currentBlock.Hash.Substring(0, Difficulty) != new string('0', Difficulty))
            {
                return false;
            }
        }
        return true;
    }
}
