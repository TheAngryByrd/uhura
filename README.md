# uhura
# uhura

MacOS/Linux | Windows
--- | ---
[![Travis Badge](https://travis-ci.org/TheAngryByrd/uhura.svg?branch=master)](https://travis-ci.org/TheAngryByrd/uhura) | [![Build status](https://ci.appveyor.com/api/projects/status/github/TheAngryByrd/uhura?svg=true)](https://ci.appveyor.com/project/TheAngryByrd/uhura)
[![Build History](https://buildstats.info/travisci/chart/TheAngryByrd/uhura)](https://travis-ci.org/TheAngryByrd/uhura/builds) | [![Build History](https://buildstats.info/appveyor/chart/TheAngryByrd/uhura)](https://ci.appveyor.com/project/TheAngryByrd/uhura)  


## Nuget 


Stable | Prerelease
--- | ---
[![NuGet Badge](https://buildstats.info/nuget/uhura)](https://www.nuget.org/packages/uhura/) | [![NuGet Badge](https://buildstats.info/nuget/uhura?includePreReleases=true)](https://www.nuget.org/packages/uhura/)




This is a super simple url router for Owin/Kestrel.  It's designed be super simple with use from F# for microservices.  If you need something more robust I suggest [Suave](https://suave.io/), [Nancy](http://nancyfx.org/) or [Asp.Net](https://www.asp.net/core)

   
## Hello World 



```FSharp
open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Hosting
open Microsoft.AspNetCore.Http
open Uhura.Web
open Uhura.Web.Routing
let helloWorldHandler groups (ctx : HttpContext) =
    ctx.Response.WriteAsync("Hello world from Uhura on Kestrel!") 

let routes =
    [
        GET "/" helloWorldHandler
    ]
[<EntryPoint>]
let main argv =
    WebHostBuilder()
        .UseUrls("http://localhost:8083")
        .UseKestrel()
        .Configure(fun appBuilder -> openHailingFrequencies appBuilder routes)
        .Build()
        .Run()

    0
```