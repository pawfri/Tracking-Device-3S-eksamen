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

	public Device(int id, string name)
	{
		Id = id;
		Name = name;
	}
}
