using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShAbedi.PayaSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Initialize : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OwnerName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ShebaNumber = table.Column<string>(type: "nvarchar(26)", maxLength: 26, nullable: false),
                    Balance = table.Column<long>(type: "bigint", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ShebaRequests",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Price = table.Column<long>(type: "bigint", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    FromShebaNumber = table.Column<string>(type: "nvarchar(26)", maxLength: 26, nullable: false),
                    ToShebaNumber = table.Column<string>(type: "nvarchar(26)", maxLength: 26, nullable: false),
                    Note = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    FromAccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PendingDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ConfirmDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CancelDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShebaRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ShebaRequests_Accounts_FromAccountId",
                        column: x => x.FromAccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Transactions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Amount = table.Column<long>(type: "bigint", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Reference = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Note = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Transactions_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AmountLocks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    AccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Amount = table.Column<long>(type: "bigint", nullable: false),
                    ShebaRequestId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AmountLocks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AmountLocks_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AmountLocks_ShebaRequests_ShebaRequestId",
                        column: x => x.ShebaRequestId,
                        principalTable: "ShebaRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_ShebaNumber",
                table: "Accounts",
                column: "ShebaNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AmountLocks_AccountId",
                table: "AmountLocks",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_AmountLocks_ShebaRequestId",
                table: "AmountLocks",
                column: "ShebaRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_ShebaRequests_FromAccountId",
                table: "ShebaRequests",
                column: "FromAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_AccountId",
                table: "Transactions",
                column: "AccountId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AmountLocks");

            migrationBuilder.DropTable(
                name: "Transactions");

            migrationBuilder.DropTable(
                name: "ShebaRequests");

            migrationBuilder.DropTable(
                name: "Accounts");
        }
    }
}
