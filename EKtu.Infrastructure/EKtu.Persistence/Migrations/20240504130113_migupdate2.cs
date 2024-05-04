using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EKtu.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class migupdate2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Attendances_StudentChooseLessonId",
                table: "Attendances");

            migrationBuilder.CreateIndex(
                name: "IX_Attendances_StudentChooseLessonId",
                table: "Attendances",
                column: "StudentChooseLessonId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Attendances_StudentChooseLessonId",
                table: "Attendances");

            migrationBuilder.CreateIndex(
                name: "IX_Attendances_StudentChooseLessonId",
                table: "Attendances",
                column: "StudentChooseLessonId",
                unique: true);
        }
    }
}
