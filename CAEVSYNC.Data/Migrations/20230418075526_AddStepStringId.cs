using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CAEVSYNC.API.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddStepStringId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_EventTransformationSteps",
                table: "EventTransformationSteps");

            migrationBuilder.AddColumn<string>(
                name: "Id",
                table: "EventTransformationSteps",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EventTransformationSteps",
                table: "EventTransformationSteps",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
    }
}
