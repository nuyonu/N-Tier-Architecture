FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

COPY ["./src/N-Tier.API/N-Tier.API.csproj", "src/N-Tier.API/"]
COPY ["./src/N-Tier.Application/N-Tier.Application.csproj", "src/N-Tier.Application/"]
COPY ["./src/N-Tier.Core/N-Tier.Core.csproj", "src/N-Tier.Core/"]
COPY ["./src/N-Tier.DataAccess/N-Tier.DataAccess.csproj", "src/N-Tier.DataAccess/"]
COPY ["./src/N-Tier.Shared/N-Tier.Shared.csproj", "src/N-Tier.Shared/"]

RUN dotnet restore "src/N-Tier.API/N-Tier.API.csproj"

COPY . .

WORKDIR "src/N-Tier.API/"
RUN dotnet build -c Release -o /app/build

FROM build AS publish
RUN dotnet publish -c Release -o /app/publish

FROM base AS runtime
WORKDIR /app

COPY --from=publish /app/publish .

ENTRYPOINT [ "dotnet", "N-Tier.API.dll" ]
