FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

WORKDIR /src

COPY "ActionUpdateVcpkgRepository/" "ActionUpdateVcpkgRepository/"

WORKDIR /src/ActionUpdateVcpkgRepository
RUN dotnet restore
RUN dotnet build --no-restore

ENTRYPOINT [
"dotnet", 
"run", 
"--project /src/ActionUpdateVcpkgRepository/ActionUpdateVcpkgRepository.csproj", 
"-c Release", 
"--no-restore", 
"--no-build"
]
