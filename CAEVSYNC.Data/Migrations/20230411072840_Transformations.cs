using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CAEVSYNC.API.Data.Migrations
{
    /// <inheritdoc />
    public partial class Transformations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EventTransformationStep",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SyncRuleId = table.Column<int>(type: "int", nullable: false),
                    PropertyName = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    PropertyType = table.Column<int>(type: "int", nullable: false),
                    TransformationType = table.Column<int>(type: "int", nullable: false),
                    IntReplacement = table.Column<int>(type: "int", nullable: true),
                    StringReplacement = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    BoolReplacement = table.Column<bool>(type: "bit", nullable: true),
                    IntFilter = table.Column<int>(type: "int", nullable: true),
                    StringFilter = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    BoolFilter = table.Column<bool>(type: "bit", nullable: true),
                    ExtraMinutesBefore = table.Column<int>(type: "int", nullable: true),
                    ExtraMinutesAfter = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventTransformationStep", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EventTransformationStep_SyncRules_SyncRuleId",
                        column: x => x.SyncRuleId,
                        principalTable: "SyncRules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EventTransformationStep_SyncRuleId",
                table: "EventTransformationStep",
                column: "SyncRuleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EventTransformationStep");
        }
    }
}
