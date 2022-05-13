##
## General
##
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS runtime
RUN apt-get update
WORKDIR /dive
RUN mkdir storage
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS sdk
RUN apt-get update
WORKDIR /dive
RUN mkdir storage
COPY Directory.Build.props .
COPY *.sln .
COPY resources resources
COPY src src
COPY tests tests
RUN dotnet tool install --global dotnet-ef
RUN dotnet restore
ENV PATH="~/.dotnet/tools:${PATH}"
RUN curl -sLO https://github.com/tailwindlabs/tailwindcss/releases/latest/download/tailwindcss-linux-x64
RUN chmod +x tailwindcss-linux-x64
RUN mv tailwindcss-linux-x64 /dive/resources/style/tailwindcss

##
## Development
##
FROM sdk as develop
RUN apt-get install -y procps && rm -rf /var/lib/apt/lists/*
RUN curl -sSL https://aka.ms/getvsdbgsh | /bin/sh /dev/stdin -v latest -l /vsdbg
ENV SolutionDir /dive
WORKDIR /dive
RUN /dive/resources/style/tailwindcss \
    -c /dive/resources/style/tailwind.config.js \
    -i /dive/resources/style/tailwind.css \
    -o /dive/resources/wwwroot/css/style.css
CMD dotnet watch -v run --urls=http://+:5000/ --project /dive/src/Dive.csproj --launch-profile 'Dive'

##
## Production
##
FROM sdk AS publish
RUN dotnet build "/dive/src/Dive.csproj" -c Release -o /dive/build
RUN dotnet publish "/dive/src/Dive.csproj" -c Release -o /dive/publish
RUN /dive/resources/style/tailwindcss \
    -c /dive/resources/style/tailwind.config.js \
    -i /dive/resources/style/tailwind.css \
    -o /dive/resources/style/style.css --minify

FROM runtime AS final
COPY --from=sdk /dive/src/Properties /dive/app/Properties
COPY --from=sdk /dive/resources/wwwroot /dive/resources/wwwroot
COPY --from=publish /dive/publish /dive/app
COPY --from=publish /dive/resources/style/style.css /dive/resources/wwwroot/css
WORKDIR /dive/app
ENTRYPOINT ["dotnet", "/dive/app/Dive.dll"]