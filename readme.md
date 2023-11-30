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

## Run

### With Docker Compose

```sh
docker compose --env-file .env -f ./build/docker-compose.yml -p taskedo up --build
```
