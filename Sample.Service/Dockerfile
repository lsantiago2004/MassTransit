FROM mcr.microsoft.com/dotnet/core/runtime:3.1-buster-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Sample.Service/Sample.Service.csproj", "Sample.Service/"]
COPY ["Sample.Contracts/Sample.Contracts.csproj", "Sample.Contracts/"]
COPY ["Sample.Components/Sample.Components.csproj", "Sample.Components/"]
RUN dotnet restore "Sample.Service/Sample.Service.csproj"
COPY . .
WORKDIR "/src/Sample.Service"
RUN dotnet build "Sample.Service.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Sample.Service.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Sample.Service.dll"]
