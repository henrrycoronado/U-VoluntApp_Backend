FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["U-VoluntApp_Backend.csproj", "./"]
RUN dotnet restore "U-VoluntApp_Backend.csproj"

COPY . .
RUN dotnet publish "U-VoluntApp_Backend.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app

ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production

EXPOSE 8080

COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "U-VoluntApp_Backend.dll"]