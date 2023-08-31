FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/FileShare/FileShare.csproj", "FileShare/"]
RUN dotnet restore "FileShare/FileShare.csproj"
COPY ./src .
WORKDIR "/src/FileShare"
RUN dotnet build "FileShare.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "FileShare.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FileShare.dll"]