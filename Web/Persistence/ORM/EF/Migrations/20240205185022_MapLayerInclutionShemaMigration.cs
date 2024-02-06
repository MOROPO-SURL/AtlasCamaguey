using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MOROPO.CIMAC.Atlas.Camaguey.Web.Persistence.ORM.EF.Migrations
{
    /// <inheritdoc />
    public partial class MapLayerInclutionShemaMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "crs",
                table: "maps",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "eastboundlatitude",
                table: "maps",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "epsg",
                table: "maps",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "northboundlatitude",
                table: "maps",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "southboundlatitude",
                table: "maps",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "westboundlongitude",
                table: "maps",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateTable(
                name: "layer",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    title = table.Column<string>(type: "text", nullable: false),
                    stylepresentantionjsblock = table.Column<string>(type: "text", nullable: true),
                    interactivepresentantionjsblock = table.Column<string>(type: "text", nullable: true),
                    crs = table.Column<int>(type: "integer", nullable: false),
                    epsg = table.Column<int>(type: "integer", nullable: false),
                    westboundlongitude = table.Column<decimal>(type: "numeric", nullable: false),
                    eastboundlatitude = table.Column<decimal>(type: "numeric", nullable: false),
                    southboundlatitude = table.Column<decimal>(type: "numeric", nullable: false),
                    northboundlatitude = table.Column<decimal>(type: "numeric", nullable: false),
                    mapid = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_layer", x => x.id);
                    table.ForeignKey(
                        name: "fk_layer_maps_mapid",
                        column: x => x.mapid,
                        principalTable: "maps",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_layer_mapid",
                table: "layer",
                column: "mapid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "layer");

            migrationBuilder.DropColumn(
                name: "crs",
                table: "maps");

            migrationBuilder.DropColumn(
                name: "eastboundlatitude",
                table: "maps");

            migrationBuilder.DropColumn(
                name: "epsg",
                table: "maps");

            migrationBuilder.DropColumn(
                name: "northboundlatitude",
                table: "maps");

            migrationBuilder.DropColumn(
                name: "southboundlatitude",
                table: "maps");

            migrationBuilder.DropColumn(
                name: "westboundlongitude",
                table: "maps");
        }
    }
}
