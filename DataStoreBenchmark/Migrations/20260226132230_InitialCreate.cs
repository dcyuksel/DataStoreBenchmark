using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataStoreBenchmark.Migrations;

/// <inheritdoc />
public partial class InitialCreate : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.EnsureSchema(
            name: "dbo");

        migrationBuilder.CreateTable(
            name: "Movies",
            schema: "dbo",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                Director = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                ReleaseYear = table.Column<int>(type: "int", nullable: true),
                Genre = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                CreatedUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Movies", x => x.Id);
            });
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "Movies",
            schema: "dbo");
    }
}
