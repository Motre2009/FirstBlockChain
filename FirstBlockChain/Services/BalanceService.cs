using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FirstBlockChain.Models;

namespace FirstBlockChain.Services;

public class BalanceService
{
    public decimal GetBalance(string address, List<Block> blocks)
    {
        decimal balance = 0;
        foreach (var block in blocks)
        {
            foreach (var tx in block.Transactions)
            {
                if (tx.To == address)
                {
                    balance += tx.Amount;
                }
                if (tx.From == address)
                {
                    balance -= tx.Amount;
                }
            }
        }
        return balance;
    }

    public bool HasSufficientBalance(string address, decimal amount, List<Block> blocks, List<Transaction> transactions)
    {
        var balance = GetBalance(address, blocks);
        foreach (var tx in transactions) 
        {
            if (tx.From == address)
            {
                balance -= tx.Amount;
            }
        }
        return balance >= amount;
    }
}
