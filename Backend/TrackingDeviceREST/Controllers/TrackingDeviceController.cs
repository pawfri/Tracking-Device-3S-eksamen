using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrackingDeviceLib.Dtos;
using TrackingDeviceLib.Models;
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


    // GET: api/<TrackingDeviceController>
    //[HttpGet]
    //public IEnumerable<Location> Get()
    //{
    //	return _repo.GetAll();
    //}

    [HttpGet]
    public async Task<IEnumerable<LocationDto>> Get()
    {
        var locations = _repo.GetAll();

        var geocodingService = new GeocodingService(new HttpClient(),
            new ConfigurationBuilder().AddJsonFile("appsettings.json").Build());

        var result = new List<LocationDto>();

        foreach (var loc in locations)
        {
            string? address = await geocodingService.ReverseGeocode(loc.Latitude, loc.Longitude);

            result.Add(new LocationDto
            {
                Latitude = loc.Latitude,
                Longitude = loc.Longitude,
                Date = loc.Date,
                Address = address
            });
        }

        return result;
    }

    // GET api/<TrackingDeviceController>/5
    [HttpGet("{id}")]
	public Location? GetById(int id)
	{
		return _repo.GetById(id);
	}

    // POST api/<TrackingDeviceController>
    [HttpPost("trackingbutton")]
    public IActionResult PostTrackButton()
    {
        if (_latestLocation == null)
            return BadRequest("No location received yet");

        return Ok(_repo.Add(_latestLocation));
    }

    [HttpPost("")]
    public IActionResult Post([FromBody] Location value)
    {
        if (value == null)
            return BadRequest("Location is empty");

        // Gem den nyeste lokation i memory
        _latestLocation = value;

        // Hvis det er første gang eller der er gået 3 minutter
        if ((DateTime.UtcNow - _lastSavedTime).TotalMinutes >= 3)
        {
            _lastSavedTime = DateTime.UtcNow;
            return Ok(_repo.Add(value));
        }

        // ..ellers gemmes lokationen ikke i databasen endnu
        return Ok("Location updated, but not saved yet");
    }

    // API RELATERET METODER
    [HttpGet("latest-with-address")]
    public async Task<IActionResult> GetLatestWithAddress()
    {
        if (_latestLocation == null)
            return BadRequest("No location available");

        // Use your GeocodingService instead of calling HttpClient manually
        var geocodingService = new GeocodingService(new HttpClient(), new ConfigurationBuilder().AddJsonFile("appsettings.json").Build());
        string? address = await geocodingService.ReverseGeocode(_latestLocation.Latitude, _latestLocation.Longitude);

        var dto = new LocationDto
        {
            Latitude = _latestLocation.Latitude,
            Longitude = _latestLocation.Longitude,
            Date = _latestLocation.Date,
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
