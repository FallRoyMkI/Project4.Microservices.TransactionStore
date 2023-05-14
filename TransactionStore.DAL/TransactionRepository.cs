using TransactionStore.Contracts;

namespace TransactionStore.DAL;

public class TransactionRepository : ITransactionRepository
{
    private readonly Context _context;

    public TransactionRepository(Context context)
    {
        _context = context;
    }
}