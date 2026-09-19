using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobApplication.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddRecruiterEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ClosedAt",
                table: "Jobs",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ClosedBy",
                table: "Jobs",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RecruiterId",
                table: "Jobs",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Recruiters",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Recruiters", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Jobs_RecruiterId",
                table: "Jobs",
                column: "RecruiterId");

            migrationBuilder.AddForeignKey(
                name: "FK_Jobs_Recruiters_RecruiterId",
                table: "Jobs",
                column: "RecruiterId",
                principalTable: "Recruiters",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Jobs_Recruiters_RecruiterId",
                table: "Jobs");

            migrationBuilder.DropTable(
                name: "Recruiters");

            migrationBuilder.DropIndex(
                name: "IX_Jobs_RecruiterId",
                table: "Jobs");

            migrationBuilder.DropColumn(
                name: "ClosedAt",
                table: "Jobs");

            migrationBuilder.DropColumn(
                name: "ClosedBy",
                table: "Jobs");

            migrationBuilder.DropColumn(
                name: "RecruiterId",
                table: "Jobs");
        }
    }
}
