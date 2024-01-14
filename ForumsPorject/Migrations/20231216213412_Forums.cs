using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ForumsProject.Migrations
{
    public partial class Forums : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppRoles",
                columns: table => new
                {
                    AppRoleId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SimpleRole = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ManagerRole = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppRoles", x => x.AppRoleId);
                });

            migrationBuilder.CreateTable(
                name: "AppRoleUtilisateur",
                columns: table => new
                {
                    AppRoleId = table.Column<int>(type: "int", nullable: false),
                    UtilisateurId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppRoleUtilisateur", x => new { x.AppRoleId, x.UtilisateurId });
                });

            migrationBuilder.CreateTable(
                name: "categories",
                columns: table => new
                {
                    categorie_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    titre_categorie = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    description_categorie = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("categorie_pk", x => x.categorie_id);
                });

            migrationBuilder.CreateTable(
                name: "utilisateurs",
                columns: table => new
                {
                    utilisateur_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    pseudonyme = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    password = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    inscrit = table.Column<bool>(type: "bit", nullable: false),
                    valid = table.Column<bool>(type: "bit", nullable: false),
                    cheminavatar = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    signature = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    actif = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "((0))")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_utilisateurs", x => x.utilisateur_id);
                });

            migrationBuilder.CreateTable(
                name: "forums",
                columns: table => new
                {
                    forum_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    titre_forum = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    dateCreation_forum = table.Column<DateTime>(type: "date", nullable: false),
                    discription_forum = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    categorieid = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_forums", x => x.forum_id);
                    table.ForeignKey(
                        name: "forum_categorie_id_fk",
                        column: x => x.categorieid,
                        principalTable: "categories",
                        principalColumn: "categorie_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UtilisateurRoles",
                columns: table => new
                {
                    UtilisateurID = table.Column<int>(type: "int", nullable: false),
                    AppRoleId = table.Column<int>(type: "int", nullable: false),
                    UtilisateurId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UtilisateurRoles", x => new { x.UtilisateurID, x.AppRoleId });
                    table.ForeignKey(
                        name: "FK_UtilisateurRoles_AppRoleId",
                        column: x => x.AppRoleId,
                        principalTable: "AppRoles",
                        principalColumn: "AppRoleId");
                    table.ForeignKey(
                        name: "FK_UtilisateurRoles_UtilisateurId",
                        column: x => x.UtilisateurId,
                        principalTable: "utilisateurs",
                        principalColumn: "utilisateur_id");
                    table.ForeignKey(
                        name: "FK_UtilisateurRoles_utilisateurs_UtilisateurID",
                        column: x => x.UtilisateurID,
                        principalTable: "utilisateurs",
                        principalColumn: "utilisateur_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "themes",
                columns: table => new
                {
                    theme_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    titre_theme = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    dateCreation_theme = table.Column<DateTime>(type: "date", nullable: false),
                    forumid = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_themes", x => x.theme_id);
                    table.ForeignKey(
                        name: "thème_forum_id_fk",
                        column: x => x.forumid,
                        principalTable: "forums",
                        principalColumn: "forum_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "discussions",
                columns: table => new
                {
                    discussion_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    titre_discussion = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    dateCreation_discussion = table.Column<DateTime>(type: "date", nullable: false),
                    themeid = table.Column<int>(type: "int", nullable: true),
                    utilisateurid = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_discussions", x => x.discussion_id);
                    table.ForeignKey(
                        name: "FK_discussions_themes",
                        column: x => x.themeid,
                        principalTable: "themes",
                        principalColumn: "theme_id");
                    table.ForeignKey(
                        name: "FK_discussions_utilisateurs",
                        column: x => x.utilisateurid,
                        principalTable: "utilisateurs",
                        principalColumn: "utilisateur_id");
                });

            migrationBuilder.CreateTable(
                name: "messages",
                columns: table => new
                {
                    messages_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    contenu_message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    datecréation_message = table.Column<DateTime>(type: "date", nullable: false),
                    lu = table.Column<bool>(type: "bit", nullable: false),
                    archive = table.Column<bool>(type: "bit", nullable: false),
                    auteur_id = table.Column<int>(type: "int", nullable: false),
                    discussionid = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("message_pk", x => x.messages_id);
                    table.ForeignKey(
                        name: "message_auteur_id_fk",
                        column: x => x.auteur_id,
                        principalTable: "utilisateurs",
                        principalColumn: "utilisateur_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "message_discussion_id_fk",
                        column: x => x.discussionid,
                        principalTable: "discussions",
                        principalColumn: "discussion_id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_discussions_themeid",
                table: "discussions",
                column: "themeid");

            migrationBuilder.CreateIndex(
                name: "IX_discussions_utilisateurid",
                table: "discussions",
                column: "utilisateurid");

            migrationBuilder.CreateIndex(
                name: "IX_forums_categorieid",
                table: "forums",
                column: "categorieid");

            migrationBuilder.CreateIndex(
                name: "IX_messages_auteur_id",
                table: "messages",
                column: "auteur_id");

            migrationBuilder.CreateIndex(
                name: "IX_messages_discussionid",
                table: "messages",
                column: "discussionid");

            migrationBuilder.CreateIndex(
                name: "IX_themes_forumid",
                table: "themes",
                column: "forumid");

            migrationBuilder.CreateIndex(
                name: "IX_UtilisateurRoles_AppRoleId",
                table: "UtilisateurRoles",
                column: "AppRoleId");

            migrationBuilder.CreateIndex(
                name: "IX_UtilisateurRoles_UtilisateurId",
                table: "UtilisateurRoles",
                column: "UtilisateurId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppRoleUtilisateur");

            migrationBuilder.DropTable(
                name: "messages");

            migrationBuilder.DropTable(
                name: "UtilisateurRoles");

            migrationBuilder.DropTable(
                name: "discussions");

            migrationBuilder.DropTable(
                name: "AppRoles");

            migrationBuilder.DropTable(
                name: "themes");

            migrationBuilder.DropTable(
                name: "utilisateurs");

            migrationBuilder.DropTable(
                name: "forums");

            migrationBuilder.DropTable(
                name: "categories");
        }
    }
}
