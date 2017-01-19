open System.IO

open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Hosting
open Microsoft.AspNetCore.Http
open Microsoft.Extensions.DependencyInjection
open Microsoft.Extensions.Logging

open Suave.AspNetCore

type Startup() =
  member __.Configure (app : IApplicationBuilder)
                      (env : IHostingEnvironment)
                      (loggerFactory : ILoggerFactory) =
    app.UseSuave(Playground.App.route) |> ignore

[<EntryPoint>]
let main argv = 
  let host =
    WebHostBuilder()
      .UseUrls("http://0.0.0.0:8080")
      .UseKestrel()
      .UseStartup<Startup>()
      .Build()
  host.Run()
  0
