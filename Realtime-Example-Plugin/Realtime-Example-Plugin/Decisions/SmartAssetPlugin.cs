using RedPoint.Resonance.Web.Shared;
using RedPoint.Resonance.Web.Shared.ConfigurationModels;
using RedPoint.Resonance.Web.Shared.Plugins;
using RedPoint.Resonance.Web.Shared.RealtimeServiceModels;
using RedPoint.Shared.Configuration.Core;

namespace RealtimeExamplePlugin.Decisions;

/// <summary>
/// Factory class to initialize a new instance of a smart asset plugin
/// Smart asset plugins allowing for the updating and processing of all decision results releated to a smart asset request.
/// In this example, the visitor profile is updated with a counter and the results are augmented with extra information.
/// </summary>
public class SmartAssetPluginFactory : FilterableRealtimePluginFactoryBase
{
    /// <summary>
    /// Get an instance of the plugin
    /// </summary>
    /// <returns></returns>
    public override IRealtimePlugin GetPlugin()
    {
        return new SmartAssetExamplePlugin { ParameterName = ParameterName };
    }

    /// <summary>
    /// Apply configuration settings
    /// </summary>
    /// <param name="name"></param>
    /// <param name="settings"></param>
    public override void Initialize(string name, List<KeyValueConfig> settings)
    {
        Name = name;
        ParameterName = WebConfigurationHelper.GetKeyValue("ParameterName", settings, string.Empty);

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

public class SmartAssetPlugin : ISmartAssetResultsPlugin
{
    public string? ParameterName { get; set; }

    /// <summary>
    /// Execute the plugin logic
    /// </summary>
    /// <param name="visitorID"></param>
    /// <param name="visitorDetails"></param>
    /// <param name="request"></param>
    /// <param name="results"></param>
    /// <returns></returns>
    public List<DecisionResult> Execute(string visitorID, WebVisitor visitorDetails, SmartAssetRequest request, List<DecisionResult> results)
    {
        //Write a log message
        TraceLogHelper.SendTraceInformation($"Running my smart asset plugin for visitor {visitorID}");

        //Update the results
        for (int i = 0; i < results.Count; i++)
        {
            var result = results[i];
            result.ContextTag = $"This is result {i + 1} of {results.Count}";
        }

        //Save a counter in the profile
        if (!string.IsNullOrWhiteSpace(ParameterName))
        {
            var currentCount = visitorDetails.GetParameterOnName(ParameterName);
            if (currentCount?.ValueSingle is int currentVal)
            {
                visitorDetails.SetValue(ParameterName, currentVal + 1, true);
            }
            else
            {
                visitorDetails.SetValue(ParameterName, 1, true);
            }
        }

        return results;
    }
}
