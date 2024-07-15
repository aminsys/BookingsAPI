using System.ComponentModel.DataAnnotations;
using BookingsAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookingsAPI.Controllers;

[ApiController]
public class PassengerController : ControllerBase
{

    private readonly PassengerContext _context;
    private readonly ILogger<PassengerController> _logger;

    public PassengerController(ILogger<PassengerController> logger, PassengerContext context)
    {
        _logger = logger;
        _context = context;
    }

    [HttpGet("passengers/{bookingNumber}")]
    public IEnumerable<Passenger> GetPassengersWithBookingNumber(string bookingNumber)
    {
        _logger.LogInformation($"Fetching passengers with booking number {bookingNumber}...");

        var result = new List<Passenger>();

        try
        {
            result = _context.Passenger.Where(p => p.BookingNumber == bookingNumber).Select(p => new Passenger
            {
                BookingNumber = p.BookingNumber,
                Name = p.Name,
                PassengerId = p.PassengerId,
                Nationality = p.Nationality,
                IssuingCountry = p.IssuingCountry,
                BirthDate = p.BirthDate,
                ExpirationDate = p.ExpirationDate,
                PassportNumber = p.PassportNumber
            }).ToList();
        }

        catch (Exception ex)
        {
            _logger.LogError($"Error while getting data from database.\n{ex.Message}");
        }

        return result;
    }

    [HttpPost("passenger")]
    public ActionResult AddNewPassenger(Passenger passenger)
    {
        _logger.LogInformation($"Creating new entry for passenger {passenger.Name}...");

        try
        {
            _context.Passenger.Add(passenger);
            _context.SaveChanges();
            return Created();
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error while inserting passenger to database.\n{ex.Message}");
            return BadRequest();
        }
    }

    [HttpPut("passengers/{passengerId}")]
    public ActionResult UpdatePassenger(long passengerId, Passenger passenger)
    {
        try
        {
            _logger.LogInformation($"Updating passenger with Id {passengerId}...");
            var oldPassengerData = _context.Passenger.Find(passengerId);
            if (oldPassengerData is null)
            {
                _logger.LogInformation($"Passenger with Id {passengerId} doesn't exist");
                return BadRequest();
            }

            oldPassengerData.Name = passenger.Name;
            oldPassengerData.PassportNumber = passenger.PassportNumber;
            oldPassengerData.BirthDate = passenger.BirthDate;
            oldPassengerData.ExpirationDate = passenger.ExpirationDate;
            oldPassengerData.BookingNumber = passenger.BookingNumber;
            oldPassengerData.IssuingCountry = passenger.IssuingCountry;

            _context.Passenger.Update(oldPassengerData);
            _context.SaveChanges();

            return NoContent();
        }

        catch (Exception ex)
        {
            _logger.LogError($"Error while inserting data to database.\n{ex.Message}");
            return BadRequest();
        }
    }

    [HttpDelete("passengers/{passengerId}")]
    public ActionResult DeletePassenger(long passengerId)
    {
        _logger.LogInformation($"Deleting passenger with Id {passengerId}...");

        try
        {
            var passengerToDelete = _context.Passenger.Find(passengerId);
            if (passengerToDelete is null)
            {
                _logger.LogInformation($"Passenger with Id {passengerId} doesn't exist");
                return BadRequest();
            }

            _context.Passenger.Remove(passengerToDelete);
            _context.SaveChanges();

            return NoContent();
        }

        catch (Exception ex)
        {
            _logger.LogError($"Error while deleting passenger from database.\n{ex.Message}");
            return BadRequest();
        }
    }

    [HttpDelete("booking/{bookingNumber}")]
    public ActionResult DeleteBookingWithPassengers(string bookingNumber)
    {
        _logger.LogInformation($"Deleting booking with number {bookingNumber}...");
        try
        {
            var passengers = _context.Passenger.Where(p => p.BookingNumber == bookingNumber);
            if(passengers is null)
            {
                _logger.LogInformation($"Booking with number {bookingNumber} doesn't exist");
                return BadRequest();
            }

            _context.Passenger.RemoveRange(passengers);
            _context.SaveChanges();
            _logger.LogInformation($"Deleted booking with number {bookingNumber}");

            return NoContent();
        }

        catch(Exception ex)
        {
            _logger.LogError($"Error while deleting a booking with its passenger(s) from database.\n{ex.Message}");
            return BadRequest();
        }
    }

}