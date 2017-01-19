# .NET Core / F# / Suave Playground

This provides an environment to make it easy to experiment with .NET core in a
Docker container and write web services using F#

## Docker setup

In order to use the newer MSBuild-based project system, you need preview4 of
the core SDK. Getting the right versions of things can be a little frustrating,
this seems to provide the best links to the various versions that are out there:

https://github.com/dotnet/core/tree/master/release-notes

According to https://github.com/dotnet/dotnet-docker/pull/177, there isn't an official image for preview4
from Microsoft yet, the local [Dockerfile](./Dockerfile) builds a version that has
the updated SDK. Build it with:

```sh
$ docker build -t dotnet:1.0-preview4 .
```

Should be able to switch to an official version shortly. Also it would be nice to look
into the Alpine Linux version once that's available: https://github.com/dotnet/coreclr/issues/917

## F# App / Docker

The basic app was generated with `dotnet new -l fsharp`, and then updated to reference
Suave and update the version of Microsoft.FSharp.Core.netcore.

The built-in Suave TCP server doesn't restart well on OSX/Linux (TODO: need to
investigate this more and find a workaround/file an issue), but the Kestrel web server
doesn't have the same problem.

The [Suave.AspNetCore](https://github.com/dustinmoris/Suave.AspNetCore) project
provides a library for integrating Suave as OWIN/Kestrel middleware so I've included it
to run the Suave app.

Also [dotnet-watch](https://github.com/aspnet/DotNetTools/tree/dev/src/Microsoft.DotNet.Watcher.Tools)
was added to support auto reloading of the services with `dotnet watch run`.

Run the app by spinning up a docker container and running `dotnet restore/watch run`:

```sh
$ docker run -it --rm -v `pwd`/app:/app -p 8080:8080 dotnet:1.0-preview4
root@280e064c362a:/# cd app
root@280e064c362a:/app# dotnet restore
  Restoring packages for /app/app.fsproj...
  Restoring packages for /app/app.fsproj...
  ...
  Restore completed in 14158.442ms for /app/app.fsproj.
  
  NuGet Config files used:
      /root/.nuget/NuGet/NuGet.Config
  
  Feeds used:
      https://api.nuget.org/v3/index.json
  
  Installed:
      300 package(s) to /app/app.fsproj
root@280e064c362a:/app# dotnet watch run
[DotNetWatcher] info: dotnet process id: 105
[DotNetWatcher] info: Running dotnet with the following arguments: run
Hosting environment: Production
Content root path: /app/bin/Debug/netcoreapp1.0
Now listening on: http://0.0.0.0:8080
Application started. Press Ctrl+C to shut down.
```

Open http://localhost:8080 and you should see the app, and if you change any of the .fs
files and save it will automatically restart.

## docker-compose

The [docker-compose.yml](./docker-compose.yml) file simplifies launching the container and
automatically starts the application. Just run `docker-compose up`

This also makes it easy to fire up additional services (MongoDB, redis, whatever) and
reference them from the app. See
[the documentation](https://docs.docker.com/compose/overview/) for more details.

## Other links

So they don't get lost...

* http://ionide.io/
* http://banashek.com/posts/20160602-fsharp-live-development-docker.html
* https://github.com/Krzysztof-Cieslak/Suave.Kestrel
* http://blog.2mas.xyz/fsharp-suave-app-on-dotnet-core-on-kubernetes-on-google-cloud/