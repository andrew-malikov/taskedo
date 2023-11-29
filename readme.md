# Taskedo

- [Taskedo](#taskedo)

## Overview

## Develop

### Environment Variables

```sh
export $(cat .env | xargs)
```

### Database Migrations

```sh
docker compose --env-file .env -f ./build/docker-compose.yml -p taskedo up
```

```sh
dotnet ef migrations add -p Taskedo.Tasks.Database.Changes -s Taskedo.Tasks.Database.Startup "Add Task table"
```
