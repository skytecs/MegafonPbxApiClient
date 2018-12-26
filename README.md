# API client for the Megafon cloud pbx
Nuget package which allows connecting to the Megafon VATS service and registers endpoints for callbacks

Getting started
---
1. Install [Skytecs.MegafonPbxApiClient NuGet package](https://www.nuget.org/packages/Skytecs.MegafonPbxApiClient)
2. Add namespace reference to your Startup.cs
```csharp
using Skytecs.MegafonPbxApiClient;
```
3. Place this code in your `ConfigureServices` method:
```csharp
services.AddMegafonApi(options =>
{
    options.ApiToken = "your-api-key";
    options.PbxEndpoint = "https://your-pbx-endpoint.megapbx.ru/sys/crm_api.wcgp";
});
```
This code will register `IMegafonApiClient` service as a singleton in the DI container.

To add callback support
---
1. Add the following code to the `ConfigureServices` method.
```csharp
services.AddMegafonCallbacks(options =>
{
    options.OnEvent = async x =>
    {
        Console.WriteLine(x.Phone);
    };
});
```
2. And place this code in the `Configure` method.
```csharp
app.MapMegafonCallbacks("/<relative-url-to-callbacks>", "<your-crm-token>");
```
Now you can provide your own implementation for any combination of the following callbacks:
* `OnEvent`
* `OnHistory`
* `OnContact`




