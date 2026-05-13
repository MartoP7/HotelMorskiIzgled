using ASPMorskiIzgled.Models;

namespace ASPMorskiIzgled.Data
{
    public static class DbSeeder
    {
        public static void SeedRooms(ApplicationDbContext context)
        {
            context.Database.EnsureCreated();

            if (!context.RoomTypes.Any())
            {
                context.RoomTypes.AddRange(
                    new RoomType
                    {
                        Name = "Единична стая",
                        Description = "Подходяща за един гост."
                    },
                    new RoomType
                    {
                        Name = "Двойна стая",
                        Description = "Комфортна стая за двама."
                    },
                    new RoomType
                    {
                        Name = "Студио",
                        Description = "Просторно студио."
                    },
                    new RoomType
                    {
                        Name = "Апартамент",
                        Description = "Луксозен апартамент."
                    }
                );

                context.SaveChanges();
            }

            if (context.Rooms.Any(r => r.Name.StartsWith("Стая ")))
            {
                return;
            }

            var singleRoom = context.RoomTypes.First(r => r.Name == "Единична стая");
            var doubleRoom = context.RoomTypes.First(r => r.Name == "Двойна стая");
            var studio = context.RoomTypes.First(r => r.Name == "Студио");
            var apartment = context.RoomTypes.First(r => r.Name == "Апартамент");

            var rooms = new List<Room>();

            for (int floor = 1; floor <= 6; floor++)
            {
                for (int number = 1; number <= 8; number++)
                {
                    int roomNumber = floor * 100 + number;

                    RoomType roomType;
                    decimal price;

                    if (number <= 2)
                    {
                        roomType = singleRoom;
                        price = 70 + (floor * 5);
                    }
                    else if (number <= 5)
                    {
                        roomType = doubleRoom;
                        price = 100 + (floor * 5);
                    }
                    else if (number <= 7)
                    {
                        roomType = studio;
                        price = 140 + (floor * 5);
                    }
                    else
                    {
                        roomType = apartment;
                        price = 190 + (floor * 10);
                    }

                    rooms.Add(new Room
                    {
                        Name = $"Стая {roomNumber}",
                        Description = $"Стая {roomNumber} с комфортно обзавеждане и приятна атмосфера.",
                        RoomTypeId = roomType.Id,
                        Price = price,
                        SleepingCot = number % 2 == 0,
                        SofaBed = number >= 5,
                        Photo = $"/images/rooms/room{number}-1.jpg;/images/rooms/room{number}-2.jpg;/images/rooms/room{number}-3.jpg"
                    });
                }
            }

            context.Rooms.AddRange(rooms);
            context.SaveChanges();
        }
    }
}