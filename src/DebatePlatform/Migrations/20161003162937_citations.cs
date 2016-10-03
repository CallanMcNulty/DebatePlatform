using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DebatePlatform.Migrations
{
    public partial class citations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Citations",
                columns: table => new
                {
                    CitationId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ArgumentId = table.Column<int>(nullable: false),
                    Creator = table.Column<string>(nullable: true),
                    Date = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Format = table.Column<string>(nullable: true),
                    Institution = table.Column<string>(nullable: true),
                    Text = table.Column<string>(nullable: true),
                    Title = table.Column<string>(nullable: true),
                    URL = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Citations", x => x.CitationId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Citations");
        }
    }
}
