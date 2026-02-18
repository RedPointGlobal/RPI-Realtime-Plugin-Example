using RedPoint.Resonance.Web.Shared.Plugins;
using RedPoint.Resonance.Web.Shared.RealtimeServiceModels;
using RedPoint.Shared.Configuration.Core;

namespace RealtimeExamplePlugin.Forms;

/// <summary>
/// Factory class to initialize a new instance of a Form plugin
/// Form plugins allowing for the updating and custom processing of realtime form data
/// In this example, the configuration values for this plugin are added to any form data received by realtime
/// </summary>
public class FormProcessingFactory : IFormProcessingPluginFactory
{
    /// <summary>
    /// Plugin Name
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Custom property to store extra form values
    /// </summary>
    public List<KeyValueConfig> FormParameters { get; set; } = [];


    /// <summary>
    /// Configuration values can be persisted as required during the factory initialization.
    /// </summary>
    /// <param name="name"></param>
    /// <param name="settings"></param>
    public void Initialize(string name, List<KeyValueConfig> settings)
    {
        Name = name;
        FormParameters = [.. settings];
    }

    /// <summary>
    ///  Create a new instance of the Event plugin
    /// </summary>
    public IRealtimePlugin GetPlugin()
    {
        return new FormProcessingPlugin { FormParameters = FormParameters };
    }
}

/// <summary>
/// The plugin contains the code that will execute the plugin logic in realtime.
/// </summary>
public class FormProcessingPlugin : IFormProcessingPlugin
{
    /// <summary>
    /// Custom property to store extra form values
    /// </summary>
    public List<KeyValueConfig> FormParameters { get; set; } = [];

    /// <summary>
    /// Modify or apply custom processing on the form data
    /// </summary>
    /// <param name="formData"></param>
    public Task ExecuteAsync(RPIFormData formData)
    {
        if (FormParameters != null)
        {
            foreach (var item in FormParameters)
            {
                formData.Items.Add(new RPIFormDataItem { Key = item.Key, Value = item.Value, IsCustomField = true });
            }
        }
        return Task.CompletedTask;  
    }
}