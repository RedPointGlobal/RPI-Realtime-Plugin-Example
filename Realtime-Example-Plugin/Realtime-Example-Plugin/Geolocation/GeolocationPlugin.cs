using RedPoint.Resonance.Web.Shared;
using RedPoint.Resonance.Web.Shared.Decisions.Geolocation;
using RedPoint.Shared.Configuration.Core;
using System.Xml.Linq;

namespace RealtimeExamplePlugin.Geolocation;

/// <summary>
/// Geolocation Plugin are used to support geolocation decision criteria
/// </summary>
public class GeolocationPlugin : IGeolocationProvider
{
    /// <summary>
    /// Initialize the plugin with any configuration settings
    /// </summary>
    /// <param name="settings"></param>
    public void Initialize(List<KeyValueConfig> settings)
    {

    }

    /// <summary>
    /// Get current weather observations based on the geolocation details provided.
    /// </summary>
    /// <param name="geoDetails"></param>
    /// <param name="unit"></param>
    /// <returns></returns>
    public Task<WebWeather> GetCurrentObservationsAsync(WebGeolocation geoDetails, string unit)
    {
        //Always sunny
        return Task.FromResult(new WebWeather
        {
            ForecastDate = DateTime.Now,
            ForecastIndex = 0,
            HasPrecipitation = false,
            HoursOfSun = 12,
            HoursOfPrecipitation = 0,
            IceProbability = 0,
            IsDaytime = true,
            RainProbability = 0,
            SnowProbability = 0,
            Summary = "Sunny",
            Temperature = 25,
            Units = unit,
            UVIndex = 5,
            WindSpeed = 0
        });
    }

    /// <summary>
    /// Get distances based on the geolocation details provided.
    /// </summary>
    /// <param name="geoDetails"></param>
    /// <param name="distSearch"></param>
    /// <param name="unit"></param>
    /// <exception cref="NotImplementedException"></exception>
    public Task GetDistancesAsync(WebGeolocation geoDetails, GeoDistances distSearch, string unit)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Get geofence results based on the geolocation details provided.
    /// </summary>
    /// <param name="geoDetails"></param>
    /// <param name="geofenceOp"></param>
    /// <param name="points"></param>
    /// <param name="searchBuffer"></param>
    /// <returns></returns>
    public Task<bool> GetGeofenceDetailsAsync(WebGeolocation geoDetails, string geofenceOp, XElement points, int searchBuffer)
    {
        return Task.FromResult(true);
    }

    /// <summary>
    /// Get weather forecast based on the geolocation details provided.
    /// </summary>
    /// <param name="geoDetails"></param>
    /// <param name="unit"></param>
    /// <param name="forecastIndex"></param>
    /// <returns></returns>
    public Task<WebWeather> GetWeatherForecastAsync(WebGeolocation geoDetails, string unit, int forecastIndex)
    {
        //Always Sunny
        return Task.FromResult(new WebWeather
        {
            ForecastDate = DateTime.Now,
            ForecastIndex = forecastIndex,
            HasPrecipitation = false,
            HoursOfSun = 12,
            HoursOfPrecipitation = 0,
            IceProbability = 0,
            IsDaytime = true,
            RainProbability = 0,
            SnowProbability = 0,
            Summary = "Sunny",
            Temperature = 25,
            Units = unit,
            UVIndex = 5,
            WindSpeed = 0
        });
    }

    /// <summary>
    /// Load geolocation details based on the provided coordinates or search string
    /// </summary>
    /// <param name="geoDetails"></param>
    /// <returns></returns>
    public Task<bool> LoadDetailsAsync(WebGeolocation geoDetails)
    {
        //Always Wellesley
        geoDetails.Country = "USA";
        geoDetails.CoordinatesUpdated = DateTime.Now;
        geoDetails.CountryCode = "US";
        geoDetails.PostalCode = "MA";
        geoDetails.Longitude = "42.299941";
        geoDetails.Latitude = "-71.288318";
        return Task.FromResult(true);
    }



    public Task<bool> SetCoordinatesFromSearchStringAsync(WebGeolocation geoDetails)
    {
        //Always Wellesley
        geoDetails.Country = "USA";
        geoDetails.CoordinatesUpdated = DateTime.Now;
        geoDetails.CountryCode = "US";
        geoDetails.PostalCode = "MA";
        geoDetails.Longitude = "42.299941";
        geoDetails.Latitude = "-71.288318";
        return Task.FromResult(true);
    }    
}