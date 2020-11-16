using Microsoft.EntityFrameworkCore.Migrations;

namespace Ticket_Manager.Migrations
{
    public partial class AddedProjectIdToTicket : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProjectID",
                table: "Ticket",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProjectID",
                table: "Ticket");
        }
    }
}
