using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace authTwo.Migrations
{
    /// <inheritdoc />
    public partial class ser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserName",
                schema: "AuthUser",
                table: "user",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserName",
                schema: "AuthUser",
                table: "user");
        }
    }
}
