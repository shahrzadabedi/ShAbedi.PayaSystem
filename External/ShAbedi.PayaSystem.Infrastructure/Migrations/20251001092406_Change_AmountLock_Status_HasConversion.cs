using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShAbedi.PayaSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Change_AmountLock_Status_HasConversion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "AmountLocks",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "Locked",
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "AmountLocks",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldDefaultValue: "Locked");
        }
    }
}
