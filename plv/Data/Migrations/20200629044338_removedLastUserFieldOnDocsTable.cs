using Microsoft.EntityFrameworkCore.Migrations;

namespace plv.Data.Migrations
{
    public partial class removedLastUserFieldOnDocsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastUser",
                table: "Documents");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LastUser",
                table: "Documents",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
