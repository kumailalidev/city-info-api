using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CityInfo.API.Migrations
{
    /// <inheritdoc />
    public partial class SeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Cities",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { 1, "The largest city of Pakistan, famous for its beaches, historical buildings, vibrant food culture, and Quaid-e-Azam's Mausoleum.", "Karachi" },
                    { 2, "The cultural capital of Pakistan, famous for Badshahi Mosque, Lahore Fort, Shalimar Gardens, and its delicious traditional food.", "Lahore" },
                    { 3, "The capital city of Pakistan, known for its beautiful greenery, modern infrastructure, Faisal Mosque, and Margalla Hills.", "Islamabad" },
                    { 4, "One of the oldest cities in South Asia, famous for Qissa Khwani Bazaar, Peshawar Museum, traditional chapli kebabs, and rich Pashtun culture.", "Peshawar" },
                    { 5, "The capital of Balochistan, surrounded by mountains and known for its beautiful landscapes, fruit orchards, and traditional Balochi culture.", "Quetta" }
                });

            migrationBuilder.InsertData(
                table: "PointOfInterests",
                columns: new[] { "Id", "CityId", "Description", "Name" },
                values: new object[,]
                {
                    { 1, 1, "The mausoleum of Muhammad Ali Jinnah, the founder of Pakistan, and one of Karachi's most iconic landmarks.", "Mazar-e-Quaid" },
                    { 2, 1, "A popular beach along the Arabian Sea, known for its scenic coastline, camel rides, and recreational activities.", "Clifton Beach" },
                    { 3, 1, "A maritime museum showcasing Pakistan's naval history, ships, aircraft, and naval heritage.", "Pakistan Maritime Museum" },
                    { 4, 2, "A magnificent Mughal-era mosque known for its grand architecture and historical significance.", "Badshahi Mosque" },
                    { 5, 3, "One of Pakistan's largest mosques, famous for its unique modern architecture and scenic location.", "Faisal Mosque" },
                    { 6, 4, "A museum showcasing Gandhara art, Buddhist sculptures, and the region's rich archaeological history.", "Peshawar" },
                    { 7, 5, "A picturesque lake surrounded by mountains, popular for its beautiful scenery and recreational activities.", "Hanna Lake" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "PointOfInterests",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "PointOfInterests",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "PointOfInterests",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "PointOfInterests",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "PointOfInterests",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "PointOfInterests",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "PointOfInterests",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 5);
        }
    }
}
