using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Journey_of_faith.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class topic3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Quiz_Topic_TopicId",
                table: "Quiz");

            migrationBuilder.AlterColumn<int>(
                name: "TopicId",
                table: "Quiz",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Quiz_Topic_TopicId",
                table: "Quiz",
                column: "TopicId",
                principalTable: "Topic",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Quiz_Topic_TopicId",
                table: "Quiz");

            migrationBuilder.AlterColumn<int>(
                name: "TopicId",
                table: "Quiz",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Quiz_Topic_TopicId",
                table: "Quiz",
                column: "TopicId",
                principalTable: "Topic",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
