using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackingDeviceLib.Dtos;

public class LocationDto
{
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public DateTime Date { get; set; }
    public string? Address { get; set; }
}
