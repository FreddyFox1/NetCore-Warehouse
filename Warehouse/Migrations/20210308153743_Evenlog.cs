using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Warehouse.Migrations
{
    public partial class Evenlog : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ItemLogs",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LogItemName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LogUserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LogOldStorage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LogCurStorage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LogItemArticle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LogDateTransfer = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemLogs", x => x.ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ItemLogs");
        }
    }
}
