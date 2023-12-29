using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace URLShortener.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddColumnToken : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "token",
                table: "URLInfos",
                type: "nvarchar(7)",
                maxLength: 7,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "token",
                table: "URLInfos");
        }
    }
}
