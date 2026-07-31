FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["U-VoluntApp_Core.csproj", "./"]
RUN dotnet restore "U-VoluntApp_Core.csproj"

COPY . .
RUN dotnet publish "U-VoluntApp_Core.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app

COPY --from=build /app/publish .
RUN chown -R app:app /app

USER app

ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production

EXPOSE 8080

ENTRYPOINT ["dotnet", "U-VoluntApp_Core.dll"]