using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace tcc_web_api.Migrations
{
    public partial class AddTeamToOccurrence : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TeamId",
                table: "Occurrences",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Occurrences_TeamId",
                table: "Occurrences",
                column: "TeamId");

            migrationBuilder.AddForeignKey(
                name: "FK_Occurrences_Teams_TeamId",
                table: "Occurrences",
                column: "TeamId",
                principalTable: "Teams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Occurrences_Teams_TeamId",
                table: "Occurrences");

            migrationBuilder.DropIndex(
                name: "IX_Occurrences_TeamId",
                table: "Occurrences");

            migrationBuilder.DropColumn(
                name: "TeamId",
                table: "Occurrences");
        }
    }
}
