# Dive
The following technologies are used:

- Docker
- ASP.NET Core 6
- ASP.NET Core Identity
- Entity Framework Core 6 with PostgreSQL
- xUnit for unit and integration tests
- Tailwind CSS
- Alpine.js

## Production
### Environment variables
The following environment variables are available for the Dive image:
```
ConnectionStrings__PostgreSQL=Server=HOST; Port=PORT; Database=DATABASE; Username=USERNAME; Password=PASSWORD;
```

### Running the production image
This project provides several Make command to build and run Docker commands. To run the application in a production like environment use the following commands:

- `make image/production` to create the production image from the Dockerfile.
- `make docker/up` to start the application on port `8080`.
- `make docker/down` to terminate all running Docker processes.

Go to `localhost:8080` in your browser.

## Development
### Docker
This project provides several Make command to build and run Docker commands. The following commands are available:

- `make image/dev` to create the development image from the Dockerfile.
- `make docker/develop` to start the dev image with watch enabled on port `5000`.
- `make docker/down` to terminate all running Docker processes.

Go to `localhost:5000` in your browser. The file watcher is enabled and will rebuild the application when a file in the `./src` folder is changed.

### Commands
- `make test/unit` Run the unit tests.
- `make test/integration` Run the integration tests.
- `make shell/app` Create a bash shell into the application container (containers must be running).
- `make shell/db` Create a SQL shell into the database container (containers must be running).
- `make ef/migration name=MIGRATION_NAME` Create a new migration (containers must be running).
- `make ef/migrate` Run migrations on the database (containers must be running).
- `make db/purge` Remove all tables from the database (containers must be running).

### Debugger
The following options are available to debug the application while in development mode:

- **Visual Studio Code**: The `Attach debugger` launch configuration attaches the debugger to the Dive image. The Docker container must be running before running the command.