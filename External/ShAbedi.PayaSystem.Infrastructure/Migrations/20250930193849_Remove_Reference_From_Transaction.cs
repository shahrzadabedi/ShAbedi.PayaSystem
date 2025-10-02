using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShAbedi.PayaSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Remove_Reference_From_Transaction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Reference",
                table: "Transactions");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Reference",
                table: "Transactions",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);
        }
    }
}
