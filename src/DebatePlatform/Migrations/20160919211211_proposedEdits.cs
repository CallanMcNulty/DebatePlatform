using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DebatePlatform.Migrations
{
    public partial class proposedEdits : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProposedEdits",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ArgumentId = table.Column<int>(nullable: false),
                    IsAffirmative = table.Column<bool>(nullable: false),
                    IsDelete = table.Column<bool>(nullable: false),
                    ParentId = table.Column<int>(nullable: false),
                    Reason = table.Column<string>(nullable: true),
                    Text = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: true),
                    Votes = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProposedEdits", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProposedEdits_Arguments_ArgumentId",
                        column: x => x.ArgumentId,
                        principalTable: "Arguments",
                        principalColumn: "ArgumentId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EditVotes",
                columns: table => new
                {
                    EditVoteId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ProposedEditId = table.Column<int>(nullable: false),
                    UserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EditVotes", x => x.EditVoteId);
                    table.ForeignKey(
                        name: "FK_EditVotes_ProposedEdits_ProposedEditId",
                        column: x => x.ProposedEditId,
                        principalTable: "ProposedEdits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EditVotes_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EditVotes_ProposedEditId",
                table: "EditVotes",
                column: "ProposedEditId");

            migrationBuilder.CreateIndex(
                name: "IX_EditVotes_UserId",
                table: "EditVotes",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ProposedEdits_ArgumentId",
                table: "ProposedEdits",
                column: "ArgumentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EditVotes");

            migrationBuilder.DropTable(
                name: "ProposedEdits");
        }
    }
}
