namespace Uhura

module Infrastructure =
    open System
    let inline stringJoin (separator : string) (values : string seq) =
        String.Join(separator,values)

module Web =
    open Infrastructure
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
        open System.Text.RegularExpressions
        open System
        open System.Linq
        open System.Collections.Generic

        type RouteHandler = IDictionary<string,string> ->  HttpContext -> Job<unit>
        type Route = {
            Method : string
            Path : string
            Handler : RouteHandler
        }

        let inline createRoute method path handler = 
            { Method = method; Path = path ; Handler = handler }
        let inline GET path handler =
            createRoute Http.Methods.GET path handler
        let inline POST path handler =
            createRoute Http.Methods.POST path handler
        let inline PUT path handler =
            createRoute Http.Methods.GET path handler
        let inline DELETE path handler =
            createRoute Http.Methods.DELETE path handler
            
        let paramRegex = new Regex(@":(?<name>[A-Za-z0-9_]*)", RegexOptions.Compiled)
        let inline createRegexFromPath (route : string) =
            let parts = route.Split([|"/"|] , StringSplitOptions.RemoveEmptyEntries)
            let parts' =
                    parts
                    |> Seq.map(fun part -> 
                        if not <| paramRegex.IsMatch(part)
                        then part
                        else 
                            paramRegex
                                .Matches(part)
                                |> Seq.cast<Match>
                                |> Seq.filter(fun m -> m.Success)
                                |> Seq.map(fun m -> m.Groups.["name"].Value.Replace(".", @"\.") |> sprintf "(?<%s>.+?)")
                            |> stringJoin ""
                    )
            Regex("^/" + String.Join("/", parts') + "$", RegexOptions.Compiled ||| RegexOptions.IgnoreCase ||| RegexOptions.ExplicitCapture)
            

        [<Literal>]
        let RegexMatchesKey = "uhura-regex-matches"
        let inline matches route (regex : Regex) (httpContext : HttpContext) =
            if 
                httpContext.Request.Method = route.Method 
            then
                let m = regex.Match(httpContext.Request.Path.Value)
                if m.Success
                then
                    httpContext.Items.Add(RegexMatchesKey,m)
                    true
                else false
            else
                false

        let inline getMatches (httpContext : HttpContext) =
            let m = httpContext.Items.[RegexMatchesKey] :?> Match
            let groups =
                m.Groups
                |> Seq.cast<Group>
                |> Seq.map(fun gc ->
                    (gc.Name, gc.Value)
                )
                |> dict
            groups

        let inline unitTaskToTask (task : Task<unit>) =
            task :> Task
        let inline applyRoutes (appBuilder : IApplicationBuilder) routes =
            routes
            |> Seq.iter(fun route ->
                let regex = createRegexFromPath route.Path
                appBuilder.MapWhen(
                    (fun httpContext -> matches route regex httpContext), 
                    fun app -> app.Run (fun httpContext -> (httpContext |> route.Handler (getMatches httpContext) |> Hopac.startAsTask |> unitTaskToTask ))) 
                |> ignore
            )

        let inline tryGetNamedParam (groups : IDictionary<string,string>) key =
            match groups.TryGetValue key with
            | (true,v) -> Some v
            | _ -> None

