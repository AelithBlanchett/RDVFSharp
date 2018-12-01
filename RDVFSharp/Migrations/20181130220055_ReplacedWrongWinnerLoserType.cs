using Microsoft.EntityFrameworkCore.Migrations;

namespace RDVFSharp.Migrations
{
    public partial class ReplacedWrongWinnerLoserType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Fights_Fights_LoserId",
                table: "Fights");

            migrationBuilder.DropForeignKey(
                name: "FK_Fights_Fights_WinnerId",
                table: "Fights");

            migrationBuilder.DropIndex(
                name: "IX_Fights_LoserId",
                table: "Fights");

            migrationBuilder.CreateIndex(
                name: "IX_Fights_LoserId",
                table: "Fights",
                column: "LoserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Fights_Fighters_LoserId",
                table: "Fights",
                column: "LoserId",
                principalTable: "Fighters",
                principalColumn: "Name",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Fights_Fighters_WinnerId",
                table: "Fights",
                column: "WinnerId",
                principalTable: "Fighters",
                principalColumn: "Name",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Fights_Fighters_LoserId",
                table: "Fights");

            migrationBuilder.DropForeignKey(
                name: "FK_Fights_Fighters_WinnerId",
                table: "Fights");

            migrationBuilder.DropIndex(
                name: "IX_Fights_LoserId",
                table: "Fights");

            migrationBuilder.CreateIndex(
                name: "IX_Fights_LoserId",
                table: "Fights",
                column: "LoserId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Fights_Fights_LoserId",
                table: "Fights",
                column: "LoserId",
                principalTable: "Fights",
                principalColumn: "FightId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Fights_Fights_WinnerId",
                table: "Fights",
                column: "WinnerId",
                principalTable: "Fights",
                principalColumn: "FightId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
