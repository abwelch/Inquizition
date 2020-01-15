using Microsoft.EntityFrameworkCore.Migrations;

namespace Inquizition.Migrations
{
    public partial class RemoveAudioAndLink : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Audio",
                table: "FlashCards");

            migrationBuilder.DropColumn(
                name: "Image",
                table: "FlashCards");

            migrationBuilder.AlterColumn<string>(
                name: "CardBody",
                table: "FlashCards",
                maxLength: 400,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(400)",
                oldMaxLength: 400,
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "CardBody",
                table: "FlashCards",
                type: "nvarchar(400)",
                maxLength: 400,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 400);

            migrationBuilder.AddColumn<string>(
                name: "Audio",
                table: "FlashCards",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "FlashCards",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
