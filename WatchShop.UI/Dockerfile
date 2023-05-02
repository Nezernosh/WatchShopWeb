#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["WatchShop.UI/WatchShop.UI.csproj", "WatchShop.UI/"]
COPY ["WatchShop.BLL/WatchShop.BLL.csproj", "WatchShop.BLL/"]
COPY ["WatchShop.DAL/WatchShop.DAL.csproj", "WatchShop.DAL/"]
COPY ["WatchShop.Entities/WatchShop.Entities.csproj", "WatchShop.Entities/"]
COPY ["WatchShop.TelegramBot/WatchShop.TelegramBot.csproj", "WatchShop.TelegramBot/"]
RUN dotnet restore "WatchShop.UI/WatchShop.UI.csproj"
COPY . .
WORKDIR "/src/WatchShop.UI"
RUN dotnet build "WatchShop.UI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WatchShop.UI.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WatchShop.UI.dll"]