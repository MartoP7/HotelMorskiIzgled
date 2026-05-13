using ASPMorskiIzgled.Models;

namespace ASPMorskiIzgled.Models.ViewModels
{
    public class ReservationReportView
    {
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }

        public int TotalReservations { get; set; }
        public int TotalNights { get; set; }
        public decimal TotalIncome { get; set; }
        public decimal AverageReservationIncome { get; set; }

        public List<ReservationReportItem> Reservations { get; set; } = new List<ReservationReportItem>();
    }

    public class ReservationReportItem
    {
        public int Id { get; set; }
        public string RoomName { get; set; }
        public string ClientName { get; set; }

        public DateTime DateIn { get; set; }
        public DateTime DateOut { get; set; }

        public int Nights { get; set; }
        public decimal PricePerNight { get; set; }
        public decimal TotalPrice { get; set; }
    }
}