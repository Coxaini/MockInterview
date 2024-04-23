using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MockInterview.Interviews.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddEndTimeColumnToInterview : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "EndTime",
                schema: "interviews",
                table: "Interviews",
                type: "timestamp with time zone",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndTime",
                schema: "interviews",
                table: "Interviews");
        }
    }
}
