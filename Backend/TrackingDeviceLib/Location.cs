namespace TrackingDeviceLib;

public class Location
{
	public int Id { get; set; }
	public double Longitude { get; set; }
	public double Latitude { get; set; }
	public DateTime Date { get; set; }

	public Location(int id, double longitude, double latitude, DateTime date)
	{
		Id = id;
		Longitude = longitude;
		Latitude = latitude;
		Date = date;
	}
}
