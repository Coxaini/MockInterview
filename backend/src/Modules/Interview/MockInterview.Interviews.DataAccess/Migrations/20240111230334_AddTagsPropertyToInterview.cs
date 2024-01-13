using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MockInterview.Interviews.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddTagsPropertyToInterview : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string[]>(
                name: "Tags",
                schema: "interviews",
                table: "Interviews",
                type: "text[]",
                nullable: false,
                defaultValue: new string[0]);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Tags",
                schema: "interviews",
                table: "Interviews");
        }
    }
}
