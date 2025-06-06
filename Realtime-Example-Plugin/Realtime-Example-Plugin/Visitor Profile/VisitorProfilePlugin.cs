using RedPoint.Resonance.Web.Shared.Plugins;
using RedPoint.Resonance.Web.Shared.RealtimeServiceModels;
using RedPoint.Resonance.Web.Shared;
using RedPoint.Shared.Configuration.Core;
using RedPoint.Resonance.Web.Shared.ConfigurationModels;

namespace RealtimeExamplePlugin.Visitor;

/// <summary>
/// Factory class to initialize a new instance of a visitor profile plugin
/// Visitor profile plugins allowing for the updating and processing of a visitor profile during the registration of visitor visit
/// In this example, the visitor profile is updated with a configurable parameter
/// </summary>
public class VisitorCachePluginFactory : FilterableRealtimePluginFactoryBase
{
    public override IRealtimePlugin GetPlugin()
    {
        return new VisitorCacheExamplePlugin { ParameterName = ParameterName };
    }

    /// <summary>
    /// Apply configuration settings
    /// </summary>
    /// <param name="name"></param>
    /// <param name="settings"></param>
    public override void Initialize(string name, List<KeyValueConfig> settings)
    {
        this.Name = name;
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
public class VisitorCachePlugin : IVisitorCachePlugin
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
    public void Execute(string visitorID, WebVisitor visitor, VisitorPageRegistration details, bool isVisitRegistration)
    {
        visitor.SetValue("3539924d-a6aa-45dc-b692-6d30efa95811", ParameterName, $"{DateTime.Now:dddd hh:mmtt}", true);
    }
}
