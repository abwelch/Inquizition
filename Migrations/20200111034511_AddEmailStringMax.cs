using Microsoft.EntityFrameworkCore.Migrations;

namespace Inquizition.Migrations
{
    public partial class AddEmailStringMax : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "UserOverviewInfo",
                newName: "ID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ID",
                table: "UserOverviewInfo",
                newName: "Id");
        }
    }
}
