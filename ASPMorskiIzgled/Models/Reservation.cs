using Humanizer;
using Microsoft.VisualBasic;

namespace ASPMorskiIzgled.Models
{
    public class Reservation
    {
        public int Id { get; set; } 

        public int RoomId { get; set; }//FK
        public Room Rooms { get; set; } //table

        public string ClientId { get; set; }//FK
        public Client Clients { get; set; }//table

        public DateTime DateIn { get; set; }
        public DateTime DateOut { get; set; }
        public DateTime ReservationDate { get; set; }


    }
}
