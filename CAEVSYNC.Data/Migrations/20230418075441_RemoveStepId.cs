using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CAEVSYNC.API.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemoveStepId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_EventTransformationSteps",
                table: "EventTransformationSteps");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "EventTransformationSteps");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EventTransformationSteps",
                table: "EventTransformationSteps",
                columns: new[] { "PropertyName", "PropertyType" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_EventTransformationSteps",
                table: "EventTransformationSteps");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "EventTransformationSteps",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EventTransformationSteps",
                table: "EventTransformationSteps",
                column: "Id");
        }
    }
}
