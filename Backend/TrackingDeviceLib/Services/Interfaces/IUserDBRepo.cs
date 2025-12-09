using TrackingDeviceLib.Models;

namespace TrackingDeviceLib.Services.Interfaces;

public interface IUserDBRepo
{
	User? GetByUsername(string username);
}