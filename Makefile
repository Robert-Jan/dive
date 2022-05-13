.PHONY: help image dev-image develop unit integration up down shell

help: ## Shows all make command that are available (this list) 
	@grep -E '^[a-zA-Z_-]+:.*?## .*$$' $(MAKEFILE_LIST) | awk 'BEGIN {FS = ":.*?## "}; {printf "\033[36m%-20s\033[0m %s\n", $$1, $$2}'

image: ## Docker build the production image
	@DOCKER_BUILDKIT=1 docker build --target final -t rjvdelst/dive .

dev-image: ## Docker build the development image
	@DOCKER_BUILDKIT=1 docker build --target develop -t rjvdelst/dive:dev .

develop: ## Start the development environment
	@docker-compose -f docker-compose.yml -f docker-compose.develop.yml -p 'dive' up

unit: ## Run unit tests
	@docker run \
	-v "$(shell pwd)/src":/dive/src \
	-v "$(shell pwd)/tests/Unit":/dive/tests/Unit \
	-v "$(shell pwd)/tests/Artefacts":/dive/tests/Artefacts \
	--entrypoint "dotnet" \
	rjvdelst/dive:dev \
	test /dive/tests/Unit/UnitTests.csproj --logger:'trx;LogFileName=log.unit.trx'

integration: ## Run integration tests
	@docker-compose -f docker-compose.yml -f docker-compose.tests.yml -p 'dive' up \
	--abort-on-container-exit \
	--exit-code-from app \

up: ## Start the production image local
	@docker-compose -f docker-compose.yml -p 'dive' up

down: ## Stop all Dive Docker processes
	@docker-compose -p 'dive' down \
	--remove-orphans \
	--rmi 'local'

shell: ## Start a shell connection to the app container
	@docker-compose -p 'dive' exec app /bin/bash
