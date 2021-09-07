.PHONY: image dev-image develop tests unit integration up down shell db

image:
	@DOCKER_BUILDKIT=1 docker build --target final -t rjvdelst/dive .

dev-image:
	@DOCKER_BUILDKIT=1 docker build --target develop -t rjvdelst/dive:dev .

develop:
	@docker-compose -f docker-compose.yml -f docker-compose.develop.yml -p 'dive' up

tests: unit integration

unit:
	@docker run \
	-v "$(shell pwd)/src":/dive/src \
	-v "$(shell pwd)/tests/Unit":/dive/tests/Unit \
	-v "$(shell pwd)/tests/Artefacts":/dive/tests/Artefacts \
	--entrypoint "dotnet" \
	rjvdelst/dive:dev \
	test /dive/tests/Unit/UnitTests.csproj --logger:'trx;LogFileName=log.unit.trx'

integration:
	@docker-compose -f docker-compose.yml -f docker-compose.tests.yml -p 'dive' up \
	--abort-on-container-exit \
	--exit-code-from app \

up:
	@docker-compose -f docker-compose.yml -p 'dive' up

down:
	@docker-compose -p 'dive' down \
	--remove-orphans \
	--rmi 'local'

shell:
	@docker-compose -p 'dive' exec app /bin/ash

db:
	@docker-compose -p 'dive' exec db psql dive dive