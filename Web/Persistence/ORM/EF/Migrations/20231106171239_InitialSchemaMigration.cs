using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace MOROPO.CIMAC.Atlas.Camaguey.Web.Persistence.ORM.EF.Migrations
{
    /// <inheritdoc />
    public partial class InitialSchemaMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "guamaframework_location");

            migrationBuilder.EnsureSchema(
                name: "guamaframework_media");

            migrationBuilder.EnsureSchema(
                name: "guamaframework_identity");

            migrationBuilder.EnsureSchema(
                name: "guamaframework_core_security_common");

            migrationBuilder.EnsureSchema(
                name: "guamaframework_security_identity");

            migrationBuilder.CreateTable(
                name: "countries",
                schema: "guamaframework_location",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    isocode = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_countries", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "emailcontactitems",
                schema: "guamaframework_identity",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    emailaddress = table.Column<string>(type: "text", nullable: false),
                    label = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_emailcontactitems", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "keywords",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_keywords", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "maps",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    priority = table.Column<int>(type: "integer", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    scale = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_maps", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "permissions",
                schema: "guamaframework_core_security_common",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    code = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    family = table.Column<string>(type: "text", nullable: true),
                    description = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_permissions", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "phonecontactitems",
                schema: "guamaframework_identity",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    phonenumber = table.Column<string>(type: "text", nullable: false),
                    label = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_phonecontactitems", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "roles",
                schema: "guamaframework_core_security_common",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_roles", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "socialcontactitems",
                schema: "guamaframework_identity",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    socialid = table.Column<string>(type: "text", nullable: false),
                    label = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_socialcontactitems", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "thumbnails",
                schema: "guamaframework_media",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    contenttype = table.Column<string>(type: "text", nullable: false),
                    length = table.Column<long>(type: "bigint", nullable: false),
                    about = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_thumbnails", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "traces",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    type = table.Column<int>(type: "integer", nullable: false),
                    author = table.Column<string>(type: "text", nullable: true),
                    registered = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    entityname = table.Column<string>(type: "text", nullable: false),
                    beforedata = table.Column<string>(type: "text", nullable: false),
                    afterdata = table.Column<string>(type: "text", nullable: true),
                    nextid = table.Column<long>(type: "bigint", nullable: false),
                    signature = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_traces", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "websitecontactitems",
                schema: "guamaframework_identity",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    url = table.Column<string>(type: "text", nullable: false),
                    label = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_websitecontactitems", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "regions",
                schema: "guamaframework_location",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    countryid = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_regions", x => x.id);
                    table.ForeignKey(
                        name: "fk_regions_countries_countryid",
                        column: x => x.countryid,
                        principalSchema: "guamaframework_location",
                        principalTable: "countries",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "mapkeywordbridges",
                columns: table => new
                {
                    prowid = table.Column<string>(type: "text", nullable: false),
                    sternid = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_mapkeywordbridges", x => new { x.prowid, x.sternid });
                    table.ForeignKey(
                        name: "fk_mapkeywordbridges_keywords_sternid",
                        column: x => x.sternid,
                        principalTable: "keywords",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_mapkeywordbridges_maps_prowid",
                        column: x => x.prowid,
                        principalTable: "maps",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "rolepermissionbridges",
                schema: "guamaframework_core_security_common",
                columns: table => new
                {
                    prowid = table.Column<Guid>(type: "uuid", nullable: false),
                    sternid = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_rolepermissionbridges", x => new { x.prowid, x.sternid });
                    table.ForeignKey(
                        name: "fk_rolepermissionbridges_permissions_sternid",
                        column: x => x.sternid,
                        principalSchema: "guamaframework_core_security_common",
                        principalTable: "permissions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_rolepermissionbridges_roles_prowid",
                        column: x => x.prowid,
                        principalSchema: "guamaframework_core_security_common",
                        principalTable: "roles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "documents",
                schema: "guamaframework_media",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    contenttype = table.Column<string>(type: "text", nullable: false),
                    length = table.Column<long>(type: "bigint", nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    thumbnailid = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_documents", x => x.id);
                    table.ForeignKey(
                        name: "fk_documents_thumbnails_thumbnailid",
                        column: x => x.thumbnailid,
                        principalSchema: "guamaframework_media",
                        principalTable: "thumbnails",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "images",
                schema: "guamaframework_media",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    createat = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    contenttype = table.Column<string>(type: "text", nullable: false),
                    length = table.Column<long>(type: "bigint", nullable: false),
                    about = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    height = table.Column<int>(type: "integer", nullable: false),
                    width = table.Column<int>(type: "integer", nullable: false),
                    thumbnailid = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_images", x => x.id);
                    table.ForeignKey(
                        name: "fk_images_thumbnails_thumbnailid",
                        column: x => x.thumbnailid,
                        principalSchema: "guamaframework_media",
                        principalTable: "thumbnails",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "videos",
                schema: "guamaframework_media",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    createat = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    extension = table.Column<string>(type: "text", nullable: false),
                    length = table.Column<long>(type: "bigint", nullable: false),
                    about = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    duration = table.Column<TimeSpan>(type: "interval", nullable: false),
                    format = table.Column<string>(type: "text", nullable: false),
                    framesize = table.Column<string>(type: "text", nullable: false),
                    colormodel = table.Column<string>(type: "text", nullable: false),
                    fps = table.Column<double>(type: "double precision", nullable: false),
                    audioformat = table.Column<string>(type: "text", nullable: false),
                    audiochanneloutput = table.Column<string>(type: "text", nullable: false),
                    thumbnailid = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_videos", x => x.id);
                    table.ForeignKey(
                        name: "fk_videos_thumbnails_thumbnailid",
                        column: x => x.thumbnailid,
                        principalSchema: "guamaframework_media",
                        principalTable: "thumbnails",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "states",
                schema: "guamaframework_location",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    regionid = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_states", x => x.id);
                    table.ForeignKey(
                        name: "fk_states_regions_regionid",
                        column: x => x.regionid,
                        principalSchema: "guamaframework_location",
                        principalTable: "regions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "mapdocumentbridges",
                columns: table => new
                {
                    prowid = table.Column<string>(type: "text", nullable: false),
                    sternid = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_mapdocumentbridges", x => new { x.prowid, x.sternid });
                    table.ForeignKey(
                        name: "fk_mapdocumentbridges_documents_sternid",
                        column: x => x.sternid,
                        principalSchema: "guamaframework_media",
                        principalTable: "documents",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_mapdocumentbridges_maps_prowid",
                        column: x => x.prowid,
                        principalTable: "maps",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "mapimagebridges",
                columns: table => new
                {
                    prowid = table.Column<string>(type: "text", nullable: false),
                    sternid = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_mapimagebridges", x => new { x.prowid, x.sternid });
                    table.ForeignKey(
                        name: "fk_mapimagebridges_images_sternid",
                        column: x => x.sternid,
                        principalSchema: "guamaframework_media",
                        principalTable: "images",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_mapimagebridges_maps_prowid",
                        column: x => x.prowid,
                        principalTable: "maps",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "organizationidentities",
                schema: "guamaframework_identity",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    ownerid = table.Column<Guid>(type: "uuid", nullable: true),
                    imageid = table.Column<Guid>(type: "uuid", nullable: true),
                    name = table.Column<string>(type: "text", nullable: false),
                    borndate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    about = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_organizationidentities", x => x.id);
                    table.ForeignKey(
                        name: "fk_organizationidentities_images_imageid",
                        column: x => x.imageid,
                        principalSchema: "guamaframework_media",
                        principalTable: "images",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_organizationidentities_organizationidentities_ownerid",
                        column: x => x.ownerid,
                        principalSchema: "guamaframework_identity",
                        principalTable: "organizationidentities",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "personidentities",
                schema: "guamaframework_identity",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    lastname = table.Column<string>(type: "text", nullable: true),
                    alias = table.Column<string>(type: "text", nullable: true),
                    sex = table.Column<int>(type: "integer", nullable: false),
                    civilstatus = table.Column<int>(type: "integer", nullable: false),
                    imageid = table.Column<Guid>(type: "uuid", nullable: true),
                    name = table.Column<string>(type: "text", nullable: false),
                    borndate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    about = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_personidentities", x => x.id);
                    table.ForeignKey(
                        name: "fk_personidentities_images_imageid",
                        column: x => x.imageid,
                        principalSchema: "guamaframework_media",
                        principalTable: "images",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "sections",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    priority = table.Column<int>(type: "integer", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    imageid = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_sections", x => x.id);
                    table.ForeignKey(
                        name: "fk_sections_images_imageid",
                        column: x => x.imageid,
                        principalSchema: "guamaframework_media",
                        principalTable: "images",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "mapvideobridges",
                columns: table => new
                {
                    prowid = table.Column<string>(type: "text", nullable: false),
                    sternid = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_mapvideobridges", x => new { x.prowid, x.sternid });
                    table.ForeignKey(
                        name: "fk_mapvideobridges_maps_prowid",
                        column: x => x.prowid,
                        principalTable: "maps",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_mapvideobridges_videos_sternid",
                        column: x => x.sternid,
                        principalSchema: "guamaframework_media",
                        principalTable: "videos",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "municipalities",
                schema: "guamaframework_location",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    stateid = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_municipalities", x => x.id);
                    table.ForeignKey(
                        name: "fk_municipalities_states_stateid",
                        column: x => x.stateid,
                        principalSchema: "guamaframework_location",
                        principalTable: "states",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "institutions",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    identityid = table.Column<Guid>(type: "uuid", nullable: false),
                    alias = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_institutions", x => x.id);
                    table.ForeignKey(
                        name: "fk_institutions_organizationidentities_identityid",
                        column: x => x.identityid,
                        principalSchema: "guamaframework_identity",
                        principalTable: "organizationidentities",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "organizationemailcontactitembridges",
                schema: "guamaframework_identity",
                columns: table => new
                {
                    prowid = table.Column<Guid>(type: "uuid", nullable: false),
                    sternid = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_organizationemailcontactitembridges", x => new { x.prowid, x.sternid });
                    table.ForeignKey(
                        name: "fk_organizationemailcontactitembridges_emailcontactitems_stern~",
                        column: x => x.sternid,
                        principalSchema: "guamaframework_identity",
                        principalTable: "emailcontactitems",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_organizationemailcontactitembridges_organizationidentities_~",
                        column: x => x.prowid,
                        principalSchema: "guamaframework_identity",
                        principalTable: "organizationidentities",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "organizationphonecontactitembridges",
                schema: "guamaframework_identity",
                columns: table => new
                {
                    prowid = table.Column<Guid>(type: "uuid", nullable: false),
                    sternid = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_organizationphonecontactitembridges", x => new { x.prowid, x.sternid });
                    table.ForeignKey(
                        name: "fk_organizationphonecontactitembridges_organizationidentities_~",
                        column: x => x.prowid,
                        principalSchema: "guamaframework_identity",
                        principalTable: "organizationidentities",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_organizationphonecontactitembridges_phonecontactitems_stern~",
                        column: x => x.sternid,
                        principalSchema: "guamaframework_identity",
                        principalTable: "phonecontactitems",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "organizationsocialcontactitembridges",
                schema: "guamaframework_identity",
                columns: table => new
                {
                    prowid = table.Column<Guid>(type: "uuid", nullable: false),
                    sternid = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_organizationsocialcontactitembridges", x => new { x.prowid, x.sternid });
                    table.ForeignKey(
                        name: "fk_organizationsocialcontactitembridges_organizationidentities~",
                        column: x => x.prowid,
                        principalSchema: "guamaframework_identity",
                        principalTable: "organizationidentities",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_organizationsocialcontactitembridges_socialcontactitems_ste~",
                        column: x => x.sternid,
                        principalSchema: "guamaframework_identity",
                        principalTable: "socialcontactitems",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "organizationwebsitecontactitembridges",
                schema: "guamaframework_identity",
                columns: table => new
                {
                    prowid = table.Column<Guid>(type: "uuid", nullable: false),
                    sternid = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_organizationwebsitecontactitembridges", x => new { x.prowid, x.sternid });
                    table.ForeignKey(
                        name: "fk_organizationwebsitecontactitembridges_organizationidentitie~",
                        column: x => x.prowid,
                        principalSchema: "guamaframework_identity",
                        principalTable: "organizationidentities",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_organizationwebsitecontactitembridges_websitecontactitems_s~",
                        column: x => x.sternid,
                        principalSchema: "guamaframework_identity",
                        principalTable: "websitecontactitems",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "organizationpersonmembershipbridges",
                schema: "guamaframework_identity",
                columns: table => new
                {
                    prowid = table.Column<Guid>(type: "uuid", nullable: false),
                    sternid = table.Column<Guid>(type: "uuid", nullable: false),
                    role = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_organizationpersonmembershipbridges", x => new { x.prowid, x.sternid });
                    table.ForeignKey(
                        name: "fk_organizationpersonmembershipbridges_organizationidentities_~",
                        column: x => x.prowid,
                        principalSchema: "guamaframework_identity",
                        principalTable: "organizationidentities",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_organizationpersonmembershipbridges_personidentities_sternid",
                        column: x => x.sternid,
                        principalSchema: "guamaframework_identity",
                        principalTable: "personidentities",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "personemailcontactitembridges",
                schema: "guamaframework_identity",
                columns: table => new
                {
                    prowid = table.Column<Guid>(type: "uuid", nullable: false),
                    sternid = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_personemailcontactitembridges", x => new { x.prowid, x.sternid });
                    table.ForeignKey(
                        name: "fk_personemailcontactitembridges_emailcontactitems_sternid",
                        column: x => x.sternid,
                        principalSchema: "guamaframework_identity",
                        principalTable: "emailcontactitems",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_personemailcontactitembridges_personidentities_prowid",
                        column: x => x.prowid,
                        principalSchema: "guamaframework_identity",
                        principalTable: "personidentities",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "personphonecontactitembridges",
                schema: "guamaframework_identity",
                columns: table => new
                {
                    prowid = table.Column<Guid>(type: "uuid", nullable: false),
                    sternid = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_personphonecontactitembridges", x => new { x.prowid, x.sternid });
                    table.ForeignKey(
                        name: "fk_personphonecontactitembridges_personidentities_prowid",
                        column: x => x.prowid,
                        principalSchema: "guamaframework_identity",
                        principalTable: "personidentities",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_personphonecontactitembridges_phonecontactitems_sternid",
                        column: x => x.sternid,
                        principalSchema: "guamaframework_identity",
                        principalTable: "phonecontactitems",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "personsocialcontactitembridges",
                schema: "guamaframework_identity",
                columns: table => new
                {
                    prowid = table.Column<Guid>(type: "uuid", nullable: false),
                    sternid = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_personsocialcontactitembridges", x => new { x.prowid, x.sternid });
                    table.ForeignKey(
                        name: "fk_personsocialcontactitembridges_personidentities_prowid",
                        column: x => x.prowid,
                        principalSchema: "guamaframework_identity",
                        principalTable: "personidentities",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_personsocialcontactitembridges_socialcontactitems_sternid",
                        column: x => x.sternid,
                        principalSchema: "guamaframework_identity",
                        principalTable: "socialcontactitems",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "personwebsitecontactitembridges",
                schema: "guamaframework_identity",
                columns: table => new
                {
                    prowid = table.Column<Guid>(type: "uuid", nullable: false),
                    sternid = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_personwebsitecontactitembridges", x => new { x.prowid, x.sternid });
                    table.ForeignKey(
                        name: "fk_personwebsitecontactitembridges_personidentities_prowid",
                        column: x => x.prowid,
                        principalSchema: "guamaframework_identity",
                        principalTable: "personidentities",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_personwebsitecontactitembridges_websitecontactitems_sternid",
                        column: x => x.sternid,
                        principalSchema: "guamaframework_identity",
                        principalTable: "websitecontactitems",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "users",
                schema: "guamaframework_security_identity",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    enabled = table.Column<bool>(type: "boolean", nullable: false),
                    password = table.Column<string>(type: "text", nullable: false),
                    passwordchangerequired = table.Column<bool>(type: "boolean", nullable: false),
                    hasfullaccess = table.Column<bool>(type: "boolean", nullable: false),
                    created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    lastsessiondate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    failauthenticationcount = table.Column<int>(type: "integer", nullable: false),
                    identityid = table.Column<Guid>(type: "uuid", nullable: false),
                    signature = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_users", x => x.id);
                    table.ForeignKey(
                        name: "fk_users_personidentities_identityid",
                        column: x => x.identityid,
                        principalSchema: "guamaframework_identity",
                        principalTable: "personidentities",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "epigraphs",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    priority = table.Column<int>(type: "integer", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    imageid = table.Column<Guid>(type: "uuid", nullable: true),
                    layout = table.Column<int>(type: "integer", nullable: false),
                    sectionid = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_epigraphs", x => x.id);
                    table.ForeignKey(
                        name: "fk_epigraphs_images_imageid",
                        column: x => x.imageid,
                        principalSchema: "guamaframework_media",
                        principalTable: "images",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_epigraphs_sections_sectionid",
                        column: x => x.sectionid,
                        principalTable: "sections",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "localities",
                schema: "guamaframework_location",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    municipalityid = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_localities", x => x.id);
                    table.ForeignKey(
                        name: "fk_localities_municipalities_municipalityid",
                        column: x => x.municipalityid,
                        principalSchema: "guamaframework_location",
                        principalTable: "municipalities",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "authors",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    identityid = table.Column<Guid>(type: "uuid", nullable: false),
                    institutionid = table.Column<Guid>(type: "uuid", nullable: true),
                    nameprefix = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_authors", x => x.id);
                    table.ForeignKey(
                        name: "fk_authors_institutions_institutionid",
                        column: x => x.institutionid,
                        principalTable: "institutions",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_authors_personidentities_identityid",
                        column: x => x.identityid,
                        principalSchema: "guamaframework_identity",
                        principalTable: "personidentities",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "userrolebridges",
                schema: "guamaframework_security_identity",
                columns: table => new
                {
                    prowid = table.Column<string>(type: "text", nullable: false),
                    sternid = table.Column<Guid>(type: "uuid", nullable: false),
                    signature = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_userrolebridges", x => new { x.prowid, x.sternid });
                    table.ForeignKey(
                        name: "fk_userrolebridges_roles_sternid",
                        column: x => x.sternid,
                        principalSchema: "guamaframework_core_security_common",
                        principalTable: "roles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_userrolebridges_users_prowid",
                        column: x => x.prowid,
                        principalSchema: "guamaframework_security_identity",
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "epigraphdocumentbridges",
                columns: table => new
                {
                    prowid = table.Column<Guid>(type: "uuid", nullable: false),
                    sternid = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_epigraphdocumentbridges", x => new { x.prowid, x.sternid });
                    table.ForeignKey(
                        name: "fk_epigraphdocumentbridges_documents_sternid",
                        column: x => x.sternid,
                        principalSchema: "guamaframework_media",
                        principalTable: "documents",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_epigraphdocumentbridges_epigraphs_prowid",
                        column: x => x.prowid,
                        principalTable: "epigraphs",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "epigraphimagebridges",
                columns: table => new
                {
                    prowid = table.Column<Guid>(type: "uuid", nullable: false),
                    sternid = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_epigraphimagebridges", x => new { x.prowid, x.sternid });
                    table.ForeignKey(
                        name: "fk_epigraphimagebridges_epigraphs_prowid",
                        column: x => x.prowid,
                        principalTable: "epigraphs",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_epigraphimagebridges_images_sternid",
                        column: x => x.sternid,
                        principalSchema: "guamaframework_media",
                        principalTable: "images",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "epigraphkeywordbridges",
                columns: table => new
                {
                    prowid = table.Column<Guid>(type: "uuid", nullable: false),
                    sternid = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_epigraphkeywordbridges", x => new { x.prowid, x.sternid });
                    table.ForeignKey(
                        name: "fk_epigraphkeywordbridges_epigraphs_prowid",
                        column: x => x.prowid,
                        principalTable: "epigraphs",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_epigraphkeywordbridges_keywords_sternid",
                        column: x => x.sternid,
                        principalTable: "keywords",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "epigraphmapbridges",
                columns: table => new
                {
                    prowid = table.Column<Guid>(type: "uuid", nullable: false),
                    sternid = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_epigraphmapbridges", x => new { x.prowid, x.sternid });
                    table.ForeignKey(
                        name: "fk_epigraphmapbridges_epigraphs_prowid",
                        column: x => x.prowid,
                        principalTable: "epigraphs",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_epigraphmapbridges_maps_sternid",
                        column: x => x.sternid,
                        principalTable: "maps",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "epigraphvideobridges",
                columns: table => new
                {
                    prowid = table.Column<Guid>(type: "uuid", nullable: false),
                    sternid = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_epigraphvideobridges", x => new { x.prowid, x.sternid });
                    table.ForeignKey(
                        name: "fk_epigraphvideobridges_epigraphs_prowid",
                        column: x => x.prowid,
                        principalTable: "epigraphs",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_epigraphvideobridges_videos_sternid",
                        column: x => x.sternid,
                        principalSchema: "guamaframework_media",
                        principalTable: "videos",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "locations",
                schema: "guamaframework_location",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    discriminator = table.Column<string>(type: "text", nullable: false),
                    localityid = table.Column<Guid>(type: "uuid", nullable: true),
                    municipalityid = table.Column<Guid>(type: "uuid", nullable: true),
                    stateid = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_locations", x => x.id);
                    table.ForeignKey(
                        name: "fk_locations_localities_localityid",
                        column: x => x.localityid,
                        principalSchema: "guamaframework_location",
                        principalTable: "localities",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "fk_locations_municipalities_municipalityid",
                        column: x => x.municipalityid,
                        principalSchema: "guamaframework_location",
                        principalTable: "municipalities",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "fk_locations_states_stateid",
                        column: x => x.stateid,
                        principalSchema: "guamaframework_location",
                        principalTable: "states",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "mapauthorbridges",
                columns: table => new
                {
                    prowid = table.Column<string>(type: "text", nullable: false),
                    sternid = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_mapauthorbridges", x => new { x.prowid, x.sternid });
                    table.ForeignKey(
                        name: "fk_mapauthorbridges_authors_sternid",
                        column: x => x.sternid,
                        principalTable: "authors",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_mapauthorbridges_maps_prowid",
                        column: x => x.prowid,
                        principalTable: "maps",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "postalcontactitems",
                schema: "guamaframework_identity",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    address = table.Column<string>(type: "text", nullable: false),
                    locationid = table.Column<Guid>(type: "uuid", nullable: true),
                    zipcode = table.Column<int>(type: "integer", nullable: false),
                    longitude = table.Column<decimal>(type: "numeric", nullable: false),
                    latitude = table.Column<decimal>(type: "numeric", nullable: false),
                    label = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_postalcontactitems", x => x.id);
                    table.ForeignKey(
                        name: "fk_postalcontactitems_locations_locationid",
                        column: x => x.locationid,
                        principalSchema: "guamaframework_location",
                        principalTable: "locations",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "organizationpostalcontactitembridges",
                schema: "guamaframework_identity",
                columns: table => new
                {
                    prowid = table.Column<Guid>(type: "uuid", nullable: false),
                    sternid = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_organizationpostalcontactitembridges", x => new { x.prowid, x.sternid });
                    table.ForeignKey(
                        name: "fk_organizationpostalcontactitembridges_organizationidentities~",
                        column: x => x.prowid,
                        principalSchema: "guamaframework_identity",
                        principalTable: "organizationidentities",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_organizationpostalcontactitembridges_postalcontactitems_ste~",
                        column: x => x.sternid,
                        principalSchema: "guamaframework_identity",
                        principalTable: "postalcontactitems",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "personpostalcontactitembridges",
                schema: "guamaframework_identity",
                columns: table => new
                {
                    prowid = table.Column<Guid>(type: "uuid", nullable: false),
                    sternid = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_personpostalcontactitembridges", x => new { x.prowid, x.sternid });
                    table.ForeignKey(
                        name: "fk_personpostalcontactitembridges_personidentities_prowid",
                        column: x => x.prowid,
                        principalSchema: "guamaframework_identity",
                        principalTable: "personidentities",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_personpostalcontactitembridges_postalcontactitems_sternid",
                        column: x => x.sternid,
                        principalSchema: "guamaframework_identity",
                        principalTable: "postalcontactitems",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_authors_identityid",
                table: "authors",
                column: "identityid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_authors_institutionid",
                table: "authors",
                column: "institutionid");

            migrationBuilder.CreateIndex(
                name: "ix_countries_isocode",
                schema: "guamaframework_location",
                table: "countries",
                column: "isocode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_countries_name",
                schema: "guamaframework_location",
                table: "countries",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_documents_thumbnailid",
                schema: "guamaframework_media",
                table: "documents",
                column: "thumbnailid");

            migrationBuilder.CreateIndex(
                name: "ix_epigraphdocumentbridges_sternid",
                table: "epigraphdocumentbridges",
                column: "sternid");

            migrationBuilder.CreateIndex(
                name: "ix_epigraphimagebridges_sternid",
                table: "epigraphimagebridges",
                column: "sternid");

            migrationBuilder.CreateIndex(
                name: "ix_epigraphkeywordbridges_sternid",
                table: "epigraphkeywordbridges",
                column: "sternid");

            migrationBuilder.CreateIndex(
                name: "ix_epigraphmapbridges_sternid",
                table: "epigraphmapbridges",
                column: "sternid");

            migrationBuilder.CreateIndex(
                name: "ix_epigraphs_imageid",
                table: "epigraphs",
                column: "imageid");

            migrationBuilder.CreateIndex(
                name: "ix_epigraphs_sectionid",
                table: "epigraphs",
                column: "sectionid");

            migrationBuilder.CreateIndex(
                name: "ix_epigraphvideobridges_sternid",
                table: "epigraphvideobridges",
                column: "sternid");

            migrationBuilder.CreateIndex(
                name: "ix_images_thumbnailid",
                schema: "guamaframework_media",
                table: "images",
                column: "thumbnailid");

            migrationBuilder.CreateIndex(
                name: "ix_institutions_identityid",
                table: "institutions",
                column: "identityid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_localities_municipalityid",
                schema: "guamaframework_location",
                table: "localities",
                column: "municipalityid");

            migrationBuilder.CreateIndex(
                name: "ix_localities_name_municipalityid",
                schema: "guamaframework_location",
                table: "localities",
                columns: new[] { "name", "municipalityid" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_locations_localityid",
                schema: "guamaframework_location",
                table: "locations",
                column: "localityid");

            migrationBuilder.CreateIndex(
                name: "ix_locations_municipalityid",
                schema: "guamaframework_location",
                table: "locations",
                column: "municipalityid");

            migrationBuilder.CreateIndex(
                name: "ix_locations_stateid",
                schema: "guamaframework_location",
                table: "locations",
                column: "stateid");

            migrationBuilder.CreateIndex(
                name: "ix_mapauthorbridges_sternid",
                table: "mapauthorbridges",
                column: "sternid");

            migrationBuilder.CreateIndex(
                name: "ix_mapdocumentbridges_sternid",
                table: "mapdocumentbridges",
                column: "sternid");

            migrationBuilder.CreateIndex(
                name: "ix_mapimagebridges_sternid",
                table: "mapimagebridges",
                column: "sternid");

            migrationBuilder.CreateIndex(
                name: "ix_mapkeywordbridges_sternid",
                table: "mapkeywordbridges",
                column: "sternid");

            migrationBuilder.CreateIndex(
                name: "ix_mapvideobridges_sternid",
                table: "mapvideobridges",
                column: "sternid");

            migrationBuilder.CreateIndex(
                name: "ix_municipalities_name_stateid",
                schema: "guamaframework_location",
                table: "municipalities",
                columns: new[] { "name", "stateid" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_municipalities_stateid",
                schema: "guamaframework_location",
                table: "municipalities",
                column: "stateid");

            migrationBuilder.CreateIndex(
                name: "ix_organizationemailcontactitembridges_sternid",
                schema: "guamaframework_identity",
                table: "organizationemailcontactitembridges",
                column: "sternid");

            migrationBuilder.CreateIndex(
                name: "ix_organizationidentities_imageid",
                schema: "guamaframework_identity",
                table: "organizationidentities",
                column: "imageid");

            migrationBuilder.CreateIndex(
                name: "ix_organizationidentities_ownerid",
                schema: "guamaframework_identity",
                table: "organizationidentities",
                column: "ownerid");

            migrationBuilder.CreateIndex(
                name: "ix_organizationpersonmembershipbridges_sternid",
                schema: "guamaframework_identity",
                table: "organizationpersonmembershipbridges",
                column: "sternid");

            migrationBuilder.CreateIndex(
                name: "ix_organizationphonecontactitembridges_sternid",
                schema: "guamaframework_identity",
                table: "organizationphonecontactitembridges",
                column: "sternid");

            migrationBuilder.CreateIndex(
                name: "ix_organizationpostalcontactitembridges_sternid",
                schema: "guamaframework_identity",
                table: "organizationpostalcontactitembridges",
                column: "sternid");

            migrationBuilder.CreateIndex(
                name: "ix_organizationsocialcontactitembridges_sternid",
                schema: "guamaframework_identity",
                table: "organizationsocialcontactitembridges",
                column: "sternid");

            migrationBuilder.CreateIndex(
                name: "ix_organizationwebsitecontactitembridges_sternid",
                schema: "guamaframework_identity",
                table: "organizationwebsitecontactitembridges",
                column: "sternid");

            migrationBuilder.CreateIndex(
                name: "ix_personemailcontactitembridges_sternid",
                schema: "guamaframework_identity",
                table: "personemailcontactitembridges",
                column: "sternid");

            migrationBuilder.CreateIndex(
                name: "ix_personidentities_imageid",
                schema: "guamaframework_identity",
                table: "personidentities",
                column: "imageid");

            migrationBuilder.CreateIndex(
                name: "ix_personphonecontactitembridges_sternid",
                schema: "guamaframework_identity",
                table: "personphonecontactitembridges",
                column: "sternid");

            migrationBuilder.CreateIndex(
                name: "ix_personpostalcontactitembridges_sternid",
                schema: "guamaframework_identity",
                table: "personpostalcontactitembridges",
                column: "sternid");

            migrationBuilder.CreateIndex(
                name: "ix_personsocialcontactitembridges_sternid",
                schema: "guamaframework_identity",
                table: "personsocialcontactitembridges",
                column: "sternid");

            migrationBuilder.CreateIndex(
                name: "ix_personwebsitecontactitembridges_sternid",
                schema: "guamaframework_identity",
                table: "personwebsitecontactitembridges",
                column: "sternid");

            migrationBuilder.CreateIndex(
                name: "ix_postalcontactitems_locationid",
                schema: "guamaframework_identity",
                table: "postalcontactitems",
                column: "locationid");

            migrationBuilder.CreateIndex(
                name: "ix_regions_countryid",
                schema: "guamaframework_location",
                table: "regions",
                column: "countryid");

            migrationBuilder.CreateIndex(
                name: "ix_rolepermissionbridges_sternid",
                schema: "guamaframework_core_security_common",
                table: "rolepermissionbridges",
                column: "sternid");

            migrationBuilder.CreateIndex(
                name: "ix_sections_imageid",
                table: "sections",
                column: "imageid");

            migrationBuilder.CreateIndex(
                name: "ix_states_regionid",
                schema: "guamaframework_location",
                table: "states",
                column: "regionid");

            migrationBuilder.CreateIndex(
                name: "ix_userrolebridges_sternid",
                schema: "guamaframework_security_identity",
                table: "userrolebridges",
                column: "sternid");

            migrationBuilder.CreateIndex(
                name: "ix_users_identityid",
                schema: "guamaframework_security_identity",
                table: "users",
                column: "identityid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_videos_thumbnailid",
                schema: "guamaframework_media",
                table: "videos",
                column: "thumbnailid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "epigraphdocumentbridges");

            migrationBuilder.DropTable(
                name: "epigraphimagebridges");

            migrationBuilder.DropTable(
                name: "epigraphkeywordbridges");

            migrationBuilder.DropTable(
                name: "epigraphmapbridges");

            migrationBuilder.DropTable(
                name: "epigraphvideobridges");

            migrationBuilder.DropTable(
                name: "mapauthorbridges");

            migrationBuilder.DropTable(
                name: "mapdocumentbridges");

            migrationBuilder.DropTable(
                name: "mapimagebridges");

            migrationBuilder.DropTable(
                name: "mapkeywordbridges");

            migrationBuilder.DropTable(
                name: "mapvideobridges");

            migrationBuilder.DropTable(
                name: "organizationemailcontactitembridges",
                schema: "guamaframework_identity");

            migrationBuilder.DropTable(
                name: "organizationpersonmembershipbridges",
                schema: "guamaframework_identity");

            migrationBuilder.DropTable(
                name: "organizationphonecontactitembridges",
                schema: "guamaframework_identity");

            migrationBuilder.DropTable(
                name: "organizationpostalcontactitembridges",
                schema: "guamaframework_identity");

            migrationBuilder.DropTable(
                name: "organizationsocialcontactitembridges",
                schema: "guamaframework_identity");

            migrationBuilder.DropTable(
                name: "organizationwebsitecontactitembridges",
                schema: "guamaframework_identity");

            migrationBuilder.DropTable(
                name: "personemailcontactitembridges",
                schema: "guamaframework_identity");

            migrationBuilder.DropTable(
                name: "personphonecontactitembridges",
                schema: "guamaframework_identity");

            migrationBuilder.DropTable(
                name: "personpostalcontactitembridges",
                schema: "guamaframework_identity");

            migrationBuilder.DropTable(
                name: "personsocialcontactitembridges",
                schema: "guamaframework_identity");

            migrationBuilder.DropTable(
                name: "personwebsitecontactitembridges",
                schema: "guamaframework_identity");

            migrationBuilder.DropTable(
                name: "rolepermissionbridges",
                schema: "guamaframework_core_security_common");

            migrationBuilder.DropTable(
                name: "traces");

            migrationBuilder.DropTable(
                name: "userrolebridges",
                schema: "guamaframework_security_identity");

            migrationBuilder.DropTable(
                name: "epigraphs");

            migrationBuilder.DropTable(
                name: "authors");

            migrationBuilder.DropTable(
                name: "documents",
                schema: "guamaframework_media");

            migrationBuilder.DropTable(
                name: "keywords");

            migrationBuilder.DropTable(
                name: "maps");

            migrationBuilder.DropTable(
                name: "videos",
                schema: "guamaframework_media");

            migrationBuilder.DropTable(
                name: "emailcontactitems",
                schema: "guamaframework_identity");

            migrationBuilder.DropTable(
                name: "phonecontactitems",
                schema: "guamaframework_identity");

            migrationBuilder.DropTable(
                name: "postalcontactitems",
                schema: "guamaframework_identity");

            migrationBuilder.DropTable(
                name: "socialcontactitems",
                schema: "guamaframework_identity");

            migrationBuilder.DropTable(
                name: "websitecontactitems",
                schema: "guamaframework_identity");

            migrationBuilder.DropTable(
                name: "permissions",
                schema: "guamaframework_core_security_common");

            migrationBuilder.DropTable(
                name: "roles",
                schema: "guamaframework_core_security_common");

            migrationBuilder.DropTable(
                name: "users",
                schema: "guamaframework_security_identity");

            migrationBuilder.DropTable(
                name: "sections");

            migrationBuilder.DropTable(
                name: "institutions");

            migrationBuilder.DropTable(
                name: "locations",
                schema: "guamaframework_location");

            migrationBuilder.DropTable(
                name: "personidentities",
                schema: "guamaframework_identity");

            migrationBuilder.DropTable(
                name: "organizationidentities",
                schema: "guamaframework_identity");

            migrationBuilder.DropTable(
                name: "localities",
                schema: "guamaframework_location");

            migrationBuilder.DropTable(
                name: "images",
                schema: "guamaframework_media");

            migrationBuilder.DropTable(
                name: "municipalities",
                schema: "guamaframework_location");

            migrationBuilder.DropTable(
                name: "thumbnails",
                schema: "guamaframework_media");

            migrationBuilder.DropTable(
                name: "states",
                schema: "guamaframework_location");

            migrationBuilder.DropTable(
                name: "regions",
                schema: "guamaframework_location");

            migrationBuilder.DropTable(
                name: "countries",
                schema: "guamaframework_location");
        }
    }
}
