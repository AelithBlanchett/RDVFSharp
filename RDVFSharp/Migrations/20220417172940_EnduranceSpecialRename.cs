using Microsoft.EntityFrameworkCore.Migrations;

namespace RDVFSharp.Migrations
{
    public partial class EnduranceSpecialRename : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Special",
                table: "Fighters",
                newName: "Willpower");

            migrationBuilder.RenameColumn(
                name: "Endurance",
                table: "Fighters",
                newName: "Spellpower");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Willpower",
                table: "Fighters",
                newName: "Special");

            migrationBuilder.RenameColumn(
                name: "Spellpower",
                table: "Fighters",
                newName: "Endurance");
        }
    }
}
