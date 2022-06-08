FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
WORKDIR /app

COPY *.csproj ./
RUN dotnet restore
COPY . ./

RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build-env /app/out .

ARG PASSWORD
ENV ASPNETCORE_ENVIRONMENT=Production
ENV DATABASE_PROD=
ENV TOKEN=

EXPOSE 5001

ENTRYPOINT ["dotnet", "AsistenciaBack.dll"]
