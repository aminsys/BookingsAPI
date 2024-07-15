using System.ComponentModel.DataAnnotations;
using BookingsAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookingsAPI.Controllers;

[ApiController]
public class PassangerController : ControllerBase
{

    private readonly PassangerContext _context;
    private readonly ILogger<PassangerController> _logger;

    public PassangerController(ILogger<PassangerController> logger, PassangerContext context)
    {
        _logger = logger;
        _context = context;
    }

    [HttpGet("passangers/{bookingNumber}")]
    public IEnumerable<Passanger> GetPassangersWithBookingNumber(string bookingNumber)
    {
        _logger.LogInformation($"Fetching passangers with booking number {bookingNumber}...");

        var result = new List<Passanger>();

        try
        {
            result = _context.Passanger.Where(p => p.BookingNumber == bookingNumber).Select(p => new Passanger
            {
                BookingNumber = p.BookingNumber,
                Name = p.Name,
                PassangerId = p.PassangerId,
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

    [HttpPost("passanger")]
    public ActionResult AddNewPassanger(Passanger passanger)
    {
        _logger.LogInformation($"Creating new entry for passanger {passanger.Name}...");

        try
        {
            _context.Passanger.Add(passanger);
            _context.SaveChanges();
            return Created();
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error while inserting passanger to database.\n{ex.Message}");
            return BadRequest();
        }
    }

    [HttpPut("passangers/{passangerId}")]
    public ActionResult UpdatePassanger(long passangerId, Passanger passanger)
    {
        try
        {
            _logger.LogInformation($"Updating passanger with Id {passangerId}...");
            var oldPassangerData = _context.Passanger.Find(passangerId);
            if (oldPassangerData is null)
            {
                _logger.LogInformation($"Passanger with Id {passangerId} doesn't exist");
                return BadRequest();
            }

            oldPassangerData.Name = passanger.Name;
            oldPassangerData.PassportNumber = passanger.PassportNumber;
            oldPassangerData.BirthDate = passanger.BirthDate;
            oldPassangerData.ExpirationDate = passanger.ExpirationDate;
            oldPassangerData.BookingNumber = passanger.BookingNumber;
            oldPassangerData.IssuingCountry = passanger.IssuingCountry;

            _context.Passanger.Update(oldPassangerData);
            _context.SaveChanges();

            return NoContent();
        }

        catch (Exception ex)
        {
            _logger.LogError($"Error while inserting data to database.\n{ex.Message}");
            return BadRequest();
        }
    }

    [HttpDelete("passangers/{passangerId}")]
    public ActionResult DeletePassanger(long passangerId)
    {
        _logger.LogInformation($"Deleting passanger with Id {passangerId}...");

        try
        {
            var passangerToDelete = _context.Passanger.Find(passangerId);
            if (passangerToDelete is null)
            {
                _logger.LogInformation($"Passanger with Id {passangerId} doesn't exist");
                return BadRequest();
            }

            _context.Passanger.Remove(passangerToDelete);
            _context.SaveChanges();

            return NoContent();
        }

        catch (Exception ex)
        {
            _logger.LogError($"Error while deleting passanger from database.\n{ex.Message}");
            return BadRequest();
        }
    }

    [HttpDelete("booking/{bookingNumber}")]
    public ActionResult DeleteBookingWithPassangers(string bookingNumber)
    {
        _logger.LogInformation($"Deleting booking with number {bookingNumber}...");
        try
        {
            var passangers = _context.Passanger.Where(p => p.BookingNumber == bookingNumber);
            if(passangers is null)
            {
                _logger.LogInformation($"Booking with number {bookingNumber} doesn't exist");
                return BadRequest();
            }

            _context.Passanger.RemoveRange(passangers);
            _context.SaveChanges();
            _logger.LogInformation($"Deleted booking with number {bookingNumber}");

            return NoContent();
        }

        catch(Exception ex)
        {
            _logger.LogError($"Error while deleting a booking with its passanger(s) from database.\n{ex.Message}");
            return BadRequest();
        }
    }

}