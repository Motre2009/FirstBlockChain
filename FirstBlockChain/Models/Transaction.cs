using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstBlockChain.Models;

public class Transaction
{
    public string Id { get; set; }
    public string From { get; set; }
    public string To { get; set; }
    public decimal Amount { get; set; }
    public byte[] SenderPublicKey { get; set; }
    public byte[] Signature { get; set; }
    public DateTime TimeStamp { get; set; }

    public Transaction(string from, string to, decimal amount)
    {
        Id = Guid.NewGuid().ToString();
        From = from;
        To = to;
        Amount = amount;
        TimeStamp = DateTime.UtcNow;
    }

    public byte[] GetDataToSign()
    {
        var data = $"{Id}{From}{To}{Amount}{TimeStamp}";
        return Encoding.UTF8.GetBytes(data);
    }

    public string ToRawString()
    {
        return $"Transation Id: {Id} -  From: {From} ->  To: {To}: Amount: {Amount} ({TimeStamp})";
    }
}
