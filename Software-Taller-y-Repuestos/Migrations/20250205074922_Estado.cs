using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Software_Taller_y_Repuestos.Migrations
{
    /// <inheritdoc />
    public partial class Estado : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Estado",
                table: "Usuarios",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Estado",
                table: "Usuarios");
        }
    }
}
