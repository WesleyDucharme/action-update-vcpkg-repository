FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

WORKDIR /src

COPY "TestApp/" "TestApp/"

WORKDIR /src/TestApp
RUN dotnet restore
RUN dotnet build --no-restore

ENTRYPOINT ["dotnet", "run", "-c Release", "--no-restore", "--no-build"]