using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrackingDeviceLib.Dtos;
using TrackingDeviceLib.Models;
using TrackingDeviceLib.Services;
using TrackingDeviceLib.Services.Interfaces;
using TrackingDeviceLib.Services.Repositories;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TrackingDeviceREST.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TrackingDeviceController : ControllerBase
{
	private ITrackingDeviceRepo _repo;
	public static Location? _latestLocation;
    public static DateTime _lastSavedTime = DateTime.MinValue;


	public TrackingDeviceController(ITrackingDeviceRepo repo)
	{
		_repo = repo;
	}


    //GET: api/<TrackingDeviceController>
    [HttpGet]
    public IEnumerable<Location> Get()
    {
    	return _repo.GetAll();
    }

    // GET api/<TrackingDeviceController>/5
    [HttpGet("{id}")]
	public Location? GetById(int id)
	{
		return _repo.GetById(id);
	}

    // POST api/<TrackingDeviceController>
    [HttpPost("trackingbutton")]
    public async Task<IActionResult> PostTrackButton()
    {
        if (_latestLocation == null)
            return BadRequest("No location received yet");

        // Geocoding
        var geocoding = new Geocoding(
        new HttpClient(),
        "mmvpt-tracker/1.0 (clib@hotmail.com)"
        );

        _latestLocation.Address = await geocoding.ReverseGeocode(
            _latestLocation.Latitude,
            _latestLocation.Longitude
        );

        _latestLocation.Source = "Manuelt";
        return Ok(_repo.Add(_latestLocation));
    }

    [HttpPost("")]
    public async Task<IActionResult> Post([FromBody] Location value)
    {
        if (value == null)
            return BadRequest("Location is empty");

        // Geocode address
        var geocoding = new Geocoding(
            new HttpClient(),
            "mmvpt-tracker/1.0 (clib@hotmail.com)"
        );

        value.Address = await geocoding.ReverseGeocode(value.Latitude, value.Longitude);

        // Gem den nyeste lokation i memory
        _latestLocation = value;

        // Hvis det er første gang eller der er gået 3 minutter
        if ((DateTime.UtcNow - _lastSavedTime).TotalMinutes >= 3)
        {
            _lastSavedTime = DateTime.UtcNow;
            value.Date = DateTime.UtcNow;
            value.Source = "Automatisk";
            return Ok(_repo.Add(value));
        }

        // ..ellers gemmes lokationen ikke i databasen endnu
        return Ok("Location updated, but not saved yet");
    }

    // API RELATERET METODER
    [HttpGet("latest-with-address")]
    public async Task<IActionResult> GetLatestWithAddress()
    {
        var latest = _repo.GetAll().OrderByDescending(l => l.Date).FirstOrDefault();
        if (latest == null)
            return BadRequest("No location available");

        var geocodingService = new Geocoding(
            new HttpClient(),
            "mmvpt-tracker/1.0 (clib@hotmail.com)"
        );

        string? address = await geocodingService.ReverseGeocode(latest.Latitude, latest.Longitude);

        var dto = new LocationDto
        {
            Latitude = latest.Latitude,
            Longitude = latest.Longitude,
            Date = latest.Date,
            Address = address
        };

        return Ok(dto);
    }

    //// PUT api/<TrackingDeviceController>/5
    //[HttpPut("{id}")]
    //public void Put(int id, [FromBody] string value)
    //{
    //}

    //// DELETE api/<TrackingDeviceController>/5
    //[HttpDelete("{id}")]
    //public void Delete(int id)
    //{
    //}
}
