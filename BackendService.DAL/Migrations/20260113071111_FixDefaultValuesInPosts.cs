using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BackendService.DAL.Migrations
{
    /// <inheritdoc />
    public partial class FixDefaultValuesInPosts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "DateUpdate",
                table: "Posts",
                type: "timestamp without time zone",
                nullable: true,
                defaultValue: null,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");


            migrationBuilder.AlterColumn<bool>(
                name: "Deleted",
                table: "Posts",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "boolean"
                );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "DateUpdate",
                table: "Posts",
                type: "timestamp without time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AlterColumn<bool>(
               name: "Deleted",
               table: "Posts",
               type: "boolean",
               nullable: false,
               oldClrType: typeof(bool),
               oldType: "boolean"
               );
        }
    }
}
