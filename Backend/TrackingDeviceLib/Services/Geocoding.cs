using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace TrackingDeviceLib.Services;

public class Geocoding
{
    private readonly HttpClient _http;
    private readonly string _userAgent;

    public Geocoding(HttpClient http, string userAgent)
    {
        _http = http;
        _userAgent = userAgent;
    }

    public async Task<string> ReverseGeocode (double lat, double lon)
    {
        var request = new HttpRequestMessage(HttpMethod.Get,
            $"https://nominatim.openstreetmap.org/reverse?format=json&lat={lat}&lon={lon}");

        request.Headers.Add("User-Agent", _userAgent);

        var response = await _http.SendAsync(request);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        using var doc = JsonDocument.Parse(json);

        if (doc.RootElement.TryGetProperty("address", out var addr))
        {
            string? road = addr.TryGetProperty("road", out var r) ? r.GetString() : "";
            string? house = addr.TryGetProperty("house_number", out var h) ? h.GetString() : "";
            string? postcode = addr.TryGetProperty("postcode", out var p) ? p.GetString() : "";
            string? city = addr.TryGetProperty("city", out var c) ? c.GetString() : "";
            string? country = addr.TryGetProperty("country", out var co) ? co.GetString() : "";

            return $"{road} {house}, {postcode} {city}, {country}";
        }

        return doc.RootElement.GetProperty("display_name").GetString() ?? "Ukendt adresse";
    }
}

