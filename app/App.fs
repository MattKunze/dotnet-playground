namespace Playground

open Newtonsoft.Json
open Suave
open Suave.Filters
open Suave.Operators
open Suave.RequestErrors
open Suave.Successful

open Suave.AspNetCore

module App =
  let private dumpContext =
    fun (ctx : HttpContext) ->
      let json = JsonConvert.SerializeObject(ctx.request, Formatting.Indented)
      OK json
      >=> Writers.setMimeType "application/json"
      <| ctx

  let route =
    choose [
      path "/" >=> OK "howdy"
      path "/context" >=> dumpContext
      NOT_FOUND "wuh?"
    ]
