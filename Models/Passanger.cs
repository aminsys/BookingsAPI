
using System.ComponentModel.DataAnnotations;

public class Passanger
{
    [Key]
    public long PassangerId { get; set; }
    public required string Name { get; set; }
    public required string PassportNumber { get; set; }

    public required string Nationality { get; set; }

    public DateTime BirthDate { get; set; }

    public DateTime ExpirationDate { get; set; }

    public string IssuingCountry { get; set; }

    public required string BookingNumber { get; set; }
}