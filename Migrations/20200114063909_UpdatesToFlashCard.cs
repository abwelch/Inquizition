using Microsoft.EntityFrameworkCore.Migrations;

namespace Inquizition.Migrations
{
    public partial class UpdatesToFlashCard : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CardColor",
                table: "FlashCards");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CardColor",
                table: "FlashCards",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
