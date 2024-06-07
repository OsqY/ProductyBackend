using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Producty.Migrations
{
    /// <inheritdoc />
    public partial class ChangedTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Expense_Users_UserId",
                table: "Expense");

            migrationBuilder.DropForeignKey(
                name: "FK_Income_Users_UserId",
                table: "Income");

            migrationBuilder.DropForeignKey(
                name: "FK_JournalEntry_Users_UserId",
                table: "JournalEntry");

            migrationBuilder.DropPrimaryKey(
                name: "PK_JournalEntry",
                table: "JournalEntry");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Income",
                table: "Income");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Expense",
                table: "Expense");

            migrationBuilder.RenameTable(
                name: "JournalEntry",
                newName: "JournalEntries");

            migrationBuilder.RenameTable(
                name: "Income",
                newName: "Incomes");

            migrationBuilder.RenameTable(
                name: "Expense",
                newName: "Expenses");

            migrationBuilder.RenameIndex(
                name: "IX_JournalEntry_UserId",
                table: "JournalEntries",
                newName: "IX_JournalEntries_UserId");

            migrationBuilder.RenameColumn(
                name: "Topic",
                table: "Incomes",
                newName: "Category");

            migrationBuilder.RenameIndex(
                name: "IX_Income_UserId",
                table: "Incomes",
                newName: "IX_Incomes_UserId");

            migrationBuilder.RenameColumn(
                name: "Topic",
                table: "Expenses",
                newName: "Category");

            migrationBuilder.RenameIndex(
                name: "IX_Expense_UserId",
                table: "Expenses",
                newName: "IX_Expenses_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_JournalEntries",
                table: "JournalEntries",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Incomes",
                table: "Incomes",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Expenses",
                table: "Expenses",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Expenses_Users_UserId",
                table: "Expenses",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Incomes_Users_UserId",
                table: "Incomes",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_JournalEntries_Users_UserId",
                table: "JournalEntries",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Expenses_Users_UserId",
                table: "Expenses");

            migrationBuilder.DropForeignKey(
                name: "FK_Incomes_Users_UserId",
                table: "Incomes");

            migrationBuilder.DropForeignKey(
                name: "FK_JournalEntries_Users_UserId",
                table: "JournalEntries");

            migrationBuilder.DropPrimaryKey(
                name: "PK_JournalEntries",
                table: "JournalEntries");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Incomes",
                table: "Incomes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Expenses",
                table: "Expenses");

            migrationBuilder.RenameTable(
                name: "JournalEntries",
                newName: "JournalEntry");

            migrationBuilder.RenameTable(
                name: "Incomes",
                newName: "Income");

            migrationBuilder.RenameTable(
                name: "Expenses",
                newName: "Expense");

            migrationBuilder.RenameIndex(
                name: "IX_JournalEntries_UserId",
                table: "JournalEntry",
                newName: "IX_JournalEntry_UserId");

            migrationBuilder.RenameColumn(
                name: "Category",
                table: "Income",
                newName: "Topic");

            migrationBuilder.RenameIndex(
                name: "IX_Incomes_UserId",
                table: "Income",
                newName: "IX_Income_UserId");

            migrationBuilder.RenameColumn(
                name: "Category",
                table: "Expense",
                newName: "Topic");

            migrationBuilder.RenameIndex(
                name: "IX_Expenses_UserId",
                table: "Expense",
                newName: "IX_Expense_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_JournalEntry",
                table: "JournalEntry",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Income",
                table: "Income",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Expense",
                table: "Expense",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Expense_Users_UserId",
                table: "Expense",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Income_Users_UserId",
                table: "Income",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_JournalEntry_Users_UserId",
                table: "JournalEntry",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
