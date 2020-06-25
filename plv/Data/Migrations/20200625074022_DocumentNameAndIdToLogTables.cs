using Microsoft.EntityFrameworkCore.Migrations;

namespace plv.Data.Migrations
{
    public partial class DocumentNameAndIdToLogTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DocumentId",
                table: "Downloads",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "DocumentName",
                table: "Downloads",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DocumentId",
                table: "DocumentEdits",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "DocumentName",
                table: "DocumentEdits",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DocumentId",
                table: "Downloads");

            migrationBuilder.DropColumn(
                name: "DocumentName",
                table: "Downloads");

            migrationBuilder.DropColumn(
                name: "DocumentId",
                table: "DocumentEdits");

            migrationBuilder.DropColumn(
                name: "DocumentName",
                table: "DocumentEdits");
        }
    }
}
