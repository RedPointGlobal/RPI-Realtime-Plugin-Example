![rp_cdp_logo](https://github.com/RedPointGlobal/redpoint-rpi/assets/42842390/432d779f-de4e-4936-80fe-3caa4d732603)

## Redpoint Interaction (RPI) | Realtime Plugins

RPI Realtime consists of a suite of functionality that allows you to make decisions about the most appropriate content to be displayed to a person of interest in real time.
Typically, RPI Realtime is used to make such decisions in the context of a web page (either an RPI landing page, or an externally-hosted web page), with the most appropriate content being rendered to a page visitor. 
However, its capabilities extend beyond this use case, with the RPI Realtime API (Application Programmer's Interface) facilitating leverage of its capabilities in a range of contexts e.g. Internet of Things (IoT) devices and inbound call centers.

Developers can enhace the functionality of RPI Realtime by creating plugins that extend the capabilities of the RPI Realtime API. There are a number of different types of plugins that can be created, these are detailed below.

## Table of Contents
- [Getting Started ](#getting-started)
- [Plugin Types ](#plugin-types)
    - [Decisions](#decision-plugins)
    - [Events](#event-plugins)
    - [Forms](#form-plugins)
    - [Visitor Profile](#visitor-profile-plugins)
    - [Geolocaton](#geolocation-plugins)
- [Configuration ](#configuration)

## Getting Started
The Realtime-Example-Plugin c# project provides an example of building each type of plugin. Plugin should be built in a class library using .Net 9.  
The plugin project will require the following two references as per the example project
- RedPoint.Resonance.Web.Shared.dll
- RedPoint.Shared.Configuration.dll

All plugins except the Geolocation plugins require two classes
- Factory class. This initializes new plugins, setting any custom configuration settings.
- Plugin class. This contains the code to execute the plugin logic.

Geolocation plugins only require a plugin class.

Once the plugin is compliled, the dll can be mounted on the realtime containers. Any configuration settings are defined in the ```values.yaml``` located in our [RPI Helm Chart](https://github.com/RedPointGlobal/redpoint-rpi) 

## Plugin Types

### Decision plugins
These plugins can execute code at different points of the decision flow, they can influence the decison made, modify the result or add extra processing where required.

![decision_flow](https://github.com/user-attachments/assets/fed74165-cea2-4c8e-a26e-a40edc7bb202)

#### Pre-decision plugins
Pre-decision plugins execute prior to the decision being made. They can be used to modify the request, add additional data to the request, or perform any other processing that needs to occur before the decision is made.

| Plugin Info |  |
|----------------------------------------------------------------------------------------|--|
| Endpoint | /api/v2/smart-assets/results |
| Factory Base Class / Interface | RedPoint.Resonance.Web.Shared.Plugins.FilterableRealtimePluginFactoryBase |
| Plugin Interface | RedPoint.Resonance.Web.Shared.Plugins.IPredecisionPlugin |
| Inputs     | Decision Request Details, Visitor Profile |
| Outputs    | Visitor Profile |
| Configuration Type | Predecision |


#### Post-decision plugins
Post-decision plugins execute immediately after a decision is made. They can be used to update the result, or perform any other processing that needs to occur before the decision result is returned. 

| Plugin Info |  |
|----------------------------------------------------------------------------------------|--|
| Endpoint | /api/v2/smart-assets/results |
| Factory Base Class / Interface | RedPoint.Resonance.Web.Shared.Plugins.IRealtimePluginFactory |
| Plugin Interface | RedPoint.Resonance.Web.Shared.Plugins.IDecisionContentPlugin |
| Inputs     | Decision Result, Visitor Profile |
| Outputs    | Decision Result |
| Configuration Type | N/A |

#### Smart Asset plugins
Smart Asset plugins execute once all the decision responses associated with a Smart Asset request have completed. They can be used to modify or process the full response instead of individual decisions.

| Plugin Info |  |
|----------------------------------------------------------------------------------------|--|
| Endpoint | /api/v2/smart-assets/results |
| Factory Base Class / Interface | RedPoint.Resonance.Web.Shared.Plugins.FilterableRealtimePluginFactoryBase |
| Plugin Interface | RedPoint.Resonance.Web.Shared.Plugins.ISmartAssetResultsPlugin |
| Inputs     | Request Details, Collection of Decision Results, Visitor Profile |
| Outputs    | Collection of Decision Results |
| Configuration Type | SmartAssetResults |

### Event plugins
Event plugins can be used to modify or process any realtime event e.g. Page Visit

| Plugin Info |  |
|----------------------------------------------------------------------------------------|--|
| Endpoint | /api/v2/events |
| Factory Base Class / Interface | RedPoint.Resonance.Web.Shared.Plugins.FilterableRealtimePluginFactoryBase |
| Plugin Interface | RedPoint.Resonance.Web.Shared.Plugins.IEventPlugin |
| Inputs     | Realtime Event |
| Outputs    | Realtime Event |
| Configuration Type | Event |

### Form plugins
Form Plugins can be used to modify or process any web form submission data passed to RPI Realtime for ingestion by RPI.

| Plugin Info |  |
|----------------------------------------------------------------------------------------|--|
| Endpoint | /api/v2/form-data |
| Factory Base Class / Interface | RedPoint.Resonance.Web.Shared.Plugins.IFormProcessingPluginFactory |
| Plugin Interface | RedPoint.Resonance.Web.Shared.Plugins.IFormProcessingPlugin |
| Inputs     | Form Data |
| Outputs    | Form Data |
| Configuration Type | N/A |

### Visitor Profile plugins
Visitor Profile plugins can be used to modify or process the visitor profile when a profile is added or updated through a call to the Visitor Registraion endpoint.

| Plugin Info |  |
|----------------------------------------------------------------------------------------|--|
| Endpoint | /api/v2/cache/visit |
| Factory Base Class / Interface | RedPoint.Resonance.Web.Shared.Plugins.FilterableRealtimePluginFactoryBase |
| Plugin Interface | RedPoint.Resonance.Web.Shared.Plugins.IVisitorCachePlugin |
| Inputs     | Visitor Profile, Registration Request Details |
| Outputs    | Visitor Profile |
| Configuration Type | Visitor |

### Geolocation Decision plugins
Geolocation decision plugins can be used to integrate new geolocation service providers to RPI Realtime to enable Geolocation rules be used in realtime decisions.

#### Geolocation Plugins
The gelocation plugin can be used to lookup address information, weather observations and forecasts and geofence information. This can be done using either longitude and latitude coordinates or a search string.

| Plugin Info |  |
|----------------------------------------------------------------------------------------|--|
| Endpoint | /api/v2/smart-assets/results |
| Factory Base Class / Interface | N/A |
| Plugin Interface | IGeolocationProvider |
| Inputs     | Long/Lat or Search String |
| Outputs    | Address, Weather & Geofence Results |
| Configuration Type | N/A |

#### IP to Geolocation Plugin
The IP to Geolocation plugin can be used to make use of long/lat lookup services using a supplied IP address. This enhances the number of opportunites for making geolocation based decisions.

| Plugin Info |  |
|----------------------------------------------------------------------------------------|--|
| Endpoint | /api/v2/smart-assets/results |
| Factory Base Class / Interface | N/A |
| Plugin Interface | IGeoIPLookupPlugin |
| Inputs     | IP Address |
| Outputs    | Long/Lat |
| Configuration Type | N/A |

## Configuration

### Plugin Path

After compiling the plugin, the resulting DLL must be made available to the RPI Realtime container. The Helm chart is configured to automatically mount the corresponding volume and set the location of the plugins but you are responsible for provisioning the underlying storage based on your hosting platform’s procedure and uploading the DLL to that location. Once the storage is provisioned, create a PersistentVolumeClaim (PVC) and reference its name in the [RPI Helm chart](https://github.com/RedPointGlobal/redpoint-rpi) ```values.yaml``` file as shown below

```
storage:
  persistentVolumeClaims:
    Plugins:
      enabled: true
      claimName: realtimeplugins
      mountPath: /app/plugins
```

### Application Settings

The plugins are configured using the ```values.yaml``` file located in our [RPI Helm Chart](https://github.com/RedPointGlobal/redpoint-rpi). This is located under the ```realtimeapi``` section

```
realtimeapi:
  plugins:
    
```

If the configuration options provided in the ```values.yaml``` are not sufficient for your use case, you can set additional environment variables by using a tool such as [Kustomize](https://kustomize.io/) to patch the Helm chart and extend the deployment as needed. Any patched environment variables must follow the naming convention below, which aligns with the structure expected by the application.

```
RealtimeAPIConfiguration__Plugins__0__Name=Pre-Decision Example
RealtimeAPIConfiguration__Plugins__0__Factory__Assembly=RedPoint.Realtime.Example.Plugins
RealtimeAPIConfiguration__Plugins__0__Factory__Type=RealtimeExamplePlugin.Decisions.PreDecisionPluginFactory
RealtimeAPIConfiguration__Plugins__0__Type=Predecision
RealtimeAPIConfiguration__Plugins__0__Settings__0__Key=Param
RealtimeAPIConfiguration__Plugins__0__Settings__0__Value=my-custom-parameter
```

All plugin types can be configured with a collection of Settings. This allows for custom configuration values to be supplied to the plugin.
Settings are a collection of Key/Value pairs.
If a setting requires a collection of values, these can be supplied as per the example below.

```
RealtimeAPIConfiguration__Plugins__0__Settings__0__Key=Param
RealtimeAPIConfiguration__Plugins__0__Settings__0__Values__0=my-custom-parameter-1
RealtimeAPIConfiguration__Plugins__0__Settings__0__Values__1=my-custom-parameter-2
```

