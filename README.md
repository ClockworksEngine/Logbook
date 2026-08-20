# Logbook [![Build Status](https://img.shields.io/github/actions/workflow/status/ClockworksEngine/Logbook/ci.yml?branch=master)](https://github.com/ClockworksEngine/Logbook/actions) 

<img align="right" width="160px" height="160px" alt="Clockworks logo" src="https://avatars.githubusercontent.com/u/315957268?s=200&v=4">

A Unity logging package that replaces Unity's default logger with a custom Serilog solution capable of additional log levels, interfacing with libraries that use Microosft's ILogger, and more.

## Installation

### Unity Package MAnager

#### UPM

TODO

#### Git 

1. Open Unity Package Manager
2. Click the `+` button and select "Add package from git URL..."
3. Enter: `https://github.com/ClockworksEngine/Logbook.git?path=/Packages/com.digitaldescent.logbook`


### Manual Installation

> [!TIP]
> The Clockworks team recommends cloning the repository into your ``Packages`` folder as Git Submodule when possible based on release tags.

1. Clone or download this repository
2. Copy the `Packages/com.digitaldescent.logbook` folder to your Unity project's `Packages` directory


## Usage

Logbook by default will replace the default behaviour of Unity's Debug class. Allowing existing code and libraries to route their log messages through Logbook. 

> [!TIP]
> For best practices, treat the original Unity Debug class logging methods as "legacyh". You should use Logbook's static Logging class directly to allow for additional context and log levels that are otherwise not supported by Unity's Debug class.

```csharp
using DigitalDescent.Logbook;

// Simple logging
Logging.Info("Application started");
Logging.Debug("Debug information");
Logging.Warning("Something might be wrong");
Logging.Error("An error occurred");

// With Unity object context
Logging.Info("Player spawned", gameObject);
Logging.Error("Failed to load asset", this);

// Exception logging
try 
{
    // Your code
}
catch (Exception ex)
{
    Logging.Exception(ex, this);
}
```

### Configuration

Logbook initializes automatically at startup and reads configuration from a LogbookSettings asset.

Create a persistent settings asset:
1. In the Project window, select `Create > Logbook > Settings`.
2. Save it as `LogbookSettings.asset` in a `Resources` folder. Be sure its at the root leve.

Available settings:
- **UseLoggingColors** - Enables color formatting.
- **MinimumLevel** - Minimum `Serilog.Events.LogEventLevel` to emit.
- **Targets** - `LogbookTarget` assets to initialize during startup.

## Advanced Features

### Microsoft ILogger and ILoggerFactory Support

Logbook supports interfacing with libraries and tools that expect Microsoft's standard ILogger interface. You can access the compatible logger factory 
via `Logging.LoggerFactory`. The resulting ILoggerFactory and its ILoggers will translate to Logbook's logging system.

```csharp
using DigitalDescent.Logbook;
using Microsoft.Extensions.Logging;

var loggerFactory = Logging.LoggerFactory;
if (loggerFactory != null)
{
    var logger = loggerFactory.CreateLogger("Gameplay");
    logger.LogInformation("Information message");
    logger.LogWarning("Warning message");
    logger.LogError("Error message");
}
```

### Capture Scope

```csharp
using DigitalDescent.Logbook;

using (var scope = Logging.WithCaptureScope())
{
    Logging.Info("Message inside capture scope");
}

// Retrieve the captured events.
var events = scope.CapturedEvents;
```

### Custom Log Targets

```csharp
using DigitalDescent.Logbook;
using Serilog;
using UnityEngine;

[CreateAssetMenu(fileName = "My Logbook Target", menuName = "Logbook/Targets/My Target")]
public sealed class MyCustomTarget : LogbookTarget
{
    public override string Name => "My Target";

    public override void Initialize(LoggerConfiguration config)
    {
        // Example: config.WriteTo.File("my-target.log");
    }
}
```

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.
