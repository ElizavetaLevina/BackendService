using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BackendService.DAL.Migrations
{
    /// <inheritdoc />
    public partial class MakePostIdNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PostsPending_Posts_PostId",
                table: "PostsPending");

            migrationBuilder.AlterColumn<int>(
                name: "PostId",
                table: "PostsPending",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddForeignKey(
                name: "FK_PostsPending_Posts_PostId",
                table: "PostsPending",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PostsPending_Posts_PostId",
                table: "PostsPending");

            migrationBuilder.AlterColumn<int>(
                name: "PostId",
                table: "PostsPending",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_PostsPending_Posts_PostId",
                table: "PostsPending",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
