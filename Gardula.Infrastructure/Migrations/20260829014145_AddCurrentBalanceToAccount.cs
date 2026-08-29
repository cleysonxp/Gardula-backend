using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gardula.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCurrentBalanceToAccount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "CurrentBalance",
                table: "Accounts",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.Sql("""
                UPDATE Accounts
                SET CurrentBalance = InitialBalance;
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CurrentBalance",
                table: "Accounts");
        }
    }
}