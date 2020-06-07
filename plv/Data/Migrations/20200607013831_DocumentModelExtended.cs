using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace plv.Data.Migrations
{
    public partial class DocumentModelExtended : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AddedBy",
                table: "Documents",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CurrentUser",
                table: "Documents",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateAdded",
                table: "Documents",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DateReceived",
                table: "Documents",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "LastUser",
                table: "Documents",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Receiver",
                table: "Documents",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Sender",
                table: "Documents",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ShortOptionalDescription",
                table: "Documents",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AddedBy",
                table: "Documents");

            migrationBuilder.DropColumn(
                name: "CurrentUser",
                table: "Documents");

            migrationBuilder.DropColumn(
                name: "DateAdded",
                table: "Documents");

            migrationBuilder.DropColumn(
                name: "DateReceived",
                table: "Documents");

            migrationBuilder.DropColumn(
                name: "LastUser",
                table: "Documents");

            migrationBuilder.DropColumn(
                name: "Receiver",
                table: "Documents");

            migrationBuilder.DropColumn(
                name: "Sender",
                table: "Documents");

            migrationBuilder.DropColumn(
                name: "ShortOptionalDescription",
                table: "Documents");
        }
    }
}
