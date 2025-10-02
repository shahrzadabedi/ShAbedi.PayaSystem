using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShAbedi.PayaSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Add_ModifiedAt_To__Account_And_Change_ShebaRequest_Fields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PendingDateTime",
                table: "ShebaRequests",
                newName: "ReadyToCompleteDateTime");

            migrationBuilder.RenameColumn(
                name: "ConfirmDateTime",
                table: "ShebaRequests",
                newName: "CompleteDateTime");

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedAt",
                table: "Accounts",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ModifiedAt",
                table: "Accounts");

            migrationBuilder.RenameColumn(
                name: "ReadyToCompleteDateTime",
                table: "ShebaRequests",
                newName: "PendingDateTime");

            migrationBuilder.RenameColumn(
                name: "CompleteDateTime",
                table: "ShebaRequests",
                newName: "ConfirmDateTime");
        }
    }
}
