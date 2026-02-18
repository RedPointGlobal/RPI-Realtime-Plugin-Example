using RedPoint.Resonance.Web.Shared;
using RedPoint.Resonance.Web.Shared.Logging;
using RedPoint.Resonance.Web.Shared.Plugins;
using RedPoint.Resonance.Web.Shared.RealtimeServiceModels;
using RedPoint.Shared.Configuration.Core;

namespace RealtimeExamplePlugin.Events;

/// <summary>
/// Factory class to initialize a new instance of an Event plugin
/// Event plugins allowing for the updating and custom processing of realtime events
/// In this example, some event information is written out to the trace log
/// </summary>
public class EventPluginFactory : FilterableRealtimePluginFactoryBase
{
    /// <summary>
    /// Configuration values can be persisted as required during the factory initialization.
    /// </summary>
    /// <param name="name"></param>
    /// <param name="settings"></param>
    public override void Initialize(string name, List<KeyValueConfig> settings)
    {
        this.Name = name;            
    }

    /// <summary>
    /// Create a new instance of the Event plugin
    /// </summary>
    /// <returns></returns>
    public override IRealtimePlugin GetPlugin()
    {
        return new EventPlugin();
    }
}

/// <summary>
/// The plugin contains the code that will execute the plugin logic in realtime.
/// </summary>
public class EventPlugin : IEventPlugin
{
    /// <summary>
    /// The Execute call allows for updating and processing of the realtime event object.
    /// </summary>
    /// <param name="realtimeEvent"></param>
    public Task ExecuteAsync(RealtimeEvent realtimeEvent)
    {
        TraceLogHelper.SendTraceInformation($"*** EVENT PLUGIN ***   Visitor ID: {realtimeEvent.VisitorID} - Event: {realtimeEvent.EventName}", category: RealtimeLogCategory.Plugin);
        return Task.CompletedTask;
    }
}