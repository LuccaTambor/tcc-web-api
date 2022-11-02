using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace tcc_web_api.Migrations
{
    public partial class AddOccurrence : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Occurrences",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeveloperId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ProjectId = table.Column<int>(type: "int", nullable: false),
                    OccurrenceType = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Occurrences", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Occurrences_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Occurrences_Users_DeveloperId",
                        column: x => x.DeveloperId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Occurrences_DeveloperId",
                table: "Occurrences",
                column: "DeveloperId");

            migrationBuilder.CreateIndex(
                name: "IX_Occurrences_ProjectId",
                table: "Occurrences",
                column: "ProjectId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Occurrences");
        }
    }
}
