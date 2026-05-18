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
                        Description = "Уютна стая за един гост."
                    },
                    new RoomType
                    {
                        Name = "Двойна стая",
                        Description = "Комфортна стая за двама."
                    },
                    new RoomType
                    {
                        Name = "Студио",
                        Description = "Просторно студио с кът за отдих."
                    },
                    new RoomType
                    {
                        Name = "Апартамент",
                        Description = "Апартамент с отделни помещения."
                    }
                );

                context.SaveChanges();
            }

            var single = context.RoomTypes.First(x => x.Name == "Единична стая");
            var dbl = context.RoomTypes.First(x => x.Name == "Двойна стая");
            var studio = context.RoomTypes.First(x => x.Name == "Студио");
            var apartment = context.RoomTypes.First(x => x.Name == "Апартамент");

            AddFloorType1(context, 1, single.Id, dbl.Id, studio.Id, apartment.Id);
            AddFloorType2(context, 2, single.Id, dbl.Id, studio.Id, apartment.Id);
            AddFloorType1(context, 3, single.Id, dbl.Id, studio.Id, apartment.Id);
            AddFloorType1(context, 4, single.Id, dbl.Id, studio.Id, apartment.Id);
            AddFloorType2(context, 5, single.Id, dbl.Id, studio.Id, apartment.Id);
            AddFloorType2(context, 6, single.Id, dbl.Id, studio.Id, apartment.Id);

            context.SaveChanges();
        }

        private static void AddFloorType1(ApplicationDbContext context, int floor, int singleId, int doubleId, int studioId, int apartmentId)
        {
            AddRoom(context, floor, 1, "Двойна стая", doubleId);
            AddRoom(context, floor, 2, "Единична стая", singleId);
            AddRoom(context, floor, 3, "Студио", studioId);
            AddRoom(context, floor, 4, "Единична стая", singleId);
            AddRoom(context, floor, 5, "Двойна стая", doubleId);
            AddRoom(context, floor, 6, "Студио", studioId);
            AddRoom(context, floor, 7, "Апартамент", apartmentId);
            AddRoom(context, floor, 8, "Единична стая", singleId);
        }

        private static void AddFloorType2(ApplicationDbContext context, int floor, int singleId, int doubleId, int studioId, int apartmentId)
        {
            AddRoom(context, floor, 1, "Двойна стая", doubleId);
            AddRoom(context, floor, 2, "Студио", studioId);
            AddRoom(context, floor, 3, "Студио", studioId);
            AddRoom(context, floor, 4, "Апартамент", apartmentId);
            AddRoom(context, floor, 5, "Студио", studioId);
            AddRoom(context, floor, 6, "Апартамент", apartmentId);
            AddRoom(context, floor, 7, "Апартамент", apartmentId);
        }

        private static void AddRoom(ApplicationDbContext context, int floor, int position, string typeName, int roomTypeId)
        {
            int roomNumber = floor * 100 + position;
            string roomName = $"Стая {roomNumber}";

            if (context.Rooms.Any(r => r.Name == roomName))
                return;

            context.Rooms.Add(new Room
            {
                Name = roomName,
                RoomTypeId = roomTypeId,
                Description = GetDescription(typeName, floor),
                Price = GetPrice(typeName, floor),
                SleepingCot = typeName != "Единична стая",
                SofaBed = typeName == "Студио" || typeName == "Апартамент",
                Photo = GetPhotos(typeName, roomNumber),
                DateReg = DateTime.Now
            });
        }

        private static int GetPrice(string typeName, int floor)
        {
            return typeName switch
            {
                "Единична стая" => 70 + (floor * 4),
                "Двойна стая" => 105 + (floor * 5),
                "Студио" => 145 + (floor * 6),
                "Апартамент" => 220 + (floor * 10),
                _ => 100
            };
        }

        private static string GetDescription(string typeName, int floor)
        {
            return typeName switch
            {
                "Единична стая" => $"Уютна единична стая на {floor} етаж с баня и удобства.",
                "Двойна стая" => $"Комфортна двойна стая на {floor} етаж с тераса.",
                "Студио" => $"Просторно студио на {floor} етаж с кът за отдих.",
                "Апартамент" => $"Апартамент на {floor} етаж с отделни помещения и тераси.",
                _ => $"Стая на {floor} етаж."
            };
        }

        private static string GetPhotos(string typeName, int roomNumber)
        {
            switch (typeName)
            {
                case "Единична стая":
                    return roomNumber % 2 == 0
                        ? "/images/rooms/single-1.jpg;/images/rooms/bath-1.png;/images/rooms/terrace-1.png"
                        : "/images/rooms/single-2.jpg;/images/rooms/bath-2.png;/images/rooms/terrace-2.png";

                case "Двойна стая":
                    return roomNumber % 2 == 0
                        ? "/images/rooms/double-1.jpg;/images/rooms/bath-1.png;/images/rooms/terrace-1.png"
                        : "/images/rooms/double-2.jpg;/images/rooms/bath-2.png;/images/rooms/terrace-2.png";

                case "Студио":
                    return roomNumber % 2 == 0
                        ? "/images/rooms/studio-1.jpg;/images/rooms/bath-1.png;/images/rooms/terrace-1.png"
                        : "/images/rooms/studio-2.jpg;/images/rooms/bath-2.png;/images/rooms/terrace-2.png";

                case "Апартамент":
                    return roomNumber % 2 == 0
                        ? "/images/rooms/apartment-1.jpg;/images/rooms/bath-1.png;/images/rooms/terrace-1.png;/images/rooms/bath-2.png"
                        : "/images/rooms/apartment-1.jpg;/images/rooms/bath-2.png;/images/rooms/terrace-2.png;/images/rooms/bath-1.png";

                default:
                    return "/images/no-image.jpg";
            }
        }
    }
}