using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace TrackingDeviceLib.Models;

public class GeocodingService
{
    private readonly HttpClient _http;
    private readonly string _apiKey;

    public GeocodingService(HttpClient http, IConfiguration config)
    {
        _http = http;
        _apiKey = config["LocationIq:ApiKey"];
    }

    public async Task<string?> ReverseGeocode(double lat, double lon)
    {
        var url = $"https://eu1.locationiq.com/v1/reverse?key={_apiKey}&lat={lat}&lon={lon}&format=json";

        var response = await _http.GetAsync(url);
        if (!response.IsSuccessStatusCode)
            return null;

        var json = await response.Content.ReadAsStringAsync();
        var doc = JsonDocument.Parse(json);

        if (!doc.RootElement.TryGetProperty("display_name", out var displayName))
            return null;

        return displayName.GetString();
    }
}
