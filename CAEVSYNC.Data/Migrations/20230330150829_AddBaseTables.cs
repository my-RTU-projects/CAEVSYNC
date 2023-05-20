using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CAEVSYNC.API.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddBaseTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Calendars",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Calendars", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ConnectedAccounts",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    AccountType = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConnectedAccounts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Events",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FromDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ToDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsReminderOn = table.Column<bool>(type: "bit", nullable: false),
                    ReminderMinutesBeforeStart = table.Column<int>(type: "int", nullable: false),
                    IsAllDay = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SyncedEventData",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OriginCalendarId = table.Column<string>(type: "nvarchar(200)", nullable: false),
                    EventIdInOriginCalendar = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TargetCalendarId = table.Column<string>(type: "nvarchar(200)", nullable: false),
                    EventIdInTargetCalendarId = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SyncedEventData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SyncedEventData_Calendars_OriginCalendarId",
                        column: x => x.OriginCalendarId,
                        principalTable: "Calendars",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SyncedEventData_Calendars_TargetCalendarId",
                        column: x => x.TargetCalendarId,
                        principalTable: "Calendars",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AccountToCalendarConnections",
                columns: table => new
                {
                    AccountId = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    CalendarId = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ReadOnly = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountToCalendarConnections", x => new { x.AccountId, x.CalendarId });
                    table.ForeignKey(
                        name: "FK_AccountToCalendarConnections_Calendars_CalendarId",
                        column: x => x.CalendarId,
                        principalTable: "Calendars",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AccountToCalendarConnections_ConnectedAccounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "ConnectedAccounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserToAccountConnections",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ConnectedAccountId = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserToAccountConnections", x => new { x.UserId, x.ConnectedAccountId });
                    table.ForeignKey(
                        name: "FK_UserToAccountConnections_ConnectedAccounts_ConnectedAccountId",
                        column: x => x.ConnectedAccountId,
                        principalTable: "ConnectedAccounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CalendarToEventConnections",
                columns: table => new
                {
                    CalendarId = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    EventId = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CalendarToEventConnections", x => new { x.CalendarId, x.EventId });
                    table.ForeignKey(
                        name: "FK_CalendarToEventConnections_Calendars_CalendarId",
                        column: x => x.CalendarId,
                        principalTable: "Calendars",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CalendarToEventConnections_Events_EventId",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AccountToCalendarConnections_CalendarId",
                table: "AccountToCalendarConnections",
                column: "CalendarId");

            migrationBuilder.CreateIndex(
                name: "IX_CalendarToEventConnections_EventId",
                table: "CalendarToEventConnections",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_SyncedEventData_OriginCalendarId",
                table: "SyncedEventData",
                column: "OriginCalendarId");

            migrationBuilder.CreateIndex(
                name: "IX_SyncedEventData_TargetCalendarId",
                table: "SyncedEventData",
                column: "TargetCalendarId");

            migrationBuilder.CreateIndex(
                name: "IX_UserToAccountConnections_ConnectedAccountId",
                table: "UserToAccountConnections",
                column: "ConnectedAccountId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccountToCalendarConnections");

            migrationBuilder.DropTable(
                name: "CalendarToEventConnections");

            migrationBuilder.DropTable(
                name: "SyncedEventData");

            migrationBuilder.DropTable(
                name: "UserToAccountConnections");

            migrationBuilder.DropTable(
                name: "Events");

            migrationBuilder.DropTable(
                name: "Calendars");

            migrationBuilder.DropTable(
                name: "ConnectedAccounts");
        }
    }
}
