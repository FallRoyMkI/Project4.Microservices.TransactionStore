﻿using TransactionStore.Models.Enums;

namespace TransactionStore.Models.Models;

public class Transaction
{
    public int Id { get; set; }
    public int AccountId { get; set; }
    public string Type { get; set; }
    public int Amount { get; set; }
    public DateTime Time { get; set; }
}