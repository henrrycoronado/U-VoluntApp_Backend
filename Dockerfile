FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["UVoluntapp.API/UVoluntapp.API.csproj", "UVoluntapp.API/"]
RUN dotnet restore "UVoluntapp.API/UVoluntapp.API.csproj"

COPY . .
WORKDIR "/src/UVoluntapp.API"
RUN dotnet publish "UVoluntapp.API.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_ENVIRONMENT=Production

EXPOSE 8080
ENTRYPOINT ["dotnet", "UVoluntapp.API.dll"]