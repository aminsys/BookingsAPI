using Microsoft.EntityFrameworkCore;

namespace BookingsAPI.Models;

public class PassengerContext : DbContext
{
    public PassengerContext(DbContextOptions<PassengerContext> options)
        : base(options)
    {
        Database.EnsureCreated();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Passenger>().HasData(
            new Passenger
            {
                PassengerId = 100001,
                Name = "Kalle Karlsson",
                BookingNumber = "BDT394",
                Nationality = "Swedish",
                BirthDate = new DateTime(2000, 10, 15),
                ExpirationDate = new DateTime(2027, 01, 21),
                IssuingCountry = "Sweden",
                PassportNumber = "R394223-2"
            },
            new Passenger
            {
                PassengerId = 100002,
                Name = "Saturo Jotaro",
                BookingNumber = "BDT394",
                Nationality = "Japanese",
                BirthDate = new DateTime(2003, 08, 02),
                ExpirationDate = new DateTime(2030, 12, 01),
                IssuingCountry = "Japan",
                PassportNumber = "9302-2-JP"
            },
            new Passenger
            {
                PassengerId = 100003,
                Name = "Axel Kjällström",
                BookingNumber = "BDT394",
                Nationality = "Swedish",
                BirthDate = new DateTime(1999, 09, 02),
                ExpirationDate = new DateTime(2026, 10, 11),
                IssuingCountry = "Sweden",
                PassportNumber = "M29003-4"
            },
            new Passenger
            {
                PassengerId = 100004,
                Name = "Mustafa Qamar",
                BookingNumber = "ABC001",
                Nationality = "Saudi",
                BirthDate = new DateTime(1987, 01, 19),
                ExpirationDate = new DateTime(2026, 12, 01),
                IssuingCountry = "KSA",
                PassportNumber = "IE02342"
            },
            new Passenger
            {
                PassengerId = 100005,
                Name = "Batbayar Enkhbold",
                BookingNumber = "ABC001",
                Nationality = "Mangolian",
                BirthDate = new DateTime(1990, 04, 22),
                ExpirationDate = new DateTime(2028, 12, 11),
                IssuingCountry = "Mangolia",
                PassportNumber = "7462323"
            }
        );
    }

    public DbSet<Passenger> Passenger { get; set; } = null!;
}