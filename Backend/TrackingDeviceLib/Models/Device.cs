using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace TrackingDeviceLib.Models;

public class Device
{
	public int Id { get; set; }
	public string Name { get; set; }

    // Foreign key to User
    public int? UserId { get; set; }
    public User User { get; set; }

    // Navigation property for Locations used by EF Core
    public ICollection<Location> Locations { get; set; } = new List<Location>();

    public Device() { }

    public Device(int id, string name)
	{
		Id = id;
		Name = name;
	}
}
