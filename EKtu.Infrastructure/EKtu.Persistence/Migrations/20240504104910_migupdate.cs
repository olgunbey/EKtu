using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EKtu.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class migupdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Attendances_StudentChooseLessonId",
                table: "Attendances");

            migrationBuilder.AddColumn<int>(
                name: "ClassId",
                table: "Attendances",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Attendances_ClassId",
                table: "Attendances",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_Attendances_StudentChooseLessonId",
                table: "Attendances",
                column: "StudentChooseLessonId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Attendances_Class_ClassId",
                table: "Attendances",
                column: "ClassId",
                principalTable: "Class",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attendances_Class_ClassId",
                table: "Attendances");

            migrationBuilder.DropIndex(
                name: "IX_Attendances_ClassId",
                table: "Attendances");

            migrationBuilder.DropIndex(
                name: "IX_Attendances_StudentChooseLessonId",
                table: "Attendances");

            migrationBuilder.DropColumn(
                name: "ClassId",
                table: "Attendances");

            migrationBuilder.CreateIndex(
                name: "IX_Attendances_StudentChooseLessonId",
                table: "Attendances",
                column: "StudentChooseLessonId");
        }
    }
}
