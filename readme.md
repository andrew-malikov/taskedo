# Taskedo

- [Taskedo](#taskedo)
  - [Overview](#overview)
  - [Develop](#develop)
    - [Unit Tests](#unit-tests)
    - [Environment Variables](#environment-variables)
    - [Database Migrations](#database-migrations)
  - [Run an Example](#run-an-example)
    - [With Docker Compose](#with-docker-compose)
    - [Manually Yourself](#manually-yourself)

## Overview

A simple REST API to manage Tasks.

## Develop

First off fetch dependencies:

```sh
dotnet restore
```

### Unit Tests

```sh
dotnet test
```

### Environment Variables

Copy the example to the repository root and set them up:

```sh
cp .config/.env.example .env
```

If you need them for a local run, just print them out to shell:

```sh
source .env
```

### Database Migrations

Startup a separate DB container to calculate changes:

```sh
docker compose --env-file .env -f ./build/docker-compose.migrations.yml -p taskedomigrations up
```

Apply already created migrations on the DB:

```sh
dotnet ef database update -p Taskedo.Tasks.Database.Changes -s Taskedo.Tasks.Database.Startup
```

Create a new migration:

```sh
dotnet ef migrations add -p Taskedo.Tasks.Database.Changes -s Taskedo.Tasks.Database.Startup "Add_Task_Table"
```

## Run an Example

### With Docker Compose

Just spin off the docker compose with a MSSQL DB and Taskedo API (docker will build the image and run it):

```sh
docker compose --env-file .env -f ./build/docker-compose.yml -p taskedo up --build
```

> Keep in mind, Taskedo migrates the DB on startup by default, so you don't need to do it manually. In a production environment, it should be done as part of CD pipeline.

You can check out Swagger/OpenAPI page on [http://localhost:5050/swagger/index.html](http://localhost:5050/swagger/index.html).

### Manually Yourself

If you have no docker thus no docker compose you need to bring your own DB and setup connection string the way you want. There is one ENV to manage it `Db__ConnectionString`, you can set that up or update `appsetings.json` (not tested but should work) and add:

```json
{
  "Db": {
    "ConnectionString": "YourConnectionHere"
  }
}
```

And then run:

```sh
dotnet run --project Taskedo.WebApi
```

Or build and run later when you need:

```sh
dotnet publish "Taskedo.WebApi/Taskedo.WebApi.csproj" -c Release -o ./app/publish /p:PublishSingleFile=true
```

You can find the artifacts under `./app/publish` folder.
