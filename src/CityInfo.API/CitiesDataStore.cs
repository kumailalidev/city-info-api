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
                Description = "The largest city of Pakistan, famous for its beaches, historical buildings, vibrant food culture, and Quaid-e-Azam's Mausoleum.",
                PointsOfInterest =
                [
                    new()
                    {
                        Id = 1,
                        Name = "Mazar-e-Quaid",
                        Description = "The mausoleum of Muhammad Ali Jinnah, the founder of Pakistan, and one of Karachi's most iconic landmarks."
                    },
                    new()
                    {
                        Id = 2,
                        Name = "Clifton Beach",
                        Description = "A popular beach along the Arabian Sea, known for its scenic coastline, camel rides, and recreational activities."
                    },
                    new()
                    {
                        Id = 3,
                        Name = "Pakistan Maritime Museum",
                        Description = "A maritime museum showcasing Pakistan's naval history, ships, aircraft, and naval heritage."
                    }
                ]
            },
            new()
            {
                Id = 2,
                Name = "Lahore",
                Description = "The cultural capital of Pakistan, famous for Badshahi Mosque, Lahore Fort, Shalimar Gardens, and its delicious traditional food.",
                PointsOfInterest =
                [
                    new()
                    {
                        Id = 1,
                        Name = "Badshahi Mosque",
                        Description = "A magnificent Mughal-era mosque known for its grand architecture and historical significance."
                    },
                    new()
                    {
                        Id = 2,
                        Name = "Lahore Fort",
                        Description = "A historic Mughal fortress featuring beautiful palaces, gardens, and architectural masterpieces."
                    },
                    new()
                    {
                        Id = 3,
                        Name = "Shalimar Gardens",
                        Description = "A UNESCO World Heritage Site featuring beautiful Mughal gardens, fountains, and terraces."
                    }
                ]
            },
            new()
            {
                Id = 3,
                Name = "Islamabad",
                Description = "The capital city of Pakistan, known for its beautiful greenery, modern infrastructure, Faisal Mosque, and Margalla Hills.",
                PointsOfInterest =
                [
                    new()
                    {
                        Id = 1,
                        Name = "Faisal Mosque",
                        Description = "One of Pakistan's largest mosques, famous for its unique modern architecture and scenic location."
                    },
                    new()
                    {
                        Id = 2,
                        Name = "Pakistan Monument",
                        Description = "A national monument representing Pakistan's four provinces and three territories."
                    },
                    new()
                    {
                        Id = 3,
                        Name = "Daman-e-Koh",
                        Description = "A popular viewpoint in the Margalla Hills offering panoramic views of Islamabad."
                    }
                ]
            },
            new()
            {
                Id = 4,
                Name = "Peshawar",
                Description = "One of the oldest cities in South Asia, famous for Qissa Khwani Bazaar, Peshawar Museum, traditional chapli kebabs, and rich Pashtun culture.",
                PointsOfInterest =
                [
                    new()
                    {
                        Id = 1,
                        Name = "Qissa Khwani Bazaar",
                        Description = "A historic marketplace known as the Street of Storytellers, famous for its traditional shops and cultural heritage."
                    },
                    new()
                    {
                        Id = 2,
                        Name = "Peshawar Museum",
                        Description = "A museum showcasing Gandhara art, Buddhist sculptures, and the region's rich archaeological history."
                    }
                ]
            },
            new()
            {
                Id = 5,
                Name = "Quetta",
                Description = "The capital of Balochistan, surrounded by mountains and known for its beautiful landscapes, fruit orchards, and traditional Balochi culture.",
                PointsOfInterest =
                [
                    new()
                    {
                        Id = 1,
                        Name = "Hanna Lake",
                        Description = "A picturesque lake surrounded by mountains, popular for its beautiful scenery and recreational activities."
                    },
                    new()
                    {
                        Id = 2,
                        Name = "Hazarganji-Chiltan National Park",
                        Description = "A national park known for its mountainous landscapes, wildlife, and natural beauty."
                    }
                ]
            },
            new()
            {
                Id = 6,
                Name = "Multan",
                Description = "Known as the City of Saints, famous for its historic shrines, blue pottery, traditional handicrafts, and delicious Sohan Halwa.",
                PointsOfInterest =
                [
                    new()
                    {
                        Id = 1,
                        Name = "Shrine of Shah Rukn-e-Alam",
                        Description = "A magnificent Sufi shrine famous for its distinctive architecture and blue dome."
                    },
                    new()
                    {
                        Id = 2,
                        Name = "Multan Fort",
                        Description = "A historic fort area known for its ancient ruins, panoramic views, and religious landmarks."
                    }
                ]
            },
            new()
            {
                Id = 7,
                Name = "Faisalabad",
                Description = "An important industrial city of Pakistan, famous for its textile industry, Clock Tower, and eight bazaars extending from the city center.",
                PointsOfInterest =
                [
                    new()
                    {
                        Id = 1,
                        Name = "Ghanta Ghar (Clock Tower)",
                        Description = "An iconic clock tower surrounded by eight traditional bazaars arranged in a distinctive radial pattern."
                    },
                    new()
                    {
                        Id = 2,
                        Name = "Jinnah Garden",
                        Description = "A popular public park offering green spaces, walking paths, and recreational areas."
                    }
                ]
            },
            new()
            {
                Id = 8,
                Name = "Hyderabad",
                Description = "A historic city in Sindh, famous for Sindhi culture, traditional bangles, Rani Bagh, and the historic Pacco Qillo.",
                PointsOfInterest =
                [
                    new()
                    {
                        Id = 1,
                        Name = "Pacco Qillo",
                        Description = "A historic fort built during the Kalhora dynasty, representing Hyderabad's rich architectural heritage."
                    },
                    new()
                    {
                        Id = 2,
                        Name = "Rani Bagh",
                        Description = "A historic public garden and recreational park popular among local families."
                    }
                ]
            },
            new()
            {
                Id = 9,
                Name = "Murree",
                Description = "A popular hill station in Punjab, known for its cool weather, pine forests, beautiful mountain views, and Mall Road.",
                PointsOfInterest =
                [
                    new()
                    {
                        Id = 1,
                        Name = "Mall Road",
                        Description = "A lively shopping and entertainment street featuring restaurants, shops, and local markets."
                    },
                    new()
                    {
                        Id = 2,
                        Name = "Patriata (New Murree)",
                        Description = "A scenic hill destination famous for its chairlift, cable car, and beautiful mountain views."
                    }
                ]
            },
            new()
            {
                Id = 10,
                Name = "Skardu",
                Description = "A breathtaking city in Gilgit-Baltistan, famous for its mountains, lakes, Shangrila Resort, and as a gateway to some of the world's highest peaks.",
                PointsOfInterest =
                [
                    new()
                    {
                        Id = 1,
                        Name = "Shangrila Resort",
                        Description = "A scenic resort beside a heart-shaped lake, surrounded by dramatic mountain landscapes."
                    },
                    new()
                    {
                        Id = 2,
                        Name = "Upper Kachura Lake",
                        Description = "A beautiful alpine lake known for its crystal-clear water and stunning mountain scenery."
                    },
                    new()
                    {
                        Id = 3,
                        Name = "Shigar Fort",
                        Description = "A historic fort restored as a heritage hotel, showcasing traditional Balti architecture."
                    }
                ]
            },
            new()
            {
                Id = 11,
                Name = "Gilgit",
                Description = "A beautiful mountain city in northern Pakistan, known for its scenic valleys, rivers, mountain landscapes, and rich cultural heritage.",
                PointsOfInterest =
                [
                    new()
                    {
                        Id = 1,
                        Name = "Kargah Buddha",
                        Description = "An ancient rock carving of Buddha dating back to the Buddhist period in the Gilgit region."
                    },
                    new()
                    {
                        Id = 2,
                        Name = "Gilgit River",
                        Description = "A scenic river flowing through the region, offering beautiful views of the surrounding mountains."
                    }
                ]
            },
            new()
            {
                Id = 12,
                Name = "Sialkot",
                Description = "An internationally recognized industrial city, famous for manufacturing sports goods, surgical instruments, and leather products.",
                PointsOfInterest =
                [
                    new()
                    {
                        Id = 1,
                        Name = "Iqbal Manzil",
                        Description = "The birthplace and childhood home of Allama Muhammad Iqbal, now preserved as a museum."
                    },
                    new()
                    {
                        Id = 2,
                        Name = "Sialkot Fort",
                        Description = "An ancient fort representing the city's long and rich historical heritage."
                    }
                ]
            },
            new()
            {
                Id = 13,
                Name = "Bahawalpur",
                Description = "A historic city in southern Punjab, famous for Noor Mahal, Derawar Fort, the Cholistan Desert, and its royal heritage.",
                PointsOfInterest =
                [
                    new()
                    {
                        Id = 1,
                        Name = "Noor Mahal",
                        Description = "A magnificent palace built during the princely state of Bahawalpur, featuring Italian-style architecture."
                    },
                    new()
                    {
                        Id = 2,
                        Name = "Derawar Fort",
                        Description = "A massive historical fortress in the Cholistan Desert, famous for its enormous bastions."
                    }
                ]
            },
            new()
            {
                Id = 14,
                Name = "Gwadar",
                Description = "A coastal city in Balochistan, known for its deep-sea port, beautiful beaches, Hammerhead Peninsula, and stunning Arabian Sea views.",
                PointsOfInterest =
                [
                    new()
                    {
                        Id = 1,
                        Name = "Hammerhead Peninsula",
                        Description = "A distinctive rocky headland offering spectacular views of Gwadar's coastline and Arabian Sea."
                    },
                    new()
                    {
                        Id = 2,
                        Name = "Koh-e-Batil",
                        Description = "A hill overlooking Gwadar city and its deep-sea port, offering panoramic coastal views."
                    }
                ]
            },
            new()
            {
                Id = 15,
                Name = "Abbottabad",
                Description = "A scenic city in Khyber Pakhtunkhwa, known for its pleasant weather, green hills, educational institutions, and beautiful surroundings.",
                PointsOfInterest =
                [
                    new()
                    {
                        Id = 1,
                        Name = "Ilyasi Mosque",
                        Description = "A historic mosque known for its beautiful surroundings and a natural spring nearby."
                    },
                    new()
                    {
                        Id = 2,
                        Name = "Thandiani",
                        Description = "A scenic hill station near Abbottabad, famous for its pine forests, cool climate, and mountain views."
                    }
                ]
            }
        ];
    }
}