using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ResourceManageGroup.Migrations
{
    /// <inheritdoc />
    public partial class REsourceMan : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.CreateSequence<int>(
                name: "EmployeeSequence",
                schema: "dbo");

            migrationBuilder.CreateSequence<int>(
                name: "ManagerSequence",
                schema: "dbo");

            migrationBuilder.CreateSequence<int>(
                name: "RecruiterSequence",
                schema: "dbo");

            migrationBuilder.CreateTable(
                name: "Employee_Details",
                columns: table => new
                {
                    EmployeeId = table.Column<string>(type: "nvarchar(450)", nullable: false, defaultValueSql: "CONCAT('23EM', RIGHT('00' + CAST(NEXT VALUE FOR EmployeeSequence AS VARCHAR(2)), 2))"),
                    EmployeeName = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    EmployeeEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmployeeNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmployeePassword = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmployeeConfirmPassword = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EmployeeVacationStartTime = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmployeeVacationEndTime = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmployeeVacationReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmployeeImage = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    EmployeeWorkingStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmployeeVacationStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmployeeTechnology = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmployeeTrainerName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmployeeTrainingStartTime = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmployeeTrainingEndTime = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmployeeProject = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employee_Details", x => x.EmployeeId);
                });

            migrationBuilder.CreateTable(
                name: "Manager_Details",
                columns: table => new
                {
                    ManagerId = table.Column<string>(type: "nvarchar(450)", nullable: false, defaultValueSql: "CONCAT('23PM', RIGHT('00' + CAST(NEXT VALUE FOR ManagerSequence AS VARCHAR(2)), 2))"),
                    ManagerName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ManagerEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ManagerNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ManagerPassword = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Manager_Details", x => x.ManagerId);
                });

            migrationBuilder.CreateTable(
                name: "Project_Details",
                columns: table => new
                {
                    project_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    project_name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    project_description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    project_start_time = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    project_end_time = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    project_lead = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    project_type = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Project_Details", x => x.project_id);
                });

            migrationBuilder.CreateTable(
                name: "Recruiter_Details",
                columns: table => new
                {
                    RecruiterId = table.Column<string>(type: "nvarchar(450)", nullable: false, defaultValueSql: "CONCAT('23HR', RIGHT('00' + CAST(NEXT VALUE FOR RecruiterSequence AS VARCHAR(2)), 2))"),
                    RecruiterName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RecruiterEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RecruiterNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RecruiterPassword = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Recruiter_Details", x => x.RecruiterId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Employee_Details");

            migrationBuilder.DropTable(
                name: "Manager_Details");

            migrationBuilder.DropTable(
                name: "Project_Details");

            migrationBuilder.DropTable(
                name: "Recruiter_Details");

            migrationBuilder.DropSequence(
                name: "EmployeeSequence",
                schema: "dbo");

            migrationBuilder.DropSequence(
                name: "ManagerSequence",
                schema: "dbo");

            migrationBuilder.DropSequence(
                name: "RecruiterSequence",
                schema: "dbo");
        }
    }
}
