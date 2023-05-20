using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CAEVSYNC.API.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddSyncRules : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SyncedEventData_Calendars_OriginCalendarId",
                table: "SyncedEventData");

            migrationBuilder.DropForeignKey(
                name: "FK_SyncedEventData_Calendars_TargetCalendarId",
                table: "SyncedEventData");

            migrationBuilder.DropIndex(
                name: "IX_SyncedEventData_OriginCalendarId",
                table: "SyncedEventData");

            migrationBuilder.DropIndex(
                name: "IX_SyncedEventData_TargetCalendarId",
                table: "SyncedEventData");

            migrationBuilder.DropColumn(
                name: "OriginCalendarId",
                table: "SyncedEventData");

            migrationBuilder.DropColumn(
                name: "TargetCalendarId",
                table: "SyncedEventData");

            migrationBuilder.AddColumn<int>(
                name: "SyncRuleId",
                table: "SyncedEventData",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "SyncRules",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", maxLength: 200, nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OriginCalendarId = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    TargetCalendarId = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SyncRules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SyncRules_Calendars_OriginCalendarId",
                        column: x => x.OriginCalendarId,
                        principalTable: "Calendars",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SyncRules_Calendars_TargetCalendarId",
                        column: x => x.TargetCalendarId,
                        principalTable: "Calendars",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SyncedEventData_SyncRuleId",
                table: "SyncedEventData",
                column: "SyncRuleId");

            migrationBuilder.CreateIndex(
                name: "IX_SyncRules_OriginCalendarId",
                table: "SyncRules",
                column: "OriginCalendarId");

            migrationBuilder.CreateIndex(
                name: "IX_SyncRules_TargetCalendarId",
                table: "SyncRules",
                column: "TargetCalendarId");

            migrationBuilder.AddForeignKey(
                name: "FK_SyncedEventData_SyncRules_SyncRuleId",
                table: "SyncedEventData",
                column: "SyncRuleId",
                principalTable: "SyncRules",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SyncedEventData_SyncRules_SyncRuleId",
                table: "SyncedEventData");

            migrationBuilder.DropTable(
                name: "SyncRules");

            migrationBuilder.DropIndex(
                name: "IX_SyncedEventData_SyncRuleId",
                table: "SyncedEventData");

            migrationBuilder.DropColumn(
                name: "SyncRuleId",
                table: "SyncedEventData");

            migrationBuilder.AddColumn<string>(
                name: "OriginCalendarId",
                table: "SyncedEventData",
                type: "nvarchar(200)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TargetCalendarId",
                table: "SyncedEventData",
                type: "nvarchar(200)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_SyncedEventData_OriginCalendarId",
                table: "SyncedEventData",
                column: "OriginCalendarId");

            migrationBuilder.CreateIndex(
                name: "IX_SyncedEventData_TargetCalendarId",
                table: "SyncedEventData",
                column: "TargetCalendarId");

            migrationBuilder.AddForeignKey(
                name: "FK_SyncedEventData_Calendars_OriginCalendarId",
                table: "SyncedEventData",
                column: "OriginCalendarId",
                principalTable: "Calendars",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SyncedEventData_Calendars_TargetCalendarId",
                table: "SyncedEventData",
                column: "TargetCalendarId",
                principalTable: "Calendars",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
