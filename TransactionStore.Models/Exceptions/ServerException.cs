using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransactionStore.Models.Exceptions
{
    public class ServerException : Exception
    {
        public ServerException(string message) : base(message) { }
    }
}
