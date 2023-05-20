using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CAEVSYNC.API.Data.Migrations
{
    /// <inheritdoc />
    public partial class Transformations2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EventTransformationStep_SyncRules_SyncRuleId",
                table: "EventTransformationStep");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EventTransformationStep",
                table: "EventTransformationStep");

            migrationBuilder.RenameTable(
                name: "EventTransformationStep",
                newName: "EventTransformationSteps");

            migrationBuilder.RenameIndex(
                name: "IX_EventTransformationStep_SyncRuleId",
                table: "EventTransformationSteps",
                newName: "IX_EventTransformationSteps_SyncRuleId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EventTransformationSteps",
                table: "EventTransformationSteps",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_EventTransformationSteps_SyncRules_SyncRuleId",
                table: "EventTransformationSteps",
                column: "SyncRuleId",
                principalTable: "SyncRules",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EventTransformationSteps_SyncRules_SyncRuleId",
                table: "EventTransformationSteps");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EventTransformationSteps",
                table: "EventTransformationSteps");

            migrationBuilder.RenameTable(
                name: "EventTransformationSteps",
                newName: "EventTransformationStep");

            migrationBuilder.RenameIndex(
                name: "IX_EventTransformationSteps_SyncRuleId",
                table: "EventTransformationStep",
                newName: "IX_EventTransformationStep_SyncRuleId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EventTransformationStep",
                table: "EventTransformationStep",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_EventTransformationStep_SyncRules_SyncRuleId",
                table: "EventTransformationStep",
                column: "SyncRuleId",
                principalTable: "SyncRules",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
