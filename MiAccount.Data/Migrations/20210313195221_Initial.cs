using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MiAccount.Data.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    TokenTimeout = table.Column<int>(nullable: true),
                    Active = table.Column<bool>(nullable: false),
                    CreatedById = table.Column<long>(nullable: false),
                    CreateTime = table.Column<DateTimeOffset>(nullable: false),
                    UpdatedById = table.Column<long>(nullable: false),
                    UpdateTime = table.Column<DateTimeOffset>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Accounts");
        }
    }
}
