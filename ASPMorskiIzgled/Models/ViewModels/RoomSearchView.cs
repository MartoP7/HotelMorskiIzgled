using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace ASPMorskiIzgled.Models.ViewModels
{
    [NotMapped]
    public class RoomSearchView
    {
        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "От дата")]
        public DateTime DateIn { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "До дата")]
        public DateTime DateOut { get; set; }

        [Required]
        [Range(1, 20)]
        [Display(Name = "Брой възрастни")]
        public int Adults { get; set; }

        [Range(0, 20)]
        [Display(Name = "Брой деца")]
        public int Children { get; set; }

        [Range(0, 10)]
        [Display(Name = "Брой бебета")]
        public int Babies { get; set; }

        public List<Room> AvailableRooms { get; set; } = new List<Room>();
    }
}
