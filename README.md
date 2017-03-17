[![Build status](https://ci.appveyor.com/api/projects/status/gor9xoo1tibmiuqi?svg=true)](https://ci.appveyor.com/project/SergeyBaranchenkov/dotnet-extensions-logging-testing)

# DotNet.Extensions.Logging.Testing

Test and Observable loggers for Microsoft.Extensions.Logging.

# Usage

## Using LogCollector to collect logs produced by a component that depends on ILogger

```csharp
  var logCollector = new LogCollector();
  
  using(logCollector.CollectLogs())
  {
    ILogger logger = logCollector.Logger;
    
    // code that uses logger instance.
  }
  
  Assert.AreEquals(5, logCollector.Logs);
```

