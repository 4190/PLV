using Microsoft.EntityFrameworkCore.Migrations;

namespace plv.Data.Migrations
{
    public partial class DocumetsSections : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Sections",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sections", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DocumentsSections",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DocumentInDBId = table.Column<int>(nullable: false),
                    SectionId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentsSections", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DocumentsSections_Documents_DocumentInDBId",
                        column: x => x.DocumentInDBId,
                        principalTable: "Documents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DocumentsSections_Sections_SectionId",
                        column: x => x.SectionId,
                        principalTable: "Sections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DocumentsSections_DocumentInDBId",
                table: "DocumentsSections",
                column: "DocumentInDBId");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentsSections_SectionId",
                table: "DocumentsSections",
                column: "SectionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DocumentsSections");

            migrationBuilder.DropTable(
                name: "Sections");
        }
    }
}
