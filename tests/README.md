## Unit tests
- Framework: `xUnit`
- Report: `tests/Artefacts/log.unit.trx`
- Run: `make test/unit` (in the project root folder)

The unit tests run in isolation on the Docker image. No external services or storages are available during the tests. 

## Integration tests
- Framework: `xUnit`
- Report: `tests/Artefacts/log.integration.trx`
- Run: `make test/integration` (in the project root folder)

The integration test are running on Docker with all services available. Every test class implements the `TestApplicationFixture` witch wraps the `WebApplicationFactory`. This allows the test to interact with the actual application trough HTTP calls.

> :warning: The application and database are instantiated on a class basis. Separate tests into their own files to test in a clean state.

#### `TestApplicationFixture` functions:
- `CreateClient()`: Create a `HttpClient` to interact with the application.
- `CreateAuthenticatedClient()`: Create a `HttpClient` with the user `john` (id: `1`) authenticated to interact with the application.
- `CreateContext()`: Create a instance of the `DiveContext` to interact with the database inside the tests. This can be useful to prepare the database for the test beforehand or assert that the database is in a certain state after the test.