using TrackingDeviceLib.Models;

namespace TrackingDeviceLib.Services.Interfaces
{
	public interface ILocationRepo
	{
		Location Add(Location location);
		List<Location> GetAll();
		Location? GetById(int id);
        void Delete(int id);
    }
}