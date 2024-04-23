using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MockInterview.Interviews.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddQuestionsListFeedbackColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Feedback",
                schema: "interviews",
                table: "InterviewQuestionsLists",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Feedback",
                schema: "interviews",
                table: "InterviewQuestionsLists");
        }
    }
}
