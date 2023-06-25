using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RMGapi.Migrations
{
    /// <inheritdoc />
    public partial class RMGAPiiii : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Sample_Details",
                columns: table => new
                {
                    EmployeeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmployeeSalary = table.Column<double>(type: "float", nullable: false),
                    EmployeeAge = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sample_Details", x => x.EmployeeId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Sample_Details");
        }
    }
}
