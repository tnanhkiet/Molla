using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KShop.Migrations
{
    public partial class updateMember : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "members",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LoginName",
                table: "members",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "members",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "members",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "members");

            migrationBuilder.DropColumn(
                name: "LoginName",
                table: "members");

            migrationBuilder.DropColumn(
                name: "Password",
                table: "members");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "members");
        }
    }
}
