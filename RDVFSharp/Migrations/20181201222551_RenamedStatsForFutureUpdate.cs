using Microsoft.EntityFrameworkCore.Migrations;

namespace RDVFSharp.Migrations
{
    public partial class RenamedStatsForFutureUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Willpower",
                table: "Fighters",
                newName: "Special");

            migrationBuilder.RenameColumn(
                name: "Spellpower",
                table: "Fighters",
                newName: "Resilience");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Special",
                table: "Fighters",
                newName: "Willpower");

            migrationBuilder.RenameColumn(
                name: "Resilience",
                table: "Fighters",
                newName: "Spellpower");
        }
    }
}
