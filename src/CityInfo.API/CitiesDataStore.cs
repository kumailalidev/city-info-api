using CityInfo.API.Models;

namespace CityInfo.API;

public class CitiesDataStore
{
    public static CitiesDataStore Current { get; } = new();
    public List<CityDto> Cities { get; set; }

    public CitiesDataStore()
    {
        Cities =
        [
            new()
            {
                Id = 1,
                Name = "Karachi",
                Description = "The largest city of Pakistan, famous for its beaches, historical buildings, vibrant food culture, and Quaid-e-Azam's Mausoleum."
            },
            new()
            {
                Id = 2,
                Name = "Lahore",
                Description = "The cultural capital of Pakistan, famous for Badshahi Mosque, Lahore Fort, Shalimar Gardens, and its delicious traditional food."
            },
            new()
            {
                Id = 3,
                Name = "Islamabad",
                Description = "The capital city of Pakistan, known for its beautiful greenery, modern infrastructure, Faisal Mosque, and Margalla Hills."
            },
            new()
            {
                Id = 4,
                Name = "Peshawar",
                Description = "One of the oldest cities in South Asia, famous for Qissa Khwani Bazaar, Peshawar Museum, traditional chapli kebabs, and rich Pashtun culture."
            },
            new()
            {
                Id = 5,
                Name = "Quetta",
                Description = "The capital of Balochistan, surrounded by mountains and known for its beautiful landscapes, fruit orchards, and traditional Balochi culture."
            },
            new()
            {
                Id = 6,
                Name = "Multan",
                Description = "Known as the City of Saints, famous for its historic shrines, blue pottery, traditional handicrafts, and delicious Sohan Halwa."
            },
            new()
            {
                Id = 7,
                Name = "Faisalabad",
                Description = "An important industrial city of Pakistan, famous for its textile industry, Clock Tower, and eight bazaars extending from the city center."
            },
            new()
            {
                Id = 8,
                Name = "Hyderabad",
                Description = "A historic city in Sindh, famous for Sindhi culture, traditional bangles, Rani Bagh, and the historic Pacco Qillo."
            },
            new()
            {
                Id = 9,
                Name = "Murree",
                Description = "A popular hill station in Punjab, known for its cool weather, pine forests, beautiful mountain views, and Mall Road."
            },
            new()
            {
                Id = 10,
                Name = "Skardu",
                Description = "A breathtaking city in Gilgit-Baltistan, famous for its mountains, lakes, Shangrila Resort, and as a gateway to some of the world's highest peaks."
            },
            new()
            {
                Id = 11,
                Name = "Gilgit",
                Description = "A beautiful mountain city in northern Pakistan, known for its scenic valleys, rivers, mountain landscapes, and rich cultural heritage."
            },
            new()
            {
                Id = 12,
                Name = "Sialkot",
                Description = "An internationally recognized industrial city, famous for manufacturing sports goods, surgical instruments, and leather products."
            },
            new()
            {
                Id = 13,
                Name = "Bahawalpur",
                Description = "A historic city in southern Punjab, famous for Noor Mahal, Derawar Fort, the Cholistan Desert, and its royal heritage."
            },
            new()
            {
                Id = 14,
                Name = "Gwadar",
                Description = "A coastal city in Balochistan, known for its deep-sea port, beautiful beaches, Hammerhead Peninsula, and stunning Arabian Sea views."
            },
            new()
            {
                Id = 15,
                Name = "Abbottabad",
                Description = "A scenic city in Khyber Pakhtunkhwa, known for its pleasant weather, green hills, educational institutions, and beautiful surroundings."
            }
        ];
    }
}