using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Journey_of_faith.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddChurchImages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "QuizLevel",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Score",
                table: "QuizLevel",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "QuestionType",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "QuestionType",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "QuestionCategory",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "QuestionCategory",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "MassSchedule",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Descriptions",
                table: "AspNetRoles",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ChurchImages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ChurchId = table.Column<int>(type: "int", nullable: false),
                    ImageName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedUser = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChurchImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChurchImages_Church_ChurchId",
                        column: x => x.ChurchId,
                        principalTable: "Church",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Liturgy",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MassScheduleId = table.Column<int>(type: "int", nullable: false),
                    ReadingOne = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResponsorialPsalm = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GoodNew = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EndWord = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateActive = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Liturgy", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Liturgy_MassSchedule_MassScheduleId",
                        column: x => x.MassScheduleId,
                        principalTable: "MassSchedule",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChurchImages_ChurchId",
                table: "ChurchImages",
                column: "ChurchId");

            migrationBuilder.CreateIndex(
                name: "IX_Liturgy_MassScheduleId",
                table: "Liturgy",
                column: "MassScheduleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChurchImages");

            migrationBuilder.DropTable(
                name: "Liturgy");

            migrationBuilder.DropColumn(
                name: "Code",
                table: "QuizLevel");

            migrationBuilder.DropColumn(
                name: "Score",
                table: "QuizLevel");

            migrationBuilder.DropColumn(
                name: "Code",
                table: "QuestionType");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "QuestionType");

            migrationBuilder.DropColumn(
                name: "Code",
                table: "QuestionCategory");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "QuestionCategory");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "MassSchedule");

            migrationBuilder.DropColumn(
                name: "Descriptions",
                table: "AspNetRoles");
        }
    }
}
