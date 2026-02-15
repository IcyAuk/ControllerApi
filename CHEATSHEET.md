# Docker + Docker Compose Cheat Sheet
quick refs

## Core concepts
- Image: immutable template (built from Dockerfile)
- Container: running instance of an image
- Registry: place to store images (Docker Hub, GHCR, etc.)
- Volume: persistent data managed by Docker
- Network: virtual LAN for containers to talk

## Install and verify
- docker version
- docker info
- docker compose version

## Images
- Build: docker build -t myapp:dev .
- List: docker images
- Remove: docker rmi <image>
- Prune dangling: docker image prune
- Prune all unused: docker image prune -a

## Containers
- Run: docker run --name myapp -p 8080:8080 myapp:dev
- Run detached: docker run -d --name myapp -p 8080:8080 myapp:dev
- List running: docker ps
- List all: docker ps -a
- Stop: docker stop <container>
- Start: docker start <container>
- Remove: docker rm <container>
- Remove force: docker rm -f <container>
- Logs: docker logs <container>
- Follow logs: docker logs -f <container>
- Exec shell: docker exec -it <container> sh
- Exec bash: docker exec -it <container> bash

## Volumes
- List: docker volume ls
- Inspect: docker volume inspect <volume>
- Remove: docker volume rm <volume>
- Prune unused: docker volume prune

## Networks
- List: docker network ls
- Inspect: docker network inspect <network>
- Remove: docker network rm <network>

## Cleanup (safe-ish)
- Remove stopped containers: docker container prune
- Remove unused networks: docker network prune
- Remove unused volumes: docker volume prune
- Remove unused images: docker image prune -a
- Remove everything unused: docker system prune -a

## Dockerfile basics
- FROM mcr.microsoft.com/dotnet/aspnet:9.0
- WORKDIR /app
- COPY . .
- RUN dotnet restore
- RUN dotnet publish -c Release -o out
- ENTRYPOINT ["dotnet", "MyApp.dll"]

### Multi-stage example (ASP.NET Core)
- Use SDK image to build, ASP.NET image to run
- Keep final image small

## .dockerignore basics
- bin/
- obj/
- .git/
- .vs/
- **/.idea/
- **/.vscode/

## Compose basics
- Start: docker compose up
- Start detached: docker compose up -d
- Rebuild: docker compose up --build
- Stop: docker compose stop
- Stop and remove: docker compose down
- Down + remove volumes: docker compose down -v
- Logs: docker compose logs
- Logs follow: docker compose logs -f
- Exec: docker compose exec <service> sh

## Compose file structure
- services: containers you run
- ports: host:container mapping
- environment: env vars
- volumes: host:container or named volumes
- depends_on: startup order (not readiness)
- networks: shared networks

### Minimal compose example
version: "3.9"
services:
  web:
    build: .
    ports:
      - "8080:8080"
    environment:
      - ASPNETCORE_URLS=http://+:8080

## Common ASP.NET Core container tips
- Bind to 0.0.0.0 in container
- Use ASPNETCORE_URLS=http://+:8080
- Expose port in Dockerfile: EXPOSE 8080
- Use 8080 or 5000 inside container, map to any host port

## Debugging checklist
- Image build fails: check Dockerfile paths and .dockerignore
- Container exits: check docker logs <container>
- App not reachable: check port mapping and ASPNETCORE_URLS
- Compose dependency order: add healthchecks if needed
- Volume issues: verify mount path and permissions

## Handy inspection
- docker inspect <container>
- docker inspect <image>
- docker compose ps
- docker compose config

## Registry basics
- Login: docker login
- Tag: docker tag myapp:dev myuser/myapp:dev
- Push: docker push myuser/myapp:dev
- Pull: docker pull myuser/myapp:dev

## Environment files
- Compose loads .env automatically
- Reference: environment:
    - KEY=${VALUE}

## Healthchecks (Compose)
- healthcheck:
    test: ["CMD", "curl", "-f", "http://localhost:8080/health"]
    interval: 10s
    timeout: 3s
    retries: 5

## Common errors
- "port is already allocated": pick a different host port
- "connection refused": app not listening on 0.0.0.0 or wrong port
- "no such file or directory": COPY path wrong or .dockerignore excluded it

## Quick template: build + run
- docker build -t myapp:dev .
- docker run --rm -p 8080:8080 myapp:dev

## Quick template: compose
- docker compose up --build
- docker compose down

# .NET CLI Cheat Sheet
quick refs

## Install and verify
- dotnet --info
- dotnet --version
- dotnet --list-sdks
- dotnet --list-runtimes

## New projects
- dotnet new list
- dotnet new webapi -n MyApi
- dotnet new console -n MyApp
- dotnet new sln -n MySolution
- dotnet sln add .\MyApi\MyApi.csproj

## Restore, build, run
- dotnet restore
- dotnet build
- dotnet run
- dotnet run --project .\MyApi\MyApi.csproj

## Test
- dotnet test
- dotnet test --filter FullyQualifiedName~MyTests

## Add and list packages
- dotnet add package Microsoft.EntityFrameworkCore
- dotnet add package Microsoft.EntityFrameworkCore.InMemory
- dotnet list package

## Tooling
- dotnet tool list -g
- dotnet tool install -g dotnet-ef
- dotnet tool update -g dotnet-ef
- dotnet tool uninstall -g dotnet-ef

## EF Core (dotnet-ef)
- dotnet ef migrations add InitialCreate
- dotnet ef database update
- dotnet ef migrations remove
- dotnet ef database drop

## Cleaning
- dotnet clean
- dotnet clean -c Release

## Publish
- dotnet publish -c Release -o .\publish
- dotnet publish -c Release -r win-x64 --self-contained false

## NuGet cache (fix weird restore issues)
- dotnet nuget locals all --clear

## Common files
- .csproj: project definition, packages, target framework
- appsettings.json: base config
- appsettings.Development.json: dev-only overrides
- launchSettings.json: dev launch profiles

## Common errors
- "assets file doesn't have a target": run dotnet restore
- "package reference could not be found": verify package name/version
- "SDK not found": install matching .NET SDK or set global.json

## Quick template: new Web API
- dotnet new webapi -n MyApi
- cd .\MyApi
- dotnet run
