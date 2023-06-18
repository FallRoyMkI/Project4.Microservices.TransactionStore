using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransactionStore.Models.Enums;

namespace TransactionStore.Models.Models
{
    public class TransferTransactionResponse
    {
        public int WithdrawId { get; set; }
        public int AccountId { get; set; }
        public decimal Amount { get; set; }
        public int DepositId { get; set; }
        public int TargetAccountId { get; set; }
        public decimal TargetAmount { get; set; }
        public TransactionType Type { get; set; }
        public DateTime time { get; set; }

        public TransferTransactionResponse(Transaction withdraw, Transaction deposit) 
        {
            WithdrawId = withdraw.Id;
            AccountId = withdraw.AccountId;
            Amount = withdraw.Amount;
            DepositId = deposit.Id;
            TargetAccountId = deposit.AccountId; 
            TargetAmount = deposit.Amount;
            time = deposit.Time;
            Type = TransactionType.Transfer;
        }
    }
}
