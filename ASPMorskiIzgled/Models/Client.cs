using Microsoft.AspNetCore.Identity;

namespace ASPMorskiIzgled.Models
{
    public class Client : IdentityUser
    {
        //public string Id {  get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        //public string Email { get; set; }
        //public string PhoneNumber { get; set; }
        public DateTime DateReg { get; set; }
        public ICollection<Reservation> Reservations { get; set; }

    }
}
