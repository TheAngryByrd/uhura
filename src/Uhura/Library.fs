namespace Uhura

module Web =
        module Http =
            module Methods =
                [<Literal>] 
                let GET = "GET"
                [<Literal>] 
                let POST = "POST"
                [<Literal>] 
                let PUT = "PUT"
                [<Literal>] 
                let DELETE = "DELETE"
                [<Literal>] 
                let HEAD = "HEAD"
                [<Literal>] 
                let OPTIONS = "OPTIONS"
                [<Literal>] 
                let CONNECT = "CONNECT"
        module Routing =
            open Hopac
            open Microsoft.AspNetCore.Builder
            open Microsoft.AspNetCore.Http
            open System.Threading.Tasks

            type RouteHandler = HttpContext -> Job<unit>
            type Route = {
                Method : string
                Path : string
                Handler : RouteHandler
            }
            let createRoute method path handler = 
              { Method = method; Path = path ; Handler = handler }
            let GET path handler =
              createRoute Http.Methods.GET path handler
            let POST path handler =
              createRoute Http.Methods.POST path handler
            let PUT path handler =
              createRoute Http.Methods.GET path handler
            let DELETE path handler =
              createRoute Http.Methods.DELETE path handler
             

            let inline toLowerInvariant (path : string) =
                path.ToLowerInvariant()
            let normalize = toLowerInvariant
            let inline matches route (httpContext : HttpContext)=
                httpContext.Request.Method = route.Method && (normalize httpContext.Request.Path.Value) = (normalize route.Path)
            let inline unitTaskToTask (task : Task<unit>) =
                task :> Task
            let inline applyRoutes (appBuilder : IApplicationBuilder) routes =
                routes
                |> Seq.iter(fun route ->
                    appBuilder.MapWhen(
                        (fun httpContext -> matches route httpContext), 
                        fun app -> app.Run (fun a -> (a |> route.Handler |> Hopac.startAsTask |> unitTaskToTask ))) 
                    |> ignore
                )