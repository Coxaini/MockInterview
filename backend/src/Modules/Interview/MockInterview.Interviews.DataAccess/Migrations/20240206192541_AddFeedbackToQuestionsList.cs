using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MockInterview.Interviews.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddFeedbackToQuestionsList : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_InterviewQuestionsLists_InterviewOrderId",
                schema: "interviews",
                table: "InterviewQuestionsLists");

            migrationBuilder.AddColumn<bool>(
                name: "IsFeedbackSent",
                schema: "interviews",
                table: "InterviewQuestionsLists",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_InterviewQuestionsLists_InterviewOrderId",
                schema: "interviews",
                table: "InterviewQuestionsLists",
                column: "InterviewOrderId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_InterviewQuestionsLists_InterviewOrderId",
                schema: "interviews",
                table: "InterviewQuestionsLists");

            migrationBuilder.DropColumn(
                name: "IsFeedbackSent",
                schema: "interviews",
                table: "InterviewQuestionsLists");

            migrationBuilder.CreateIndex(
                name: "IX_InterviewQuestionsLists_InterviewOrderId",
                schema: "interviews",
                table: "InterviewQuestionsLists",
                column: "InterviewOrderId");
        }
    }
}
