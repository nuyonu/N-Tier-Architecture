# Utilizăm imaginea oficială .NET 6 SDK pentru a construi aplicația noastră
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build

# Setăm directorul de lucru la folderul radăcină al aplicației
WORKDIR /app

# Copiem fișierele de proiect și csproj în directorul curent
COPY src/N-Tier.API/*.csproj ./src/N-Tier.API/
COPY src/N-Tier.Application/*.csproj ./src/N-Tier.Application/
COPY src/N-Tier.Core/*.csproj ./src/N-Tier.Core/
COPY src/N-Tier.DataAccess/*.csproj ./src/N-Tier.DataAccess/
COPY src/N-Tier.Shared/*.csproj ./src/N-Tier.Shared/

# Restaurăm pachetele NuGet în directorul corespunzător
WORKDIR /app/src/N-Tier.API
RUN dotnet restore

# Copiem întregul conținut al soluției în container
WORKDIR /app
COPY . .

# Setăm directorul de lucru la proiectul API
WORKDIR /app/src/N-Tier.API

# Construim aplicația
RUN dotnet build -c Release -o /app/build

# Stage final pentru a crea imaginea de producție
FROM build AS publish
RUN dotnet publish -c Release -o /app/publish

# Creăm imaginea finală, folosind imaginea oficială .NET 6 pentru aplicații ASP.NET
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "N-Tier.API.dll"]
