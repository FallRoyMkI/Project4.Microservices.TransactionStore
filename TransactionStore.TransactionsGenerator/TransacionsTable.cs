using Microsoft.Data.SqlClient;
using System.Data;
using TransactionStore.BLL;
using TransactionStore.Models.Entities;
using TransactionStore.Models.Enums;

namespace TransactionStore.TransactionsGenerator;
public class TransacionsTable
{
    public static DataTable _transactionsTable = new DataTable("Transactions");
    public DataRow row = _transactionsTable.NewRow();
    public int _id = 1;
    public readonly string _connectionString = Environment.GetEnvironmentVariable("TStoreConnectionString")!;

    CurrencyRate _currencyRate = new();
    public void MakeTable(Dictionary<int, List<Accounts>> leadId_AccountId)
    {
        DataColumn Id = new DataColumn();
        Id.DataType = Type.GetType("System.Int32");
        Id.ColumnName = "Id";
        Id.AutoIncrement = true;
        Id.AllowDBNull = false;
        _transactionsTable.Columns.Add(Id);

        DataColumn accountId = new DataColumn();
        accountId.DataType = Type.GetType("System.Int32");
        accountId.ColumnName = "AccountId";
        accountId.AllowDBNull = false;
        _transactionsTable.Columns.Add(accountId);

        DataColumn type = new DataColumn();
        type.DataType = Type.GetType("System.Int32");
        type.ColumnName = "Type";
        type.AllowDBNull = false;
        _transactionsTable.Columns.Add(type);

        DataColumn amount = new DataColumn();
        amount.DataType = Type.GetType("System.Decimal");
        amount.ColumnName = "Amount";
        amount.AllowDBNull = false;
        _transactionsTable.Columns.Add(amount);

        DataColumn time = new DataColumn();
        time.DataType = Type.GetType("System.DateTime");
        time.ColumnName = "Time";
        time.AllowDBNull = false;
        _transactionsTable.Columns.Add(time);

        try
        {
            foreach (var lead in leadId_AccountId)
            {
                int countAccounts = lead.Value.Count();

                if (countAccounts == 1)
                {
                    TransactionDeposit(lead.Value[0].Id);
                    TransactionDeposit(lead.Value[0].Id);
                    TransactionWithdraw(lead.Value[0].Id);
                }
                else
                {
                    for (int j = 0; j < countAccounts * 0.7 * 3; ++j)
                    {
                        TransactionTransfer(lead.Value);
                    }

                    for (int j = 0; j < countAccounts * 0.2 * 3; ++j)
                    {
                        int accountIndex = new Random().Next(countAccounts);
                        TransactionDeposit(lead.Value[accountIndex].Id);
                    }

                    for (int j = 0; j < countAccounts * 0.1 * 3; ++j)
                    {
                        int accountIndex = new Random().Next(countAccounts);
                        TransactionWithdraw(lead.Value[accountIndex].Id);
                    }
                }

                if (_transactionsTable.Rows.Count > 500000)
                {
                    WriteToServer();
                }
            }

        }
        finally
        {
            WriteToServer();
        }

        Console.WriteLine("Спасибо за внимание!");
    }

    private void WriteToServer()
    {
        _transactionsTable.AcceptChanges();
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            connection.Open();

            using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connection))
            {
                bulkCopy.DestinationTableName =
                    "dbo.Transactions";

                try
                {
                    bulkCopy.WriteToServer(_transactionsTable);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            _transactionsTable.Rows.Clear();
        }
    }

    private void TransactionDeposit(int accountId)
    {
        int amountDeposit = new Random().Next(1, 10000);
        TransactionEntity transaction = new()
        {
            AccountId = accountId,
            Amount = amountDeposit,
            Type = TransactionType.Deposit,
            Time = DateTime.UtcNow
        };

        AddRow(transaction);
    }

    private void TransactionWithdraw(int accountId)
    {
        int amountWithdraw = new Random().Next(-10000, -1);
        TransactionEntity transaction = new()
        {
            AccountId = accountId,
            Amount = amountWithdraw,
            Type = TransactionType.Withdraw,
            Time = DateTime.UtcNow
        };

        AddRow(transaction);
    }

    private void AddRow(TransactionEntity transaction)
    {
        row = _transactionsTable.NewRow();
        row["Id"] = _id;
        ++_id;
        row["AccountId"] = transaction.AccountId;
        row["Type"] = transaction.Type;
        row["Amount"] = transaction.Amount;
        row["Time"] = transaction.Time;
        _transactionsTable.Rows.Add(row);
    }

    private void TransactionTransfer(List<Accounts> accounts)
    {
        int accountIndex = 0;
        int targetIndex = 0;
        int countAccounts = accounts.Count();

        while (accountIndex == targetIndex)
        {
            accountIndex = new Random().Next(countAccounts);
            targetIndex = new Random().Next(countAccounts);
        }

        DateTime time = DateTime.UtcNow;
        decimal amount = new Random().Next(1, 1000);

        row = _transactionsTable.NewRow();
        row["Id"] = _id;
        ++_id;
        row["AccountId"] = accounts[accountIndex].Id;
        row["Type"] = TransactionType.TransferWithdraw;
        row["Amount"] = -amount;
        row["Time"] = time;
        _transactionsTable.Rows.Add(row);

        row = _transactionsTable.NewRow();
        row["Id"] = _id;
        ++_id;
        row["AccountId"] = accounts[targetIndex].Id;
        row["Type"] = TransactionType.TransferDeposit;
        row["Amount"] = amount * _currencyRate.GetRate(accounts[accountIndex].Currency, accounts[targetIndex].Currency);
        row["Time"] = time;
        _transactionsTable.Rows.Add(row);
    }
}