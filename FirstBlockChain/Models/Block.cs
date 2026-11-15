using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstBlockChain.Models;

public class Block
{
    public int Index { get; set; }

    public DateTime Timestamp { get; set; }
    public List<Transaction> Transactions { get; set; }
    public string Hash { get; set; }
    public string PreviousHash { get; set; }
    public int Nonce { get; set; }
    public double MiningDuration { get; set; }
    }

