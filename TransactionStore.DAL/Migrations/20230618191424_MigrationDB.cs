using System;
using Microsoft.EntityFrameworkCore.Migrations;
using TransactionStore.Models.Enums;

#nullable disable

namespace TransactionStore.DAL.Migrations
{
    /// <inheritdoc />
    public partial class MigrationDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Transactions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountId = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Time = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.Id);
                });
            migrationBuilder.Sql(
    @"EXEC (' CREATE PROCEDURE [dbo].[AddTransaction]
            	@AccountId Int,
            	@Type Int,
            	@Amount Decimal
                AS
                Declare @time DateTime2 = GETUTCDATE();
                INSERT INTO dbo.[Transactions]
                (
                	[AccountId],
                	[Type],
                	[Amount],
                	[Time]
                )
                VALUES
                (
                	@AccountId,
                	@Type,
                	@Amount,
                	@time
                )
                Select [Id] from dbo.Transactions As T where T.Time = @time ')");

            migrationBuilder.Sql(
                @"EXEC (' CREATE PROCEDURE [dbo].[AddTransactionInTransfer]
            	@AccountId Int,
            	@Type Int,
            	@Amount Decimal,
            	@time DateTime2
                AS
                INSERT INTO dbo.[Transactions]
                (
                	[AccountId],
                	[Type],
                	[Amount],
                	[Time]
                )
                VALUES
                (
                	@AccountId,
                	@Type,
                	@Amount,
                	@time
                )
                Return SCOPE_IDENTITY();  ')");


            migrationBuilder.Sql(
                @"EXEC (' CREATE PROCEDURE [dbo].[AddTransfer]
            	@AccountId1 Int,
            	@Type1 Int,
            	@Amount1 Decimal,
            	@AccountId2 Int,
            	@Type2 Int,
            	@Amount2 Decimal
                AS
                Declare @time DateTime2 = GETUTCDATE(), @TransactionId1 Int, @TransactionId2 Int; 
                EXEC @TransactionId1 = AddTransactionInTransfer @AccountId1, @Type1, @Amount1, @time
                EXEC @TransactionId2 = AddTransactionInTransfer @AccountId2, @Type2, @Amount2, @time
                Select [Id] from dbo.Transactions As T where T.Time = @time  ')");

            migrationBuilder.Sql(
               @$"EXEC (' CREATE PROCEDURE [dbo].[GetTransactionsByAccountId]
                @AccountId int
                AS
                BEGIN
                    SET NOCOUNT ON;

                    DECLARE @objects TABLE(
                        Id int,
                        AccountId int,
                        Type nvarchar(max),
                        Amount Decimal,
                        Time datetime2
                    )


                    INSERT INTO @objects
                    select* from dbo.Transactions AS T
                    where T.AccountId = @AccountId and(T.Type = {(int)TransactionType.TransferWithdraw} or T.Type = {(int)TransactionType.TransferDeposit})


                    DECLARE @time Table(
                        Id int Identity,
                        Time DateTime2
                    )
                    Declare @tmp int
                    set  @tmp = 1


                    Insert Into @time
                    select O.Time FROM @objects as O


                    Declare @count_table_time int
                    select @count_table_time = COUNT(*) FROM @time


                    Declare @transaction_time DateTime2


                    while @tmp < @count_table_time + 1
                    BEGIN
                        set @transaction_time = (select T.Time from @time AS T where T.Id = @tmp)
                        IF EXISTS(
                            select* from dbo.Transactions AS T
                            WHERE T.AccountId<> @AccountId AND T.Time = @transaction_time
                        )
                        BEGIN
                            INSERT INTO @objects
                            select* from dbo.Transactions AS T
                            WHERE T.AccountId<> @AccountId AND T.Time = @transaction_time
                        END
                        set @tmp = @tmp + 1
                    END

                    INSERT INTO @objects
                    select* from dbo.Transactions AS T
                    where T.AccountId = @AccountId and(T.Type = {(int)TransactionType.Withdraw} or T.Type = {(int)TransactionType.Deposit})


                    SELECT*
                    FROM @objects as o
                    order by o.Time ASC, o.Type DESC
                END  ')");
            migrationBuilder.CreateIndex("NonClusteredIndex", "Transactions", "AccountId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Transactions");
        }
    }
}
