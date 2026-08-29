using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gardula.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddTotalAmountToCreditCardInvoice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "TotalAmount",
                table: "CreditCardInvoices",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalAmount",
                table: "CreditCardInvoices");
        }
    }
}
