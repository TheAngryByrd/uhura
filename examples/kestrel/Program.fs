module Main

open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Hosting
open Microsoft.AspNetCore.Http

open Uhura.Web
open Uhura.Web.Routing
open Hopac
let helloWorldHandler (ctx : HttpContext) = job {
    do! ctx.Response.WriteAsync("Hello world") |> Job.awaitUnitTask
}

let routes =
    [
        GET "/" helloWorldHandler
    ]
[<EntryPoint>]
let main argv =

    WebHostBuilder()
        .UseUrls("http://localhost:8080")
        .UseKestrel()
        .Configure(fun appBuilder -> applyRoutes appBuilder routes)
        .Build()
        .Run()


    0 // return an integer exit code
