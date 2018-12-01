using Microsoft.EntityFrameworkCore.Migrations;

namespace RDVFSharp.Migrations
{
    public partial class AddFights : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Fights",
                columns: table => new
                {
                    FightId = table.Column<string>(nullable: false),
                    Room = table.Column<string>(nullable: false),
                    WinnerId = table.Column<string>(nullable: false),
                    LoserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fights", x => x.FightId);
                    table.ForeignKey(
                        name: "FK_Fights_Fights_LoserId",
                        column: x => x.LoserId,
                        principalTable: "Fights",
                        principalColumn: "FightId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Fights_Fights_WinnerId",
                        column: x => x.WinnerId,
                        principalTable: "Fights",
                        principalColumn: "FightId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Fights_LoserId",
                table: "Fights",
                column: "LoserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Fights_WinnerId",
                table: "Fights",
                column: "WinnerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Fights");
        }
    }
}
