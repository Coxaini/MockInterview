using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MockInterview.Interviews.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "interviews");

            migrationBuilder.CreateTable(
                name: "Interviews",
                schema: "interviews",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    StartTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ProgrammingLanguage = table.Column<string>(type: "text", nullable: false),
                    Tags = table.Column<string[]>(type: "text[]", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Interviews", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                schema: "interviews",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Username = table.Column<string>(type: "text", nullable: false),
                    AvatarUrl = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "InterviewOrders",
                schema: "interviews",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CandidateId = table.Column<Guid>(type: "uuid", nullable: false),
                    StartDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ProgrammingLanguage = table.Column<string>(type: "text", nullable: false),
                    Technologies = table.Column<string[]>(type: "text[]", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InterviewOrders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InterviewOrders_Users_CandidateId",
                        column: x => x.CandidateId,
                        principalSchema: "interviews",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InterviewMembers",
                schema: "interviews",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    InterviewId = table.Column<Guid>(type: "uuid", nullable: false),
                    InterviewOrderId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InterviewMembers", x => new { x.UserId, x.InterviewId });
                    table.ForeignKey(
                        name: "FK_InterviewMembers_InterviewOrders_InterviewOrderId",
                        column: x => x.InterviewOrderId,
                        principalSchema: "interviews",
                        principalTable: "InterviewOrders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_InterviewMembers_Interviews_InterviewId",
                        column: x => x.InterviewId,
                        principalSchema: "interviews",
                        principalTable: "Interviews",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InterviewMembers_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "interviews",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InterviewQuestionsLists",
                schema: "interviews",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    InterviewId = table.Column<Guid>(type: "uuid", nullable: true),
                    AuthorId = table.Column<Guid>(type: "uuid", nullable: false),
                    InterviewOrderId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InterviewQuestionsLists", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InterviewQuestionsLists_InterviewOrders_InterviewOrderId",
                        column: x => x.InterviewOrderId,
                        principalSchema: "interviews",
                        principalTable: "InterviewOrders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_InterviewQuestionsLists_Interviews_InterviewId",
                        column: x => x.InterviewId,
                        principalSchema: "interviews",
                        principalTable: "Interviews",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_InterviewQuestionsLists_Users_AuthorId",
                        column: x => x.AuthorId,
                        principalSchema: "interviews",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InterviewQuestions",
                schema: "interviews",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    InterviewQuestionsListId = table.Column<Guid>(type: "uuid", nullable: false),
                    OrderIndex = table.Column<int>(type: "integer", nullable: false),
                    Rating = table.Column<int>(type: "integer", nullable: true),
                    Feedback = table.Column<string>(type: "text", nullable: true),
                    Text = table.Column<string>(type: "text", nullable: false),
                    DifficultyLevel = table.Column<int>(type: "integer", nullable: false),
                    Tag = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InterviewQuestions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InterviewQuestions_InterviewQuestionsLists_InterviewQuestio~",
                        column: x => x.InterviewQuestionsListId,
                        principalSchema: "interviews",
                        principalTable: "InterviewQuestionsLists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InterviewMembers_InterviewId",
                schema: "interviews",
                table: "InterviewMembers",
                column: "InterviewId");

            migrationBuilder.CreateIndex(
                name: "IX_InterviewMembers_InterviewOrderId",
                schema: "interviews",
                table: "InterviewMembers",
                column: "InterviewOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_InterviewOrders_CandidateId",
                schema: "interviews",
                table: "InterviewOrders",
                column: "CandidateId");

            migrationBuilder.CreateIndex(
                name: "IX_InterviewQuestions_InterviewQuestionsListId",
                schema: "interviews",
                table: "InterviewQuestions",
                column: "InterviewQuestionsListId");

            migrationBuilder.CreateIndex(
                name: "IX_InterviewQuestionsLists_AuthorId",
                schema: "interviews",
                table: "InterviewQuestionsLists",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_InterviewQuestionsLists_InterviewId",
                schema: "interviews",
                table: "InterviewQuestionsLists",
                column: "InterviewId");

            migrationBuilder.CreateIndex(
                name: "IX_InterviewQuestionsLists_InterviewOrderId",
                schema: "interviews",
                table: "InterviewQuestionsLists",
                column: "InterviewOrderId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InterviewMembers",
                schema: "interviews");

            migrationBuilder.DropTable(
                name: "InterviewQuestions",
                schema: "interviews");

            migrationBuilder.DropTable(
                name: "InterviewQuestionsLists",
                schema: "interviews");

            migrationBuilder.DropTable(
                name: "InterviewOrders",
                schema: "interviews");

            migrationBuilder.DropTable(
                name: "Interviews",
                schema: "interviews");

            migrationBuilder.DropTable(
                name: "Users",
                schema: "interviews");
        }
    }
}
