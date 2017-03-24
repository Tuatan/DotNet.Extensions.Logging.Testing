[![Build status](https://ci.appveyor.com/api/projects/status/gor9xoo1tibmiuqi?svg=true)](https://ci.appveyor.com/project/SergeyBaranchenkov/dotnet-extensions-logging-testing)
[![Nuget](https://img.shields.io/nuget/vpre/DotNet.Extensions.Logging.Testing.svg)](https://www.nuget.org/packages/DotNet.Extensions.Logging.Testing/)

# DotNet.Extensions.Logging.Testing

Test and Observable loggers for [Microsoft.Extensions.Logging](https://www.nuget.org/packages/Microsoft.Extensions.Logging/).

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

## Using LogCollector to collect logs produced by a component that depends on ILogger<>

```csharp
  var logCollector = new LogCollector<MyClass>();
  
  using(logCollector.CollectLogs())
  {
    ILogger<MyClass> logger = logCollector.Logger;
    
    // code that uses logger instance.
  }
  
  Assert.AreEquals(5, logCollector.Logs);
```

## Using ObservableLogger to dump logs produced by a component that depends on ILogger

```csharp
  using (var subject = new Subject<LogEvent>();
  using (var logger = new ObservableLogger("Category", (a,b) => true, subject))
  {
    subject.Dump();
     
    // code that uses logger instance.
  
    Console.ReadLine();
  }  
```


