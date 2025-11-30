using TrackingDeviceLib.Models;

namespace TrackingDeviceLib.Services.Interfaces
{
	public interface ITrackingDeviceRepo
	{
		Location Add(Location location);
		List<Location> GetAll();
		Location? GetById(int id);
	}
}