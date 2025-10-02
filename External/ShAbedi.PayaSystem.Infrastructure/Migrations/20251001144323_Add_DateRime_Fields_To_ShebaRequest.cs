using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShAbedi.PayaSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Add_DateRime_Fields_To_ShebaRequest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ReadyToCancelDateTime",
                table: "ShebaRequests",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ReadyToRetryDateTime",
                table: "ShebaRequests",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReadyToCancelDateTime",
                table: "ShebaRequests");

            migrationBuilder.DropColumn(
                name: "ReadyToRetryDateTime",
                table: "ShebaRequests");
        }
    }
}
