using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MindfulJournal.Migrations
{
    /// <inheritdoc />
    public partial class AddUserNameToJournalEntries : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "JournalEntries",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserName",
                table: "JournalEntries");
        }
    }
}
