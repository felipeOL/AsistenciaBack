# Requisitos
* .NET 6
* Base de datos MySQL

# Instalación

1. git clone https://github.com/felipeOL/AsistenciaBack.git
2. git checkout stage (Opcional)
4. dotnet tool install --global dotnet-ef
5. dotnet restore
6. dotnet user-secrets init
7. dotnet user-secrets set "Token" "[Token de la aplicación]"
8. dotnet user-secrets set "ConnectionStrings:Dev" "[String de conexión hacia la base de datos MySQL]"
9. dotnet ef database update

# Ejecución

1. dotnet run
