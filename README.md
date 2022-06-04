# Requisitos
* Git
* .NET 6 SDK
* Base de datos MySQL

# Instalación del entorno de desarrollo (DEV) en Linux
1. git clone https://github.com/felipeOL/AsistenciaBack.git
2. git checkout dev
3. dotnet restore
4. dotnet tool install --global dotnet-ef
5. export PATH="$PATH:$HOME/.dotnet/tools/"
6. nano ~/.bash_profile
7. export DATABASE_DEV='server=HOST;port=PORT;username=USERNAME;password=PASSWORD;database=DATABASE'
8. export ASPNETCORE_ENVIRONMENT='Development'
9. export TOKEN='TOKEN'
10. Exit
11. source ~/.bash_profile

# Instalación del entorno de desarollo (DEV) en Windows
1. git clone https://github.com/felipeOL/AsistenciaBack.git
2. git checkout dev
3. dotnet restore
4. dotnet tool install --global dotnet-ef
5. [System.Environment]::SetEnvironmentVariable('DATABASE_DEV','server=HOST;port=PORT;username=USERNAME;password=PASSWORD;database=DATABASE')
6. [System.Environment]::SetEnvironmentVariable('TOKEN','TOKEN')
7. [System.Environment]::SetEnvironmentVariable('ASPNETCORE_ENVIRONMENT','Development')

# Migraciones
1. dotnet ef migrations add MIGRATION

# Base de datos
1. dotnet ef database drop
2. dotnet ef database update

# Ejecución
1. dotnet run
