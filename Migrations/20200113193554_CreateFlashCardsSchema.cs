using Microsoft.EntityFrameworkCore.Migrations;

namespace Inquizition.Migrations
{
    public partial class CreateFlashCardsSchema : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FlashCards",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Creator = table.Column<string>(nullable: true),
                    InquizitorName = table.Column<string>(maxLength: 55, nullable: true),
                    IsPrivate = table.Column<bool>(nullable: false),
                    CardNumber = table.Column<int>(nullable: false),
                    CardTitle = table.Column<string>(maxLength: 45, nullable: false),
                    CardBody = table.Column<string>(maxLength: 300, nullable: true),
                    CardAnswer = table.Column<string>(maxLength: 400, nullable: false),
                    Image = table.Column<string>(nullable: true),
                    Audio = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FlashCards", x => x.ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FlashCards");
        }
    }
}
