using RedPoint.Resonance.Web.Shared;
using RedPoint.Resonance.Web.Shared.ConfigurationModels;
using RedPoint.Resonance.Web.Shared.Decisions.Geolocation;
using RedPoint.Resonance.Web.Shared.Logging;
using RedPoint.Shared.Configuration.Core;

namespace RealtimeExamplePlugin.Geolocation;

/// <summary>
/// Geo IP Lookup Plugin are use to lookup up a geolocation based on an IP address.
/// </summary>
public class GeoIPLookupPlugin : IGeoIPLookupPlugin
{
    private string? _apiKey;

    /// <summary>
    /// Apply any configuration settings
    /// </summary>
    /// <param name="settings"></param>
    public void Initialize(List<KeyValueConfig> settings)
    {
        _apiKey = WebConfigurationHelper.GetKeyValue("apiKey", settings, string.Empty);
    }

    /// <summary>
    /// Use the IP Address to look up the geolocation details
    /// </summary>
    /// <param name="IPAddress"></param>
    /// <returns></returns>
    public Task<WebGeolocation> GetLocationAsync(string IPAddress)
    {
        try
        {
            return Task.FromResult(new WebGeolocation()
            {
                CoordinatesUpdated = DateTime.Now,
                Country = "country_name",
                CountryCode = "country_code",
                PostalCode = "postal",
                AdminDistrict1 = "city",
                AdminDistrict2 = "region",
                AdminDistrict3 = "continent_name",
                AdminDistrict4 = "continent_code",
                Longitude = "longitude",
                Latitude = "latitude"
            });

        }
        catch (Exception ex)
        {
            TraceLogHelper.SendTraceError(ex, $"Error looking up IP Address {IPAddress}", category: RealtimeLogCategory.Plugin);
            throw;
        }
    }
}