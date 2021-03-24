# Dive
Dive is a web based application to make Kanban boards in your browser. The purpose of this project is to learn new technologies for the author. The following technologies are used:

- Docker
- ASP.NET Core 5
- Entity Framework Core 5 with PostgreSQL
- xUnit for unit and integration tests
- Tailwind CSS

## Production
### Environment variables
The following environment variables are available for the Dive image:
```
ConnectionStrings__PostgreSQL=Server=HOST; Port=PORT; Database=DATABASE; Username=USERNAME; Password=PASSWORD;
```

### Running the production image
This project provides several Make command to build and run Docker commands. To run the application in a production like environment use the following commands:

- `make image` to create the production image from the Dockerfile.
- `make up` to start the application on port `8080` in the detached modus.
- `make down` to terminate all running Docker processes.

Go to `localhost:8080` in your browser.

## Development
### Docker
This project provides several Make command to build and run Docker commands. The following commands are available:

- `make dev-image` to create the development image from the Dockerfile.
- `make develop` to start the dev image with watch enabled on port `5000`.
- `make down` to terminate all running Docker processes.

Go to `localhost:5000` in your browser. The file watcher is enabled and will rebuild the application when a file in the `./Src` folder is changed.

### Debugger
The following options are available to debug the application while in development mode:

- **Visual Studio Code**: The `Attach debugger` launch configuration attaches the debugger to the Dive image. The Docker container must be running before running the command.

### Database Migrations
Run the following commands to update the database schema:

- Open a shell into the running app container with `make shell`
- Run the command `dotnet ef database update --no-build`. The "no build" flag is nessesery because the application is already running and a build will fail.
