using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TrackingDeviceLib.Models;

public class Location
{
    private DateTime _date;

    public int Id { get; set; }
	public double Longitude { get; set; }
	public double Latitude { get; set; }
    public string? Source { get; set; }
    public string? Address {  get; set; }

    // Foreign key to Device
    public int? DeviceId { get; set; }

    // Optional navigation property for EF Core
    public Device? Device { get; set; }

    [JsonPropertyName("timestamp")]
    public DateTime Date
    {
        get => _date;
        set
        {
            // Treat incoming value as UTC, then convert to UTC+1 (CET)
            var utcDate = DateTime.SpecifyKind(value, DateTimeKind.Utc);
            _date = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(utcDate, "Central European Standard Time");
        }
    }

    public Location() { }

    public Location(double longitude, double latitude, DateTime date, string source, string address)
	{
		Longitude = longitude;
		Latitude = latitude;
		Date = date;
        Source = source;
        Address = address;
    }

    public Location(int id, double longitude, double latitude, DateTime date, string source, string address)
    {
        Id = id;
        Longitude = longitude;
        Latitude = latitude;
        Date = date;
        Source = source;
        Address = address;
    }
}
