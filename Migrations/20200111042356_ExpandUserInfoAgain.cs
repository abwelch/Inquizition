using Microsoft.EntityFrameworkCore.Migrations;

namespace Inquizition.Migrations
{
    public partial class ExpandUserInfoAgain : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Banned",
                table: "UserOverviewInfo",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "ReportedInstances",
                table: "UserOverviewInfo",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Banned",
                table: "UserOverviewInfo");

            migrationBuilder.DropColumn(
                name: "ReportedInstances",
                table: "UserOverviewInfo");
        }
    }
}
