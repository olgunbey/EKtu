using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EKtu.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class migrationLessonConfirmationTableAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExamNote_StudentChooseLessons_StudentChooseLessonId",
                table: "ExamNote");

            migrationBuilder.RenameColumn(
                name: "StudentChooseLessonId",
                table: "ExamNote",
                newName: "LessonConfirmationId");

            migrationBuilder.RenameIndex(
                name: "IX_ExamNote_StudentChooseLessonId",
                table: "ExamNote",
                newName: "IX_ExamNote_LessonConfirmationId");

            migrationBuilder.CreateTable(
                name: "LessonConfirmation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentId = table.Column<int>(type: "int", nullable: false),
                    LessonId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LessonConfirmation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LessonConfirmation_Lesson_LessonId",
                        column: x => x.LessonId,
                        principalTable: "Lesson",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LessonConfirmation_Student_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Student",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LessonConfirmation_LessonId",
                table: "LessonConfirmation",
                column: "LessonId");

            migrationBuilder.CreateIndex(
                name: "IX_LessonConfirmation_StudentId",
                table: "LessonConfirmation",
                column: "StudentId");

            migrationBuilder.AddForeignKey(
                name: "FK_ExamNote_LessonConfirmation_LessonConfirmationId",
                table: "ExamNote",
                column: "LessonConfirmationId",
                principalTable: "LessonConfirmation",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExamNote_LessonConfirmation_LessonConfirmationId",
                table: "ExamNote");

            migrationBuilder.DropTable(
                name: "LessonConfirmation");

            migrationBuilder.RenameColumn(
                name: "LessonConfirmationId",
                table: "ExamNote",
                newName: "StudentChooseLessonId");

            migrationBuilder.RenameIndex(
                name: "IX_ExamNote_LessonConfirmationId",
                table: "ExamNote",
                newName: "IX_ExamNote_StudentChooseLessonId");

            migrationBuilder.AddForeignKey(
                name: "FK_ExamNote_StudentChooseLessons_StudentChooseLessonId",
                table: "ExamNote",
                column: "StudentChooseLessonId",
                principalTable: "StudentChooseLessons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
