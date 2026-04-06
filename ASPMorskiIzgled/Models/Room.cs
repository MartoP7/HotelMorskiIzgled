using System.ComponentModel.DataAnnotations.Schema;

namespace ASPMorskiIzgled.Models
{
    public class Room
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public int RoomTypeId { get; set; } //FK
        public RoomType RoomTypes { get; set; } //table
        public string Description { get; set; }
        public bool SleepingCot { get; set; }
        public bool SofaBed { get; set; }
        public string Photo { get; set; }
        [Column(TypeName ="decimal(10,2)")]
        public decimal Price { get; set; }
        public ICollection<Reservation> Reservations { get; set; }
        public DateTime DateReg { get; set; }
    }
}
