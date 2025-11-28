namespace TrackingDeviceLib;

public class Location
{
	public int Id { get; set; }
	public string Longtitude { get; set; }
	public string Latitude { get; set; }
	public DateTime Date { get; set; }

	public Location(int id, string longtitude, string latitude, DateTime date)
	{
		Id = id;
		Longtitude = longtitude;
		Latitude = latitude;
		Date = date;
	}
}
