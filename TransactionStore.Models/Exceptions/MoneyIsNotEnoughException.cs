using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransactionStore.Models.Exceptions
{
    public class MoneyIsNotEnoughException : Exception
    {
        public MoneyIsNotEnoughException(string message) : base(message) { }
    }
}
