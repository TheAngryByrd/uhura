module Main

open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Hosting
open Microsoft.AspNetCore.Http

open Uhura.Web
open Uhura.Web.Routing
open Hopac
let helloWorldHandler groups (ctx : HttpContext) = job {
    do! ctx.Response.WriteAsync("Hello world") |> Job.awaitUnitTask
}

let helloNameHandler groups (ctx : HttpContext) = job {
    let name =  tryGetNamedParam groups "name" |> Option.get
    do! ctx.Response.WriteAsync(sprintf "Hello %s" name) |> Job.awaitUnitTask
}
 
let getUsers  groups (ctx : HttpContext)= job {
    let id = tryGetNamedParam groups "id" |> Option.get
    do! ctx.Response.WriteAsync(sprintf "Hello %s" id) |> Job.awaitUnitTask
}

let routes =
    [
        GET "/" helloWorldHandler
        GET "/what/do" helloWorldHandler
        GET "/users/(?<id>\d{1,5})/do" getUsers
        GET "/:name" helloNameHandler
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
