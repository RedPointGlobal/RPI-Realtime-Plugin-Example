using RedPoint.Resonance.Web.Shared;
using RedPoint.Resonance.Web.Shared.ConfigurationModels;
using RedPoint.Resonance.Web.Shared.Plugins;
using RedPoint.Shared.Configuration.Core;

namespace RealtimeExamplePlugin.Decisions;

/// <summary>
/// Factory class to initialize a new instance of a post decision plugin
/// Post decision plugins allowing for the updating and custom processing of realtime decision result
/// The selection of a post decision plugin is done in the RPI Client when configuraing the Smart Asset
/// In this example, the decision result is updated based on configuraiton values
/// </summary>
public class PostDecisionFactory : IRealtimePluginFactory
{
    /// <summary>
    /// Plugin Name
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Apply configuration settings
    /// </summary>
    /// <param name="name"></param>
    /// <param name="settings"></param>
    public void Initialize(string name, List<KeyValueConfig> settings)
    {
        Name = name;
        Prefix = WebConfigurationHelper.GetKeyValue("Prefix", settings, string.Empty);
        Suffix = WebConfigurationHelper.GetKeyValue("Suffix", settings, string.Empty);
    }

    /// <summary>
    /// Prefix
    /// </summary>
    public string? Prefix { get; set; }

    /// <summary>
    /// Suffix
    /// </summary>
    public string? Suffix { get; set; }

    /// <summary>
    /// Get an instance of the plugin
    /// </summary>
    /// <returns></returns>
    public IRealtimePlugin GetPlugin()
    {
        return new PostDecisionPlugin { Prefix = Prefix, Suffix = Suffix };
    }
}

/// <summary>
/// Plugin logic
/// </summary>
public class PostDecisionPlugin : IDecisionContentPlugin
{
    /// <summary>
    /// Prefix
    /// </summary>
    public string? Prefix { get; set; }

    /// <summary>
    /// Suffix
    /// </summary>
    public string? Suffix { get; set; }

    /// <summary>
    /// Execute the plugin logic
    /// </summary>
    /// <param name="visitorID">Visitor ID</param>
    /// <param name="visitorDetails">Vistitor Details</param>
    /// <param name="result">Content result</param>
    public DecisionResult Execute(string visitorID, WebVisitor visitorDetails, DecisionResult result)
    {
        try
        {
            if (result.IsEmptyResult)
            {
                return result;
            }

            var contentString = string.Empty;

            if (result.IsCachedContent)
            {
                if (result.ResultContent is string valString) //it should always be string
                {
                    contentString = valString;
                }
            }
            else if (!string.IsNullOrWhiteSpace(result.ContentPath))
            {
                using var httpClient = new HttpClient();
                using var response = httpClient.GetAsync(result.ContentPath).Result;
                contentString = response.Content.ReadAsStringAsync().Result;
            }
            else
            {
                return result;
            }

            return new DecisionResult
            {
                ContentID = result.ContentID,
                ContentPath = string.Empty,
                DefaultKey = result.DefaultKey,
                DivName = result.DivName,
                IsCachedContent = true,
                IsEmptyResult = false,
                Result = result.Result,
                ResultContent = $"{Prefix}{result.ResultContent}{Suffix}",
                PluginTag = result.PluginTag,
                PluginTagDefault = result.PluginTagDefault,
                ContentFormat = result.ContentFormat
            };

        }
        catch (Exception ex)
        {
            TraceLogHelper.SendTraceError(ex, @"Error executing plugin");
            return result;
        }
    }
}