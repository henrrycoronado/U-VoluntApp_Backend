FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["U-VoluntApp_Backend.csproj", "./"]
RUN dotnet restore "U-VoluntApp_Backend.csproj"

COPY . .
RUN dotnet publish "U-VoluntApp_Backend.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app

# Las imágenes de .NET 8.0+ ya incluyen un usuario 'app' (UID 16534)
# Solo necesitamos asegurarnos de que el directorio /app le pertenezca
COPY --from=build /app/publish .
RUN chown -R app:app /app

USER app

ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production

EXPOSE 8080

ENTRYPOINT ["dotnet", "U-VoluntApp_Backend.dll"]