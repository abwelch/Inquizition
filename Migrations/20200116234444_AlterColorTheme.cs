using Microsoft.EntityFrameworkCore.Migrations;

namespace Inquizition.Migrations
{
    public partial class AlterColorTheme : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Publish");

            migrationBuilder.AddColumn<string>(
                name: "Creator",
                table: "ColorTheme",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Creator",
                table: "ColorTheme");

            migrationBuilder.CreateTable(
                name: "Publish",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InquizitorName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Published = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Publish", x => x.ID);
                });
        }
    }
}
