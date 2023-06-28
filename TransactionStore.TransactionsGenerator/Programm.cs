
using Microsoft.Data.SqlClient;
using System.Data;
using TransactionStore.Models.Entities;
using TransactionStore.Models.Enums;
using TransactionStore.Models.Models;
using TransactionStore.TransactionsGenerator;

public class Programm
{
    static void Main()
    {
        string connectionString = @"Data Source= 194.87.210.5;Initial Catalog = CrmBourseDB;
                                TrustServerCertificate=True;User ID = student;Password=qwe!23;";
        string query = "SELECT * FROM dbo.Accounts where LeadId < 4080000";

        Dictionary<int, List<Accounts>> leadId_AccountId= new Dictionary<int, List<Accounts>>();

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int id = reader.GetInt32(0);
                        int leadId = reader.GetInt32(1); 
                        string currency = reader.GetString(3);

                        if (!leadId_AccountId.ContainsKey(leadId))
                        {
                            leadId_AccountId[leadId] = new List<Accounts>();
                            leadId_AccountId[leadId].Add(new Accounts() { Id = id, Currency = currency });
                        }
                        else
                        {
                            leadId_AccountId[leadId].Add(new Accounts() { Id = id, Currency = currency });
                        }  
                    }
                }
            }
        }

        TransacionsTable transacionsTable = new();
        transacionsTable.MakeTable(leadId_AccountId);

    }
}
