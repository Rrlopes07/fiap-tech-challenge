#nullable disable

namespace Infrastructure.Migrations;

public partial class firstMigration : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Ddd",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                Name = table.Column<string>(type: "varchar(100)", nullable: false),
                DddNumber = table.Column<short>(type: "smallint", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Ddd", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "Client",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                Name = table.Column<string>(type: "varchar(100)", nullable: false),
                Telephone = table.Column<string>(type: "varchar(100)", nullable: false),
                Email = table.Column<string>(type: "varchar(100)", nullable: false),
                DddId = table.Column<Guid>(type: "uuid", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Client", x => x.Id);
                table.ForeignKey(
                    name: "FK_Client_Ddd_DddId",
                    column: x => x.DddId,
                    principalTable: "Ddd",
                    principalColumn: "Id");
            });

        migrationBuilder.CreateIndex(
            name: "IX_Client_DddId",
            table: "Client",
            column: "DddId");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "Client");

        migrationBuilder.DropTable(
            name: "Ddd");
    }
}
