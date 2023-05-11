using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AdaStore.Shared.Migrations
{
    /// <inheritdoc />
    public partial class InsertSomeProducts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
            table: "Products",
            columns: new[] { "Name", "Description", "Stock", "Price", "CreatedAt", "UpdatedAt", "IsDeleted", "ImageUrl" },
            values: new object[] 
            { 
                "Aspiradora y Mopeadora IROBOT 2 en 1 R1118 Verde", 
                "Aspira el polvo y la suciedad de suelos y alfombras gracias a su cepillo en forma de V y su potente aspiración. Sus cepillos para esquinas y bordes limpian la suciedad a lo largo de paredes y esquinas. Su sistema mopeado utiliza múltiples modos de limpieza y un paño de microfibra con una textura especialmente diseñada. Aprende tus hábitos de limpieza y te sugiere horarios que se adaptan a ti.", 
                4, 
                1599900, 
                "2023-04-14T00:28:01.1201016", 
                "2023-04-14T00:28:01.1201016",
                false, 
                "https://images.pexels.com/photos/3616746/pexels-photo-3616746.jpeg" 
            });

            migrationBuilder.InsertData(
            table: "Products",
            columns: new[] { "Name", "Description", "Stock", "Price", "CreatedAt", "UpdatedAt", "IsDeleted", "ImageUrl" },
            values: new object[]
            {
                "Consola XBOX Series X + 1 Control Inalámbrico + Paquete Forza Horizon 5",
                "Comienza tu aventura definitiva con el Bundle de Consola Xbox Series X y la Edición Premium de Forza Horizon 5. Explora los vibrantes y siempre cambiantes paisajes del mundo abierto de México en los mejores autos del mundo. Los juegos que se han optimizado para Xbox Series X|S usan toda la potencia de las nuevas consolas para ofrecer velocidades de fotogramas más altas y estables de hasta 120 FPS durante el juego.Compra ya la tuya!!",
                12,
                3870000,
                "2023-04-14T00:28:01.1201016",
                "2023-04-14T00:28:01.1201016",
                false,
                "https://http2.mlstatic.com/D_NQ_NP_909736-MCO45130702349_032021-O.jpg"
            });

            migrationBuilder.InsertData(
            table: "Products",
            columns: new[] { "Name", "Description", "Stock", "Price", "CreatedAt", "UpdatedAt", "IsDeleted", "ImageUrl" },
            values: new object[]
            {
                "Minicomponente SAMSUNG MX-T40 300 Watts Negro Torre de Sonido",
                "La exclusiva Torre de Sonido Samsung MX-T40 tiene una potencia de salida de 300 vatios para que disfrutes de una experiencia de sonido bidireccional envolvente, adicional, cuenta con un panel de control que ofrece una resistencia a salpicaduras y con la aplicación Samsung Giga Party Audio podrás controlar las luces, crear listas de canciones, hacer mezclas con el ecualizador y colocar efectos tipo DJ  ¡Que comience la fiesta!",
                22,
                449000,
                "2023-04-14T00:28:01.1201016",
                "2023-04-14T00:28:01.1201016",
                false,
                "https://www.lacomer.com.co/wp-content/uploads/2020/11/8806090686214-013-750Wx750H.jpg"
            });

            migrationBuilder.InsertData(
            table: "Products",
            columns: new[] { "Name", "Description", "Stock", "Price", "CreatedAt", "UpdatedAt", "IsDeleted", "ImageUrl" },
            values: new object[]
            {
                "Computador Portátil HP 15.6",
                "Pulgadas ef2519la AMD Ryzen 5 - RAM 8GB', N'Portátil HP. Diseñado para mantener la productividad y estar entretenido en cualquier parte. Mira más, lleva menos gracias a su diseño fino y ligero, ideal para viajar. Mira más fotos, videos y proyectos en una pantalla con bisel y microbordes de 6,5 mm. Supera los días de mayor actividad gracias al rendimiento de su confiable procesador y conserva más de lo que amas con el abundante almacenamiento.",
                9,
                2119000,
                "2023-04-14T00:28:01.1201016",
                "2023-04-14T00:28:01.1201016",
                false,
                "https://www.ktronix.com/medias/195042341829-003-1400Wx1400H?context=bWFzdGVyfGltYWdlc3wyOTM0Mjl8aW1hZ2UvanBlZ3xpbWFnZXMvaDkyL2hkNC85ODM1NTgwNzE5MTM0LmpwZ3wzNmJjYTM5YjZmMjMwNDI0MjJkNWM1YTllZjMzOGNhMTczZTI2Mzk2YTZkYjQ1YWVlM2E3NDc0YjRjOGY4MzFm"
            });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
