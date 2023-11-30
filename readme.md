# Taskedo

- [Taskedo](#taskedo)
  - [Overview](#overview)
  - [Develop](#develop)
    - [Environment Variables](#environment-variables)
    - [Database Migrations](#database-migrations)
  - [Run](#run)
    - [With Docker Compose](#with-docker-compose)

## Overview

## Develop

### Environment Variables

Copy the example to the repository root and set them up:

```sh
cp .config/.env.example .env
```

If you need them for a local run just print them out to shell:

```sh
source .env
```

### Database Migrations

```sh
docker compose --env-file .env -f ./build/docker-compose.migrations.yml -p taskedomigrations up
```

```sh
dotnet ef database update -p Taskedo.Tasks.Database.Changes -s Taskedo.Tasks.Database.Startup
```

```sh
dotnet ef migrations add -p Taskedo.Tasks.Database.Changes -s Taskedo.Tasks.Database.Startup "Add Task table"
```

## Run

### With Docker Compose

```sh
docker compose --env-file .env -f ./build/docker-compose.yml -p taskedo up --build
```
