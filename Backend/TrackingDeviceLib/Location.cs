namespace TrackingDeviceLib;

public class Location
{
	public int Id { get; set; }
	public string Longitude { get; set; }
	public string Latitude { get; set; }
	public DateTime Date { get; set; }

	public Location(int id, string longitude, string latitude, DateTime date)
	{
		Id = id;
		Longitude = longitude;
		Latitude = latitude;
		Date = date;
	}
}
