using Microsoft.AspNetCore.Mvc;
using TrackingDeviceLib;
using TrackingDeviceLib.Services.Repositories;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TrackingDeviceREST.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TrackingDeviceController : ControllerBase
{
	private TrackingDeviceRepo _repo;

	public TrackingDeviceController(TrackingDeviceRepo repo)
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
	public Location Post([FromBody] Location value)
	{
		return _repo.Add(value);
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
