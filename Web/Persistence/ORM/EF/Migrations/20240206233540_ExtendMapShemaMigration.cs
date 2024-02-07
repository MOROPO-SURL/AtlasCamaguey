using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MOROPO.CIMAC.Atlas.Camaguey.Web.Persistence.ORM.EF.Migrations
{
    /// <inheritdoc />
    public partial class ExtendMapShemaMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "imageid",
                table: "maps",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "ix_maps_imageid",
                table: "maps",
                column: "imageid");

            migrationBuilder.AddForeignKey(
                name: "fk_maps_images_imageid",
                table: "maps",
                column: "imageid",
                principalSchema: "guamaframework_media",
                principalTable: "images",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_maps_images_imageid",
                table: "maps");

            migrationBuilder.DropIndex(
                name: "ix_maps_imageid",
                table: "maps");

            migrationBuilder.DropColumn(
                name: "imageid",
                table: "maps");
        }
    }
}
