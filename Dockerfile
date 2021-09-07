##
## General
##
FROM mcr.microsoft.com/dotnet/aspnet:5.0-alpine AS runtime
WORKDIR /dive
RUN mkdir storage
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0-alpine AS sdk
WORKDIR /dive
RUN mkdir storage
COPY Directory.Build.props .
COPY *.sln .
COPY resources/wwwroot resources/wwwroot
COPY src src
COPY tests tests
RUN dotnet tool install --global dotnet-ef
RUN dotnet restore
ENV PATH="~/.dotnet/tools:${PATH}"

FROM node:current-alpine as tailwind
WORKDIR /resources
COPY resources/style/. .
COPY src/Views/. Views
RUN npm ci --production

##
## Development
##
FROM tailwind as resources-development
RUN npm run build

FROM sdk as develop
RUN apk update && \
    apk add unzip && \
    apk add procps
RUN curl -sSL https://aka.ms/getvsdbgsh | /bin/sh /dev/stdin -v latest -l /vsdbg
ENV DOTNET_USE_POLLING_FILE_WATCHER 1
ENV SolutionDir /dive
WORKDIR /dive
COPY --from=resources-development /resources/style.css resources/wwwroot/css
ENTRYPOINT dotnet watch run --urls=http://+:5000/ --project src/Dive.csproj

##
## Production
##
FROM tailwind as resources-production
RUN npm run production

FROM sdk AS publish
RUN dotnet build "/dive/src/Dive.csproj" -c Release -o /dive/build
RUN dotnet publish "/dive/src/Dive.csproj" -c Release -o /dive/publish

FROM runtime AS final
WORKDIR /dive
COPY --from=sdk /dive/resources/wwwroot resources/wwwroot
COPY --from=publish /dive/publish app
COPY --from=sdk /dive/src/Properties app/Properties
COPY --from=resources-production /resources/style.css resources/wwwroot/css
WORKDIR /dive/app
ENTRYPOINT ["dotnet", "Dive.dll"]