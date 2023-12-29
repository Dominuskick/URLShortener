using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace URLShortener.DAL.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "URLInfos",
                columns: table => new
                {
                    urlId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    fullURL = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    shortenURL = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    createdBy = table.Column<int>(type: "int", nullable: false),
                    createdDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_URLInfos", x => x.urlId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "URLInfos");
        }
    }
}
