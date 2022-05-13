.PHONY: help image/production image/dev docker/develop docker/up docker/down test/unit test/integration shell/app shell/db ef/migration ef/migrate db/purge

help: ## Shows all make command that are available (this list) 
	@grep -E '^[a-zA-Z/_-]+:.*?## .*$$' $(MAKEFILE_LIST) | awk 'BEGIN {FS = ":.*?## "}; {printf "\033[36m%-20s\033[0m %s\n", $$1, $$2}'

image/production: ## Docker build the production image
	@DOCKER_BUILDKIT=1 docker build --target final -t rjvdelst/dive .

image/dev: ## Docker build the development image
	@DOCKER_BUILDKIT=1 docker build --target develop -t rjvdelst/dive:dev .

docker/develop: ## Start the development environment
	@docker-compose -f docker-compose.yml -f docker-compose.develop.yml -p 'dive' up

docker/up: ## Start the production image local
	@docker-compose -f docker-compose.yml -p 'dive' up

docker/down: ## Stop all Dive Docker processes
	@docker-compose -p 'dive' down \
	--remove-orphans \
	--rmi 'local'

test/unit: ## Run unit tests
	@docker run \
	-v "$(shell pwd)/src":/dive/src \
	-v "$(shell pwd)/tests/Unit":/dive/tests/Unit \
	-v "$(shell pwd)/tests/Artefacts":/dive/tests/Artefacts \
	--entrypoint "dotnet" \
	rjvdelst/dive:dev \
	test /dive/tests/Unit/UnitTests.csproj --logger:'trx;LogFileName=log.unit.trx'

test/integration: ## Run integration tests
	@docker-compose -f docker-compose.yml -f docker-compose.tests.yml -p 'dive' up \
	--abort-on-container-exit \
	--exit-code-from app \

shell/app: ## Start a shell connection to the app container
	@docker-compose -p 'dive' exec app /bin/bash

shell/db: ## Start a shell connection to PostgreSQL in the database container
	@docker-compose -p 'dive' exec db psql dive dive

ef/migration: ## Make migration with name=MIGRATION_NAME
	@docker-compose -p 'dive' exec app dotnet ef migrations add $(name) --output-dir Data/Migrations \
	--project "/dive/src/Dive.csproj" --msbuildprojectextensionspath "/dive/.net/obj/Dive" --no-build

ef/migrate: ## Run migrations
	@docker-compose -p 'dive' exec app dotnet ef database update \
	--project "/dive/src/Dive.csproj" --msbuildprojectextensionspath "/dive/.net/obj/Dive" --no-build

db/purge: ## Remove all tables inside the database
	@docker-compose -p 'dive' exec db psql dive dive -c \
	"DO \$$\$$ DECLARE r RECORD; BEGIN FOR r IN (SELECT tablename FROM pg_tables WHERE schemaname = current_schema()) LOOP\
    EXECUTE 'DROP TABLE ' || quote_ident(r.tablename) || ' CASCADE'; END LOOP; END \$$\$$;"

