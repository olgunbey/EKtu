using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EKtu.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class MigStudentChooseLesson : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExamNote_StudentChooseLesson_StudentChooseLessonId",
                table: "ExamNote");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentChooseLesson_Lesson_LessonId",
                table: "StudentChooseLesson");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentChooseLesson_Student_StudentId",
                table: "StudentChooseLesson");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StudentChooseLesson",
                table: "StudentChooseLesson");

            migrationBuilder.RenameTable(
                name: "StudentChooseLesson",
                newName: "StudentChooseLessons");

            migrationBuilder.RenameIndex(
                name: "IX_StudentChooseLesson_StudentId",
                table: "StudentChooseLessons",
                newName: "IX_StudentChooseLessons_StudentId");

            migrationBuilder.RenameIndex(
                name: "IX_StudentChooseLesson_LessonId",
                table: "StudentChooseLessons",
                newName: "IX_StudentChooseLessons_LessonId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StudentChooseLessons",
                table: "StudentChooseLessons",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ExamNote_StudentChooseLessons_StudentChooseLessonId",
                table: "ExamNote",
                column: "StudentChooseLessonId",
                principalTable: "StudentChooseLessons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentChooseLessons_Lesson_LessonId",
                table: "StudentChooseLessons",
                column: "LessonId",
                principalTable: "Lesson",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentChooseLessons_Student_StudentId",
                table: "StudentChooseLessons",
                column: "StudentId",
                principalTable: "Student",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExamNote_StudentChooseLessons_StudentChooseLessonId",
                table: "ExamNote");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentChooseLessons_Lesson_LessonId",
                table: "StudentChooseLessons");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentChooseLessons_Student_StudentId",
                table: "StudentChooseLessons");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StudentChooseLessons",
                table: "StudentChooseLessons");

            migrationBuilder.RenameTable(
                name: "StudentChooseLessons",
                newName: "StudentChooseLesson");

            migrationBuilder.RenameIndex(
                name: "IX_StudentChooseLessons_StudentId",
                table: "StudentChooseLesson",
                newName: "IX_StudentChooseLesson_StudentId");

            migrationBuilder.RenameIndex(
                name: "IX_StudentChooseLessons_LessonId",
                table: "StudentChooseLesson",
                newName: "IX_StudentChooseLesson_LessonId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StudentChooseLesson",
                table: "StudentChooseLesson",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ExamNote_StudentChooseLesson_StudentChooseLessonId",
                table: "ExamNote",
                column: "StudentChooseLessonId",
                principalTable: "StudentChooseLesson",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentChooseLesson_Lesson_LessonId",
                table: "StudentChooseLesson",
                column: "LessonId",
                principalTable: "Lesson",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentChooseLesson_Student_StudentId",
                table: "StudentChooseLesson",
                column: "StudentId",
                principalTable: "Student",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
