using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace plv.Data.Migrations
{
    public partial class DocEditsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Sender",
                table: "Documents",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Receiver",
                table: "Documents",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "DocumentEdits",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EditedBy = table.Column<string>(nullable: true),
                    PreviousDateIssued = table.Column<DateTime>(nullable: false),
                    NewDateIssued = table.Column<DateTime>(nullable: false),
                    PreviousUser = table.Column<string>(nullable: true),
                    NewUser = table.Column<string>(nullable: true),
                    PreviousReceiver = table.Column<string>(nullable: true),
                    NewReceiver = table.Column<string>(nullable: true),
                    PreviousSender = table.Column<string>(nullable: true),
                    NewSender = table.Column<string>(nullable: true),
                    PreviousDescription = table.Column<string>(nullable: true),
                    NewDescription = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentEdits", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DocumentEdits");

            migrationBuilder.AlterColumn<string>(
                name: "Sender",
                table: "Documents",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "Receiver",
                table: "Documents",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string));
        }
    }
}
