using Microsoft.EntityFrameworkCore.Migrations;

namespace RDVFSharp.Migrations
{
    public partial class MultiTeamSupport : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AdditionalLosersId",
                table: "Fights",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AdditionalWinnersId",
                table: "Fights",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdditionalLosersId",
                table: "Fights");

            migrationBuilder.DropColumn(
                name: "AdditionalWinnersId",
                table: "Fights");
        }
    }
}
