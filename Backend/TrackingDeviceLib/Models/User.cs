using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace TrackingDeviceLib.Models;

public class User
{
	public int Id { get; set; }
	public string Email { get; set; }
	public string Password { get; set; }

    // Navigation property for Devices used by EF Core
    public ICollection<Device> Devices { get; set; } = new List<Device>();

    public User() { }

    public User(int id, string email, string password)
	{
		Id = id;
		Email = email;
		Password = password;
	}

}
