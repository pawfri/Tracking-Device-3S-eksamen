using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
	public Location _latestLocation;
    //private List<Location> _locationBuffer = new List<Location>();
    //private static DateTime _lastFlushTime = DateTime.UtcNow;


	public TrackingDeviceController(ITrackingDeviceRepo repo)
	{
		_repo = repo;
	}


	// GET: api/<TrackingDeviceController>
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
	[HttpPost]
	public Location? PostTrackButton([FromBody] Location value)
	{
		value = _latestLocation;
        return _repo.Add(value);

		
	}

    [HttpPost]
    public Location? Post([FromBody] Location value)
    {
        value = _latestLocation;

        if((DateTime.UtcNow.Minute - value.Date.Minute) >= 3)
        {
            return _repo.Add(value);
        }
        else
        {
            return null;
        }

        //if (value.Date.Minute <= 3 DateTime.Now)
        //{
        //    value = _latestLocation;
        //    return _repo.Add(value);
        //}
        //else
        //{
        //    return null;
        //}
    }

    //[HttpPost("broadcast-location")]
    //public IActionResult BroadcastLocation([FromBody] Location locationDto)
    //{
    //    var newLocation = new Location
    //    {
    //        Longitude = locationDto.Longitude,
    //        Latitude = locationDto.Latitude,
    //        Date = locationDto.Date
    //    };

    //    _locationBuffer.Add(newLocation);

    //    // Tjek om der er gået 10 minutter
    //    if ((DateTime.UtcNow - _lastFlushTime).TotalMinutes >= 10)
    //    {
    //        _repo.Locations.AddRange(_locationBuffer);
    //        _repo.Add();

    //        _locationBuffer.Clear();
    //        _lastFlushTime = DateTime.UtcNow;
    //    }

    //    return Ok("Location buffered");
    //}


    //[HttpPost]
    //public Location TrackButtonPressed(int value)
    //{
    //	return
    //}

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
