using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CAEVSYNC.API.Data.Migrations
{
    /// <inheritdoc />
    public partial class AccountStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CalendarToEventConnections_Calendars_CalendarId",
                table: "CalendarToEventConnections");

            migrationBuilder.DropForeignKey(
                name: "FK_CalendarToEventConnections_Events_EventId",
                table: "CalendarToEventConnections");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Events",
                table: "Events");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CalendarToEventConnections",
                table: "CalendarToEventConnections");

            migrationBuilder.RenameTable(
                name: "Events",
                newName: "Event");

            migrationBuilder.RenameTable(
                name: "CalendarToEventConnections",
                newName: "CalendarToEventConnection");

            migrationBuilder.RenameIndex(
                name: "IX_CalendarToEventConnections_EventId",
                table: "CalendarToEventConnection",
                newName: "IX_CalendarToEventConnection_EventId");

            migrationBuilder.AddColumn<int>(
                name: "AccountStatus",
                table: "ConnectedAccounts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Event",
                table: "Event",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CalendarToEventConnection",
                table: "CalendarToEventConnection",
                columns: new[] { "CalendarId", "EventId" });

            migrationBuilder.AddForeignKey(
                name: "FK_CalendarToEventConnection_Calendars_CalendarId",
                table: "CalendarToEventConnection",
                column: "CalendarId",
                principalTable: "Calendars",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CalendarToEventConnection_Event_EventId",
                table: "CalendarToEventConnection",
                column: "EventId",
                principalTable: "Event",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CalendarToEventConnection_Calendars_CalendarId",
                table: "CalendarToEventConnection");

            migrationBuilder.DropForeignKey(
                name: "FK_CalendarToEventConnection_Event_EventId",
                table: "CalendarToEventConnection");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Event",
                table: "Event");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CalendarToEventConnection",
                table: "CalendarToEventConnection");

            migrationBuilder.DropColumn(
                name: "AccountStatus",
                table: "ConnectedAccounts");

            migrationBuilder.RenameTable(
                name: "Event",
                newName: "Events");

            migrationBuilder.RenameTable(
                name: "CalendarToEventConnection",
                newName: "CalendarToEventConnections");

            migrationBuilder.RenameIndex(
                name: "IX_CalendarToEventConnection_EventId",
                table: "CalendarToEventConnections",
                newName: "IX_CalendarToEventConnections_EventId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Events",
                table: "Events",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CalendarToEventConnections",
                table: "CalendarToEventConnections",
                columns: new[] { "CalendarId", "EventId" });

            migrationBuilder.AddForeignKey(
                name: "FK_CalendarToEventConnections_Calendars_CalendarId",
                table: "CalendarToEventConnections",
                column: "CalendarId",
                principalTable: "Calendars",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CalendarToEventConnections_Events_EventId",
                table: "CalendarToEventConnections",
                column: "EventId",
                principalTable: "Events",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
