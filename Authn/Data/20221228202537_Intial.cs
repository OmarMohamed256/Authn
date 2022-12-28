using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Authn.Data
{
    public partial class Intial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppUsers",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Provider = table.Column<string>(type: "TEXT", maxLength: 250, nullable: true),
                    NameIdentifier = table.Column<string>(type: "TEXT", maxLength: 250, nullable: true),
                    Username = table.Column<string>(type: "TEXT", maxLength: 250, nullable: true),
                    Password = table.Column<string>(type: "TEXT", maxLength: 250, nullable: true),
                    Email = table.Column<string>(type: "TEXT", maxLength: 250, nullable: true),
                    Firstname = table.Column<string>(type: "TEXT", maxLength: 250, nullable: true),
                    Lastname = table.Column<string>(type: "TEXT", maxLength: 250, nullable: true),
                    Mobile = table.Column<string>(type: "TEXT", maxLength: 250, nullable: true),
                    Roles = table.Column<string>(type: "TEXT", maxLength: 250, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUsers", x => x.UserId);
                });

            migrationBuilder.InsertData(
                table: "AppUsers",
                columns: new[] { "UserId", "Email", "Firstname", "Lastname", "Mobile", "NameIdentifier", "Password", "Provider", "Roles", "Username" },
                values: new object[] { 1, "omar@gmail.com", "omar", "mohamed", "0102598746", null, "pizza", "Cookies", "Admin", "omar@gmail.com" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppUsers");
        }
    }
}
