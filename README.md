
# Uhura

This is a super simple url router for Owin/Kestrel.  It's designed be super simple with use from F# for microservices.  If you need something more robust I suggest [Suave](https://suave.io/), [Nancy](http://nancyfx.org/) or [Asp.Net](https://www.asp.net/core)

In order to build run

    > build.cmd // on windows    
    $ ./build.sh  // on unix
    
    
## Hello World 

module Main

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
    
## Maintainer(s)

- [theangrybyrd](https://github.com/theangrybyrd)

