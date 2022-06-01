FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
WORKDIR /app

COPY *.csproj ./
RUN dotnet restore
COPY . ./

RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build-env /app/out .

ARG CERTIFICATE
ARG PASSWORD
COPY $CERTIFICATE ./
ENV ASPNETCORE_Kestrel__Certificates__Default__Password=$PASSWORD
ENV ASPNETCORE_Kestrel__Certificates__Default__Path=./aspnetapp.pfx
ENV ASPNETCORE_URLS=https://*:7000
ENV ASPNETCORE_ENVIRONMENT=Production
ENV DATABASE_PROD=
ENV TOKEN=

EXPOSE 7000

ENTRYPOINT ["dotnet", "AsistenciaBack.dll"]
