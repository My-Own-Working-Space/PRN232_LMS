using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PRN232.LMS.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class AddStudentCode : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "StudentCode",
                table: "Student",
                type: "varchar(20)",
                unicode: false,
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.Sql(@"
                UPDATE Student
                SET StudentCode = CASE 
                    WHEN Email LIKE '%@%' THEN UPPER(SUBSTRING(Email, 1, CHARINDEX('@', Email) - 1))
                    ELSE 'ST' + RIGHT('000000' + CAST(StudentId AS VARCHAR(6)), 6)
                END
            ");

            migrationBuilder.CreateIndex(
                name: "UQ_Student_Code",
                table: "Student",
                column: "StudentCode",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "UQ_Student_Code",
                table: "Student");

            migrationBuilder.DropColumn(
                name: "StudentCode",
                table: "Student");
        }
    }
}
