using RedPoint.Resonance.Web.Shared;
using RedPoint.Resonance.Web.Shared.ConfigurationModels;
using RedPoint.Resonance.Web.Shared.Plugins;
using RedPoint.Shared.Configuration.Core;

namespace RealtimeExamplePlugin.Decisions;

/// <summary>
/// Factory class to initialize a new instance of a pre-decision plugin
/// Pre-decision plugins allowing for the updating and processing of the visitor profile before it is used for decision making
/// In this example, the visitor profile is updated based on a configuration value
/// </summary>
public class PreDecisionPluginFactory : FilterableRealtimePluginFactoryBase
{
    /// <summary>
    /// Get an instance of the plugin
    /// </summary>
    /// <returns></returns>
    public override IRealtimePlugin GetPlugin()
    {
        return new PreDecisionPlugin { ParameterName = ParameterName };
    }

    /// <summary>
    /// Apply configuration settings
    /// </summary>
    /// <param name="name"></param>
    /// <param name="settings"></param>
    public override void Initialize(string name, List<KeyValueConfig> settings)
    {
        Name = name;
        ParameterName = WebConfigurationHelper.GetKeyValue("Param", settings, string.Empty);

        if (string.IsNullOrWhiteSpace(ParameterName))
        {
            ParameterName = name;
        }
    }

    public string? ParameterName { get; set; }
}


/// <summary>
/// Plugin logic
/// </summary>

public class PreDecisionPlugin : IPredecisionPlugin
{
    public string? ParameterName { get; set; }

    /// <summary>
    /// Execute the plugin logic
    /// </summary>
    /// <param name="visitorID"></param>
    /// <param name="decisionContext"></param>
    /// <param name="decisionSlotName"></param>
    /// <param name="contentID"></param>
    /// <param name="visitor"></param>
    /// <param name="apiContext"></param>
    public Task ExecuteAsync(string visitorID, string decisionContext, string decisionSlotName, string contentID, WebVisitor visitor, string apiContext)
    {
        visitor.SetValue(ParameterName, apiContext, false);
        return Task.CompletedTask;
    }
}