using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShAbedi.PayaSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Add_RetryCount_To_ShebaRequest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "FailedDateTime",
                table: "ShebaRequests",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RetryCount",
                table: "ShebaRequests",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FailedDateTime",
                table: "ShebaRequests");

            migrationBuilder.DropColumn(
                name: "RetryCount",
                table: "ShebaRequests");
        }
    }
}
