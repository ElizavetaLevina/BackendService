using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BackendService.DAL.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePostPendingEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<HashSet<int>>(
                name: "ImageIds",
                table: "PostsPending",
                type: "integer[]",
                nullable: false);

            migrationBuilder.AddColumn<HashSet<int>>(
                name: "TagIds",
                table: "PostsPending",
                type: "integer[]",
                nullable: false);

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "PostsPending",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageIds",
                table: "PostsPending");

            migrationBuilder.DropColumn(
                name: "TagIds",
                table: "PostsPending");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "PostsPending");
        }
    }
}
