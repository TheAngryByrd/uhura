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


[<EntryPoint>]
let main argv =
    let buildRoutes appBuilder =
        let routes =
            [
                { Method = Http.Methods.GET; Path = "/"   ; Handler = helloWorldHandler }
             
            ]

        routes |> applyRoutes appBuilder
    WebHostBuilder()
        .UseUrls("http://localhost:8080")
        .UseKestrel()
        .Configure(fun a -> buildRoutes a)
        .Build()
        .Run()


    0 // return an integer exit code
