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
                        Description = "Уютна стая за един гост с всички основни удобства."
                    },
                    new RoomType
                    {
                        Name = "Двойна стая",
                        Description = "Комфортна стая за двама гости, подходяща за двойки или приятели."
                    },
                    new RoomType
                    {
                        Name = "Студио",
                        Description = "Просторно студио с кът за отдих и повече пространство."
                    },
                    new RoomType
                    {
                        Name = "Апартамент",
                        Description = "Голям апартамент с отделни помещения, тераса и допълнителни удобства."
                    }
                );

                context.SaveChanges();
            }

            var singleRoom = context.RoomTypes.First(r => r.Name == "Единична стая");
            var doubleRoom = context.RoomTypes.First(r => r.Name == "Двойна стая");
            var studio = context.RoomTypes.First(r => r.Name == "Студио");
            var apartment = context.RoomTypes.First(r => r.Name == "Апартамент");

            AddFloorType1(context, 1, singleRoom.Id, doubleRoom.Id, studio.Id, apartment.Id);
            AddFloorType2(context, 2, singleRoom.Id, doubleRoom.Id, studio.Id, apartment.Id);
            AddFloorType1(context, 3, singleRoom.Id, doubleRoom.Id, studio.Id, apartment.Id);
            AddFloorType1(context, 4, singleRoom.Id, doubleRoom.Id, studio.Id, apartment.Id);
            AddFloorType2(context, 5, singleRoom.Id, doubleRoom.Id, studio.Id, apartment.Id);
            AddFloorType2(context, 6, singleRoom.Id, doubleRoom.Id, studio.Id, apartment.Id);

            context.SaveChanges();
        }

        private static void AddFloorType1(
            ApplicationDbContext context,
            int floor,
            int singleRoomId,
            int doubleRoomId,
            int studioId,
            int apartmentId)
        {
            AddRoom(context, floor, 1, "Двойна стая", doubleRoomId);
            AddRoom(context, floor, 2, "Единична стая", singleRoomId);
            AddRoom(context, floor, 3, "Студио", studioId);
            AddRoom(context, floor, 4, "Единична стая", singleRoomId);
            AddRoom(context, floor, 5, "Двойна стая", doubleRoomId);
            AddRoom(context, floor, 6, "Студио", studioId);
            AddRoom(context, floor, 7, "Апартамент", apartmentId);
            AddRoom(context, floor, 8, "Единична стая", singleRoomId);
        }

        private static void AddFloorType2(
            ApplicationDbContext context,
            int floor,
            int singleRoomId,
            int doubleRoomId,
            int studioId,
            int apartmentId)
        {
            AddRoom(context, floor, 1, "Двойна стая", doubleRoomId);
            AddRoom(context, floor, 2, "Студио", studioId);
            AddRoom(context, floor, 3, "Студио", studioId);
            AddRoom(context, floor, 4, "Апартамент", apartmentId);
            AddRoom(context, floor, 5, "Студио", studioId);
            AddRoom(context, floor, 6, "Апартамент", apartmentId);
            AddRoom(context, floor, 7, "Апартамент", apartmentId);
        }

        private static void AddRoom(
            ApplicationDbContext context,
            int floor,
            int roomPosition,
            string roomTypeName,
            int roomTypeId)
        {
            int roomNumber = floor * 100 + roomPosition;
            string name = $"Стая {roomNumber}";

            if (context.Rooms.Any(r => r.Name == name))
            {
                return;
            }

            var room = new Room
            {
                Name = name,
                RoomTypeId = roomTypeId,
                Description = GetDescription(roomTypeName, floor, roomNumber),
                Price = GetPrice(roomTypeName, floor),
                SleepingCot = roomTypeName == "Двойна стая" || roomTypeName == "Студио" || roomTypeName == "Апартамент",
                SofaBed = roomTypeName == "Студио" || roomTypeName == "Апартамент",
                Photo = GetPhotos(roomTypeName),
                DateReg = DateTime.Now
            };

            context.Rooms.Add(room);
        }

        private static decimal GetPrice(string roomTypeName, int floor)
        {
            return roomTypeName switch
            {
                "Единична стая" => 65 + floor * 5,
                "Двойна стая" => 95 + floor * 6,
                "Студио" => 130 + floor * 8,
                "Апартамент" => 180 + floor * 12,
                _ => 80
            };
        }

        private static string GetDescription(string roomTypeName, int floor, int roomNumber)
        {
            return roomTypeName switch
            {
                "Единична стая" =>
                    $"Единична стая {roomNumber} на {floor} етаж, подходяща за един гост. Разполага с климатик, телевизор, хладилник, баня, сешоар и уютна зона за отдих.",

                "Двойна стая" =>
                    $"Двойна стая {roomNumber} на {floor} етаж, подходяща за двама гости. Разполага с комфортно легло, климатик, телевизор, хладилник, баня, тераса и възможност за детска кошара.",

                "Студио" =>
                    $"Студио {roomNumber} на {floor} етаж с повече пространство, кът за отдих, разтегателен диван, климатик, телевизор, хладилник, баня и тераса.",

                "Апартамент" =>
                    $"Апартамент {roomNumber} на {floor} етаж с отделни помещения, просторна спалня, дневна зона, разтегателен диван, тераса, две бани и удобства за семейна почивка.",

                _ =>
                    $"Стая {roomNumber} на {floor} етаж с основни удобства за приятен престой."
            };
        }

        private static string GetPhotos(string roomTypeName)
        {
            return roomTypeName switch
            {
                "Единична стая" =>
                    "/images/rooms/single-1.jpg;/images/rooms/single-2.jpg;/images/rooms/single-3.jpg",

                "Двойна стая" =>
                    "/images/rooms/double-1.jpg;/images/rooms/double-2.jpg;/images/rooms/double-3.jpg",

                "Студио" =>
                    "/images/rooms/studio-1.jpg;/images/rooms/studio-2.jpg;/images/rooms/studio-3.jpg",

                "Апартамент" =>
                    "/images/rooms/apartment-1.jpg;/images/rooms/apartment-2.jpg;/images/rooms/apartment-3.jpg",

                _ =>
                    "/images/rooms/default-1.jpg"
            };
        }
    }
}