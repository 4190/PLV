using Microsoft.EntityFrameworkCore.Migrations;

namespace plv.Data.Migrations
{
    public partial class addingBlocktablestoDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Block",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PreviousDocIdHash = table.Column<string>(nullable: true),
                    DocIdHash = table.Column<string>(nullable: true),
                    PreviousAddedByHash = table.Column<string>(nullable: true),
                    AddedByHash = table.Column<string>(nullable: true),
                    PreviousCurrentUserHash = table.Column<string>(nullable: true),
                    CurrentUserHash = table.Column<string>(nullable: true),
                    PreviousReceiverHash = table.Column<string>(nullable: true),
                    ReceiverHash = table.Column<string>(nullable: true),
                    PreviousSenderHash = table.Column<string>(nullable: true),
                    SenderHash = table.Column<string>(nullable: true),
                    PreviousShortOptionalDescriptionHash = table.Column<string>(nullable: true),
                    ShortOptionalDescriptionHash = table.Column<string>(nullable: true),
                    PreviousDateAddedHash = table.Column<string>(nullable: true),
                    DateAddedHash = table.Column<string>(nullable: true),
                    PreviousDateReceivedHash = table.Column<string>(nullable: true),
                    DateReceivedHash = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Block", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Block");
        }
    }
}
