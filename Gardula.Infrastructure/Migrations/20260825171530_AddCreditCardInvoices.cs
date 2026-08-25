using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gardula.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCreditCardInvoices : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CreditCardInvoiceId",
                table: "Transactions",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CreditCardInvoices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    CardId = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: false),
                    ClosingDate = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: false),
                    DueDate = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    PaidAt = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CreditCardInvoices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CreditCardInvoices_Cards_CardId",
                        column: x => x.CardId,
                        principalTable: "Cards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CreditCardInvoices_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_CreditCardInvoiceId",
                table: "Transactions",
                column: "CreditCardInvoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_CreditCardInvoices_CardId",
                table: "CreditCardInvoices",
                column: "CardId");

            migrationBuilder.CreateIndex(
                name: "IX_CreditCardInvoices_CardId_StartDate_ClosingDate",
                table: "CreditCardInvoices",
                columns: new[] { "CardId", "StartDate", "ClosingDate" });

            migrationBuilder.CreateIndex(
                name: "IX_CreditCardInvoices_UserId",
                table: "CreditCardInvoices",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_CreditCardInvoices_CreditCardInvoiceId",
                table: "Transactions",
                column: "CreditCardInvoiceId",
                principalTable: "CreditCardInvoices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_CreditCardInvoices_CreditCardInvoiceId",
                table: "Transactions");

            migrationBuilder.DropTable(
                name: "CreditCardInvoices");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_CreditCardInvoiceId",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "CreditCardInvoiceId",
                table: "Transactions");
        }
    }
}
