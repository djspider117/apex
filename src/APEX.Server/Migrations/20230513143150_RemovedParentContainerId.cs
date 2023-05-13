using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace APEX.Server.Migrations
{
    /// <inheritdoc />
    public partial class RemovedParentContainerId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Files_FileContainers_ParentContainerId",
                table: "Files");

            migrationBuilder.RenameColumn(
                name: "ParentContainerId",
                table: "Files",
                newName: "FileContainerId");

            migrationBuilder.RenameIndex(
                name: "IX_Files_ParentContainerId",
                table: "Files",
                newName: "IX_Files_FileContainerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Files_FileContainers_FileContainerId",
                table: "Files",
                column: "FileContainerId",
                principalTable: "FileContainers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Files_FileContainers_FileContainerId",
                table: "Files");

            migrationBuilder.RenameColumn(
                name: "FileContainerId",
                table: "Files",
                newName: "ParentContainerId");

            migrationBuilder.RenameIndex(
                name: "IX_Files_FileContainerId",
                table: "Files",
                newName: "IX_Files_ParentContainerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Files_FileContainers_ParentContainerId",
                table: "Files",
                column: "ParentContainerId",
                principalTable: "FileContainers",
                principalColumn: "Id");
        }
    }
}
