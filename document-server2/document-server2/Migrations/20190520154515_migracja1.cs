using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace document_server2.Migrations
{
    public partial class migracja1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Name = table.Column<string>(type: "nvarchar(20)", nullable: false),
                    Add_documents = table.Column<bool>(type: "bit", nullable: false),
                    Add_comments = table.Column<bool>(type: "bit", nullable: false),
                    Change_status = table.Column<bool>(type: "bit", nullable: false),
                    Add_users = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Name);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Email = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    Login = table.Column<string>(type: "nvarchar(20)", nullable: true),
                    Password = table.Column<string>(type: "nvarchar(20)", nullable: true),
                    Role_name = table.Column<string>(type: "nvarchar(20)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Email);
                    table.ForeignKey(
                        name: "FK_Users_Roles_Role_name",
                        column: x => x.Role_name,
                        principalTable: "Roles",
                        principalColumn: "Name",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Cases",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    User_email = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(60)", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime", nullable: false),
                    Description = table.Column<string>(type: "varchar(500)", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(MAX)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(20)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cases", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cases_Users_User_email",
                        column: x => x.User_email,
                        principalTable: "Users",
                        principalColumn: "Email",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Documents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Case_id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    Url = table.Column<string>(type: "nvarchar(MAX)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Documents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Documents_Cases_Case_id",
                        column: x => x.Case_id,
                        principalTable: "Cases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Recipients",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Case_id = table.Column<int>(type: "int", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(50)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Recipients", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Recipients_Cases_Case_id",
                        column: x => x.Case_id,
                        principalTable: "Cases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Name", "Add_comments", "Add_documents", "Add_users", "Change_status" },
                values: new object[,]
                {
                    { "admin", true, true, true, true },
                    { "unregistered", false, true, false, false },
                    { "registered", true, true, false, false },
                    { "dropped", true, false, false, false },
                    { "skarga", true, false, false, true },
                    { "podanie", true, false, true, true }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cases_User_email",
                table: "Cases",
                column: "User_email");

            migrationBuilder.CreateIndex(
                name: "IX_Documents_Case_id",
                table: "Documents",
                column: "Case_id");

            migrationBuilder.CreateIndex(
                name: "IX_Recipients_Case_id",
                table: "Recipients",
                column: "Case_id");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Role_name",
                table: "Users",
                column: "Role_name");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Documents");

            migrationBuilder.DropTable(
                name: "Recipients");

            migrationBuilder.DropTable(
                name: "Cases");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Roles");
        }
    }
}
