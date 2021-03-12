.PHONY: dev-image image develop up down shell db

dev-image:
	@DOCKER_BUILDKIT=1 docker build --target develop -t robert-jan/dive:dev .

image:
	@DOCKER_BUILDKIT=1 docker build --target final -t robert-jan/dive .

develop:
	@docker-compose -f docker-compose.yml -f docker-compose.override.yml -p 'dive' up

up:
	@docker-compose -f docker-compose.yml -p 'dive' up -d

down:
	@docker-compose -p 'dive' down --remove-orphans --rmi 'local'

shell:
	@docker-compose -p 'dive' exec app /bin/ash

db:
	@docker-compose -p 'dive' exec db psql dive dive