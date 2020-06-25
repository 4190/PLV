using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace plv.Data.Migrations
{
    public partial class DownloadAndEditTimeFieldInLogTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DownloadTime",
                table: "Downloads",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "EditTime",
                table: "DocumentEdits",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DownloadTime",
                table: "Downloads");

            migrationBuilder.DropColumn(
                name: "EditTime",
                table: "DocumentEdits");
        }
    }
}
