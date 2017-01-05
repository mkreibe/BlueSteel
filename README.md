# BlueSteel

![Blue Steel](/blue-steel.png?raw=true)

## Settings

If the `MANAGEMENT_PORT` is set, then the actuators is set to that environment variable value. If the
`MANAGEMENT_PORT` is not set, then the `PORT` environment variable is read and incremented by one. If
neither is set, then the default value is set to `5001`.

## Client code

### WebHostBuilder additions

Simply add the `UseManagementHost()` to the `WebHostBuilder` variable. A parameter could be added to override
the environment variables.

```CSharp

    /// <summary>
    /// The main entry point.
    /// </summary>
    /// <param name="args">The environment variables.</param>
    public static void Main(string[] args)
    {
        var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("hosting.json", optional: true)
            .Build();

        var host = new WebHostBuilder()
            .UseConfiguration(config)
            .UseContentRoot(Directory.GetCurrentDirectory())
            .UseKestrel()
            .UseIISIntegration()
            .UseManagementHost()
            .UseStartup<Startup>()
            .Build();

        host.Run();
    }
```

### Add services via Startup.Configure()

`app.UpdateActuator<HealthActuator>((actuator) => {})`

`SimpleService`

`ExtendedService`


```CSharp

    public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
    {
        loggerFactory.AddConsole(Configuration.GetSection("Logging"));
        loggerFactory.AddDebug();

        app.UseMvc();

        // Update the health status.
        app.UpdateActuator<HealthActuator>((actuator) => {

            // This adds a simple service of name "Simple" and set its status
            // to 'UP'.
            actuator.AddService(new SimpleService
            {
                StatusCode = HealthStatusCode.Up,
                Name = "Simple"
            });

            // This defines a more complex example with extended properties that you
            // can define your self.
            actuator.AddService(new ExtendedService
            {
                StatusCode = HealthStatusCode.Up,
                Name = "Extended",
                ExtendedProperties = new Dictionary<string, JToken>()
                {
                    ["extended"] = JValue.CreateString("value")
                }
            });
        });
    }

```

```JSON
{
    "status": "UP",
    "Simple": {
        "status": "UP"
    },
    "Extended": {
        "status": "UP",
        "extended": "value"
    }
}
```

## Adding actuators



